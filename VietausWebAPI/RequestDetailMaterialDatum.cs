using System;
using System.Collections.Generic;

namespace VietausWebAPI.WebAPI;

public partial class RequestDetailMaterialDatum
{
    public int DetailId { get; set; }

    public string RequestId { get; set; } = null!;

    public int? RequestedQuantity { get; set; }

    public Guid MaterialId { get; set; }

    public string? Note { get; set; }

    public int? PurchasedQuantity { get; set; }

    public int? ReceivedQuantity { get; set; }

    public virtual ICollection<InventoryReceiptsMaterialDatum> InventoryReceiptsMaterialData { get; set; } = new List<InventoryReceiptsMaterialDatum>();

    public virtual MaterialsMaterialDatum Material { get; set; } = null!;

    public virtual ICollection<PurchaseOrderDetailsMaterialDatum> PurchaseOrderDetailsMaterialData { get; set; } = new List<PurchaseOrderDetailsMaterialDatum>();

    public virtual SupplyRequestsMaterialDatum Request { get; set; } = null!;
}
