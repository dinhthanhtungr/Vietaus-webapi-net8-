namespace VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports
{
    public class MerchandiseOrderReportHeaderDto
    {
        public int TotalOrderCount { get; set; }
        public int PaidOrderCount { get; set; }
        public int UnpaidOrderCount { get; set; }
        public int OverdueOrderCount { get; set; }
        public int CompletedOrderCount { get; set; }
        public int InProgressOrderCount { get; set; }
        public int NotDeliveredOrderCount { get; set; }

        public decimal OrderedQuantity { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public decimal RemainingQuantity { get; set; }

        public decimal TotalOrderAmount { get; set; }
        public decimal ActualSoldAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal UnpaidAmount { get; set; }

        public decimal FulfillmentRate { get; set; }
        public decimal ActualSoldRate { get; set; }

        public IReadOnlyList<MerchandiseOrderReportChartPointDto> RevenueByMonth { get; set; }
            = new List<MerchandiseOrderReportChartPointDto>();

        public IReadOnlyList<MerchandiseOrderReportChartPointDto> RevenueByManager { get; set; }
            = new List<MerchandiseOrderReportChartPointDto>();

        public IReadOnlyList<MerchandiseOrderReportChartPointDto> RevenueByCustomer { get; set; }
            = new List<MerchandiseOrderReportChartPointDto>();

        public IReadOnlyList<MerchandiseOrderReportChartPointDto> QuantityByManager { get; set; }
            = new List<MerchandiseOrderReportChartPointDto>();

        public IReadOnlyList<MerchandiseOrderReportChartPointDto> QuantityByCustomer { get; set; }
            = new List<MerchandiseOrderReportChartPointDto>();
    }

    public class MerchandiseOrderReportChartPointDto
    {
        public string Label { get; set; } = string.Empty;
        public int OrderCount { get; set; }
        public decimal OrderedQuantity { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public decimal ActualSoldAmount { get; set; }
        public decimal RemainingAmount { get; set; }
    }
}
