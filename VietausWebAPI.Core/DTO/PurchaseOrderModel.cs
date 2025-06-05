using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.PurchaseOrders;

namespace VietausWebAPI.Core.DTO
{
    public class PurchaseOrderModel
    {
        public Guid Poid { get; set; }

        public string? POCode { get; set; }

        public Guid? SupplierId { get; set; }

        public DateTime? OrderDate { get; set; }

        //public string? EmployeeId { get; set; }

        public string FullName { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string? Status { get; set; }

        public string? Note { get; set; }

        public decimal TotalAmount { get; set; } // Tổng số tiền của đơn hàng
        public decimal GrandTotal { get; set; } // Tổng số tiền bao gồm thuế VAT

        public DateTime? DeliveryDate { get; set; }
        public string? ContactName { get; set; }             // Tên người liên hệ
        public string? VendorName { get; set; }              // Tên vendor
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


        public List<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();

    }

}
