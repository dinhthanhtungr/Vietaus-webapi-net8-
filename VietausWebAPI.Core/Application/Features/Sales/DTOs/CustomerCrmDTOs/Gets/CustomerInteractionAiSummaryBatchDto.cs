using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets
{
    /// <summary>
    /// Result returned after generating AI summaries for a group of customers.
    /// </summary>
    public class CustomerInteractionAiSummaryBatchDto
    {
        public int TotalRequested { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public AiRateLimitInfoDto? RateLimit { get; set; }
        public List<CustomerInteractionAiSummaryBatchItemDto> Items { get; set; } = new();
    }

    /// <summary>
    /// Result of one customer inside a batch AI summary request.
    /// </summary>
    public class CustomerInteractionAiSummaryBatchItemDto
    {
        public Guid CustomerId { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public CustomerInteractionAiSummaryDto? Summary { get; set; }
    }
}
