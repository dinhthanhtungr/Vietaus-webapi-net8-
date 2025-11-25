using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas
{
    public class GetManufacturingFormulaMaterial
    {
        public Guid ManufacturingFormulaMaterialId { get; set; }

        public Guid ManufacturingFormulaId { get; set; }
        public Guid MaterialId { get; set; }
        public Guid CategoryId { get; set; }

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public string? MaterialNameSnapshot { get; set; }
        public string? MaterialExternalIdSnapshot { get; set; }
        public string? Unit { get; set; }
        public bool IsActive { get; set; } = true;

        // Lấy dữ liệu tồn kho
        public decimal OnHandKg { get; set; }
        public decimal? ReservedOpenAllKg { get; set; }
        public decimal? AvailableKg { get; set; }
    }
}
