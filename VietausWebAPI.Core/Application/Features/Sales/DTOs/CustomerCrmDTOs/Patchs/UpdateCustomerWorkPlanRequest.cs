using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Patchs
{
    public class UpdateCustomerWorkPlanRequest
    {
        public string? PlanName { get; set; }
        public string? Objective { get; set; }
        public string? Strategy { get; set; }
        public string? DiscussionSummary { get; set; }
        public string? NextAction { get; set; }
        public CustomerWorkPlanStatus? Status { get; set; }
        public CustomerFollowUpPriority? Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public Guid? AssignedSaleEmployeeId { get; set; }
        public bool? IsActive { get; set; }
    }
}
