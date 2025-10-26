using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs
{
    public class GetSamplePurchaseOrderDetail
    {
        public string? MaterialExternalIDSnapshot { get; set; }
        public string? MaterialNameSnapshot { get; set; }

        public decimal? UnitPriceAgreed { get; set; }

        public decimal? RequestQuantity { get; set; }
        public decimal? RealQuantity { get; set; }
    }
}
