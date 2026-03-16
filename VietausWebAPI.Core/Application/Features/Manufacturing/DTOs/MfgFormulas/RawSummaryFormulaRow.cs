using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas
{
    public class RawSummaryFormulaRow
    {
        public Guid FormulaId { get; set; }
        public string? ExternalId { get; set; }
        public string? Name { get; set; }
        public DateTime? FormulaSourceIdCreatedDate { get; set; }
        public List<RawSummaryFormulaMaterialRow> Materials { get; set; } = new();
    }
}
