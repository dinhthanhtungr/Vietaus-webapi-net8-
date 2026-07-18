using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.QuotationDTOs
{
    public sealed class QuotationQuery : PaginationQuery
    {
        public Guid? CustomerId { get; set; }
        public Guid? SaleEmployeeId { get; set; }
        public QuotationStatus? Status { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? Keyword { get; set; }
        public bool IncludeInactive { get; set; }
    }

    public sealed class CreateQuotationRequest
    {
        public Guid CustomerId { get; set; }
        public Guid? ContactId { get; set; }
        public string? ContactName { get; set; }
        public DateTime? QuotationDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string Currency { get; set; } = "VND";
        public decimal ExchangeRate { get; set; } = 1m;
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string? PaymentTerms { get; set; }
        public string? DeliveryTerms { get; set; }
        public string? Note { get; set; }
        public IReadOnlyList<CreateQuotationLineRequest> Lines { get; set; } = new List<CreateQuotationLineRequest>();
    }

    public sealed class CreateQuotationLineRequest
    {
        public Guid ProductId { get; set; }
        public string? ProductExternalIdSnapshot { get; set; }
        public string? ProductNameSnapshot { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = "kg";
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal TaxPercent { get; set; }
        public string? Note { get; set; }
        public int SortOrder { get; set; }
        public IReadOnlyList<QuotationLinePriceTierDto> PriceTiers { get; set; } = new List<QuotationLinePriceTierDto>();
    }

    public class QuotationSummaryDto
    {
        public Guid QuotationId { get; set; }
        public string ExternalId { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public Guid SaleEmployeeId { get; set; }
        public string SaleEmployeeCode { get; set; } = string.Empty;
        public string SaleEmployeeName { get; set; } = string.Empty;
        public QuotationStatus Status { get; set; }
        public DateTime QuotationDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public decimal TotalAmount { get; set; }
        public int LineCount { get; set; }
        public bool IsActive { get; set; }
    }

    public sealed class QuotationDetailDto : QuotationSummaryDto
    {
        public Guid? ContactId { get; set; }
        public string? ContactName { get; set; }
        public string Currency { get; set; } = "VND";
        public decimal ExchangeRate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string? PaymentTerms { get; set; }
        public string? DeliveryTerms { get; set; }
        public string? Note { get; set; }
        public int Version { get; set; }
        public Guid? PreviousQuotationId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public IReadOnlyList<QuotationLineDto> Lines { get; set; } = new List<QuotationLineDto>();
        public IReadOnlyList<QuotationStatusHistoryDto> StatusHistories { get; set; } = new List<QuotationStatusHistoryDto>();
    }

    public sealed class QuotationLineDto
    {
        public Guid QuotationLineId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductExternalIdSnapshot { get; set; } = string.Empty;
        public string ProductNameSnapshot { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal LineTotal { get; set; }
        public string? Note { get; set; }
        public int SortOrder { get; set; }
        public IReadOnlyList<QuotationLinePriceTierDto> PriceTiers { get; set; } = new List<QuotationLinePriceTierDto>();
    }

    public sealed class QuotationLinePriceTierDto
    {
        public decimal MinQuantity { get; set; }
        public decimal? MaxQuantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceAdjustment { get; set; }
        public string? Label { get; set; }
        public int SortOrder { get; set; }
    }

    public sealed class QuotationStatusHistoryDto
    {
        public Guid Id { get; set; }
        public QuotationStatus FromStatus { get; set; }
        public QuotationStatus ToStatus { get; set; }
        public string? Note { get; set; }
        public Guid ChangedBy { get; set; }
        public string ChangedByName { get; set; } = string.Empty;
        public DateTime ChangedDate { get; set; }
    }
}
