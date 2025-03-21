using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class MaterialSuppliersMaterialDatum
{
    public string SupplierId { get; set; } = null!;

    public string SupplierName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<InventoryReceiptsMaterialDatum> InventoryReceiptsMaterialData { get; set; } = new List<InventoryReceiptsMaterialDatum>();
}
