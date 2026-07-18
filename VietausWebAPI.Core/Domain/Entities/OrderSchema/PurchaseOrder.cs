using System;
using System.Collections.Generic;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;
using VietausWebAPI.Core.Domain.Entities.CompanySchema;
using VietausWebAPI.Core.Domain.Entities.HrSchema;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;

namespace VietausWebAPI.Core.Domain.Entities.OrderSchema;

public partial class PurchaseOrder
{
    public Guid PurchaseOrderId { get; set; }

    public string? ExternalId { get; set; }

    public string? OrderType { get; set; }

    public Guid? SupplierId { get; set; }

    public string? Comment { get; set; }
    public string? PLPUComment { get; set; }

    // Chứng từ NCC cần cung cấp khi giao hàng: phiếu giao hàng, COA, xác nhận PO, hóa đơn.
    //public bool? HasDeliveryNote { get; set; } = null;
    //public bool? HasContract { get; set; } = null;
    //public bool? HasAcceptance { get; set; } = null;
    //public bool? HasInvoice { get; set; } = null;


    public DateTime? RequestDeliveryDate { get; set; }
    public DateTime? RealDeliveryDate { get; set; }

    public string? Status { get; set; }

    public Guid? CompanyId { get; set; }

    public DateTime? CreateDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }
    public Guid? PurchaseOrderSnapshotId { get; set; }
    //public Guid? AttachmentCollectionId { get; set; }

    public bool? IsActive { get; set; } = true;
    public virtual PurchaseOrderSnapshot? PurchaseOrderSnapshot { get; set; }
    //public virtual AttachmentCollection? AttachmentCollection { get; set; }
    public virtual Company? Company { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();
    public virtual ICollection<PurchaseOrderDocument> PurchaseOrderDocuments { get; set; } = new List<PurchaseOrderDocument>();
    public virtual ICollection<PurchaseOrderLink> PurchaseOrderLinks { get; set; } = new List<PurchaseOrderLink>();
    public virtual Supplier? Supplier { get; set; }

    public virtual Employee? UpdatedByNavigation { get; set; }
}
