using System;
using System.Collections.Generic;

 namespace VietausWebAPI.Core.Domain.Entities;

public partial class FormulaMaterial
{
    public Guid FormulaMaterialId { get; set; }

    public Guid FormulaId { get; set; }

    public Guid MaterialId { get; set; }

    public string? MaterialType { get; set; }

    public double? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? LotNo { get; set; }

    public Guid? SelectedSupplierId { get; set; }

    public virtual Formula Formula { get; set; } = null!;

    public virtual Material Material { get; set; } = null!;

    public virtual Supplier? SelectedSupplier { get; set; }
}
