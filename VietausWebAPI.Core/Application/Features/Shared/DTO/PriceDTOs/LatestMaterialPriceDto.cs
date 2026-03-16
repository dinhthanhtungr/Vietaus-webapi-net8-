using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Shared.DTO.PriceDTOs
{
    public sealed class LatestMaterialPriceDto
    {
        public Guid MaterialId { get; set; }
        public decimal CurrentPrice { get; set; }

        public DateTime? PriceDate { get; set; }
        public MaterialPriceSource PriceSource { get; set; }
    }
    public enum MaterialPriceSource
    {
        Unknown = 0,
        PurchaseOrder = 1,
        MaterialSupplier = 2
    }
}
