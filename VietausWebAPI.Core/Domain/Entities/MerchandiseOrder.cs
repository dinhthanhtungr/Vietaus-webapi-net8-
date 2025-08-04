using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class MerchandiseOrder
{
    public Guid MerchandiseOrderId { get; set; }

    public string? ExternalId { get; set; }

    public Guid? CustomerId { get; set; }

    public Guid? ManagerById { get; set; }

    public string? Receiver { get; set; }

    public string? DeliveryAddress { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? PaymentType { get; set; }

    public int? Vat { get; set; }

    public string? Status { get; set; }

    public Guid? CompanyId { get; set; }

    public bool? IsPaid { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? Note { get; set; }

    public string? ShippingMethod { get; set; }

    public DateTime? CreateDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual Customer1? Customer { get; set; }

    public virtual Employee? ManagerBy { get; set; }

    public virtual ICollection<MerchandiseOrderDetail> MerchandiseOrderDetails { get; set; } = new List<MerchandiseOrderDetail>();

    public virtual ICollection<MerchandiseOrderSchedule> MerchandiseOrderSchedules { get; set; } = new List<MerchandiseOrderSchedule>();

    public virtual Employee? UpdatedByNavigation { get; set; }
}
