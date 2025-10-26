using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs
{
    public class PostPurchaseOrder
    {
        public Guid PurchaseOrderId { get; set; }

        public string? ExternalId { get; set; }
        public string? OrderType { get; set; }

        public Guid? SupplierId { get; set; }
        public DateTime? RequestDeliveryDate { get; set; }
        public DateTime? RealDeliveryDate { get; set; }
        public string? Comment { get; set; }
        public string? PLPUComment { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? CreatedBy { get; set; }

        public List<Guid> MerchadiseOrderIds { get; set; } = new List<Guid>();
        public List<PostPurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PostPurchaseOrderDetail>();
        public PurchaseOrderSnapshot PurchaseOrderSnapshot { get; set; } = new PurchaseOrderSnapshot();
    }
}
