using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseRequest;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs
{
    public class PdfPrinterDeliveryOrder
    {
        public Guid Id { get; set; }
        public string? ExternalId { get; set; }
        public string? MerchandiseOrderExternalIdList { get; set; }

        public Guid CustomerId { get; set; }
        public string? CustomerExternalIdSnapShot { get; set; }

        public string? CustomerName { get; set; }
        public string? CustomerAddress { get; set; }

        public string? Receiver { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? PaymentType { get; set; }
        public string? PaymentDeadline { get; set; }
        public string? TaxNumber { get; set; }
        public string? PhoneSnapshot { get; set; }

        public string Status { get; set; } = string.Empty; // "Pending", "In Transit", "Delivered", "Cancelled"

        public string? Note { get; set; }
        public bool IsActive { get; set; } = true;
        public bool HasPrinted { get; set; } = false;

        public Guid CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid CompanyId { get; set; }

        //public ICollection<GetDeliveryOrderDetail> Details { get; set; } = new List<GetDeliveryOrderDetail>();
        public ICollection<PdfPrinterDeliveryOrderDetail> Details { get; set; } = new List<PdfPrinterDeliveryOrderDetail>();
        public ICollection<GetDeliveryOrderDetail> DeliveryOrderDetails { get; set; } = new List<GetDeliveryOrderDetail>();
        public List<GetDeliverer> Deliverers { get; set; } = new List<GetDeliverer>();
    }
}
