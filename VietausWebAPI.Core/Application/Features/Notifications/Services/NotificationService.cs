using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Notifications.DTOs;
using VietausWebAPI.Core.Application.Features.Notifications.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Domain.Entities.Notifications;
using VietausWebAPI.Core.Identity;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.WebAPI.Helpers.Securities.Roles;

namespace VietausWebAPI.Core.Application.Features.Notifications.Services
{
    public sealed class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;

        public NotificationService(IUnitOfWork uow, ICurrentUser currentUser) => (_uow, _currentUser) = (uow, currentUser);

        public async Task<NotificationDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var companyId = _currentUser.CompanyId;
            var employeeId = _currentUser.EmployeeId;


            var roles = _currentUser.Roles ?? Enumerable.Empty<string>();
            //var teamIds = _currentUser.TeamIds ?? Enumerable.Empty<Guid>();

            // Chỉ cho xem nếu:
            // - Notification thuộc cùng Company
            // - Và user hiện tại có UserState (được nhận thông báo)
            var noti = await _uow.Notifications.Query(track: true)
                .Include(n => n.Recipients)
                .Include(n => n.UserStates)
                .FirstOrDefaultAsync(n => n.Id == id && n.CompanyId == companyId, ct);

            if (noti is null) return null;

            // Cho xem nếu:
            // 1) đã có UserState cho user
            // 2) hoặc user khớp 1 trong các loại Recipient (User / Role / Team)
            bool canView =
                noti.UserStates.Any(s => s.UserId == employeeId) ||
                noti.Recipients.Any(r =>
                    (r.TargetUserId.HasValue && r.TargetUserId.Value == employeeId) ||
                    (!string.IsNullOrEmpty(r.TargetRole) &&
                         roles.Contains(r.TargetRole!, StringComparer.OrdinalIgnoreCase))

                // Tạm thời không lấy theo Team
                //     ||
                //(r.TargetTeamId.HasValue && teamIds.Contains(r.TargetTeamId.Value))
                );

            if (!canView) return null;

            // Auto-materialize UserState lần đầu (idempotent)
            var state = noti.UserStates.FirstOrDefault(s => s.UserId == employeeId);
            if (state is null)
            {
                noti.UserStates.Add(new NotificationUserState
                {
                    NotificationId = noti.Id,
                    UserId = employeeId,
                    IsRead = false
                });
                await _uow.SaveChangesAsync();
                state = noti.UserStates.First(s => s.UserId == employeeId);
            }

            // Map DTO
            return new NotificationDto
            {
                Id = noti.Id,
                //Topic = noti.Topic,
                Severity = noti.Severity,
                Title = noti.Title,
                Message = noti.Message,
                Link = noti.Link,
                PayloadJson = noti.PayloadJson,
                CreatedDate = noti.CreatedDate,
                IsRead = state.IsRead,
                ReadDate = state.ReadDate
            };
        }

        public async Task<IReadOnlyList<NotificationDto>> GetFeedAsync(int take = 20, Guid? afterId = null, DateTime? afterCreated = null, CancellationToken ct = default)
        {
            var companyId = _currentUser.CompanyId;
            var userId = _currentUser.EmployeeId;

            // Keyset paging (CreatedDate DESC, tie-breaker by Id DESC)
            var q = _uow.Notifications.Query(track: false)
                .Where(n => n.CompanyId == companyId
                            && n.UserStates.Any(us => us.UserId == userId))
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Topic = n.Topic,
                    Severity = n.Severity,
                    Title = n.Title,
                    Message = n.Message,
                    Link = n.Link,
                    PayloadJson = n.PayloadJson,
                    CreatedDate = n.CreatedDate,
                    IsRead = n.UserStates.Where(us => us.UserId == userId).Select(us => us.IsRead).FirstOrDefault(),
                    ReadDate = n.UserStates.Where(us => us.UserId == userId).Select(us => us.ReadDate).FirstOrDefault(),
                    CreatedBy = n.CreatedBy,
                    CreatedByNameSnapshot = n.CreatedByNameSnapshot
                });

            if (afterCreated.HasValue && afterId.HasValue)
            {
                // (CreatedDate, Id) keyset: lấy “các bản ghi cũ hơn”
                q = q.Where(x => x.CreatedDate < afterCreated.Value
                      || (x.CreatedDate == afterCreated.Value && x.Id.CompareTo(afterId.Value) < 0));
            }

            return await q
                .OrderByDescending(x => x.CreatedDate).ThenByDescending(x => x.Id)
                .Take(take)
                .ToListAsync(ct);
        }

        public async Task<int> GetUnreadCountAsync(CancellationToken ct = default)
        {
            var companyId = _currentUser.CompanyId;
            var userId = _currentUser.EmployeeId;

            var q = _uow.NotificationUserStates.Query(track: false)
                .Where(us => us.Notification.CompanyId == companyId
                             && us.UserId == userId
                             && !us.IsRead);

            return await q.CountAsync(ct);
        }

        public async Task<int> MarkAllReadAsync(CancellationToken ct = default)
        {
            var userId = _currentUser.EmployeeId;
            var companyId = _currentUser.CompanyId;

            var updated = await _uow.NotificationUserStates.Query(track: false)
                .Where(x => x.UserId == userId
                            && !x.IsRead
                            && x.Notification.CompanyId == companyId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.IsRead, true)
                    .SetProperty(x => x.ReadDate, DateTime.Now), ct);

            return updated;
        }

        public async Task MarkReadAsync(Guid notiId, CancellationToken ct = default)
        {
            var userId = _currentUser.EmployeeId;
            var companyId = _currentUser.CompanyId;

            var updated = await _uow.NotificationUserStates.Query(track: false)
                .Where(x => x.NotificationId == notiId
                            && x.UserId == userId
                            && !x.IsRead
                            && x.Notification.CompanyId == companyId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.IsRead, true)
                    .SetProperty(x => x.ReadDate, DateTime.Now), ct);
        }


        public async Task<Guid> PublishAsync(PublishNotificationRequest req, CancellationToken ct = default)
        {


            var companyId = _currentUser.CompanyId;
            var employeeId = _currentUser.EmployeeId;
            var now = DateTime.Now;

            // 1) Notification (UTC)
            var n = new Notification
            {
                Id = Guid.CreateVersion7(),
                CompanyId = companyId,
                //Topic = req.Topic,
                Severity = req.Severity,
                Title = req.Title,
                Message = req.Message,
                Link = req.Link,
                PayloadJson = req.PayloadJson,
                CreatedDate = now,
                CreatedBy = employeeId,
                CreatedByNameSnapshot = _currentUser.personName
            };
            await _uow.Notifications.AddAsync(n, ct);

            // 2) Lưu dấu vết recipients (audit)
            if (req.TargetUserIds?.Any() == true)
                foreach (var eid in req.TargetUserIds.Where(x => x != Guid.Empty).Distinct())
                    await _uow.NotificationRecipients.AddAsync(new NotificationRecipient
                    {
                        Id = Guid.CreateVersion7(),
                        NotificationId = n.Id,
                        TargetUserId = eid
                    }, ct);

            if (req.TargetRoles?.Any() == true)
                foreach (var role in req.TargetRoles
                            .Where(r => !string.IsNullOrWhiteSpace(r))
                            .Select(r => r.Trim().ToUpperInvariant())
                            .Distinct(StringComparer.OrdinalIgnoreCase))
                    await _uow.NotificationRecipients.AddAsync(new NotificationRecipient
                    {
                        Id = Guid.CreateVersion7(),
                        NotificationId = n.Id,
                        TargetRole = role
                    }, ct);

            if (req.TargetTeamIds?.Any() == true)
                foreach (var tid in req.TargetTeamIds.Where(x => x != Guid.Empty).Distinct())
                    await _uow.NotificationRecipients.AddAsync(new NotificationRecipient
                    {
                        Id = Guid.CreateVersion7(),
                        NotificationId = n.Id,
                        TargetTeamId = tid
                    }, ct);

            // 3) Fan-out -> EmployeeIds (KHÔNG đổi schema)
            var resolvedEmployeeIds = new HashSet<Guid>();

            // 3.1) Direct employees
            if (req.TargetUserIds?.Any() == true)
            {
                var direct = await _uow.EmployeesRepository.Query()
                    .Where(e => e.CompanyId == companyId && e.IsActive == true
                                && req.TargetUserIds.Contains(e.EmployeeId))
                    .Select(e => e.EmployeeId)
                    .ToListAsync(ct);
                foreach (var id in direct) resolvedEmployeeIds.Add(id);
            }

            // 3.2) Roles -> Accounts -> Employees
                if (req.TargetRoles?.Any() == true)
            {
                // 1) Tách chuỗi có dấu phẩy thành từng role riêng
                var normalizedRoles = req.TargetRoles
                    .SelectMany(r => (r ?? string.Empty)
                        .Split(',', StringSplitOptions.RemoveEmptyEntries))
                    .Select(r => r.Trim())
                    .Where(r => !string.IsNullOrWhiteSpace(r))
                    .Select(r => r.ToUpperInvariant()) // khớp với NormalizedName
                    .Distinct()
                    .ToList();

                if (normalizedRoles.Count > 0)
                {
                    // 2) Lấy RoleIds theo NormalizedName
                    var roleIds = await _uow.ApplicationRoleRepository.Query()
                        .Where(r => r.NormalizedName != null && normalizedRoles.Contains(r.NormalizedName))
                        .Select(r => r.Id)
                        .ToListAsync(ct);

                    if (roleIds.Count > 0)
                    {
                        // 3) Lấy UserId (account) thuộc các role (chỉ active)
                        var accountIds = await _uow.ApplicationUserRoleRepository.Query()
                            .Where(ur => ur.IsActive && roleIds.Contains(ur.RoleId))
                            .Select(ur => ur.UserId)
                            .Distinct()
                            .ToListAsync(ct);

                        if (accountIds.Count > 0)
                        {
                            // 4) Map AccountId -> EmployeeId
                            var empIdsFromRoles = await _uow.ApplicationUserRepository.Query()
                                .Where(u => accountIds.Contains(u.Id))
                                .Select(u => u.EmployeeId)
                                .ToListAsync(ct);

                            foreach (var id in empIdsFromRoles)
                            {
                                if (id.HasValue) resolvedEmployeeIds.Add(id.Value);
                            }
                        }
                    }
                }
            }


            // 3.3) Teams -> Employees
            //if (req.TargetTeamIds?.Any() == true)
            //{
            //    var teamIds = req.TargetTeamIds.Where(x => x != Guid.Empty).Distinct().ToList();

            //    if (teamIds.Count > 0)
            //    {
            //        var empIdsFromTeams = await _uow.EmployeeTeams.AsNoTracking()
            //            .Where(et => et.IsActive && teamIds.Contains(et.TeamId))
            //            .Select(et => et.EmployeeId)
            //            .Distinct()
            //            .ToListAsync(ct);

            //        if (empIdsFromTeams.Count > 0)
            //        {
            //            var validTeamEmpIds = await _uow.Employees.AsNoTracking()
            //                .Where(e => e.CompanyId == companyId && e.IsActive
            //                            && empIdsFromTeams.Contains(e.Id))
            //                .Select(e => e.Id)
            //                .ToListAsync(ct);

            //            foreach (var id in validTeamEmpIds) resolvedEmployeeIds.Add(id);
            //        }
            //    }
            //}

            // 4) Materialize UserState (idempotent; DB có unique index (NotificationId, UserId))
            if (resolvedEmployeeIds.Count > 0)
            {
                var existing = await _uow.NotificationUserStates.Query()
                    .Where(s => s.NotificationId == n.Id && resolvedEmployeeIds.Contains(s.UserId))
                    .Select(s => s.UserId)
                    .ToListAsync(ct);

                var toCreate = resolvedEmployeeIds.Except(existing).ToList();
                foreach (var uid in toCreate)
                {
                    await _uow.NotificationUserStates.AddAsync(new NotificationUserState
                    {
                        NotificationId = n.Id,
                        UserId = uid,
                        IsRead = false,
                        ReadDate = null,
                        IsArchived = false
                    }, ct);
                }
            }

            //// (Tuỳ chọn) Nếu bạn muốn push theo Account group:
            //// Map thêm AccountId để ghi vào envelope; KHÔNG cần đổi DB.
            //List<Guid> resolvedAccountIds = new();
            //if (resolvedEmployeeIds.Count > 0)
            //{
            //    resolvedAccountIds = await _uow.EmployeesRepository.Query()
            //        .Where(e => e.CompanyId == companyId && e.IsActive
            //                    && resolvedEmployeeIds.Contains(e.Id)
            //                    && e.AccountId != null)
            //        .Select(e => e.AccountId!.Value)
            //        .Distinct()
            //        .ToListAsync(ct);
            //}

            // 5) Outbox (InAppPush) – giữ nguyên pattern
            var envelope = new OutboxEnvelope
            {
                CompanyId = companyId,
                NotificationId = n.Id,
                TargetRoles = req.TargetRoles,
                TargetUserIds = req.TargetUserIds,
                TargetTeamIds = req.TargetTeamIds,
                TargetComposite = null,
                Meta = null
            };

            var outbox = new OutboxMessage
            {
                Type = "InAppPush",
                PayloadJson = JsonSerializer.Serialize(envelope),
                CreatedAt = now
            };
            await _uow.OutboxMessages.AddAsync(outbox, ct);

            await _uow.SaveChangesAsync();
            return n.Id;
        }
    }
}
