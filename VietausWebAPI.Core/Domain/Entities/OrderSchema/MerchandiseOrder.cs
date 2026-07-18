using System;
using System.Collections.Generic;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;
using VietausWebAPI.Core.Domain.Entities.CompanySchema;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;
using VietausWebAPI.Core.Domain.Entities.HrSchema;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Core.Domain.Enums.Merchadises;

namespace VietausWebAPI.Core.Domain.Entities.OrderSchema;

public partial class MerchandiseOrder
{
    // ===== Thông tin định danh đơn hàng =====
    // ExternalId là mã đơn hàng hiển thị cho người dùng; OrderType phân biệt đơn bán hàng, đơn mẫu, v.v.
    public Guid MerchandiseOrderId { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public OrderType OrderType { get; set; } = OrderType.Merchandise;

    // ===== Tệp đính kèm =====
    // Dùng để gom hợp đồng, PO khách gửi, hình ảnh/chứng từ liên quan đến đơn hàng.
    public Guid AttachmentCollectionId { get; set; }


    // ===== Thông tin khách hàng tại thời điểm lên đơn =====
    // Các field snapshot giúp giữ nguyên dữ liệu trên đơn dù sau này thông tin khách hàng bị chỉnh sửa.
    public Guid CustomerId { get; set; }
    public string CustomerNameSnapshot { get; set; } = string.Empty;
    public string CustomerExternalIdSnapshot { get; set; } = string.Empty;
    public string PhoneSnapshot { get; set; } = string.Empty;

    // ===== Nhân viên phụ trách đơn hàng =====
    // Snapshot tên/mã nhân viên để báo cáo và chứng từ không bị đổi theo profile hiện tại.
    public Guid ManagerById { get; set; }
    public string ManagerByNameSnapshot { get; set; } = string.Empty;
    public string ManagerExternalIdSnapshot { get; set; } = string.Empty;

    // ===== Thông tin giao hàng mặc định của đơn =====
    // Detail vẫn quyết định từng dòng hàng giao gì, còn header giữ người nhận/địa chỉ chung.
    public string Receiver { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;

    // ===== Thông tin giá trị và thanh toán =====
    public decimal? TotalPrice { get; set; }

    public string? PaymentType { get; set; }

    public decimal? Vat { get; set; }

    // ===== Trạng thái nghiệp vụ của đơn hàng =====
    // Status là trạng thái tổng thể đơn hàng; IsPaid là trạng thái thanh toán; IsActive dùng cho soft delete.
    public string Status { get; set; } = string.Empty;

    public string? Currency { get; set; }
    public decimal? ExchangeRate { get; set; }

    public Guid CompanyId { get; set; }
    public bool IsPaid { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTime? PaymentDate { get; set; }

    // ===== Ghi chú và thông tin tham chiếu từ khách hàng =====
    public string? Note { get; set; }
    public string? ShippingMethod { get; set; }
    public string PONo { get; set; } = string.Empty;

    // ===== Tạm ngưng giao hàng =====
    // Khi IsDeliveryPaused = true, đơn này không được đưa vào kế hoạch giao và không được tạo phiếu giao mới.
    // DeliveryPausedFrom/To cho biết khoảng thời gian tạm ngưng; DeliveryPausedTo null nghĩa là pause chưa có ngày mở lại.
    // DeliveryPauseReason/PauseType dùng để FE hiển thị lý do cho sale/kế hoạch/kho.
    public bool IsDeliveryPaused { get; set; } = false;
    public DateTime? DeliveryPausedFrom { get; set; }
    public DateTime? DeliveryPausedTo { get; set; }
    public string? DeliveryPauseReason { get; set; }
    public string? DeliveryPauseType { get; set; }
    public Guid? DeliveryPausedBy { get; set; }

    // ===== Audit tạo/cập nhật =====
    public DateTime CreateDate { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }
    public Guid UpdatedBy { get; set; }

    // ===== Navigation properties =====
    public virtual Company? Company { get; set; }

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Employee? ManagerBy { get; set; }
    public virtual Employee? DeliveryPausedByNavigation { get; set; }


    public virtual AttachmentCollection AttachmentCollection { get; set; } = null!;
    public virtual ICollection<DeliveryOrderPO> DeliveryOrderPOs { get; set; } = new List<DeliveryOrderPO>();
    public virtual ICollection<PurchaseOrderLink> PurchaseOrderLinks { get; set; } = new List<PurchaseOrderLink>();
    //public virtual ICollection<DeliveryOrderDetail> DeliveryOrderDetails { get; set; } = new List<DeliveryOrderDetail>();
    public virtual ICollection<MerchandiseOrderDetail> MerchandiseOrderDetails { get; set; } = new List<MerchandiseOrderDetail>();

    public virtual Employee? UpdatedByNavigation { get; set; }
}
