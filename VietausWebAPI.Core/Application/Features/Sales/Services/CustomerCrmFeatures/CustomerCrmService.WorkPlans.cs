using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Patchs;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Posts;
using VietausWebAPI.Core.Application.Features.Sales.Querys.CustomerCrmQuerys;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerCrmFeatures;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerCrmFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.CustomerCrmFeatures
{
    /// <summary>
    /// Feature quản lý kế hoạch chăm sóc dài hạn theo từng khách hàng.
    /// </summary>
    public partial class CustomerCrmService
    {
        /// <summary>
        /// Tạo kế hoạch làm việc/chăm sóc khách hàng cho sale phụ trách.
        /// </summary>
        public async Task<OperationResult<Guid>> CreateWorkPlanAsync(CreateCustomerWorkPlanRequest request, CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var employeeId = _currentUser.EmployeeId;
            var companyId = _currentUser.CompanyId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var exists = await _unitOfWork.CustomerRepository.Query()
                .AnyAsync(x => x.CustomerId == request.CustomerId
                               && x.CompanyId == companyId
                               && visibleCustomerIds.Contains(x.CustomerId), ct);

            if (!exists)
                return OperationResult<Guid>.Fail("Không tìm thấy khách hàng.");

            var scopedAssignedSaleId = ResolveScopedEmployeeId(viewer, request.AssignedSaleEmployeeId, onlyMine: false);
            if (scopedAssignedSaleId == Guid.Empty)
                return OperationResult<Guid>.Fail("Bạn không có quyền giao kế hoạch cho sale ngoài phạm vi xem.");

            var plan = new CustomerWorkPlan
            {
                Id = Guid.CreateVersion7(),
                CustomerId = request.CustomerId,
                PlanName = request.PlanName,
                Objective = request.Objective,
                Strategy = request.Strategy,
                DiscussionSummary = request.DiscussionSummary,
                NextAction = request.NextAction,
                Status = request.Status,
                Priority = request.Priority,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                NextFollowUpDate = request.NextFollowUpDate,
                AssignedSaleEmployeeId = scopedAssignedSaleId ?? employeeId,
                CompanyId = companyId,
                CreatedDate = now,
                CreatedBy = employeeId,
                IsActive = true
            };

            await _workPlanRepository.AddAsync(plan, ct);
            await _unitOfWork.SaveChangesAsync();

            return OperationResult<Guid>.Ok(plan.Id);
        }

        /// <summary>
        /// Cập nhật kế hoạch chăm sóc khách hàng như mục tiêu, chiến lược, hành động tiếp theo, trạng thái và độ ưu tiên.
        /// </summary>
        public async Task<OperationResult> UpdateWorkPlanAsync(Guid id, UpdateCustomerWorkPlanRequest request, CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var companyId = _currentUser.CompanyId;
            var employeeId = _currentUser.EmployeeId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var plan = await _workPlanRepository.Query(track: true)
                .FirstOrDefaultAsync(x => x.Id == id
                                          && x.CompanyId == companyId
                                          && visibleCustomerIds.Contains(x.CustomerId), ct);

            if (plan == null)
                return OperationResult.Fail("Không tìm thấy work plan.");

            var scopedAssignedSaleId = ResolveScopedEmployeeId(viewer, request.AssignedSaleEmployeeId, onlyMine: false);
            if (scopedAssignedSaleId == Guid.Empty)
                return OperationResult.Fail("Bạn không có quyền giao kế hoạch cho sale ngoài phạm vi xem.");

            if (request.PlanName != null) plan.PlanName = request.PlanName;
            if (request.Objective != null) plan.Objective = request.Objective;
            if (request.Strategy != null) plan.Strategy = request.Strategy;
            if (request.DiscussionSummary != null) plan.DiscussionSummary = request.DiscussionSummary;
            if (request.NextAction != null) plan.NextAction = request.NextAction;
            if (request.Status.HasValue) plan.Status = request.Status.Value;
            if (request.Priority.HasValue) plan.Priority = request.Priority.Value;
            if (request.StartDate.HasValue) plan.StartDate = request.StartDate;
            if (request.EndDate.HasValue) plan.EndDate = request.EndDate;
            if (request.NextFollowUpDate.HasValue) plan.NextFollowUpDate = request.NextFollowUpDate;
            if (scopedAssignedSaleId.HasValue) plan.AssignedSaleEmployeeId = scopedAssignedSaleId;
            if (request.IsActive.HasValue) plan.IsActive = request.IsActive.Value;

            plan.UpdatedDate = now;
            plan.UpdatedBy = employeeId;

            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Ok("Cập nhật work plan thành công.");
        }

        /// <summary>
        /// Lấy danh sách kế hoạch chăm sóc của một khách hàng, có phân trang và bộ lọc.
        /// </summary>
        public async Task<OperationResult<PagedResult<GetCustomerWorkPlanDto>>> GetCustomerWorkPlansAsync(Guid customerId, CustomerWorkPlanQuery query, CancellationToken ct = default)
        {
            query ??= new CustomerWorkPlanQuery();
            query.CustomerId = customerId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var q = ApplyWorkPlanFilter(BuildWorkPlanDtoQuery(), query)
                .Where(x => x.CompanyId == _currentUser.CompanyId)
                .Where(x => visibleCustomerIds.Contains(x.CustomerId))
                .OrderBy(x => x.NextFollowUpDate ?? DateTime.MaxValue)
                .ThenByDescending(x => x.CreatedDate);

            var result = await ToPagedResultAsync(q, query.PageNumber, query.PageSize, ct);
            return OperationResult<PagedResult<GetCustomerWorkPlanDto>>.Ok(result);
        }
    }
}
