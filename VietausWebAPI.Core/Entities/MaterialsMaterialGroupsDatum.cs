using System;
using System.Collections.Generic;

namespace VietausWebAPI.Infrastructure.Models;

public partial class MaterialsMaterialGroupsDatum
{
    public string MaterialGroupId { get; set; } = null!;

    public string MaterialGroupName { get; set; } = null!;

    public virtual ICollection<InventoryReceiptsMaterialDatum> InventoryReceiptsMaterialData { get; set; } = new List<InventoryReceiptsMaterialDatum>();
}
