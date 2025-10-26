using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Entities
{
    public partial class ManufacturingFormulaMaterial
    {
        public Guid ManufacturingFormulaMaterialId { get; set; }

        public Guid ManufacturingFormulaId { get; set; }
        public Guid MaterialId { get; set; }
        public Guid CategoryId { get; set; }

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public string? LotNo { get; set; } = string.Empty;
        public Guid? StockId { get; set; }

        public string? MaterialNameSnapshot { get; set; }
        public string? MaterialExternalIdSnapshot { get; set; }
        public string? Unit { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ManufacturingFormula ManufacturingFormula { get; set; } = null!;
        public virtual Material Material { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;

    }
}
