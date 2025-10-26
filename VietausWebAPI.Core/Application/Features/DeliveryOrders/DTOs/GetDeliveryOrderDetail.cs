using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs
{
    public class GetDeliveryOrderDetail
    {
        public Guid? ProductId { get; set; }
        public string? ProductExternalIdSnapShot { get; set; }
        public string? ProductNameSnapShot { get; set; }

        public string? LotNoList { get; set; } = null;
        public string? PONo { get; set; } = null;

        public decimal Quantity { get; set; }
        public int NumOfBags { get; set; }
    }
}
