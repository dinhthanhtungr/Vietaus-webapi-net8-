using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Posts
{
    public class CreateCustomerFollowUpTaskRequest
    {
        public Guid CustomerId { get; set; }
        public Guid? CustomerInteractionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? NextAction { get; set; }
        public CustomerFollowUpTaskStatus Status { get; set; } = CustomerFollowUpTaskStatus.Pending;
        public CustomerFollowUpPriority Priority { get; set; } = CustomerFollowUpPriority.Normal;
        public DateTime? DueDate { get; set; }
        public Guid? AssignedSaleEmployeeId { get; set; }
    }
}
