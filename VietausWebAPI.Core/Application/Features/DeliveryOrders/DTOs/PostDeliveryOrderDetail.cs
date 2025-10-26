using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs
{
    public class PostDeliveryOrderDetail
    {
        public Guid? MerchandiseOrderId { get; set; }
        public Guid? MerchandiseOrderDetailId { get; set; }
        public Guid? ProductId { get; set; }

        public string? PONo { get; set; } = null; // PO Number
        public string? ProductExternalIdSnapShot { get; set; }
        public string? ProductNameSnapShot { get; set; }

        //public string? ManufacturingFormulaExternalIdSnapShot { get; set; }
        public string? LocationExternalIdSnapShot { get; set; } = null;
        public string? MerchandiseOrderExternalIdSnapShot { get; set; } = null;

        public decimal Quantity { get; set; }         // DECIMAL
        public int NumOfBags { get; set; }            // INT

        public bool IsAttach { get; set; } = true;
    }
}
