using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class PurchaseOrdersSchedule
{
    public Guid PurchaseOrdersScheduleId { get; set; }

    public Guid PurchaseOrderId { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public double? Quantity { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
}
