namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.ProductInspectionFeature
{
    public class PatchProductInspectionRequest
    {
        public string? BatchId { get; set; }
        public Guid? ProductStandardId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public decimal? Weight { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public string? Shape { get; set; }
        public bool? IsShapePass { get; set; }

        public string? ParticleSize { get; set; }
        public bool? IsParticleSizePass { get; set; }

        public string? PackingSpec { get; set; }
        public bool? IsPackingSpecPass { get; set; }

        public bool? VisualCheck { get; set; }

        public string? ColorDeltaE { get; set; }
        public bool? IsColorDeltaEPass { get; set; }

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

        public string? IntrinsicViscosity { get; set; }
        public bool? IsIntrinsicViscosity { get; set; }

        public string? MeshType { get; set; }
        public bool? IsMeshAttached { get; set; }

        public bool? DwellTime { get; set; }

        public string? BlackDots { get; set; }
        public bool? MigrationTest { get; set; }
        public bool? Defect_Impurity { get; set; }
        public bool? Defect_BlackDot { get; set; }
        public bool? Defect_ShortFiber { get; set; }
        public bool? Defect_Moist { get; set; }
        public bool? Defect_Dusty { get; set; }
        public bool? Defect_WrongColor { get; set; }

        public string? Types { get; set; }
        public bool? DeliveryAccepted { get; set; }
        public string? Notes { get; set; }
    }
}
