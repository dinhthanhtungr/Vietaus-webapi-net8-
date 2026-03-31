using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.ManufacturingVUFormulaFeatures
{
    public class ManufacturingVUPDF
    {
        public string BatchNo { get; set; } = string.Empty;
        public DateTime? RequestDate { get; set; }
        public string BagType { get; set; } = string.Empty;

        public string? FormulaSelectList { get; set; }
        public DateTime CreatedDate { get; set; }
        public GetManufacturingVUFormula getManufacturingVUFormula { get; set; } = new GetManufacturingVUFormula();
        public List<FormulaPDFMaterialDTOs> materials { get; set; } = new List<FormulaPDFMaterialDTOs>();

    }
}
