using System;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Posts
{
    public class GenerateCustomerInteractionAiSummaryRequest
    {
        public CustomerInteractionSummaryScope SummaryScope { get; set; } = CustomerInteractionSummaryScope.Monthly;
        public Guid? SaleEmployeeId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public bool ForceRegenerate { get; set; } = true;
    }
}
