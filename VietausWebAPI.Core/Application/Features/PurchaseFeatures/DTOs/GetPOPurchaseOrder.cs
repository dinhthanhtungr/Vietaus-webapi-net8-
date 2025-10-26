using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs
{
    public class GetPOPurchaseOrder
    {
        public Guid MerchandiseOrderId { get; set; }
        public string? ExternalId { get; set; }

        public string? Status { get; set; }
        public List<GetPOPurchaseOrderDetail> PODetailDeliveryOrder { get; set; } = new List<GetPOPurchaseOrderDetail>();
    }
}
