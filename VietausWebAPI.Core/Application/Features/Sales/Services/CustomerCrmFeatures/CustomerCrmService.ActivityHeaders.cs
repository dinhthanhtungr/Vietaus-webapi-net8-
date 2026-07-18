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
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.CustomerCrmFeatures
{
    /// <summary>
    /// Feature gom dữ liệu CRM thành header/card theo khách hàng để FE mở detail bằng infinite scroll.
    /// </summary>
    public partial class CustomerCrmService
    {
        /// <summary>
        /// Lấy danh sách thẻ khách hàng theo loại dữ liệu CRM: follow-up task hoặc lịch sử liên hệ.
        /// </summary>
        public async Task<OperationResult<PagedResult<CustomerCrmActivityHeaderDto>>> GetCustomerActivityHeadersAsync(CustomerCrmActivityHeaderQuery query, CancellationToken ct = default)
        {
            query ??= new CustomerCrmActivityHeaderQuery();
            if (query.PageNumber <= 0) query.PageNumber = 1;
            if (query.PageSize <= 0) query.PageSize = 15;
            if (query.PageSize > 50) query.PageSize = 50;

            var activityTypes = ResolveActivityTypes(query.ActivityTypes);
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);
            var headers = new List<CustomerCrmActivityHeaderDto>();

            if (activityTypes.Contains(EventTypeColorKey.FollowUpTask))
                headers.AddRange(await GetTaskActivityHeadersAsync(query, viewer, visibleCustomerIds, ct));

            if (activityTypes.Contains(EventTypeColorKey.Interaction))
                headers.AddRange(await GetInteractionActivityHeadersAsync(query, viewer, visibleCustomerIds, ct));

            var orderedHeaders = MergeHeadersByCustomer(headers)
                .OrderByDescending(x => x.OverdueTaskCount)
                .ThenByDescending(x => x.OpenTaskCount)
                .ThenByDescending(x => x.InteractionCount)
                .ThenByDescending(x => x.LastActivityDate)
                .ToList();

            var items = orderedHeaders
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            var result = new PagedResult<CustomerCrmActivityHeaderDto>(
                items,
                orderedHeaders.Count,
                query.PageNumber,
                query.PageSize);

            return OperationResult<PagedResult<CustomerCrmActivityHeaderDto>>.Ok(result);
        }

        private async Task<List<CustomerCrmActivityHeaderDto>> GetTaskActivityHeadersAsync(
            CustomerCrmActivityHeaderQuery query,
            ViewerScope viewer,
            IQueryable<Guid> visibleCustomerIds,
            CancellationToken ct)
        {
            var today = DateTime.Today;
            var companyId = _currentUser.CompanyId;
            Guid? employeeId = ResolveScopedEmployeeId(viewer, query.AssignedSaleEmployeeId, query.OnlyMine);

            var taskQ = _followUpTaskRepository.Query()
                .Where(x => x.CompanyId == companyId
                            && x.IsActive
                            && visibleCustomerIds.Contains(x.CustomerId));

            if (employeeId.HasValue)
                taskQ = taskQ.Where(x => x.AssignedSaleEmployeeId == employeeId.Value || x.CreatedBy == employeeId.Value);

            if (!query.IncludeCompleted)
                taskQ = taskQ.Where(x => x.Status != CustomerFollowUpTaskStatus.Done);

            if (!query.IncludeCanceled)
                taskQ = taskQ.Where(x => x.Status != CustomerFollowUpTaskStatus.Canceled);

            if (query.CustomerId.HasValue)
                taskQ = taskQ.Where(x => x.CustomerId == query.CustomerId.Value);

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var keyword = query.Keyword.Trim();
                taskQ = taskQ.Where(x =>
                    x.Customer.ExternalId.Contains(keyword)
                    || x.Customer.CustomerName.Contains(keyword)
                    || x.Title.Contains(keyword)
                    || (x.Description ?? "").Contains(keyword)
                    || (x.NextAction ?? "").Contains(keyword));
            }

            var groupedQ = taskQ
                .GroupBy(x => new
                {
                    x.CustomerId,
                    x.Customer.ExternalId,
                    x.Customer.CustomerName
                })
                .Select(g => new CustomerCrmActivityHeaderDto
                {
                    ActivityTypes = new List<string> { EventTypeColorKey.FollowUpTask.ToString() },
                    CustomerId = g.Key.CustomerId,
                    CustomerExternalId = g.Key.ExternalId,
                    CustomerName = g.Key.CustomerName,
                    TotalCount = g.Count(),
                    OpenTaskCount = g.Count(x => x.Status != CustomerFollowUpTaskStatus.Done && x.Status != CustomerFollowUpTaskStatus.Canceled),
                    CompletedTaskCount = g.Count(x => x.Status == CustomerFollowUpTaskStatus.Done),
                    CanceledTaskCount = g.Count(x => x.Status == CustomerFollowUpTaskStatus.Canceled),
                    OverdueTaskCount = g.Count(x =>
                        x.DueDate.HasValue
                        && x.DueDate.Value.Date < today
                        && x.Status != CustomerFollowUpTaskStatus.Done
                        && x.Status != CustomerFollowUpTaskStatus.Canceled),
                    InteractionCount = 0,
                    LastActivityDate = g.Max(x => x.DueDate ?? x.CreatedDate)
                });

            return await groupedQ
                .OrderByDescending(x => x.OverdueTaskCount)
                .ThenByDescending(x => x.OpenTaskCount)
                .ThenByDescending(x => x.LastActivityDate)
                .ToListAsync(ct);
        }

        private async Task<List<CustomerCrmActivityHeaderDto>> GetInteractionActivityHeadersAsync(
            CustomerCrmActivityHeaderQuery query,
            ViewerScope viewer,
            IQueryable<Guid> visibleCustomerIds,
            CancellationToken ct)
        {
            var companyId = _currentUser.CompanyId;
            Guid? employeeId = ResolveScopedEmployeeId(viewer, query.AssignedSaleEmployeeId, query.OnlyMine);

            var interactionQ = _interactionRepository.Query()
                .Where(x => x.CompanyId == companyId
                            && x.IsActive
                            && visibleCustomerIds.Contains(x.CustomerId));

            if (employeeId.HasValue)
                interactionQ = interactionQ.Where(x => x.AssignedSaleEmployeeId == employeeId.Value || x.CreatedBy == employeeId.Value);

            if (query.CustomerId.HasValue)
                interactionQ = interactionQ.Where(x => x.CustomerId == query.CustomerId.Value);

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var keyword = query.Keyword.Trim();
                interactionQ = interactionQ.Where(x =>
                    x.Customer.ExternalId.Contains(keyword)
                    || x.Customer.CustomerName.Contains(keyword)
                    || (x.Subject ?? "").Contains(keyword)
                    || x.Content.Contains(keyword)
                    || (x.Outcome ?? "").Contains(keyword)
                    || (x.NextAction ?? "").Contains(keyword));
            }

            var groupedQ = interactionQ
                .GroupBy(x => new
                {
                    x.CustomerId,
                    x.Customer.ExternalId,
                    x.Customer.CustomerName
                })
                .Select(g => new CustomerCrmActivityHeaderDto
                {
                    ActivityTypes = new List<string> { EventTypeColorKey.Interaction.ToString() },
                    CustomerId = g.Key.CustomerId,
                    CustomerExternalId = g.Key.ExternalId,
                    CustomerName = g.Key.CustomerName,
                    TotalCount = g.Count(),
                    OpenTaskCount = 0,
                    CompletedTaskCount = 0,
                    CanceledTaskCount = 0,
                    OverdueTaskCount = 0,
                    InteractionCount = g.Count(),
                    LastActivityDate = g.Max(x => x.InteractionAt)
                });

            return await groupedQ
                .OrderByDescending(x => x.LastActivityDate)
                .ToListAsync(ct);
        }

        private static List<CustomerCrmActivityHeaderDto> MergeHeadersByCustomer(IEnumerable<CustomerCrmActivityHeaderDto> headers)
        {
            return headers
                .GroupBy(x => x.CustomerId)
                .Select(g =>
                {
                    var first = g
                        .OrderByDescending(x => x.LastActivityDate)
                        .First();

                    return new CustomerCrmActivityHeaderDto
                    {
                        ActivityTypes = g
                            .SelectMany(x => x.ActivityTypes)
                            .Where(x => !string.IsNullOrWhiteSpace(x))
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .ToList(),
                        CustomerId = g.Key,
                        CustomerExternalId = first.CustomerExternalId,
                        CustomerName = first.CustomerName,
                        TotalCount = g.Sum(x => x.TotalCount),
                        OpenTaskCount = g.Sum(x => x.OpenTaskCount),
                        CompletedTaskCount = g.Sum(x => x.CompletedTaskCount),
                        CanceledTaskCount = g.Sum(x => x.CanceledTaskCount),
                        OverdueTaskCount = g.Sum(x => x.OverdueTaskCount),
                        InteractionCount = g.Sum(x => x.InteractionCount),
                        LastActivityDate = g.Max(x => x.LastActivityDate)
                    };
                })
                .ToList();
        }

        private static HashSet<EventTypeColorKey> ResolveActivityTypes(IEnumerable<string>? activityTypes)
        {
            var result = new HashSet<EventTypeColorKey>();

            if (activityTypes != null)
            {
                foreach (var activityType in activityTypes)
                {
                    if (Enum.TryParse<EventTypeColorKey>(activityType, true, out var parsed))
                        result.Add(parsed);
                }
            }

            if (result.Count == 0)
                result.Add(EventTypeColorKey.FollowUpTask);

            return result;
        }
    }
}
