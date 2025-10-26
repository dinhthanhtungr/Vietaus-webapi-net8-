using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material
{
    public class GetMaterialSummary
    {
        public Guid MaterialId { get; set; }
        public string? ExternalId { get; set; }

        public string? CustomCode { get; set; }

        public string? Name { get; set; }
        public string? Price { get; set; }
        public string? Category { get; set; }
        public Guid? CategoryId { get; set; } = null;
        public double? Weight { get; set; }
        public string? Package { get; set; }
        public string? Unit { get; set; }

        //public decimal? QuantityStock { get; set; }
    }
}
