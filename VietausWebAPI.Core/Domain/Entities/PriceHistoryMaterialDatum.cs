using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class PriceHistoryMaterialDatum
{
    public Guid PriceHistoryId { get; set; }

    public Guid MaterialId { get; set; }

    public Guid SupplierId { get; set; }

    public decimal? OldPrice { get; set; }

    public decimal? NewPrice { get; set; }

    public string? Currency { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Reason { get; set; }

    public virtual MaterialsMaterialDatum Material { get; set; } = null!;

    public virtual ICollection<MaterialsSuppliersMaterialDatum> MaterialsSuppliersMaterialData { get; set; } = new List<MaterialsSuppliersMaterialDatum>();

    public virtual SuppliersMaterialDatum Supplier { get; set; } = null!;

    public virtual EmployeesCommonDatum? UpdatedByNavigation { get; set; }
}
