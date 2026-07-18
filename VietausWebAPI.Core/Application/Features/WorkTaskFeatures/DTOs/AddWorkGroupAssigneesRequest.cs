namespace VietausWebAPI.Core.Application.Features.WorkTaskFeatures.DTOs;

public sealed class AddWorkGroupAssigneesRequest
{
    public Guid GroupId { get; set; }
    public bool AssignMembers { get; set; }
    public bool AssignLeader { get; set; }
    public bool NotifySaleAdmin { get; set; }
    public bool NotifyLeader { get; set; }
}
