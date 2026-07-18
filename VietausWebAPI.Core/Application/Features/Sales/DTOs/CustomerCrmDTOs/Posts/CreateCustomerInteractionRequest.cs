using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Posts
{
    public class CreateCustomerInteractionRequest
    {
        public Guid CustomerId { get; set; }
        public Guid? ContactId { get; set; }
        public CustomerInteractionType InteractionType { get; set; } = CustomerInteractionType.Call;
        public string? Subject { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? Outcome { get; set; }
        public string? NextAction { get; set; }
        public DateTime? InteractionAt { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public CustomerFollowUpPriority Priority { get; set; } = CustomerFollowUpPriority.Normal;
        public Guid? AssignedSaleEmployeeId { get; set; }
    }
}
