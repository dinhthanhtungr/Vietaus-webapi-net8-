using System;
using System.Collections.Generic;

 namespace VietausWebAPI.Core.Domain.Entities;

public partial class ProductStandard1
{
    public Guid ProductStandardId { get; set; }

    public string? ExternalId { get; set; }

    public Guid? ProductId { get; set; }

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

    public string? MigrationTest { get; set; }

    public Guid? CompanyId { get; set; }

    public DateTime? CreateDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public string? Package { get; set; }

    public int? Weight { get; set; }

    public string? CustomerExternalId { get; set; }

    public string? ColourCode { get; set; }

    public string? Shape { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Employee? UpdatedByNavigation { get; set; }
}
