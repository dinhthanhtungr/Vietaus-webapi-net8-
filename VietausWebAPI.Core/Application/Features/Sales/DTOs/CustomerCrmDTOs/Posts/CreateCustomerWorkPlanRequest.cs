using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Posts
{
    public class CreateCustomerWorkPlanRequest
    {
        public Guid CustomerId { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public string? Objective { get; set; }
        public string? Strategy { get; set; }
        public string? DiscussionSummary { get; set; }
        public string? NextAction { get; set; }
        public CustomerWorkPlanStatus Status { get; set; } = CustomerWorkPlanStatus.Active;
        public CustomerFollowUpPriority Priority { get; set; } = CustomerFollowUpPriority.Normal;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public Guid? AssignedSaleEmployeeId { get; set; }
    }
}
