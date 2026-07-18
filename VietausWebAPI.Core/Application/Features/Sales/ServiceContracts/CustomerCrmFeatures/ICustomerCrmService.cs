using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Patchs;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Posts;
using VietausWebAPI.Core.Application.Features.Sales.Querys.CustomerCrmQuerys;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerCrmFeatures
{
    public interface ICustomerCrmService
    {
        Task<OperationResult<Guid>> CreateInteractionAsync(CreateCustomerInteractionRequest request, CancellationToken ct = default);
        Task<OperationResult> UpdateInteractionAsync(Guid id, UpdateCustomerInteractionRequest request, CancellationToken ct = default);
        Task<OperationResult<GetCustomerInteractionDto>> GetInteractionByIdAsync(Guid id, CancellationToken ct = default);
        Task<OperationResult<PagedResult<GetCustomerInteractionDto>>> GetCustomerInteractionsAsync(Guid customerId, CustomerInteractionQuery query, CancellationToken ct = default);
        Task<OperationResult<PagedResult<CustomerCrmActivityHeaderDto>>> GetCustomerActivityHeadersAsync(CustomerCrmActivityHeaderQuery query, CancellationToken ct = default);
        Task<OperationResult<CustomerInteractionAiSummaryDto>> GenerateCustomerInteractionAiSummaryAsync(Guid customerId, GenerateCustomerInteractionAiSummaryRequest request, CancellationToken ct = default);
        Task<OperationResult<CustomerInteractionAiSummaryBatchDto>> GenerateCustomerInteractionAiSummaryBatchAsync(GenerateCustomerInteractionAiSummaryBatchRequest request, CancellationToken ct = default);
        Task<OperationResult<CustomerInteractionAiSummaryDto>> GetLatestCustomerInteractionAiSummaryAsync(Guid customerId, CancellationToken ct = default);
        Task<OperationResult<AiRateLimitInfoDto>> GetCustomerInteractionAiSummaryQuotaAsync(CancellationToken ct = default);

        Task<OperationResult<Guid>> CreateFollowUpTaskAsync(CreateCustomerFollowUpTaskRequest request, CancellationToken ct = default);
        Task<OperationResult> UpdateFollowUpTaskAsync(Guid id, UpdateCustomerFollowUpTaskRequest request, CancellationToken ct = default);
        Task<OperationResult> CompleteFollowUpTaskAsync(Guid id, CompleteCustomerFollowUpTaskRequest request, CancellationToken ct = default);
        Task<OperationResult<PagedResult<GetCustomerFollowUpTaskDto>>> GetMyFollowUpTasksAsync(CustomerFollowUpTaskQuery query, CancellationToken ct = default);
        Task<OperationResult<PagedResult<GetCustomerFollowUpTaskDto>>> GetCustomerFollowUpTasksAsync(Guid customerId, CustomerFollowUpTaskQuery query, CancellationToken ct = default);
        Task<OperationResult<CustomerFollowUpTaskAssigneeDto>> AddFollowUpTaskAssigneeAsync(Guid taskId, AddCustomerFollowUpTaskAssigneeRequest request, CancellationToken ct = default);
        Task<OperationResult<IReadOnlyList<CustomerFollowUpTaskAssigneeDto>>> AddFollowUpTaskGroupAssigneesAsync(Guid taskId, AddCustomerFollowUpTaskGroupAssigneesRequest request, CancellationToken ct = default);
        Task<OperationResult> RemoveFollowUpTaskAssigneeAsync(Guid taskId, Guid employeeId, CancellationToken ct = default);
        Task<OperationResult<IReadOnlyList<CustomerFollowUpTaskAssigneeDto>>> GetFollowUpTaskAssigneesAsync(Guid taskId, CancellationToken ct = default);

        Task<OperationResult<Guid>> CreateWorkPlanAsync(CreateCustomerWorkPlanRequest request, CancellationToken ct = default);
        Task<OperationResult> UpdateWorkPlanAsync(Guid id, UpdateCustomerWorkPlanRequest request, CancellationToken ct = default);
        Task<OperationResult<PagedResult<GetCustomerWorkPlanDto>>> GetCustomerWorkPlansAsync(Guid customerId, CustomerWorkPlanQuery query, CancellationToken ct = default);
        Task<OperationResult<IReadOnlyList<CustomerCrmCalendarEventDto>>> GetCalendarEventsAsync(CustomerCrmCalendarQuery query, CancellationToken ct = default);


        Task<OperationResult<CustomerActivityCalendarReportDto>> GetCustomerActivityCalendarReportAsync(CustomerActivityCalendarReportQuery query, CancellationToken ct = default);
        Task<byte[]> ExportCustomerActivityCalendarReportExcelAsync(CustomerActivityCalendarReportQuery query, CancellationToken ct = default);

        Task<OperationResult<SaleCrmDashboardDto>> GetSaleDashboardAsync(CancellationToken ct = default);
        Task<OperationResult<LeaderCrmDashboardDto>> GetLeaderDashboardAsync(CancellationToken ct = default);
        Task<OperationResult<DirectorCrmDashboardDto>> GetDirectorDashboardAsync(CancellationToken ct = default);
    }
}
