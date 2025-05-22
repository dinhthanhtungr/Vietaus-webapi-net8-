using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class InventoryReceiptsMaterialDatum
{
    public string RequestId { get; set; } = null!;

    public DateTime ReceiptDate { get; set; }

    public string MaterialName { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public int ReceivedQuantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public string? Note { get; set; }

    public int ReceiptId { get; set; }

    public bool? Status { get; set; }

    public string? SupplierId { get; set; }

    public Guid? MaterialGroupId { get; set; }

    public virtual MaterialGroupsMaterialDatum? MaterialGroup { get; set; }

    public virtual SupplyRequestsMaterialDatum Request { get; set; } = null!;
}
