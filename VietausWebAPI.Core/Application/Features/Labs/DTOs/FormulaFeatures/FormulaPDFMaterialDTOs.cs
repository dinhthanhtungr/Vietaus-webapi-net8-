using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures
{
    public class FormulaPDFMaterialDTOs
    {
        public string ExternalId { get; set; } = string.Empty;
        public string MaterialName { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public decimal Quantity { get; set; }
    }
}
