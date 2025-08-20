using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.ProductInspectionFeature
{
    public class ProductInspectionInformation
    {
        public string? ExternalId { get; set; } // Mã bên ngoài (VD: PI-2023-0001, PI-2023-0002...)
        public string? BatchId { get; set; }
        public Guid? ProductStandardId { get; set; } // Mã tiêu chuẩn sản phẩm
        public string? ProductName { get; set; } // Tên sản phẩm (VD: HẠT COMPOUND...)
        public string? ProductCode { get; set; }
        public int? Weight { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        // I. KIỂM TRA NGOẠI QUAN
        public string? Shape { get; set; }
        public bool? IsShapePass { get; set; }

        public string? ParticleSize { get; set; }
        public bool? IsParticleSizePass { get; set; }

        public string? PackingSpec { get; set; }
        public bool? IsPackingSpecPass { get; set; }

        public bool? VisualCheck { get; set; }

        // II. KIỂM TRA MÀU SẮC
        public string? ColorDeltaE { get; set; }
        public bool? IsColorDeltaEPass { get; set; }

        // III. CHỈ TIÊU KỸ THUẬT
        public string? Moisture { get; set; }
        public bool? IsMoisturePass { get; set; }

        public string? MFR { get; set; }
        public bool? IsMFRPass { get; set; }

        public string? FlexuralStrength { get; set; }
        public bool? IsFlexuralStrengthPass { get; set; }

        public string? Elongation { get; set; }
        public bool? IsElongationPass { get; set; }

        public string? Hardness { get; set; }
        public bool? IsHardnessPass { get; set; }

        public string? Density { get; set; }
        public bool? IsDensityPass { get; set; }

        public string? TensileStrength { get; set; }
        public bool? IsTensileStrengthPass { get; set; }

        public string? FlexuralModulus { get; set; }
        public bool? IsFlexuralModulusPass { get; set; }

        public string? ImpactResistance { get; set; }
        public bool? IsImpactResistancePass { get; set; }

        public string? Antistatic { get; set; }
        public bool? IsAntistaticPass { get; set; }

        public string? StorageCondition { get; set; }
        public bool? IsStorageConditionPass { get; set; }

        public string? MeshType { get; set; }
        public bool? IsMeshAttached { get; set; }

        public bool? DwellTime { get; set; }

        // IV. NGOẠI QUAN ĐẶC BIỆT
        public string? BlackDots { get; set; }
        public bool? MigrationTest { get; set; }
        public bool? Defect_Impurity { get; set; }
        public bool? Defect_BlackDot { get; set; }
        public bool? Defect_ShortFiber { get; set; }
        public bool? Defect_Moist { get; set; }
        public bool? Defect_Dusty { get; set; }
        public bool? Defect_WrongColor { get; set; }

        // V. KẾT LUẬN
        public string? Types { get; set; } // Loại
        public bool? DeliveryAccepted { get; set; }
        public string? machineId { get; set; }

        public string? Notes { get; set; }

        public string? CreatedBy { get; set; } // Người kiểm
    }
}
