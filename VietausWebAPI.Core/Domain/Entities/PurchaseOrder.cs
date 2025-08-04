using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class PurchaseOrder
{
    public Guid PurchaseOrderId { get; set; }

    public string? ExternalId { get; set; }

    public string? RequestSourceType { get; set; }

    public Guid? RequestSourceId { get; set; }

    public string? OrderType { get; set; }

    public Guid? SupplierId { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? Comment { get; set; }

    public string? DeliveryAddress { get; set; }

    public string? PackageType { get; set; }

    public string? PaymentDays { get; set; }

    public int? Vat { get; set; }

    public string? Status { get; set; }

    public Guid? CompanyId { get; set; }

    public DateTime? CreateDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();

    public virtual ICollection<PurchaseOrderStatusHistory> PurchaseOrderStatusHistories { get; set; } = new List<PurchaseOrderStatusHistory>();

    public virtual ICollection<PurchaseOrdersSchedule> PurchaseOrdersSchedules { get; set; } = new List<PurchaseOrdersSchedule>();

    public virtual Supplier? Supplier { get; set; }

    public virtual Employee? UpdatedByNavigation { get; set; }
}
