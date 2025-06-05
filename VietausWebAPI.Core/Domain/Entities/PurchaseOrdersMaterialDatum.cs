using System;
using System.Collections.Generic;

namespace VietausWebAPI.Core.Domain.Entities;

public partial class PurchaseOrdersMaterialDatum
{

    public Guid Poid { get; set; }

    public string? Pocode { get; set; }

    public Guid? SupplierId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? EmployeeId { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }
    public decimal TotalAmount { get; set; } // Tổng số tiền của đơn hàng
    public decimal GrandTotal { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? ContactName { get; set; }             // Tên người liên hệ
    public string? VendorAddress { get; set; }           // Địa chỉ vendor
    public string? VendorPhone { get; set; }             // SĐT vendor
    public string? InvoiceNote { get; set; }             // Ghi chú hóa đơn

    public string? DeliveryAddress { get; set; }         // Địa điểm giao hàng
    public string? DeliveryContact { get; set; }         // Người liên hệ giao hàng

    public string? Packaging { get; set; }               // Hình thức đóng gói
    public string? PaymentTerm { get; set; }             // Điều khoản thanh toán

    public string? RequiredDocuments { get; set; }       // Các chứng từ yêu cầu
    public string? RequiredDocuments_Eng { get; set; }   // Các chứng từ yêu cầu (bản tiếng Anh)

    public int VAT { get; set; }                    // Thuế VAT
   

    public virtual EmployeesCommonDatum? Employee { get; set; }

    public virtual ICollection<PurchaseOrderDetailsMaterialDatum> PurchaseOrderDetailsMaterialData { get; set; } = new List<PurchaseOrderDetailsMaterialDatum>();

    public virtual ICollection<PurchaseOrderStatusHistoryMaterialDatum> PurchaseOrderStatusHistoryMaterialData { get; set; } = new List<PurchaseOrderStatusHistoryMaterialDatum>();

    public virtual SuppliersMaterialDatum? Supplier { get; set; }
}
