using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductStandardFeature
{
    public class ProductStandardInformation
    {
        //public Guid Id { get; set; }
        public Guid? ProductStandardId { get; set; } // Id của tiêu chuẩn sản phẩm (nếu có)
        public string? ExternalId { get; set; }
        public string? ProductExternalId { get; set; }
        public string? Status { get; set; }
        public string? DeltaE { get; set; }
        public string? PelletSize { get; set; }
        public string? Moisture { get; set; }
        public string? Density { get; set; }
        public string? MeltIndex { get; set; }
        public string? TensileStrength { get; set; }
        public string? ElongationAtBreak { get; set; }
        public string? FlexuralStrength { get; set; }
        public string? FlexuralModulus { get; set; }
        public string? IzodImpactStrength { get; set; }
        public string? Hardness { get; set; }
        public string? DwellTime { get; set; }
        public string? BlackDots { get; set; }
        public string? Shape { get; set; }
        public string? MigrationTest { get; set; }
        public Guid? ProductId { get; set; }
        public string? Package { get; set; }
        public int? weight { get; set; }
        public string? customerExternalId { get; set; }
        public string? colourCode { get; set; }


        public Guid CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
