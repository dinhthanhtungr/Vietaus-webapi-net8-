using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.DTOs;
using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.Queries;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.WorkTaskFeatures.ServiceContracts;

public interface IWorkManagementService
{
    Task<OperationResult<Guid>> CreateTaskAsync(CreateWorkTaskRequest request, CancellationToken ct = default);
    Task<OperationResult> UpdateTaskAsync(Guid id, UpdateWorkTaskRequest request, CancellationToken ct = default);
    Task<OperationResult> CompleteTaskAsync(Guid id, CompleteWorkTaskRequest request, CancellationToken ct = default);
    Task<OperationResult<WorkTaskDto>> GetTaskAsync(Guid id, CancellationToken ct = default);
    Task<OperationResult<PagedResult<WorkTaskDto>>> GetTasksAsync(WorkTaskQuery query, CancellationToken ct = default);
    Task<OperationResult<WorkAssigneeDto>> AddTaskAssigneeAsync(Guid taskId, AddWorkAssigneeRequest request, CancellationToken ct = default);
    Task<OperationResult<IReadOnlyList<WorkAssigneeDto>>> AddTaskGroupAssigneesAsync(Guid taskId, AddWorkGroupAssigneesRequest request, CancellationToken ct = default);
    Task<OperationResult> RemoveTaskAssigneeAsync(Guid taskId, Guid employeeId, CancellationToken ct = default);
    Task<OperationResult<IReadOnlyList<WorkAssigneeDto>>> GetTaskAssigneesAsync(Guid taskId, CancellationToken ct = default);

    Task<OperationResult<Guid>> CreatePlanAsync(CreateWorkPlanRequest request, CancellationToken ct = default);
    Task<OperationResult> UpdatePlanAsync(Guid id, UpdateWorkPlanRequest request, CancellationToken ct = default);
    Task<OperationResult<WorkPlanDto>> GetPlanAsync(Guid id, CancellationToken ct = default);
    Task<OperationResult<PagedResult<WorkPlanDto>>> GetPlansAsync(WorkPlanQuery query, CancellationToken ct = default);
    Task<OperationResult<WorkAssigneeDto>> AddPlanAssigneeAsync(Guid planId, AddWorkAssigneeRequest request, CancellationToken ct = default);
    Task<OperationResult<IReadOnlyList<WorkAssigneeDto>>> AddPlanGroupAssigneesAsync(Guid planId, AddWorkGroupAssigneesRequest request, CancellationToken ct = default);
    Task<OperationResult> RemovePlanAssigneeAsync(Guid planId, Guid employeeId, CancellationToken ct = default);
    Task<OperationResult<IReadOnlyList<WorkAssigneeDto>>> GetPlanAssigneesAsync(Guid planId, CancellationToken ct = default);

    Task<OperationResult<IReadOnlyList<WorkCalendarEventDto>>> GetCalendarAsync(WorkCalendarQuery query, CancellationToken ct = default);
}
