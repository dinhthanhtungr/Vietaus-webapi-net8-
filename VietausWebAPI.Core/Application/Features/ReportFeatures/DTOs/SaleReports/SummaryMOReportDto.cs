using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports
{
    public class SummaryMOReportDto
    {
        public Guid MerchandiseOrderId { get; set; }
        public Guid MerchandiseOrderDetailId { get; set; }

        public string MerchandiseOrderCode { get; set; } = string.Empty;   // mã đơn hàng
        public string CustomerCode { get; set; } = string.Empty;           // mã khách hàng
        public string CustomerName { get; set; } = string.Empty;           // tên khách hàng
        public string ProductCode { get; set; } = string.Empty;            // mã hàng
        public string ProductName { get; set; } = string.Empty;            // tên hàng

        public DateTime OrderDate { get; set; }                            // ngày đặt hàng
        public DateTime DeliveryRequestDate { get; set; }                  // ngày KH yêu cầu giao
        public DateTime? ExpectedDeliveryDate { get; set; }                // ngày hứa giao / kế hoạch
        public DateTime? ActualDeliveryDate { get; set; }                  // ngày giao thực tế

        public decimal? RequestedQuantity { get; set; }                     // số lượng yêu cầu
        public decimal? DeliveredQuantity { get; set; }                     // số lượng đã giao
        public decimal? RemainingQuantity { get; set; }                     // còn lại

        public decimal? UnitPrice { get; set; }                             // giá bán
        public decimal? TotalPrice { get; set; }                            // thành tiền
        public string Status { get; set; } = string.Empty;
    }
}
