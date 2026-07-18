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
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.CustomerCrmFeatures
{
    /// <summary>
    /// Feature ghi nhận và tra cứu lịch sử tương tác với khách hàng.
    /// </summary>
    public partial class CustomerCrmService
    {
        /// <summary>
        /// Tạo một lần ghi nhận liên hệ với khách hàng và tự động tạo follow-up task nếu có ngày hẹn tiếp theo.
        /// </summary>
        public async Task<OperationResult<Guid>> CreateInteractionAsync(CreateCustomerInteractionRequest request, CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var employeeId = _currentUser.EmployeeId;
            var companyId = _currentUser.CompanyId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var customer = await _unitOfWork.CustomerRepository.Query(track: true)
                .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerId
                                          && x.CompanyId == companyId
                                          && visibleCustomerIds.Contains(x.CustomerId), ct);

            if (customer == null)
                return OperationResult<Guid>.Fail("Không tìm thấy khách hàng.");

            var scopedAssignedSaleId = ResolveScopedEmployeeId(viewer, request.AssignedSaleEmployeeId, onlyMine: false);
            if (scopedAssignedSaleId == Guid.Empty)
                return OperationResult<Guid>.Fail("Bạn không có quyền giao hoạt động CRM cho sale ngoài phạm vi xem.");

            var assignedSaleId = scopedAssignedSaleId ?? employeeId;
            var interactionAt = request.InteractionAt ?? now;

            var interaction = new CustomerInteraction
            {
                Id = Guid.CreateVersion7(),
                CustomerId = request.CustomerId,
                ContactId = request.ContactId,
                InteractionType = request.InteractionType,
                Subject = request.Subject,
                Content = request.Content,
                Outcome = request.Outcome,
                NextAction = request.NextAction,
                InteractionAt = interactionAt,
                NextFollowUpDate = request.NextFollowUpDate,
                AssignedSaleEmployeeId = assignedSaleId,
                CompanyId = companyId,
                CreatedDate = now,
                CreatedBy = employeeId,
                IsActive = true
            };

            await _interactionRepository.AddAsync(interaction, ct);

            customer.LastContactDate = interactionAt;
            customer.CurrentSaleId = assignedSaleId;
            customer.UpdatedDate = now;
            customer.UpdatedBy = employeeId;

            if (request.NextFollowUpDate.HasValue)
            {
                var task = new CustomerFollowUpTask
                {
                    Id = Guid.CreateVersion7(),
                    CustomerId = request.CustomerId,
                    CustomerInteractionId = interaction.Id,
                    Title = !string.IsNullOrWhiteSpace(request.NextAction)
                        ? request.NextAction!
                        : !string.IsNullOrWhiteSpace(request.Subject)
                            ? request.Subject!
                            : "Theo dõi khách hàng",
                    Description = request.Outcome,
                    NextAction = request.NextAction,
                    Status = CustomerFollowUpTaskStatus.Pending,
                    Priority = request.Priority,
                    DueDate = request.NextFollowUpDate,
                    AssignedSaleEmployeeId = assignedSaleId,
                    CompanyId = companyId,
                    CreatedDate = now,
                    CreatedBy = employeeId,
                    IsActive = true
                };

                await _followUpTaskRepository.AddAsync(task, ct);
                customer.NextFollowUpDate = request.NextFollowUpDate;
            }

            await _unitOfWork.SaveChangesAsync();
            return OperationResult<Guid>.Ok(interaction.Id);
        }

        /// <summary>
        /// Cập nhật nội dung của một lần tương tác khách hàng.
        /// </summary>
        public async Task<OperationResult> UpdateInteractionAsync(Guid id, UpdateCustomerInteractionRequest request, CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var companyId = _currentUser.CompanyId;
            var employeeId = _currentUser.EmployeeId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var interaction = await _interactionRepository.Query(track: true)
                .FirstOrDefaultAsync(x => x.Id == id
                                          && x.CompanyId == companyId
                                          && visibleCustomerIds.Contains(x.CustomerId), ct);

            if (interaction == null)
                return OperationResult.Fail("Không tìm thấy lịch sử liên hệ.");

            var scopedAssignedSaleId = ResolveScopedEmployeeId(viewer, request.AssignedSaleEmployeeId, onlyMine: false);
            if (scopedAssignedSaleId == Guid.Empty)
                return OperationResult.Fail("Bạn không có quyền giao hoạt động CRM cho sale ngoài phạm vi xem.");

            if (request.ContactId.HasValue) interaction.ContactId = request.ContactId;
            if (request.InteractionType.HasValue) interaction.InteractionType = request.InteractionType.Value;
            if (request.Subject != null) interaction.Subject = request.Subject;
            if (request.Content != null) interaction.Content = request.Content;
            if (request.Outcome != null) interaction.Outcome = request.Outcome;
            if (request.NextAction != null) interaction.NextAction = request.NextAction;
            if (request.InteractionAt.HasValue) interaction.InteractionAt = request.InteractionAt.Value;
            if (request.NextFollowUpDate.HasValue) interaction.NextFollowUpDate = request.NextFollowUpDate;
            if (scopedAssignedSaleId.HasValue) interaction.AssignedSaleEmployeeId = scopedAssignedSaleId;
            if (request.IsActive.HasValue) interaction.IsActive = request.IsActive.Value;

            interaction.UpdatedDate = now;
            interaction.UpdatedBy = employeeId;

            var linkedTask = await _followUpTaskRepository.Query(track: true)
                .FirstOrDefaultAsync(x =>
                    x.CustomerInteractionId == interaction.Id
                    && x.CompanyId == companyId
                    && x.IsActive
                    && x.Status != CustomerFollowUpTaskStatus.Done
                    && x.Status != CustomerFollowUpTaskStatus.Canceled,
                    ct);

            if (linkedTask != null)
            {
                if (request.NextAction != null)
                {
                    PatchHelper.SetIfRefNullable(request.NextAction, () => linkedTask.NextAction, v => linkedTask.NextAction = v);

                    if (!string.IsNullOrWhiteSpace(request.NextAction))
                    {
                        PatchHelper.SetIfRefNullable(request.NextAction, () => linkedTask.Title, v => linkedTask.Title = v);
                    }
                }

                if (request.Outcome != null)
                {
                    PatchHelper.SetIfRefNullable(request.Outcome, () => linkedTask.Description, v => linkedTask.Description = v);
                }

                if (request.NextFollowUpDate.HasValue)
                {
                    PatchHelper.SetIfNullable(request.NextFollowUpDate, () => linkedTask.DueDate, v => linkedTask.DueDate = v); 
                }

                if (request.AssignedSaleEmployeeId.HasValue)
                {
                    PatchHelper.SetIfGuidNullable(scopedAssignedSaleId, () => linkedTask.AssignedSaleEmployeeId, v => linkedTask.AssignedSaleEmployeeId = v);
                }

                linkedTask.UpdatedDate = now;
                linkedTask.UpdatedBy = employeeId;
            }

            if (linkedTask == null && interaction.NextFollowUpDate.HasValue)
            {
                var task = new CustomerFollowUpTask
                {
                    Id = Guid.CreateVersion7(),
                    CustomerId = interaction.CustomerId,
                    CustomerInteractionId = interaction.Id,
                    Title = !string.IsNullOrWhiteSpace(interaction.NextAction)
                        ? interaction.NextAction!
                        : !string.IsNullOrWhiteSpace(interaction.Subject)
                            ? interaction.Subject!
                            : "Theo dõi khách hàng",
                    Description = interaction.Outcome,
                    NextAction = interaction.NextAction,
                    Status = CustomerFollowUpTaskStatus.Pending,
                    Priority = CustomerFollowUpPriority.Normal,
                    DueDate = interaction.NextFollowUpDate,
                    AssignedSaleEmployeeId = interaction.AssignedSaleEmployeeId ?? employeeId,
                    CompanyId = companyId,
                    CreatedDate = now,
                    CreatedBy = employeeId,
                    IsActive = true
                };

                await _followUpTaskRepository.AddAsync(task, ct);
            }

            await SyncCustomerNextFollowUpDateAsync(interaction.CustomerId, ct);

            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Ok("Cập nhật lịch sử liên hệ thành công.");
        }

        /// <summary>
        /// Lấy chi tiết một lần tương tác khách hàng theo Id.
        /// </summary>
        public async Task<OperationResult<GetCustomerInteractionDto>> GetInteractionByIdAsync(Guid id, CancellationToken ct = default)
        {
            var companyId = _currentUser.CompanyId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var dto = await BuildInteractionDtoQuery()
                .FirstOrDefaultAsync(x => x.Id == id
                                          && x.CompanyId == companyId
                                          && visibleCustomerIds.Contains(x.CustomerId), ct);

            return dto == null
                ? OperationResult<GetCustomerInteractionDto>.Fail("Không tìm thấy lịch sử liên hệ.")
                : OperationResult<GetCustomerInteractionDto>.Ok(dto);
        }

        /// <summary>
        /// Lấy lịch sử tương tác của một khách hàng, có phân trang và bộ lọc theo loại/ngày/sale/phím tìm kiếm.
        /// </summary>
        public async Task<OperationResult<PagedResult<GetCustomerInteractionDto>>> GetCustomerInteractionsAsync(Guid customerId, CustomerInteractionQuery query, CancellationToken ct = default)
        {
            query ??= new CustomerInteractionQuery();
            query.CustomerId = customerId;
            NormalizeInteractionMonthRange(query);

            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var q = ApplyInteractionFilter(BuildInteractionDtoQuery(), query)
                .Where(x => x.CompanyId == _currentUser.CompanyId)
                .Where(x => visibleCustomerIds.Contains(x.CustomerId))
                .OrderByDescending(x => x.InteractionAt);

            var result = await ToPagedResultAsync(q, query.PageNumber, query.PageSize, ct);
            return OperationResult<PagedResult<GetCustomerInteractionDto>>.Ok(result);
        }
    }
}
