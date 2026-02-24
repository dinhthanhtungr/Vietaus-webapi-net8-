using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.ManufacturingVUFormulaFeatures
{
    public class PostManufacturingVUFormula
    {
        public Guid FormulaId { get; set; }

        public decimal? TotalProductionQuantity { get; set; }
        public int? NumOfBatches { get; set; }
        public string? LabNote { get; set; }
        public string? Requirement { get; set; }
        public string? QcCheck { get; set; }
    }
}
