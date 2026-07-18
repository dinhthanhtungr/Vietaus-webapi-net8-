using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Patchs
{
    public class UpdateCustomerFollowUpTaskRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? NextAction { get; set; }
        public CustomerFollowUpTaskStatus? Status { get; set; }
        public CustomerFollowUpPriority? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? AssignedSaleEmployeeId { get; set; }
        public bool? IsActive { get; set; }
    }
}
