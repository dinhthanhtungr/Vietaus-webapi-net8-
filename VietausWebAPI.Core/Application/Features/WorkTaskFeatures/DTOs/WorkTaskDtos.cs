using VietausWebAPI.Core.Domain.Enums.WorkTaskEnums;

namespace VietausWebAPI.Core.Application.Features.WorkTaskFeatures.DTOs;

public sealed class CreateWorkTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? NextAction { get; set; }
    public WorkTaskStatus Status { get; set; } = WorkTaskStatus.Pending;
    public WorkTaskPriority Priority { get; set; } = WorkTaskPriority.Normal;
    public DateTime? DueDate { get; set; }
    public Guid? AssignedToEmployeeId { get; set; }
    public List<WorkReferenceRequest> References { get; set; } = new();
}

public sealed class UpdateWorkTaskRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? NextAction { get; set; }
    public WorkTaskStatus? Status { get; set; }
    public WorkTaskPriority? Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public bool ClearDueDate { get; set; }
    public Guid? AssignedToEmployeeId { get; set; }
    public bool ClearAssignedToEmployee { get; set; }
    public bool? IsActive { get; set; }
    public List<WorkReferenceRequest>? References { get; set; }
}

public sealed class CompleteWorkTaskRequest
{
    public string? CompletionNote { get; set; }
    public WorkTaskStatus Status { get; set; } = WorkTaskStatus.Done;
}

public sealed class WorkTaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? NextAction { get; set; }
    public WorkTaskStatus Status { get; set; }
    public WorkTaskPriority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public Guid? CompletedBy { get; set; }
    public string? CompletionNote { get; set; }
    public Guid? AssignedToEmployeeId { get; set; }
    public string? AssignedToEmployeeName { get; set; }
    public bool IsOverdue { get; set; }
    public Guid CompanyId { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public bool IsActive { get; set; }
    public IReadOnlyList<WorkReferenceDto> References { get; set; } = new List<WorkReferenceDto>();
    public IReadOnlyList<WorkAssigneeDto> Assignees { get; set; } = new List<WorkAssigneeDto>();
}
