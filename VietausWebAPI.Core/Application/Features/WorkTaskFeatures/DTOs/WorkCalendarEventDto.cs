namespace VietausWebAPI.Core.Application.Features.WorkTaskFeatures.DTOs;

public sealed class WorkCalendarEventDto
{
    public string Id { get; set; } = string.Empty;
    public Guid SourceId { get; set; }
    public string SourceType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public bool IsOverdue { get; set; }
    public string ColorKey { get; set; } = string.Empty;
    public Guid? AssignedToEmployeeId { get; set; }
    public string? AssignedToEmployeeName { get; set; }
    public WorkReferenceDto? PrimaryReference { get; set; }
}
