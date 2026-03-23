using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures
{
    public class GetFormula
    {
        public Guid FormulaId { get; set; }

        public string? ExternalId { get; set; }
        public string? Note { get; set; }
        public string? Name { get; set; }
        public string? ProductCode { get; set; }

        // Trạng thái hiện tại của công thức
        public string Status { get; set; }

        public decimal? TotalPrice { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public decimal? ProductionPrice { get; set; }
        public decimal? PresidentPrice { get; set; }
        public decimal? ProfitMarginPrice { get; set; }

        public DateTime? CheckDate { get; set; }       // DATETIME
        public string? CheckNameSnapshot { get; set; }  // NVARCHAR
        public DateTime? SentDate { get; set; }       // DATETIME
        public string? SentByNameSnapshot { get; set; }  // NVARCHAR

        public DateTime? CreatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public bool? IsSelect { get; set; }

        public List<GetMaterialFormula> materialFormulas { get; set; } = new List<GetMaterialFormula>();
    }
}
