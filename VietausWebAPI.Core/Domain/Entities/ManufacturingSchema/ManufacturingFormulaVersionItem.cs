using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;

namespace VietausWebAPI.Core.Domain.Entities.ManufacturingSchema
{
    public class ManufacturingFormulaVersionItem
    {
        public Guid ManufacturingFormulaVersionItemId { get; set; }
        public Guid ManufacturingFormulaVersionId { get; set; }

        public Guid MaterialId { get; set; }              // để tính MRP/đối chiếu
        public Guid CategoryId { get; set; }

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        // snapshot hiển thị (không đổi nghĩa)
        public string MaterialNameSnapshot { get; set; } = default!;
        public string MaterialExternalIdSnapshot { get; set; } = default!;
        public string Unit { get; set; } = default!;

        public ManufacturingFormulaVersion Version { get; set; } = null!;
        public virtual Material Material { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
    }
}
