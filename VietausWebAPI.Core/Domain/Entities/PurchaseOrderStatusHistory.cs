using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class PurchaseOrderStatusHistory
{
    public int StatusHistoryId { get; set; }

    public Guid PurchaseOrderId { get; set; }

    public string? StatusFrom { get; set; }

    public string? StatusTo { get; set; }

    public Guid? EmployeeId { get; set; }

    public DateTime? ChangedDate { get; set; }

    public string? Note { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
}
