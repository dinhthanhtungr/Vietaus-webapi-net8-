using System;
using System.Collections.Generic;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Posts
{
    /// <summary>
    /// Request used to generate AI summaries for multiple customers in one API call.
    /// Each customer is still summarized and stored separately.
    /// </summary>
    public class GenerateCustomerInteractionAiSummaryBatchRequest
    {
        public List<Guid> CustomerIds { get; set; } = new();
        public CustomerInteractionSummaryScope SummaryScope { get; set; } = CustomerInteractionSummaryScope.Monthly;
        public Guid? SaleEmployeeId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public bool ForceRegenerate { get; set; } = false;
    }
}
