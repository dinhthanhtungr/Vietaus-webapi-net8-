using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.WorkTaskEnums;

namespace VietausWebAPI.Core.Application.Features.WorkTaskFeatures.Queries;

public sealed class WorkTaskQuery : PaginationQuery
{
    public Guid? AssignedToEmployeeId { get; set; }
    public WorkTaskStatus? Status { get; set; }
    public WorkTaskPriority? Priority { get; set; }
    public WorkReferenceType? ReferenceType { get; set; }
    public Guid? ReferenceId { get; set; }
    public bool? OnlyOverdue { get; set; }
    public string? Keyword { get; set; }
    public DateTime? DueFrom { get; set; }
    public DateTime? DueTo { get; set; }
    public bool IncludeInactive { get; set; }
    public bool OnlyMine { get; set; }
}

public sealed class WorkPlanQuery : PaginationQuery
{
    public Guid? AssignedToEmployeeId { get; set; }
    public WorkPlanStatus? Status { get; set; }
    public WorkTaskPriority? Priority { get; set; }
    public WorkReferenceType? ReferenceType { get; set; }
    public Guid? ReferenceId { get; set; }
    public string? Keyword { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public bool IncludeInactive { get; set; }
    public bool OnlyMine { get; set; }
}

public sealed class WorkCalendarQuery
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public Guid? AssignedToEmployeeId { get; set; }
    public WorkReferenceType? ReferenceType { get; set; }
    public Guid? ReferenceId { get; set; }
    public bool OnlyMine { get; set; }
    public bool ShowCompleted { get; set; } = true;
    public bool ShowCanceled { get; set; }
}
