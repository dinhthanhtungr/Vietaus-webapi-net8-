using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.Notifications.DTOs;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;

namespace VietausWebAPI.WebAPI.Backgrounds.Helpers
{
    internal static class NotificationTargetResolver
    {
        public static async Task<HashSet<Guid>> ResolveEmployeeIdsAsync(
            IUnitOfWork uow, Guid companyId, PublishNotificationRequest req, CancellationToken ct)
        {
            var resolved = new HashSet<Guid>();

            var hasUsers = req.TargetUserIds?.Any() == true;
            var hasRoles = req.TargetRoles?.Any() == true;
            var hasTeams = req.TargetTeamIds?.Any() == true;
            if (!hasUsers && !hasRoles && !hasTeams) return resolved;

            // 1) Direct users (lọc trước để IN list gọn)
            if (hasUsers)
            {
                var userIds = req.TargetUserIds!.Where(g => g != Guid.Empty).Distinct().ToArray();
                if (userIds.Length > 0)
                {
                    var direct = await uow.EmployeesRepository.Query()
                        .Where(e => e.CompanyId == companyId && e.IsActive && userIds.Contains(e.EmployeeId))
                        .Select(e => e.EmployeeId)
                        .ToListAsync(ct);
                    foreach (var id in direct) resolved.Add(id);
                }
            }

            // 2) Roles -> Accounts -> Employees (dùng JOIN để giảm round-trip)
            if (hasRoles)
            {
                var normalized = req.TargetRoles!
                    .SelectMany(r => (r ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries))
                    .Select(r => r.Trim())
                    .Where(r => !string.IsNullOrWhiteSpace(r))
                    .Select(r => r.ToUpperInvariant())
                    .Distinct()
                    .ToArray();

                if (normalized.Length > 0)
                {
                    // Nếu bạn muốn broadcast toàn công ty qua role đặc biệt
                    if (normalized.Contains("__ALL__") || normalized.Contains("ALL"))
                    {
                        var allEmp = await uow.EmployeesRepository.Query()
                            .Where(e => e.CompanyId == companyId && e.IsActive)
                            .Select(e => e.EmployeeId)
                            .ToListAsync(ct);
                        foreach (var id in allEmp) resolved.Add(id);
                    }
                    else
                    {
                        // JOIN: Role(NormalizedName) -> UserRole(RoleId, IsActive) -> User(Id) -> Employee(EmployeeId, CompanyId, IsActive)
                        var roleEmpIds = await (
                            from r in uow.ApplicationRoleRepository.Query().AsNoTracking()
                            join ur in uow.ApplicationUserRoleRepository.Query().AsNoTracking()
                                on r.Id equals ur.RoleId
                            join u in uow.ApplicationUserRepository.Query().AsNoTracking()
                                on ur.UserId equals u.Id
                            join e in uow.EmployeesRepository.Query().AsNoTracking()
                                on u.EmployeeId equals e.EmployeeId
                            where r.NormalizedName != null
                                  && normalized.Contains(r.NormalizedName)
                                  && ur.IsActive
                                  && e.IsActive
                                  && e.CompanyId == companyId
                            select e.EmployeeId
                        ).Distinct().ToListAsync(ct);

                        foreach (var id in roleEmpIds) resolved.Add(id);
                    }
                }
            }

            // 3) Teams (Group -> MemberInGroup -> EmployeeId) với join
            if (hasTeams)
            {
                var teamIds = req.TargetTeamIds!
                    .Where(g => g != Guid.Empty)
                    .Distinct()
                    .ToArray();

                if (teamIds.Length > 0)
                {
                    var teamEmpIds = await (
                        from m in uow.MemberInGroupRepository.Query()
                        join g in uow.GroupRepository.Query()
                            on m.GroupId equals g.GroupId
                        where teamIds.Contains(m.GroupId)
                              && m.IsActive
                              && m.Profile != null
                              && g.CompanyId == companyId
                        select m.Profile!.Value
                    ).Distinct().ToListAsync(ct);

                    foreach (var id in teamEmpIds) resolved.Add(id);
                }
            }

            return resolved;
        }
    }
}
