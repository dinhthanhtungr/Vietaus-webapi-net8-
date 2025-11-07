using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Formulas;
using VietausWebAPI.Core.Domain.Enums.Manufacturings;

namespace VietausWebAPI.Core.Domain.Entities.ManufacturingSchema
{
    public partial class ManufacturingFormula
    {
        public Guid ManufacturingFormulaId { get; set; }
        public string ExternalId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string Status { get; set; } = ManufacturingProductOrderFormula.New.ToString();
        public decimal? TotalPrice { get; set; }

        public Guid VUFormulaId { get; set; }
        public string FormulaExternalIdSnapshot { get; set; } = string.Empty;


        // Nguồn gốc (đa hình 1-trong-2)
        public Guid? SourceManufacturingFormulaId { get; set; }
        public string? SourceManufacturingExternalIdSnapshot { get; set; }
        public Guid? SourceVUFormulaId { get; set; }
        public string? SourceVUExternalIdSnapshot { get; set; }
        public FormulaSource SourceType { get; set; } // FromVA / FromVU (không nullable)


        public bool IsActive { get; set; }
        public string? Note { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public Guid CompanyId { get; set; }


        public virtual ICollection<ManufacturingFormulaMaterial> ManufacturingFormulaMaterials { get; set; } = new List<ManufacturingFormulaMaterial>();

        public ManufacturingFormula? SourceManufacturingFormula { get; set; }
        public Formula? SourceVUFormula { get; set; }
        public virtual Formula? VUFormula { get; set; }
        public virtual Employee? CreatedByNavigation { get; set; }
        public virtual Employee? UpdatedByNavigation { get; set; }
        public virtual Company? Company { get; set; }
    }
}
