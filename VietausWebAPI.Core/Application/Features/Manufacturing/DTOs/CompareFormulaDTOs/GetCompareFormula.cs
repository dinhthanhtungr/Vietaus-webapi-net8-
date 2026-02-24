using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.CompareFormulaDTOs
{
    public class GetCompareFormula
    {
        public Guid Id { get; set; }
        public string? ExternalId { get; set; }
        public string? ColourCode { get; set; }
        public virtual ICollection<GetSampleMfgFormulaMaterial> ManufacturingFormulaMaterials { get; set; } = new List<GetSampleMfgFormulaMaterial>();
    }
}
