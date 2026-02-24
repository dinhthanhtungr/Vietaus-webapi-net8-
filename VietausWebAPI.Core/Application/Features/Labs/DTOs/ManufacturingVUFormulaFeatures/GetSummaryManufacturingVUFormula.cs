using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.ManufacturingVUFormulaFeatures
{
    public class GetSummaryManufacturingVUFormula
    {
        public Guid ManufacturingVUFormulaId { get; set; }
        public string? VUFormula { get; set; }

        public string? ProductName { get; set; }
        public string? ColourCode { get; set; }

        public decimal? TotalProductionQuantity { get; set; }
        public int? NumOfBatches { get; set; }
        
        public string? CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
