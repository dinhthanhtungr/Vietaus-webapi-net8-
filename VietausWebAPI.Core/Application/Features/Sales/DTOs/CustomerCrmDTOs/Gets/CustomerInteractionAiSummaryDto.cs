using System;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets
{
    public class CustomerInteractionAiSummaryDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public Guid? SaleEmployeeId { get; set; }
        public string? SaleEmployeeCode { get; set; }
        public string? SaleEmployeeName { get; set; }
        public Guid CompanyId { get; set; }
        public CustomerInteractionSummaryScope SummaryScope { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodTo { get; set; }
        public int InteractionCount { get; set; }
        public string PreviousSummary { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string CustomerNeed { get; set; } = string.Empty;
        public string CurrentStage { get; set; } = string.Empty;
        public string NextAction { get; set; } = string.Empty;
        public string Risk { get; set; } = string.Empty;
        public string Sentiment { get; set; } = string.Empty;
        public string SourceModel { get; set; } = string.Empty;
        public string PromptVersion { get; set; } = string.Empty;
        public bool IsAiSuccess { get; set; }
        public bool IsAiSkipped { get; set; }
        public string? AiErrorMessage { get; set; }
        public DateTime? AiGeneratedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public AiRateLimitInfoDto? RateLimit { get; set; }
    }
}
