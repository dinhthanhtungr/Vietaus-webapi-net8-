using System;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Posts
{
    /// <summary>
    /// Request used to assign support employees to a CRM follow-up task by resolving employees from a company group.
    /// </summary>
    public sealed class AddCustomerFollowUpTaskGroupAssigneesRequest
    {
        public Guid GroupId { get; set; }
        public bool AssignMembers { get; set; }
        public bool AssignLeader { get; set; }
        public bool NotifySaleAdmin { get; set; }
        public bool NotifyLeader { get; set; }
    }
}
