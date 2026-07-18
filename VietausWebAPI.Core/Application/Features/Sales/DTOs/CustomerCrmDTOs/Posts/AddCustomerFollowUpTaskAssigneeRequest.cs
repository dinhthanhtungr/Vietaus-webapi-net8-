using System;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Posts
{
    /// <summary>
    /// Request used to add an employee as a support assignee for a CRM follow-up task.
    /// </summary>
    public sealed class AddCustomerFollowUpTaskAssigneeRequest
    {
        public Guid EmployeeId { get; set; }
        public bool IsPrimary { get; set; }
        public bool NotifySaleAdmin { get; set; }
        public bool NotifyLeader { get; set; }
    }
}
