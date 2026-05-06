using System;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports
{
    public class MerchandiseOrderReportDetailDto
    {
        public Guid MerchandiseOrderId { get; set; }
        public Guid MerchandiseOrderDetailId { get; set; }

        public string MerchandiseOrderCode { get; set; } = string.Empty;
        public string SampleRequestExternalId { get; set; } = string.Empty;
        public string ColourCode { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string FormulaCode { get; set; } = string.Empty;

        public DateTime? DeliveryRequestDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }

        public decimal RequestedQuantity { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public decimal RemainingQuantity { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal ActualSoldAmount { get; set; }
        public decimal RemainingAmount { get; set; }

        public string BagType { get; set; } = string.Empty;
        public string PackageWeight { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
    }
}
