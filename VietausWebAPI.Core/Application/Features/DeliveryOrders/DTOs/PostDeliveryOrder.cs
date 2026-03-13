using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseRequest;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs
{
    public class PostDeliveryOrder
    {
        public string? ExternalId { get; set; }
        public string Status { get; set; } = string.Empty; // "Pending", "In Transit", "Delivered", "Cancelled"

        public string? CustomerExternalIdSnapShot { get; set; }
        public Guid CustomerId { get; set; }

        public string? Receiver { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? PaymentType { get; set; }
        public string? PaymentDeadline { get; set; }
        public string? TaxNumber { get; set; }
        public string? PhoneSnapshot { get; set; }

        public bool? RequiresUnloading { get; set; }

        public string? Note { get; set; }
        public bool IsActive { get; set; } = true;

        public Guid CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid CompanyId { get; set; }

        public List<Guid> Deliverers { get; set; } = new List<Guid>();
        public List<Guid> SelectedPOIds { get; set; } = new List<Guid>();
        public List<PostDeliveryOrderDetail> postDeliveryOrderDetails { get; set; } = new List<PostDeliveryOrderDetail>();
    }
}
