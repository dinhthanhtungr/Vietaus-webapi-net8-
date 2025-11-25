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
using VietausWebAPI.Core.Repositories_Contracts;

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
                Topic = noti.Topic,
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
        public async Task<Guid> PublishAsync(PublishNotificationRequest req, CancellationToken ct = default)
        {
            var companyId = _currentUser.CompanyId;

            // 1) Notification
            var n = new Notification
            {
                Id = Guid.CreateVersion7(),
                CompanyId = companyId,
                Topic = req.Topic,
                Severity = req.Severity,
                Title = req.Title,
                Message = req.Message,
                Link = req.Link,
                PayloadJson = req.PayloadJson,
                CreatedDate = DateTime.Now
            };
            await _uow.Notifications.AddAsync(n, ct);

            // 2) Recipients (user / role / team)
            if (req.TargetUserIds?.Any() == true)
            {
                foreach (var uid in req.TargetUserIds.Where(x => x != Guid.Empty).Distinct())
                    await _uow.NotificationRecipients.AddAsync(new NotificationRecipient
                    {
                        Id = Guid.CreateVersion7(),
                        NotificationId = n.Id,
                        TargetUserId = uid
                    }, ct);
            }

            if (req.TargetRoles?.Any() == true)
            {
                foreach (var role in req.TargetRoles
                                        .Where(r => !string.IsNullOrWhiteSpace(r))
                                        .Select(r => r.Trim())
                                        .Distinct(StringComparer.OrdinalIgnoreCase))
                    await _uow.NotificationRecipients.AddAsync(new NotificationRecipient
                    {
                        Id = Guid.CreateVersion7(),
                        NotificationId = n.Id,
                        TargetRole = role
                    }, ct);
            }

            if (req.TargetTeamIds?.Any() == true)
            {
                foreach (var tid in req.TargetTeamIds.Where(x => x != Guid.Empty).Distinct())
                    await _uow.NotificationRecipients.AddAsync(new NotificationRecipient
                    {
                        Id = Guid.CreateVersion7(),
                        NotificationId = n.Id,
                        TargetTeamId = tid
                    }, ct);
            }

            // 3) Outbox (để đẩy realtime sau commit)
            var envelope = new OutboxEnvelope
            {
                CompanyId = companyId,
                NotificationId = n.Id,
                TargetRoles = req.TargetRoles,
                TargetUserIds = req.TargetUserIds,
                TargetTeamIds = req.TargetTeamIds,
                TargetComposite = null,                // nếu có dùng “LEADER+LABUSER” thì set tại đây
                Meta = null
            };

            var outbox = new OutboxMessage
            {
                Type = "InAppPush", // handler của OutboxProcessor
                PayloadJson = JsonSerializer.Serialize(envelope),
                CreatedAt = DateTime.Now
            };
            await _uow.OutboxMessages.AddAsync(outbox, ct);

            await _uow.SaveChangesAsync();
            return n.Id;
        }
    }
}
