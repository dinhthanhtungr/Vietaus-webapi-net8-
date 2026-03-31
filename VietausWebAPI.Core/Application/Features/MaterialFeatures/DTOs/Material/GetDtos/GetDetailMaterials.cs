using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material.GetDtos
{
    public class GetDetailMaterials
    {
        public string SupplierExternalId { get; set; } = null!;
        public string SupplierName { get; set; } = null!;

        public decimal? LastPrice { get; set; }
        public DateTime? LastPriceDate { get; set; }

        public List<GetPriceHistory> PriceHistories { get; set; } = new List<GetPriceHistory>();
    }
}
