using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders
{
    public class GetManufacturingFormulaMaterial
    {
        public Guid ManufacturingFormulaMaterialId { get; set; }
        public Guid ManufacturingFormulaId { get; set; }

        public Guid MaterialId { get; set; }
        public Guid CategoryId { get; set; }

        public decimal? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalPrice { get; set; }

        public string? LotNo { get; set; } = string.Empty;
        public Guid? StockId { get; set; }

        public string? MaterialNameSnapshot { get; set; }         // NVARCHAR
        public string? MaterialExternalIdSnapshot { get; set; }   // VARCHAR
        public string? Unit { get; set; }                         // VARCHAR
        public bool? IsActive { get; set; }
    }
}
