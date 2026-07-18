using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets
{
    public class GetCustomerFollowUpTaskDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string? CustomerExternalId { get; set; }
        public string? CustomerName { get; set; }
        public Guid? CustomerInteractionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? NextAction { get; set; }
        public CustomerFollowUpTaskStatus Status { get; set; }
        public CustomerFollowUpPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public Guid? CompletedBy { get; set; }
        public string? CompletionNote { get; set; }
        public Guid? AssignedSaleEmployeeId { get; set; }
        public string? AssignedSaleEmployeeName { get; set; }
        public bool IsOverdue { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
