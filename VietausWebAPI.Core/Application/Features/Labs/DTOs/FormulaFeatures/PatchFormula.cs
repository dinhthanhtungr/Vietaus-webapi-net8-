using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures
{
    public class PatchFormula
    {
        public Guid? FormulaMaterialId { get; set; }
        public Guid FormulaId { get; set; }

        public string? ExternalId { get; set; }
        public string? Note { get; set; }
        public string? Name { get; set; }
        public string? ProductCode { get; set; }
        // Trạng thái hiện tại của công thức
        public string Status { get; set; } = "Draft";
        public decimal? TotalPrice { get; set; }
        public string? CreatedByName { get; set; }

        public DateTime? EffectiveDate { get; set; }
        public decimal? ProductionPrice { get; set; }
        public decimal? PresidentPrice { get; set; }
        public decimal? ProfitMarginPrice { get; set; }

        public bool? IsSelect { get; set; }
        public Guid? UpdatedBy { get; set; }
        public List<PatchMaterialFormula> materialFormulas { get; set; } = new List<PatchMaterialFormula>();
    }
}
