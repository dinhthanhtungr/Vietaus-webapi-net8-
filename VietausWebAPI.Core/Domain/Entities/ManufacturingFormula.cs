using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Entities
{
    public partial class ManufacturingFormula
    {
        public Guid ManufacturingFormulaId { get; set; }
        public string? ExternalId { get; set; }
        public string? MfgProductionOrderExternalId { get; set; }
        public string? Name { get; set; }

        public Guid mfgProductionOrderId { get; set; }
        public string? Status { get; set; }
        public decimal? TotalPrice { get; set; }

        public Guid VUFormulaId { get; set; }
        public string FormulaExternalIdSnapshot { get; set; } = string.Empty;

        public Guid? SourceManufacturingFormulaId { get; set; }
        public string? SourceManufacturingExternalIdSnapshot { get; set; } // ví dụ: mã của nguồn lúc copy

        public Guid? SourceVUFormulaId { get; set; }
        public string? SourceVUExternalIdSnapshot { get; set; }

        public string? SourceType { get; set; }                     // "FromVA" hoặc "FromVU"

        public bool IsSelect { get; set; }
        public bool IsActive { get; set; }
        public bool IsStandard { get; set; }
        public string? Note { get; set; }

        public DateTime? createdDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Guid? companyId { get; set; }


        public virtual ICollection<ManufacturingFormulaMaterial> ManufacturingFormulaMaterials { get; set; } = new List<ManufacturingFormulaMaterial>();
        public ManufacturingFormula? SourceManufacturingFormula { get; set; }
        public Formula? SourceVUFormula { get; set; }

        public virtual Formula? VUFormula { get; set; }
        public virtual MfgProductionOrder? MfgProductionOrder { get; set; }
        public virtual Employee? CreatedByNavigation { get; set; }
        public virtual Employee? UpdatedByNavigation { get; set; }
        public virtual ICollection<ManufacturingFormulaLog> ManufacturingFormulaLogs { get; set; } = new List<ManufacturingFormulaLog>();
        public virtual Company? Company { get; set; }
    }
}
