using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs
{
    public class GetPODeliveryOrder
    {
        public Guid MerchandiseOrderId { get; set; }
        public string? ExternalId { get; set; }


        public Guid? CustomerId { get; set; }
        public string? CustomerNameSnapshot { get; set; }
        public string? CustomerExternalIdSnapshot { get; set; }
        public string? PhoneSnapshot { get; set; }

        public string? Note { get; set; }
        public string? ShippingMethod { get; set; }
        public string? PONo { get; set; }

        public string? Receiver { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? PaymentType { get; set; }
        public decimal? DeliveryPrice { get; set; }

        public string? Status { get; set; }
        public string? Currency { get; set; }

        public List<GetPODetailDeliveryOrder> PODetailDeliveryOrder { get; set; } = new List<GetPODetailDeliveryOrder>();
    }
}
