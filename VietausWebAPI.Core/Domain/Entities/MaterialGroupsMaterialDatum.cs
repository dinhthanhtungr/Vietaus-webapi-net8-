using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class MaterialGroupsMaterialDatum
{
    public Guid MaterialGroupId { get; set; }

    public string? ExternalId { get; set; }

    public string? MaterialGroupName { get; set; }

    public string? Detail { get; set; }

    public virtual ICollection<InventoryReceiptsMaterialDatum> InventoryReceiptsMaterialData { get; set; } = new List<InventoryReceiptsMaterialDatum>();

    public virtual ICollection<MaterialsMaterialDatum> MaterialsMaterialData { get; set; } = new List<MaterialsMaterialDatum>();

    public virtual ICollection<RequestDetailMaterialDatum> RequestDetailMaterialData { get; set; } = new List<RequestDetailMaterialDatum>();
}
