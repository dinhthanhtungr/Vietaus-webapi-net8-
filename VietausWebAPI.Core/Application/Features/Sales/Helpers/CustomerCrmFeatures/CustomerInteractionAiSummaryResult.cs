using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Application.Features.Sales.Helpers.CustomerCrmFeatures
{
    public class CustomerInteractionAiSummaryResult
    {
        public string Summary { get; set; } = string.Empty;
        public string CustomerNeed { get; set; } = string.Empty;
        public string CurrentStage { get; set; } = string.Empty;
        public string NextAction { get; set; } = string.Empty;
        public string Risk { get; set; } = string.Empty;
        public string Sentiment { get; set; } = string.Empty;
    }

    public class CustomerInteractionAiSummaryBatchResult
    {
        public List<CustomerInteractionAiSummaryBatchResultItem> Items { get; set; } = new();
    }

    public class CustomerInteractionAiSummaryBatchResultItem : CustomerInteractionAiSummaryResult
    {
        public Guid CustomerId { get; set; }
        public string? ErrorMessage { get; set; }
    }
}

