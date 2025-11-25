using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Shared.Models.SaleAndMfgs
{
    public sealed class OrderDetailSlim
    {
        public Guid MerchandiseOrderDetailId { get; set; }
        public Guid ProductId { get; set; }

        public Guid FormulaId { get; set; }
        public string? FormulaExternalIdSnapshot { get; set; }

        public decimal ExpectedQuantity { get; set; }
        public decimal? UnitPriceAgreed { get; set; }

        public DateTime? DeliveryRequestDate { get; set; }
        public string? Comment { get; set; }
        public string? BagType { get; set; }
    }
}
