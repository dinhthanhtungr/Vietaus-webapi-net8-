using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets
{
    public class GetCustomerWorkPlanDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string? CustomerExternalId { get; set; }
        public string? CustomerName { get; set; }
        public string PlanName { get; set; } = string.Empty;
        public string? Objective { get; set; }
        public string? Strategy { get; set; }
        public string? DiscussionSummary { get; set; }
        public string? NextAction { get; set; }
        public CustomerWorkPlanStatus Status { get; set; }
        public CustomerFollowUpPriority Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public Guid? AssignedSaleEmployeeId { get; set; }
        public string? AssignedSaleEmployeeName { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
