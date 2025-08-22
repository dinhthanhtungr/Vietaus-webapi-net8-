using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class SuppliersMaterialDatum
{
    public Guid SupplierId { get; set; }

    public string? ExternalId { get; set; }

    public string? Name { get; set; }

    public string? RegNo { get; set; }

    public string? TaxNo { get; set; }

    public string? Phone { get; set; }

    public string? Website { get; set; }

    public virtual ICollection<MaterialsSuppliersMaterialDatum> MaterialsSuppliersMaterialData { get; set; } = new List<MaterialsSuppliersMaterialDatum>();

    public virtual ICollection<PriceHistoryMaterialDatum> PriceHistoryMaterialData { get; set; } = new List<PriceHistoryMaterialDatum>();

    public virtual ICollection<PurchaseOrdersMaterialDatum> PurchaseOrdersMaterialData { get; set; } = new List<PurchaseOrdersMaterialDatum>();
}
