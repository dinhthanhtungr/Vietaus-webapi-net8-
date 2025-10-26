using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs
{
    public class GetPurchaseOrderDetail
    {
        public Guid PurchaseOrderDetailId { get; set; }

        public Guid PurchaseOrderId { get; set; }

        //public Guid? ExternalId { get; set; }
        public string? Package { get; set; }

        public decimal? RequestQuantity { get; set; }
        public decimal? RealQuantity { get; set; }

        public decimal? BaseCostSnapshot { get; set; }
        public DateTime? BaseDateSnapshot { get; set; }

        public decimal? UnitPriceAgreed { get; set; }
        public decimal? TotalPriceAgreed { get; set; }

        public Guid MaterialId { get; set; }
        public string? MaterialExternalIDSnapshot { get; set; }
        public string? MaterialNameSnapshot { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public string? Note { get; set; }

    }
}
