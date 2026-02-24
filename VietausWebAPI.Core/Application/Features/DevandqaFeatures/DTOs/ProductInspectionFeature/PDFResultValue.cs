using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.ProductInspectionFeature
{
    public class PDFResultValue
    {
        // Product Inspection
        public string? ExternalId { get; set; } // Mã bên ngoài (VD: PI-2023-0001, PI-2023-0002...)
        public string? BatchId { get; set; }
        public string? ProductName { get; set; } // Tên sản phẩm (VD: HẠT COMPOUND...)
        public string? ProductCode { get; set; }
        public string? bagType { get; set; } // Loại bao bì (VD: Bao Jumbo, Bao PE, Bao giấy...)
        public int? Weight { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public string? Shape { get; set; }
        public string? ParticleSize { get; set; }
        public string? PackingSpec { get; set; }
        public string? ColorDeltaE { get; set; }
        public string? Moisture { get; set; }
        public string? MFR { get; set; }
        public string? FlexuralStrength { get; set; }
        public string? Elongation { get; set; }
        public string? Hardness { get; set; }
        public string? Density { get; set; }
        public string? TensileStrength { get; set; }
        public string? FlexuralModulus { get; set; }
        public string? ImpactResistance { get; set; }
        public string? Antistatic { get; set; }
        public string? StorageCondition { get; set; }
        public bool? DwellTime { get; set; }
        public string? BlackDots { get; set; }
        public bool? MigrationTest { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
