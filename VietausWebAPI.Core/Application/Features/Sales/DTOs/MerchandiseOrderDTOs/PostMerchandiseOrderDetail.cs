using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs
{
    public class PostMerchandiseOrderDetail
    {
        public Guid ProductId { get; set; }
        public string? ProductExternalIdSnapshot { get; set; }
        public string? ProductNameSnapshot { get; set; }
        public string? ProductionType { get; set; }

        public Guid FormulaId { get; set; }
        public string? FormulaExternalIdSnapshot { get; set; }

        public decimal ExpectedQuantity { get; set; }
        public decimal? RealQuantity { get; set; }

        public string? BagType { get; set; }
        public string? PackageWeight { get; set; }

        public string? Status { get; set; }

        public string? Comment { get; set; }

        public DateTime? DeliveryRequestDate { get; set; }
        public DateTime? DeliveryActualDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }

        public decimal BaseCostSnapshot { get; set; }
        public decimal RecommendedUnitPrice { get; set; }
        public decimal UnitPriceAgreed { get; set; }
        public decimal TotalPriceAgreed { get; set; }

    }
}
