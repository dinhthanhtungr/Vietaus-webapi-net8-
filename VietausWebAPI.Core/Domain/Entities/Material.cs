using System;
using System.Collections.Generic;

 namespace VietausWebAPI.Core.Domain.Entities;

public partial class Material
{
    public Guid MaterialId { get; set; }

    public string? ExternalId { get; set; }

    public string? CustomCode { get; set; }

    public string? Name { get; set; }

    public Guid CategoryId { get; set; }

    public double? Weight { get; set; }

    public Guid UnitId { get; set; }

    public string? Package { get; set; }

    public string? Comment { get; set; }

    public double? MinQuantity { get; set; }

    public Guid CompanyId { get; set; }

    public bool? IsActive { get; set; }

    public string? Barcode { get; set; }

    public string? ImagePath { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual ICollection<FormulaMaterial> FormulaMaterials { get; set; } = new List<FormulaMaterial>();

    public virtual ICollection<MaterialsSupplier> MaterialsSuppliers { get; set; } = new List<MaterialsSupplier>();

    public virtual ICollection<PriceHistory> PriceHistories { get; set; } = new List<PriceHistory>();

    public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();

    public virtual ICollection<RequestDetail> RequestDetails { get; set; } = new List<RequestDetail>();

    public virtual Unit Unit { get; set; } = null!;

    public virtual Employee? UpdatedByNavigation { get; set; }
}
