using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs
{
    public class GetSamplePurchaseOrder
    {
        public Guid PurchaseOrderId { get; set; }
        public string? ExternalId { get; set; }

        public string? MerchediseListExternalId { get; set; }
        public string? Status { get; set; }
        public string? OrderType { get; set; }

        public string? SupplierName { get; set; }   
        public string? SupplierExternalId { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? Comment { get; set; }

        public bool IsActive { get; set; }


        public DateTime? RequestDeliveryDate { get; set; }
        public DateTime? RealDeliveryDate { get; set; }
        public DateTime? CreateDate { get; set; }

        public List<GetSamplePurchaseOrderDetail> Details { get; set; } = new List<GetSamplePurchaseOrderDetail>();
    }
}
