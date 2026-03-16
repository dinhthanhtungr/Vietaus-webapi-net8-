using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures
{
    public class GetSampleFormula
    {
        public Guid FormulaId { get; set; }

        public string? ExternalId { get; set; }
        public string? Note { get; set; }
        public string? Name { get; set; }
        public string? ProductCode { get; set; }
        public bool? IsSelect { get; set; }

        // Trạng thái hiện tại của công thức
        public string Status { get; set; }
        public decimal? TotalPrice { get; set; }

        public DateTime? EffectiveDate { get; set; }
        public decimal? ProductionPrice { get; set; }
        public decimal? PresidentPrice { get; set; }
    }
}
