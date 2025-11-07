using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.Services.MerchandiseOrderFeatures;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders
{
    public class MfgContext
    {
        public Dictionary<Guid, (Guid Id, string ColourCode, string Name, Guid CategoryId)> Products { get; init; } = default!;
        public Dictionary<Guid, (Guid VaId, string VaCode, string VuCode)> StandardVaByVu { get; init; } = default!;
        public Dictionary<Guid, List<FmItemRow>> FmItemsByVu { get; init; } = default!;
        public Dictionary<Guid, List<FmItemRow>> FmItemsByVa { get; init; } = default!;
        public Dictionary<Guid, bool> VuHasAnyVa { get; init; } = default!;
        public Dictionary<Guid, decimal> PriceMap { get; init; } = default!;
    }
}
