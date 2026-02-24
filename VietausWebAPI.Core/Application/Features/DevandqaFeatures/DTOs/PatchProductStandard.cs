using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs
{
    public class PatchProductStandard
    {
        public Guid? ProductStandardId { get; set; } // Id của tiêu chuẩn sản phẩm (nếu có)
        public string? externalId { get; set; } // Mã tiêu chuẩn sản phẩm, có thể là null nếu không có mã
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
        public string? Shape { get; set; } // "Hạt", "Viên", "Bột", "Mảnh", "Khác"
        public string? DwellTime { get; set; }
        public string? BlackDots { get; set; }
        public string? MigrationTest { get; set; }

        public Guid ProductId { get; set; }
        public string? colourCode { get; set; } // Mã màu của sản phẩm, có thể là null nếu không có mã màu
        public string? customerExternalId { get; set; } // Mã khách hàng, có thể là null nếu không có mã khách hàng
        public string? packed { get; set; }
        public int Weight { get; set; }
        public Guid CompanyId { get; set; }                  // "2cf1f439-e077-f1bc-1037-652eb22db524"

        public DateTime CreatedDate { get; set; } // Fixed: Removed parentheses from DateTime.Now
    }

}
