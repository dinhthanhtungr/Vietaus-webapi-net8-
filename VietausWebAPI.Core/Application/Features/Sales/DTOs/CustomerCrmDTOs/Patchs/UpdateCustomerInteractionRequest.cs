using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Patchs
{
    public class UpdateCustomerInteractionRequest
    {
        public Guid? ContactId { get; set; }
        public CustomerInteractionType? InteractionType { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public string? Outcome { get; set; }
        public string? NextAction { get; set; }
        public DateTime? InteractionAt { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public Guid? AssignedSaleEmployeeId { get; set; }
        public bool? IsActive { get; set; }
    }
}
