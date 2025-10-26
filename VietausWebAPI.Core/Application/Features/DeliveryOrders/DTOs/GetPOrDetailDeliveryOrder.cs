using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs
{
    public class GetPODetailDeliveryOrder
    {
        public Guid MerchandiseOrderId { get; set; }
        public string? MerchandiseOrderExternalIdSnapShot { get; set; }

        public Guid MerchandiseOrderDetailId { get; set; }

        public Guid ProductId { get; set; }
        public string? ProductExternalIdSnapShot { get; set; }
        public string? ProductNameSnapShot { get; set; }

        public string? ManufacturingFormulaExternalIdSnapShot { get; set; }
        public string? LocationNameSnapShot { get; set; } = null;
        public string? PONo { get; set; } = null; // PO Number

        public decimal Quantity { get; set; }         // DECIMAL
        public decimal StockQuantity { get; set; }

        public int NumOfBags { get; set; }            // INT
        public bool IsActive { get; set; } = true;
    }
}
