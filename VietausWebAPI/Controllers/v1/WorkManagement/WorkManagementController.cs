using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.DTOs;
using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.Queries;
using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.ServiceContracts;
using VietausWebAPI.Core.Domain.Enums.WorkTaskEnums;

namespace VietausWebAPI.WebAPI.Controllers.v1.WorkManagement;

[ApiController]
[Route("api/work-management")]
[Authorize]
public sealed class WorkManagementController : ControllerBase
{
    private readonly IWorkManagementService _service;

    public WorkManagementController(IWorkManagementService service) => _service = service;

    [HttpPost("tasks")]
    public async Task<IActionResult> CreateTask(CreateWorkTaskRequest request, CancellationToken ct)
        => ToActionResult(await _service.CreateTaskAsync(request, ct));

    [HttpPut("tasks/{id:guid}")]
    public async Task<IActionResult> UpdateTask(Guid id, UpdateWorkTaskRequest request, CancellationToken ct)
        => ToActionResult(await _service.UpdateTaskAsync(id, request, ct));

    [HttpPost("tasks/{id:guid}/complete")]
    public async Task<IActionResult> CompleteTask(Guid id, CompleteWorkTaskRequest request, CancellationToken ct)
        => ToActionResult(await _service.CompleteTaskAsync(id, request, ct));

    [HttpGet("tasks/{id:guid}")]
    public async Task<IActionResult> GetTask(Guid id, CancellationToken ct)
        => ToActionResult(await _service.GetTaskAsync(id, ct));

    [HttpGet("tasks")]
    public async Task<IActionResult> GetTasks([FromQuery] WorkTaskQuery query, CancellationToken ct)
        => ToActionResult(await _service.GetTasksAsync(query, ct));

    [HttpGet("tasks/my")]
    public async Task<IActionResult> GetMyTasks([FromQuery] WorkTaskQuery query, CancellationToken ct)
    {
        query.OnlyMine = true;
        return ToActionResult(await _service.GetTasksAsync(query, ct));
    }

    [HttpGet("references/{referenceType}/{referenceId:guid}/tasks")]
    public async Task<IActionResult> GetTasksByReference(
        WorkReferenceType referenceType,
        Guid referenceId,
        [FromQuery] WorkTaskQuery query,
        CancellationToken ct)
    {
        query.ReferenceType = referenceType;
        query.ReferenceId = referenceId;
        return ToActionResult(await _service.GetTasksAsync(query, ct));
    }

    [HttpPost("tasks/{id:guid}/assignees")]
    public async Task<IActionResult> AddTaskAssignee(Guid id, AddWorkAssigneeRequest request, CancellationToken ct)
        => ToActionResult(await _service.AddTaskAssigneeAsync(id, request, ct));

    [HttpPost("tasks/{id:guid}/assignees/group")]
    public async Task<IActionResult> AddTaskGroupAssignees(Guid id, AddWorkGroupAssigneesRequest request, CancellationToken ct)
        => ToActionResult(await _service.AddTaskGroupAssigneesAsync(id, request, ct));

    [HttpDelete("tasks/{id:guid}/assignees/{employeeId:guid}")]
    public async Task<IActionResult> RemoveTaskAssignee(Guid id, Guid employeeId, CancellationToken ct)
        => ToActionResult(await _service.RemoveTaskAssigneeAsync(id, employeeId, ct));

    [HttpGet("tasks/{id:guid}/assignees")]
    public async Task<IActionResult> GetTaskAssignees(Guid id, CancellationToken ct)
        => ToActionResult(await _service.GetTaskAssigneesAsync(id, ct));

    [HttpPost("plans")]
    public async Task<IActionResult> CreatePlan(CreateWorkPlanRequest request, CancellationToken ct)
        => ToActionResult(await _service.CreatePlanAsync(request, ct));

    [HttpPut("plans/{id:guid}")]
    public async Task<IActionResult> UpdatePlan(Guid id, UpdateWorkPlanRequest request, CancellationToken ct)
        => ToActionResult(await _service.UpdatePlanAsync(id, request, ct));

    [HttpGet("plans/{id:guid}")]
    public async Task<IActionResult> GetPlan(Guid id, CancellationToken ct)
        => ToActionResult(await _service.GetPlanAsync(id, ct));

    [HttpGet("plans")]
    public async Task<IActionResult> GetPlans([FromQuery] WorkPlanQuery query, CancellationToken ct)
        => ToActionResult(await _service.GetPlansAsync(query, ct));

    [HttpGet("plans/my")]
    public async Task<IActionResult> GetMyPlans([FromQuery] WorkPlanQuery query, CancellationToken ct)
    {
        query.OnlyMine = true;
        return ToActionResult(await _service.GetPlansAsync(query, ct));
    }

    [HttpGet("references/{referenceType}/{referenceId:guid}/plans")]
    public async Task<IActionResult> GetPlansByReference(
        WorkReferenceType referenceType,
        Guid referenceId,
        [FromQuery] WorkPlanQuery query,
        CancellationToken ct)
    {
        query.ReferenceType = referenceType;
        query.ReferenceId = referenceId;
        return ToActionResult(await _service.GetPlansAsync(query, ct));
    }

    [HttpPost("plans/{id:guid}/assignees")]
    public async Task<IActionResult> AddPlanAssignee(Guid id, AddWorkAssigneeRequest request, CancellationToken ct)
        => ToActionResult(await _service.AddPlanAssigneeAsync(id, request, ct));

    [HttpPost("plans/{id:guid}/assignees/group")]
    public async Task<IActionResult> AddPlanGroupAssignees(Guid id, AddWorkGroupAssigneesRequest request, CancellationToken ct)
        => ToActionResult(await _service.AddPlanGroupAssigneesAsync(id, request, ct));

    [HttpDelete("plans/{id:guid}/assignees/{employeeId:guid}")]
    public async Task<IActionResult> RemovePlanAssignee(Guid id, Guid employeeId, CancellationToken ct)
        => ToActionResult(await _service.RemovePlanAssigneeAsync(id, employeeId, ct));

    [HttpGet("plans/{id:guid}/assignees")]
    public async Task<IActionResult> GetPlanAssignees(Guid id, CancellationToken ct)
        => ToActionResult(await _service.GetPlanAssigneesAsync(id, ct));

    [HttpGet("calendar")]
    public async Task<IActionResult> GetCalendar([FromQuery] WorkCalendarQuery query, CancellationToken ct)
        => ToActionResult(await _service.GetCalendarAsync(query, ct));

    private IActionResult ToActionResult(VietausWebAPI.Core.Application.Shared.Models.PageModels.OperationResult result)
        => result.Success ? Ok(result) : BadRequest(result);
}
