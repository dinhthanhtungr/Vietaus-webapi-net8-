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
    /// Feature quản lý việc cần làm và lịch follow-up của sale với khách hàng.
    /// </summary>
    public partial class CustomerCrmService
    {
        /// <summary>
        /// Tạo thủ công một follow-up task cho khách hàng và đồng bộ ngày follow-up gần nhất lên hồ sơ khách hàng.
        /// </summary>
        public async Task<OperationResult<Guid>> CreateFollowUpTaskAsync(CreateCustomerFollowUpTaskRequest request, CancellationToken ct = default)
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
                return OperationResult<Guid>.Fail("Bạn không có quyền giao task cho sale ngoài phạm vi xem.");

            var task = new CustomerFollowUpTask
            {
                Id = Guid.CreateVersion7(),
                CustomerId = request.CustomerId,
                CustomerInteractionId = request.CustomerInteractionId,
                Title = request.Title,
                Description = request.Description,
                NextAction = request.NextAction,
                Status = request.Status,
                Priority = request.Priority,
                DueDate = request.DueDate,
                AssignedSaleEmployeeId = scopedAssignedSaleId ?? employeeId,
                CompanyId = companyId,
                CreatedDate = now,
                CreatedBy = employeeId,
                IsActive = true
            };

            await _followUpTaskRepository.AddAsync(task, ct);
            await SyncCustomerNextFollowUpDateAsync(request.CustomerId, ct);
            await _unitOfWork.SaveChangesAsync();

            return OperationResult<Guid>.Ok(task.Id);
        }

        /// <summary>
        /// Cập nhật thông tin follow-up task như tiêu đề, hạn xử lý, độ ưu tiên, trạng thái hoặc người phụ trách.
        /// </summary>
        public async Task<OperationResult> UpdateFollowUpTaskAsync(Guid id, UpdateCustomerFollowUpTaskRequest request, CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var companyId = _currentUser.CompanyId;
            var employeeId = _currentUser.EmployeeId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var task = await _followUpTaskRepository.Query(track: true)
                .FirstOrDefaultAsync(x => x.Id == id
                                          && x.CompanyId == companyId
                                          && visibleCustomerIds.Contains(x.CustomerId), ct);

            if (task == null)
                return OperationResult.Fail("Không tìm thấy follow-up task.");

            var scopedAssignedSaleId = ResolveScopedEmployeeId(viewer, request.AssignedSaleEmployeeId, onlyMine: false);
            if (scopedAssignedSaleId == Guid.Empty)
                return OperationResult.Fail("Bạn không có quyền giao task cho sale ngoài phạm vi xem.");

            if (request.Title != null) task.Title = request.Title;
            if (request.Description != null) task.Description = request.Description;
            if (request.NextAction != null) task.NextAction = request.NextAction;
            if (request.Status.HasValue) task.Status = request.Status.Value;
            if (request.Priority.HasValue) task.Priority = request.Priority.Value;
            if (request.DueDate.HasValue && task.DueDate != request.DueDate)
            {
                task.DueDate = request.DueDate;
                task.DueReminderSentAt = null;
            }
            if (scopedAssignedSaleId.HasValue) task.AssignedSaleEmployeeId = scopedAssignedSaleId;
            if (request.IsActive.HasValue) task.IsActive = request.IsActive.Value;

            task.UpdatedDate = now;
            task.UpdatedBy = employeeId;

            await SyncCustomerNextFollowUpDateAsync(task.CustomerId, ct);
            await _unitOfWork.SaveChangesAsync();

            return OperationResult.Ok("Cập nhật follow-up task thành công.");
        }

        /// <summary>
        /// Hoàn tất một follow-up task, ghi nhận ngày hoàn tất, người hoàn tất và ghi chú kết quả.
        /// </summary>
        public async Task<OperationResult> CompleteFollowUpTaskAsync(Guid id, CompleteCustomerFollowUpTaskRequest request, CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var companyId = _currentUser.CompanyId;
            var employeeId = _currentUser.EmployeeId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var task = await _followUpTaskRepository.Query(track: true)
                .FirstOrDefaultAsync(x => x.Id == id
                                          && x.CompanyId == companyId
                                          && x.IsActive
                                          && visibleCustomerIds.Contains(x.CustomerId), ct);

            if (task == null)
                return OperationResult.Fail("Không tìm thấy follow-up task.");

            task.Status = request.status;
            task.CompletedDate = now;
            task.CompletedBy = employeeId;
            task.CompletionNote = request.CompletionNote;
            task.UpdatedDate = now;
            task.UpdatedBy = employeeId;

            await SyncCustomerNextFollowUpDateAsync(task.CustomerId, ct);
            await _unitOfWork.SaveChangesAsync();

            return OperationResult.Ok("Hoàn tất follow-up task thành công.");
        }

        /// <summary>
        /// Lấy danh sách follow-up task của sale hiện tại, ưu tiên task quá hạn và task sắp đến hạn.
        /// </summary>
        public async Task<OperationResult<PagedResult<GetCustomerFollowUpTaskDto>>> GetMyFollowUpTasksAsync(CustomerFollowUpTaskQuery query, CancellationToken ct = default)
        {
            query ??= new CustomerFollowUpTaskQuery();
            query.AssignedSaleEmployeeId = _currentUser.EmployeeId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var q = ApplyTaskFilter(BuildTaskDtoQuery(), query)
                .Where(x => x.CompanyId == _currentUser.CompanyId)
                .Where(x => visibleCustomerIds.Contains(x.CustomerId))
                .OrderByDescending(x => x.IsOverdue)
                .ThenBy(x => x.DueDate ?? DateTime.MaxValue)
                .ThenByDescending(x => x.CreatedDate);

            var result = await ToPagedResultAsync(q, query.PageNumber, query.PageSize, ct);
            return OperationResult<PagedResult<GetCustomerFollowUpTaskDto>>.Ok(result);
        }

        /// <summary>
        /// Lấy danh sách follow-up task của một khách hàng, có phân trang và bộ lọc.
        /// </summary>
        public async Task<OperationResult<PagedResult<GetCustomerFollowUpTaskDto>>> GetCustomerFollowUpTasksAsync(Guid customerId, CustomerFollowUpTaskQuery query, CancellationToken ct = default)
        {
            query ??= new CustomerFollowUpTaskQuery();
            query.CustomerId = customerId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var q = ApplyTaskFilter(BuildTaskDtoQuery(), query)
                .Where(x => x.CompanyId == _currentUser.CompanyId)
                .Where(x => visibleCustomerIds.Contains(x.CustomerId))
                .OrderByDescending(x => x.IsOverdue)
                .ThenBy(x => x.DueDate ?? DateTime.MaxValue);

            var result = await ToPagedResultAsync(q, query.PageNumber, query.PageSize, ct);
            return OperationResult<PagedResult<GetCustomerFollowUpTaskDto>>.Ok(result);
        }
    }
}
