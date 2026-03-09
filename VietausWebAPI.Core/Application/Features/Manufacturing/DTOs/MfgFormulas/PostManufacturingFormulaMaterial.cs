using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Formulas;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas
{
    public class PostManufacturingFormulaMaterial
    {
        public Guid ManufacturingFormulaMaterialId { get; set; }

        public Guid ManufacturingFormulaId { get; set; }
        public Guid ItemId { get; set; }
        public ItemType ItemType { get; set; }
        public Guid CategoryId { get; set; }

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        
        public string? MaterialNameSnapshot { get; set; }
        public string? MaterialExternalIdSnapshot { get; set; }
        public string? Unit { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
