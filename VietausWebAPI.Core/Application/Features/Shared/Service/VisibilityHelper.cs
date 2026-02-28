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
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
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
        /// Quy tắc:
        /// - Admin/President/Developer/CustomerViewAll/Lab: thấy tất cả
        /// - Sales thường: KH mình quản + claim Work (còn hạn)
        /// - Leader: KH do group quản + claim Work của group (còn hạn)
        /// </summary>
        /// <param name="q"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public IQueryable<Customer> ApplyCustomer(IQueryable<Customer> q, ViewerScope v)
        {
            q = q.Where(c => c.IsActive == true);

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
        /// Quy tắc:
        /// - Admin/President/Developer/CustomerViewAll/Lab: thấy tất cả
        /// - Sales thường: chỉ thấy SR thuộc customer mình (hoặc group) quản lý
        /// - Leader: chỉ thấy SR thuộc customer mình (hoặc group) quản lý
        /// </summary>
        /// <param name="q"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public IQueryable<SampleRequest> ApplySampleRequest(IQueryable<SampleRequest> q, ViewerScope v)
        {
            q = q.Where(sr => sr.IsActive == true);

            if (v.ScopeType is ViewerScopeType.AdminFull or ViewerScopeType.LabFull)
                return q;

            var now = DateTime.Now;

            // visibleCustomers: IQueryable<Customer> đã được ApplyCustomer lọc theo scope (mình/group)
            var visibleCustomers = ApplyCustomer(_unitOfWork.CustomerRepository.Query(), v);

            // 1) Non-lead: theo scope là thấy
            var nonLeadIds = visibleCustomers
                .Where(c => c.IsLead == false)
                .Select(c => c.CustomerId);

            // 2) Lead: CHỈ người đang quản lý (claim trực tiếp) mới thấy
            var leadIdsOnlyMine = visibleCustomers
                .Where(c => c.IsLead == true
                    && c.CustomerClaims.Any(cc =>
                        cc.IsActive == true
                        && cc.EmployeeId == v.EmployeeId
                        && cc.ExpiresAt > now
                    // nếu bạn muốn đúng loại claim quản lý thì mở dòng này
                    // && cc.Type == ClaimType.Work
                    ))
                .Select(c => c.CustomerId);

            var allowedIds = nonLeadIds.Union(leadIdsOnlyMine);

            return q.Where(sr => allowedIds.Contains(sr.CustomerId));
        }

        /// <summary>
        /// Phương thức áp dụng phạm vi xem cho truy vấn MerchandiseOrder
        /// Quy tắc:
        /// - Admin/President/Developer/CustomerViewAll/Lab: thấy tất cả
        /// - Sales thường: chỉ thấy SR thuộc customer mình (hoặc group) quản lý
        /// - Leader: chỉ thấy SR thuộc customer mình (hoặc group) quản lý
        /// </summary>
        /// <param name="q"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public IQueryable<MerchandiseOrder> ApplyMerchandiseOrder(IQueryable<MerchandiseOrder> q, ViewerScope v)
        {
            // Base: active + tenant
            q = q.Where(sr => sr.IsActive == true);

            // Full view
            if (v.ScopeType is ViewerScopeType.AdminFull or ViewerScopeType.LabFull)
                return q;

            // Sales scoped: chỉ thấy SR thuộc customer mình (hoặc group) quản lý
            var visibleCustomers = ApplyCustomer(_unitOfWork.CustomerRepository.Query(), v);

            // Lọc theo CustomerId
            return q.Where(sr => visibleCustomers.Select(c => c.CustomerId).Contains(sr.CustomerId));
        }


    }
}
