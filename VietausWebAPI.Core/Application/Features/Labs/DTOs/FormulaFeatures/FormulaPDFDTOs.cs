using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures
{
    public class FormulaPDFDTOs
    {
        public string colourCode { get; set; } = string.Empty;
        public string productName { get; set; } = string.Empty; 
        public string FormulaExternalId { get; set; } = string.Empty;
        public double AddRate { get; set; }
        public string? Note { get; set; } = string.Empty;
        public string? Requirement { get; set; } = string.Empty;

        public DateTime RequestDate { get; set; }
        public List<FormulaPDFMaterialDTOs> materials { get; set; } = new List<FormulaPDFMaterialDTOs>();
    }
}
