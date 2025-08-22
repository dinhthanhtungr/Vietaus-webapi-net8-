using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class PurchaseOrderDetail
{
    public int PurchaseOrderDetailId { get; set; }

    public Guid PurchaseOrderId { get; set; }

    public Guid? ExternalId { get; set; }

    public Guid MaterialId { get; set; }

    public double? Quantity { get; set; }

    public decimal? Price { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public string? Note { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
}
