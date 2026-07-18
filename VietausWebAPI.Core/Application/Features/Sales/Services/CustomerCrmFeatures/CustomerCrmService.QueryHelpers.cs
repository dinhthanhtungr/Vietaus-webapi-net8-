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
using VietausWebAPI.Core.Application.Features.Shared.DTO.Visibility;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.Shared.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;
using VietausWebAPI.Core.Domain.Enums.Visibilitys;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.CustomerCrmFeatures
{
    /// <summary>
    /// Nhóm helper dựng projection, filter và phân trang cho các query CRM.
    /// </summary>
    public partial class CustomerCrmService
    {
        /// <summary>
        /// Dựng projection đọc lịch sử tương tác sang DTO, chỉ lấy các field cần trả ra API.
        /// </summary>
        private IQueryable<GetCustomerInteractionDto> BuildInteractionDtoQuery()
        {
            return _interactionRepository.Query()
                .Select(x => new GetCustomerInteractionDto
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    CustomerExternalId = x.Customer.ExternalId,
                    CustomerName = x.Customer.CustomerName,
                    ContactId = x.ContactId,
                    ContactName = x.Contact != null ? ((x.Contact.FirstName ?? "") + " " + (x.Contact.LastName ?? "")).Trim() : null,
                    InteractionType = x.InteractionType,
                    Subject = x.Subject,
                    Content = x.Content,
                    Outcome = x.Outcome,
                    NextAction = x.NextAction,
                    InteractionAt = x.InteractionAt,
                    NextFollowUpDate = x.NextFollowUpDate,
                    AssignedSaleEmployeeId = x.AssignedSaleEmployeeId,
                    AssignedSaleEmployeeName = x.AssignedSaleEmployee != null ? x.AssignedSaleEmployee.FullName : null,
                    CompanyId = x.CompanyId,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    CreatedByName = x.CreatedByNavigation.FullName,
                    IsActive = x.IsActive
                });
        }

        /// <summary>
        /// Dựng projection đọc follow-up task sang DTO, bao gồm cờ quá hạn.
        /// </summary>
        private IQueryable<GetCustomerFollowUpTaskDto> BuildTaskDtoQuery()
        {
            var today = DateTime.Today;

            return _followUpTaskRepository.Query()
                .Select(x => new GetCustomerFollowUpTaskDto
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    CustomerExternalId = x.Customer.ExternalId,
                    CustomerName = x.Customer.CustomerName,
                    CustomerInteractionId = x.CustomerInteractionId,
                    Title = x.Title,
                    Description = x.Description,
                    NextAction = x.NextAction,
                    Status = x.Status,
                    Priority = x.Priority,
                    DueDate = x.DueDate,
                    CompletedDate = x.CompletedDate,
                    CompletedBy = x.CompletedBy,
                    CompletionNote = x.CompletionNote,
                    AssignedSaleEmployeeId = x.AssignedSaleEmployeeId,
                    AssignedSaleEmployeeName = x.AssignedSaleEmployee != null ? x.AssignedSaleEmployee.FullName : null,
                    IsOverdue = x.DueDate.HasValue && x.DueDate.Value.Date < today && x.Status != CustomerFollowUpTaskStatus.Done && x.Status != CustomerFollowUpTaskStatus.Canceled,
                    CompanyId = x.CompanyId,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    IsActive = x.IsActive
                });
        }

        /// <summary>
        /// Dựng projection đọc work plan sang DTO.
        /// </summary>
        private IQueryable<GetCustomerWorkPlanDto> BuildWorkPlanDtoQuery()
        {
            return _workPlanRepository.Query()
                .Select(x => new GetCustomerWorkPlanDto
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    CustomerExternalId = x.Customer.ExternalId,
                    CustomerName = x.Customer.CustomerName,
                    PlanName = x.PlanName,
                    Objective = x.Objective,
                    Strategy = x.Strategy,
                    DiscussionSummary = x.DiscussionSummary,
                    NextAction = x.NextAction,
                    Status = x.Status,
                    Priority = x.Priority,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    NextFollowUpDate = x.NextFollowUpDate,
                    AssignedSaleEmployeeId = x.AssignedSaleEmployeeId,
                    AssignedSaleEmployeeName = x.AssignedSaleEmployee != null ? x.AssignedSaleEmployee.FullName : null,
                    CompanyId = x.CompanyId,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    IsActive = x.IsActive
                });
        }

        /// <summary>
        /// Áp dụng bộ lọc cho danh sách lịch sử tương tác.
        /// </summary>
        private static IQueryable<GetCustomerInteractionDto> ApplyInteractionFilter(IQueryable<GetCustomerInteractionDto> q, CustomerInteractionQuery query)
        {
            if (!query.IncludeInactive) q = q.Where(x => x.IsActive);
            if (query.CustomerId.HasValue) q = q.Where(x => x.CustomerId == query.CustomerId.Value);
            if (query.AssignedSaleEmployeeId.HasValue) q = q.Where(x => x.AssignedSaleEmployeeId == query.AssignedSaleEmployeeId.Value);
            if (query.InteractionType.HasValue) q = q.Where(x => x.InteractionType == query.InteractionType.Value);
            if (query.From.HasValue) q = q.Where(x => x.InteractionAt >= query.From.Value);
            if (query.To.HasValue)
            {
                var toExclusive = query.To.Value.Date.AddDays(1);
                q = q.Where(x => x.InteractionAt < toExclusive);
            }
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var kw = query.Keyword.Trim();
                q = q.Where(x => (x.Subject ?? "").Contains(kw) || x.Content.Contains(kw) || (x.Outcome ?? "").Contains(kw));
            }

            return q;
        }

        private static void NormalizeInteractionMonthRange(CustomerInteractionQuery query)
        {
            var anchorDate = query.From ?? query.To;
            if (!anchorDate.HasValue)
                return;

            var monthStart = new DateTime(anchorDate.Value.Year, anchorDate.Value.Month, 1);
            query.From = monthStart;
            query.To = monthStart.AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Áp dụng bộ lọc cho danh sách follow-up task.
        /// </summary>
        private static IQueryable<GetCustomerFollowUpTaskDto> ApplyTaskFilter(IQueryable<GetCustomerFollowUpTaskDto> q, CustomerFollowUpTaskQuery query)
        {
            if (!query.IncludeInactive) q = q.Where(x => x.IsActive);
            if (query.CustomerId.HasValue) q = q.Where(x => x.CustomerId == query.CustomerId.Value);
            if (query.AssignedSaleEmployeeId.HasValue) q = q.Where(x => x.AssignedSaleEmployeeId == query.AssignedSaleEmployeeId.Value);
            if (query.Status.HasValue) q = q.Where(x => x.Status == query.Status.Value);
            if (query.Priority.HasValue) q = q.Where(x => x.Priority == query.Priority.Value);
            if (query.OnlyOverdue == true) q = q.Where(x => x.IsOverdue);
            if (query.DueFrom.HasValue) q = q.Where(x => x.DueDate >= query.DueFrom.Value);
            if (query.DueTo.HasValue) q = q.Where(x => x.DueDate <= query.DueTo.Value);
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var kw = query.Keyword.Trim();
                q = q.Where(x => x.Title.Contains(kw) || (x.Description ?? "").Contains(kw) || (x.NextAction ?? "").Contains(kw));
            }

            return q;
        }

        /// <summary>
        /// Áp dụng bộ lọc cho danh sách work plan.
        /// </summary>
        private static IQueryable<GetCustomerWorkPlanDto> ApplyWorkPlanFilter(IQueryable<GetCustomerWorkPlanDto> q, CustomerWorkPlanQuery query)
        {
            if (!query.IncludeInactive) q = q.Where(x => x.IsActive);
            if (query.CustomerId.HasValue) q = q.Where(x => x.CustomerId == query.CustomerId.Value);
            if (query.AssignedSaleEmployeeId.HasValue) q = q.Where(x => x.AssignedSaleEmployeeId == query.AssignedSaleEmployeeId.Value);
            if (query.Status.HasValue) q = q.Where(x => x.Status == query.Status.Value);
            if (query.Priority.HasValue) q = q.Where(x => x.Priority == query.Priority.Value);
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var kw = query.Keyword.Trim();
                q = q.Where(x => x.PlanName.Contains(kw) || (x.Objective ?? "").Contains(kw) || (x.Strategy ?? "").Contains(kw));
            }

            return q;
        }

        /// <summary>
        /// Đồng bộ ngày follow-up gần nhất còn mở lên hồ sơ khách hàng.
        /// </summary>
        private async Task SyncCustomerNextFollowUpDateAsync(Guid customerId, CancellationToken ct)
        {
            var companyId = _currentUser.CompanyId;

            var customer = await _unitOfWork.CustomerRepository.Query(track: true)
                .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.CompanyId == companyId, ct);

            if (customer == null)
                return;

            customer.NextFollowUpDate = await _followUpTaskRepository.Query()
                .Where(x => x.CompanyId == companyId
                            && x.CustomerId == customerId
                            && x.IsActive
                            && x.Status != CustomerFollowUpTaskStatus.Done
                            && x.Status != CustomerFollowUpTaskStatus.Canceled
                            && x.DueDate.HasValue)
                .MinAsync(x => (DateTime?)x.DueDate, ct);
        }

        private async Task<ViewerScope> BuildCrmViewerScopeAsync(CancellationToken ct)
        {
            return await _visibilityHelper.BuildViewerScopeAsync(ct);
        }

        private IQueryable<Guid> BuildVisibleCustomerIdsQuery(ViewerScope viewer)
        {
            return _visibilityHelper
                .ApplyCustomer(_unitOfWork.CustomerRepository.Query(), viewer)
                .Where(c => c.CompanyId == viewer.CompanyId)
                .Select(c => c.CustomerId);
        }

        private static Guid? ResolveScopedEmployeeId(ViewerScope viewer, Guid? requestedEmployeeId, bool onlyMine)
        {
            if (onlyMine)
                return viewer.EmployeeId;

            if (!requestedEmployeeId.HasValue)
                return null;

            if (viewer.ScopeType is ViewerScopeType.AdminFull or ViewerScopeType.LabFull)
                return requestedEmployeeId.Value;

            return viewer.EmployeeIdsInScope.Contains(requestedEmployeeId.Value)
                ? requestedEmployeeId.Value
                : Guid.Empty;
        }

        /// <summary>
        /// Chuyển IQueryable thành PagedResult theo page number và page size.
        /// </summary>
        private static async Task<PagedResult<T>> ToPagedResultAsync<T>(IQueryable<T> q, int pageNumber, int pageSize, CancellationToken ct)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 15;

            var total = await q.CountAsync(ct);
            var items = await q.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);

            return new PagedResult<T>(items, total, pageNumber, pageSize);
        }
    }
}
