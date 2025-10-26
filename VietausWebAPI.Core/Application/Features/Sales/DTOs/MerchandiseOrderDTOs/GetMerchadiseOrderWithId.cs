using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs
{
    public class GetMerchadiseOrderWithId
    {
        public Guid MerchandiseOrderId { get; set; }
        public string? ExternalId { get; set; }

        public Guid? CustomerId { get; set; }
        public string? CustomerNameSnapshot { get; set; }
        public string? CustomerExternalIdSnapshot { get; set; }
        public string? PhoneSnapshot { get; set; }

        public Guid? ManagerById { get; set; }
        public string? ManagerByNameSnapshot { get; set; }
        public string? ManagerExternalIdSnapshot { get; set; }

        public string? Receiver { get; set; }
        public string? DeliveryAddress { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? PaymentType { get; set; }

        public decimal? Vat { get; set; }
        public string? Status { get; set; }

        public DateTime? PaymentDate { get; set; }
        //public DateTime? DeliveryRequestDate { get; set; }
        //public DateTime? DeliveryActualDate { get; set; }
        //public DateTime? ExpectedDeliveryDate { get; set; }

        public string? Note { get; set; }
        public string? ShippingMethod { get; set; }
        public string? PONo { get; set; }


        public DateTime? CreateDate { get; set; }
        public Guid? CreatedBy { get; set; }

        public List<GetMerchandiseOrderDetail> merchandiseOrderDetails { get; set; } = new List<GetMerchandiseOrderDetail>();
        public GetCustomer customer { get; set; } = new GetCustomer();
    }
}
