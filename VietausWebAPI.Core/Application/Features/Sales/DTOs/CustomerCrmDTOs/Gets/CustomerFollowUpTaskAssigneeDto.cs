using System;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets
{
    /// <summary>
    /// Read model for an employee assigned to support or follow a CRM follow-up task.
    /// </summary>
    public sealed class CustomerFollowUpTaskAssigneeDto
    {
        public Guid Id { get; set; }
        public Guid CustomerFollowUpTaskId { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
