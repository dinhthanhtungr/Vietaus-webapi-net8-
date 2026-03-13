using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.ManufacturingVUFormulaFeatures
{
    public class GetManufacturingVUFormula
    {
        public Guid ManufacturingVUFormulaId { get; set; }
        public Guid FormulaId { get; set; }

        public string? SourceVUExternalId { get; set; }
        public string? FormulaExternalId { get; set; }
        public decimal? TotalProductionQuantity { get; set; }
        public int? NumOfBatches { get; set; }

        public string? QcCheck { get; set; }

        public string? ColourCode { get; set; }
        public string? Name { get; set; }
        public string? CustomerCode { get; set; }
        public string? CustomerName { get; set; }

        public double? userRate { get; set; }

        public string? LabNote { get; set; }
        public string? Requirement { get; set; }
        public string? PlpuNote { get; set; }

        public List<GetFormulaMaterialSnapshot> MaterialSnapshots { get; set; } = new List<GetFormulaMaterialSnapshot>();
    }
}
