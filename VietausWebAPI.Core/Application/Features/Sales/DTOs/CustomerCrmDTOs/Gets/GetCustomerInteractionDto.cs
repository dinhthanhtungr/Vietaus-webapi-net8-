using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets
{
    public class GetCustomerInteractionDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string? CustomerExternalId { get; set; }
        public string? CustomerName { get; set; }
        public Guid? ContactId { get; set; }
        public string? ContactName { get; set; }
        public CustomerInteractionType InteractionType { get; set; }
        public string? Subject { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? Outcome { get; set; }
        public string? NextAction { get; set; }
        public DateTime InteractionAt { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public Guid? AssignedSaleEmployeeId { get; set; }
        public string? AssignedSaleEmployeeName { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public bool IsActive { get; set; }
    }
}
