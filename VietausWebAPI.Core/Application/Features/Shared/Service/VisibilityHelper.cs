using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Shared.DTO.Visibility;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.Shared.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;
using VietausWebAPI.Core.Domain.Enums.Visibilitys;
using VietausWebAPI.WebAPI.Helpers.Securities.Roles;

namespace VietausWebAPI.Core.Application.Features.Shared.Service
{
    public class VisibilityHelper : IVisibilityHelper
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        public VisibilityHelper(IUnitOfWork unitOfWork, ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        /// <summary>
        /// Phương thức xây dựng phạm vi xem của người dùng hiện tại
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ViewerScope> BuildViewerScopeAsync(CancellationToken ct = default)
        {
            if (!_currentUser.IsAuthenticated) throw new UnauthorizedAccessException();

            // xác định leader group
            var leaderGroupId = await _unitOfWork.MemberInGroupRepository.Query()
                .Where(g => g.Profile == _currentUser.EmployeeId && g.IsAdmin == true && g.IsActive == true)
                .Select(g => (Guid?)g.GroupId)
                .FirstOrDefaultAsync(ct);

            var isLeader = leaderGroupId.HasValue && leaderGroupId.Value != Guid.Empty;
            var groupId = isLeader ? leaderGroupId : null;

            // ai được xem full
            var hasFullView =
                  _currentUser.IsInRole(AppRoles.CustomerViewAll)
               || _currentUser.IsInRole(AppRoles.President)
               || _currentUser.IsInRole(AppRoles.LabUser)
               || _currentUser.IsInRole(AppRoles.Developer);

            var scopeType = hasFullView ? ViewerScopeType.AdminFull
                         : _currentUser.IsInRole(AppRoles.LabUser) ? ViewerScopeType.LabFull // (thực ra đã full ở trên)
                         : ViewerScopeType.SalesScoped;

            // Tập employeeIds trong phạm vi:
            HashSet<Guid> ids = new() { _currentUser.EmployeeId };
            if (isLeader && groupId is Guid gid)
            {
                var memberIds = await _unitOfWork.MemberInGroupRepository.Query()
                    .Where(m => m.GroupId == gid && m.IsActive)
                    .Select(m => m.Profile)           // Profile = EmployeeId
                    .ToListAsync(ct);

                foreach (var id in memberIds)
                    if (id.HasValue)
                        ids.Add(id.Value);
            }

            return new ViewerScope(
                CompanyId: _currentUser.CompanyId,
                EmployeeId: _currentUser.EmployeeId,
                IsLeader: isLeader,
                GroupId: groupId,
                ScopeType: hasFullView ? ViewerScopeType.AdminFull : ViewerScopeType.SalesScoped,
                Now: DateTime.Now,
                EmployeeIdsInScope: ids
            );
        }

        /// <summary>
        /// Phương thức áp dụng phạm vi xem cho truy vấn Customer
        /// </summary>
        /// <param name="q"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public IQueryable<Customer> ApplyCustomer(IQueryable<Customer> q, ViewerScope v)
        {
            q = q.Where(c => c.IsActive == true && c.CompanyId == v.CompanyId);

            if (v.ScopeType is ViewerScopeType.AdminFull or ViewerScopeType.LabFull)
                return q;

            var now = v.Now;
            // SalesScoped
            return q.Where(c =>
                c.IsLead
                || (!v.IsLeader && (
                       c.CustomerAssignments.Any(a => a.IsActive && a.EmployeeId == v.EmployeeId)
                    || c.CustomerClaims.Any(cl => cl.IsActive && cl.Type == ClaimType.Work
                                                  && cl.ExpiresAt > now && cl.EmployeeId == v.EmployeeId)))
                || (v.IsLeader && v.GroupId != null && (
                       c.CustomerAssignments.Any(a => a.IsActive && a.GroupId == v.GroupId)
                    || c.CustomerClaims.Any(cl => cl.IsActive && cl.Type == ClaimType.Work
                                                  && cl.ExpiresAt > now && cl.GroupId == v.GroupId)))
            );
        }

        /// <summary>
        /// Phương thức áp dụng phạm vi xem cho truy vấn SampleRequest
        /// </summary>
        /// <param name="q"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public IQueryable<SampleRequest> ApplySampleRequest(IQueryable<SampleRequest> q, ViewerScope v)
        {
            q = q.Where(sr => sr.IsActive == true && sr.CompanyId == v.CompanyId);

            if (v.ScopeType is ViewerScopeType.AdminFull || v.ScopeType is ViewerScopeType.LabFull)
                return q;

            var now = v.Now;
            // Kế thừa visibility từ Customer
            var scopeIds = v.EmployeeIdsInScope;
            return q.Where(sr => scopeIds.Contains(sr.CreatedBy));
        }

    }
}
