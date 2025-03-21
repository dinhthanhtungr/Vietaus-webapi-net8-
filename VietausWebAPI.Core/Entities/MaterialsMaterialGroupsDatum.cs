using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Entities;

public partial class MaterialsMaterialGroupsDatum
{
    public string MaterialGroupId { get; set; } = null!;

    public string MaterialGroupName { get; set; } = null!;

    public string? PartId { get; set; }

    public virtual ICollection<InventoryReceiptsMaterialDatum> InventoryReceiptsMaterialData { get; set; } = new List<InventoryReceiptsMaterialDatum>();

    public virtual PartsCommonDatum? Part { get; set; }

    public virtual ICollection<RequestDetailMaterialDatum> RequestDetailMaterialData { get; set; } = new List<RequestDetailMaterialDatum>();
}
