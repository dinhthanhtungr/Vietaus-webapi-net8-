using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs
{
    public class PdfPrinterDeliveryOrderDetail
    {
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string LotNumber { get; set; } = string.Empty;
        public decimal WeightKg { get; set; } = 0;
        public int BagNumber { get; set; } = 0;
        public string StockStatus { get; set; } = string.Empty;
        public string PONoNumber { get; set; } = string.Empty; 
    }
}
