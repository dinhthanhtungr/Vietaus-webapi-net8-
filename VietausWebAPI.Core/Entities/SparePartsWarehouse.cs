using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class SparePartsWarehouse
{
    public string SparePartId { get; set; } = null!;

    public string? SystemGroupId { get; set; }

    public string? MaterialGroupId { get; set; }

    public string? MaterialId { get; set; }

    public string? MaterialName { get; set; }

    public string? MaterialParameter { get; set; }

    public int? Size { get; set; }

    public string? Unit { get; set; }

    public int? Quantity { get; set; }

    public string? Location { get; set; }

    public int? MinStockLevel { get; set; }

    public string? Note { get; set; }

    public virtual Material? Material { get; set; }

    public virtual MaterialGroup? MaterialGroup { get; set; }

    public virtual ICollection<SparePartsWarehouseHistory> SparePartsWarehouseHistories { get; set; } = new List<SparePartsWarehouseHistory>();

    public virtual SystemGroup? SystemGroup { get; set; }
}
