using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs
{
    public class GetPurchaseOrder
    {
        public Guid PurchaseOrderId { get; set; }

        public string? ExternalId { get; set; }

        public string? OrderType { get; set; }

        public Guid? SupplierId { get; set; }

        public string? Comment { get; set; }
        public string? PLPUComment { get; set; }

        public string? Status { get; set; }
        public DateTime? RequestDeliveryDate { get; set; }
        public DateTime? RealDeliveryDate { get; set; }
        public Guid? CompanyId { get; set; }

        public DateTime? CreateDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public string MerchadiseOrderList { get; set; } = string.Empty;

        public List<GetPurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<GetPurchaseOrderDetail>();
        public PurchaseOrderSnapshot PurchaseOrderSnapshot { get; set; } = new PurchaseOrderSnapshot();
    }
}
