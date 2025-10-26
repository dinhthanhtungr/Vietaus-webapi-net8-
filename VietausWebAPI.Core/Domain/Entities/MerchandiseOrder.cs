using System;
using System.Collections.Generic;

 namespace VietausWebAPI.Core.Domain.Entities;

public partial class MerchandiseOrder
{
    public Guid MerchandiseOrderId { get; set; }
    public string? ExternalId { get; set; }


    public Guid? CustomerId { get; set; }
    public string? CustomerNameSnapshot { get; set; }
    public string? CustomerExternalIdSnapshot { get; set; }
    public string? PhoneSnapshot { get; set; }

    public Guid? ManagerById { get; set; }
    public string? ManagerByNameSnapshot { get; set; }
    public string? ManagerExternalIdSnapshot { get; set; }

    public string? Receiver { get; set; }
    public string? DeliveryAddress { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? PaymentType { get; set; }

    public decimal? Vat { get; set; }

    public string? Status { get; set; }
    public string? Currency { get; set; }

    public Guid? CompanyId { get; set; }
    public bool? IsPaid { get; set; }
    public bool? IsActive { get; set; }

    public DateTime? PaymentDate { get; set; }
    //public DateTime? DeliveryRequestDate { get; set; }
    //public DateTime? DeliveryActualDate { get; set; }
    //public DateTime? ExpectedDeliveryDate { get; set; }

    public string? Note { get; set; }
    public string? ShippingMethod { get; set; }
    public string? PONo { get; set; }

    public DateTime? CreateDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Employee? ManagerBy { get; set; }


    public ICollection<DeliveryOrderPO> DeliveryOrderPOs { get; set; } = new List<DeliveryOrderPO>();
    public ICollection<PurchaseOrderLink> PurchaseOrderLinks { get; set; } = new List<PurchaseOrderLink>();
    public virtual ICollection<DeliveryOrderDetail> DeliveryOrderDetails { get; set; } = new List<DeliveryOrderDetail>();
    public virtual ICollection<MerchandiseOrderDetail> MerchandiseOrderDetails { get; set; } = new List<MerchandiseOrderDetail>();
    public virtual ICollection<MerchandiseOrderLog> MerchandiseOrderLogs { get; set; } = new List<MerchandiseOrderLog>();

    public virtual ICollection<MerchandiseOrderSchedule> MerchandiseOrderSchedules { get; set; } = new List<MerchandiseOrderSchedule>();
    public virtual ICollection<MfgProductionOrder> MfgProductionOrders { get; set; } = new List<MfgProductionOrder>();
    public ICollection<OrderAttachment> Attachments { get; set; } = new List<OrderAttachment>();

    public virtual Employee? UpdatedByNavigation { get; set; }
}
