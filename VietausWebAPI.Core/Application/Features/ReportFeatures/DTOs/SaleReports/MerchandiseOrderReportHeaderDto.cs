namespace VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports
{
    public class MerchandiseOrderReportHeaderDto
    {
        // ============= ĐƠN HÀNG =============
        public int TotalOrderCount { get; set; }              // Tổng đơn trong kết quả
        public int OrdersCreatedInPeriod { get; set; }        // Đơn TẠO trong kỳ
        public int OrdersDeliveredInPeriod { get; set; }      // Đơn GIAO trong kỳ
        
        public int PaidOrderCount { get; set; }
        public int UnpaidOrderCount { get; set; }
        public int OverdueOrderCount { get; set; }
        public int CompletedOrderCount { get; set; }
        public int InProgressOrderCount { get; set; }
        public int NotDeliveredOrderCount { get; set; }

        // ============= SỐ LƯỢNG =============
        public decimal OrderedQuantity { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public decimal RemainingQuantity { get; set; }
        public decimal FulfillmentRate { get; set; }

        // ============= DOANH THU =============
        public decimal TotalOrderAmount { get; set; }         // Tổng giá trị đơn hàng
        public decimal ActualSoldAmount { get; set; }         // Doanh thu thực (đã giao)
        public decimal RemainingAmount { get; set; }          // Doanh thu còn lại
        public decimal UnpaidAmount { get; set; }             // Tiền chưa thanh toán
        public decimal ActualSoldRate { get; set; }           // Tỷ lệ doanh thu thực/tổng đơn
        
        // MỚI: So sánh kỳ
        public decimal PreviousPeriodRevenue { get; set; }    // Doanh thu kỳ trước
        public decimal RevenueGrowthRate { get; set; }        // % tăng trưởng
        public decimal RevenueGrowthAmount { get; set; }      // Số tiền tăng/giảm
        
        // MỚI: Phân tích chi tiết
        public decimal AverageOrderValue { get; set; }        // Giá trị TB/đơn
        public decimal AverageDailyRevenue { get; set; }      // Doanh thu TB/ngày
        public int DeliveryDaysCount { get; set; }            // Số ngày có giao hàng
        public DateTime? FirstDeliveryDate { get; set; }      // Ngày giao đầu tiên trong kỳ
        public DateTime? LastDeliveryDate { get; set; }       // Ngày giao cuối cùng trong kỳ

        // MỚI: Cash flow
        public decimal CollectedAmount { get; set; }          // Số tiền đã thu
        public decimal CollectionRate { get; set; }           // Tỷ lệ thu tiền

        // ============= BIỂU ĐỒ =============
        public IReadOnlyList<MerchandiseOrderReportChartPointDto> RevenueByMonth { get; set; }
            = new List<MerchandiseOrderReportChartPointDto>();

        public IReadOnlyList<MerchandiseOrderReportChartPointDto> RevenueByWeek { get; set; }
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
        
        // Đếm
        public int OrderCount { get; set; }                   // Số đơn hàng
        public int DeliveryCount { get; set; }                // Số lần giao hàng
        
        // Số lượng
        public decimal OrderedQuantity { get; set; }
        public decimal DeliveredQuantity { get; set; }
        
        // Doanh thu
        public decimal TotalOrderAmount { get; set; }
        public decimal ActualSoldAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        
        // Phân tích
        public decimal AverageDeliveryAmount { get; set; }    // Giá trị TB/lần giao
        public DateTime? PeriodStart { get; set; }            // Ngày đầu kỳ
        public DateTime? PeriodEnd { get; set; }              // Ngày cuối kỳ
    }
}
