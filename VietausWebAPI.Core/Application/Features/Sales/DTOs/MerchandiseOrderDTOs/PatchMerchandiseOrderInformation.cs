using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs
{
    public class PatchMerchandiseOrderInformation
    {
        public Guid MerchandiseOrderId { get; set; }

        public string? CustomerNameSnapshot { get; set; }
        public string? CustomerExternalIdSnapshot { get; set; }
        public string? PhoneSnapshot { get; set; }

        public string? Receiver { get; set; }
        public string? DeliveryAddress { get; set; }

        public string? PaymentType { get; set; }

        public decimal? Vat { get; set; }

        public string? Status { get; set; }
        public string? Currency { get; set; }

        public DateTime? PaymentDate { get; set; }
        //public DateTime? DeliveryRequestDate { get; set; }
        //public DateTime? DeliveryActualDate { get; set; }
        //public DateTime? ExpectedDeliveryDate { get; set; }

        public string? Note { get; set; }
        public string? ShippingMethod { get; set; }
        public string? PONo { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
