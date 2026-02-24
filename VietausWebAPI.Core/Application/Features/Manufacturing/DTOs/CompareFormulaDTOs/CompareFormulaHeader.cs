using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Formulas;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.CompareFormulaDTOs
{
    public sealed class CompareFormulaHeader
    {
        public FormulaSource Source { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid Id { get; set; }
        public string? ExternalId { get; set; }
        public string? ColourCode { get; set; }
    }
}
