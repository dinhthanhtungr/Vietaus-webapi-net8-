using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs
{
    public class GetPODetailDeliveryOrder
    {
        public Guid MerchandiseOrderDetailId { get; set; }

        public Guid ProductId { get; set; }
        public string? ProductExternalIdSnapShot { get; set; }
        public string? ProductNameSnapShot { get; set; }

        public string? PONo { get; set; } = null;
        public decimal RequestQuantity { get; set; } 

        public List<GetQuantityAndLots> getQuantityAndLots { get; set; } = new List<GetQuantityAndLots>();

    }
}
