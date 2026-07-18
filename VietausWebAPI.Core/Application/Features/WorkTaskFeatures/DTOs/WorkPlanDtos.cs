using VietausWebAPI.Core.Domain.Enums.WorkTaskEnums;

namespace VietausWebAPI.Core.Application.Features.WorkTaskFeatures.DTOs;

public sealed class CreateWorkPlanRequest
{
    public string PlanName { get; set; } = string.Empty;
    public string? Objective { get; set; }
    public string? Strategy { get; set; }
    public string? DiscussionSummary { get; set; }
    public string? NextAction { get; set; }
    public WorkPlanStatus Status { get; set; } = WorkPlanStatus.Active;
    public WorkTaskPriority Priority { get; set; } = WorkTaskPriority.Normal;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? NextFollowUpDate { get; set; }
    public Guid? AssignedToEmployeeId { get; set; }
    public List<WorkReferenceRequest> References { get; set; } = new();
}

public sealed class UpdateWorkPlanRequest
{
    public string? PlanName { get; set; }
    public string? Objective { get; set; }
    public string? Strategy { get; set; }
    public string? DiscussionSummary { get; set; }
    public string? NextAction { get; set; }
    public WorkPlanStatus? Status { get; set; }
    public WorkTaskPriority? Priority { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? NextFollowUpDate { get; set; }
    public Guid? AssignedToEmployeeId { get; set; }
    public bool ClearAssignedToEmployee { get; set; }
    public bool? IsActive { get; set; }
    public List<WorkReferenceRequest>? References { get; set; }
}

public sealed class WorkPlanDto
{
    public Guid Id { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public string? Objective { get; set; }
    public string? Strategy { get; set; }
    public string? DiscussionSummary { get; set; }
    public string? NextAction { get; set; }
    public WorkPlanStatus Status { get; set; }
    public WorkTaskPriority Priority { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? NextFollowUpDate { get; set; }
    public Guid? AssignedToEmployeeId { get; set; }
    public string? AssignedToEmployeeName { get; set; }
    public Guid CompanyId { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public bool IsActive { get; set; }
    public IReadOnlyList<WorkReferenceDto> References { get; set; } = new List<WorkReferenceDto>();
    public IReadOnlyList<WorkAssigneeDto> Assignees { get; set; } = new List<WorkAssigneeDto>();
}
