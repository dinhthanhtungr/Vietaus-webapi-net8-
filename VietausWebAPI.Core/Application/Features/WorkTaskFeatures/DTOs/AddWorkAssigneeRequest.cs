namespace VietausWebAPI.Core.Application.Features.WorkTaskFeatures.DTOs;

public sealed class AddWorkAssigneeRequest
{
    public Guid EmployeeId { get; set; }
    public bool IsPrimary { get; set; }
    public bool NotifySaleAdmin { get; set; }
    public bool NotifyLeader { get; set; }
}
