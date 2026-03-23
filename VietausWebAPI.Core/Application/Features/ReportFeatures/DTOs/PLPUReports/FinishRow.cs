using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.PLPUReports
{
    public class FinishRow
    {
        public int Stt { get; set; }

        public string DeliveryOrderCode { get; set; } = string.Empty;     // Mã số
        public string MerchandiseOrderCode { get; set; } = string.Empty;  // Đơn hàng
        public string MfgOrderCode { get; set; } = string.Empty;          // LSX

        public string CustomerName { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;

        public string BatchNo { get; set; } = string.Empty;

        public DateTime? OrderReceivedDate { get; set; }    // MfgProductionOrder.CreatedDate
        public DateTime? DeliveryRequestDate { get; set; }  // MerchandiseOrderDetail.DeliveryRequestDate
        public DateTime? ActualDeliveryDate { get; set; }   // DeliveryOrder.CreatedDate

        public int? LateDays { get; set; }

        public decimal OrderedQuantity { get; set; }
        public decimal DeliveredQuantity { get; set; }

        public string Address { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
    }
}
