using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders;
using VietausWebAPI.Core.Domain.Enums.Formulas;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas
{
    public class PatchMfgFormula
    {

        public string? noteWhyStandardChanged { get; set; } // Lý do thay đổi tiêu chuẩn
        public Guid ManufacturingFormulaId { get; set; }
        public decimal? MfgTotalPrice { get; set; }


        public Guid? mfgProductionOrderId { get; set; }
        public string? Status { get; set; }
        public decimal? TotalPrice { get; set; }

        public FormulaSource SourceType { get; set; }                     // "FromVA" hoặc "FromVU"

        public Guid? SourceManufacturingFormulaId { get; set; }
        public string? SourceManufacturingExternalIdSnapshot { get; set; } // ví dụ: mã của nguồn lúc copy

        public Guid? SourceVUFormulaId { get; set; }
        public string? SourceVUExternalIdSnapshot { get; set; }

        public bool IsSelect { get; set; }
        public bool IsActive { get; set; }
        public bool IsStandard { get; set; }
        public string? Note { get; set; }

        public DateTime UpdatedDate { get; set; }
        public Guid UpdatedBy { get; set; }

        public virtual ICollection<PatchMfgFormulaMaterial> ManufacturingFormulaMaterials { get; set; } = new List<PatchMfgFormulaMaterial>();
    }
}
