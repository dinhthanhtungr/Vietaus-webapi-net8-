namespace VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports
{
    public class MerchandiseOrderReportRowDto
    {
        public Guid MerchandiseOrderId { get; set; }
        public string MerchandiseOrderCode { get; set; } = string.Empty;
        public string PONo { get; set; } = string.Empty;

        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;

        public Guid ManagerById { get; set; }
        public string ManagerCode { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;

        public Guid? GroupId { get; set; }
        public string GroupCode { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }
        public DateTime? FirstDeliveryRequestDate { get; set; }
        public DateTime? LastDeliveryRequestDate { get; set; }
        public DateTime? LastActualDeliveryDate { get; set; }

        public decimal OrderedQuantity { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public decimal RemainingQuantity { get; set; }

        public decimal TotalOrderAmount { get; set; }
        public decimal ActualSoldAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal FulfillmentRate { get; set; }
        public decimal ActualSoldRate { get; set; }
        public decimal UnpaidAmount { get; set; }

        public string PaymentType { get; set; } = string.Empty;
        public bool IsPaid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public int OrderAgeDays { get; set; }
        public bool IsOverdue { get; set; }
        public int OverdueDays { get; set; }
        public string HealthStatus { get; set; } = string.Empty;
        public int DetailCount { get; set; }
    }
}
