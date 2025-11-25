using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas
{
    public class FmItemRow
    {
        public Guid MaterialId { get; init; }
        public Guid? CategoryId { get; init; }
        public decimal Quantity { get; init; }
        public string Unit { get; init; } = "";
        public decimal? UnitPrice { get; init; }
        public string? MaterialNameSnapshot { get; init; }
        public string? MaterialExternalIdSnapshot { get; init; }
    }
}
