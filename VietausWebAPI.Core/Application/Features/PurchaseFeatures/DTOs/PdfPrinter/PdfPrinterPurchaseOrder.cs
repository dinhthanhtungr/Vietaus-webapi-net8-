using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs.PdfPrinter
{
    public class PdfPrinterPurchaseOrder
    {
        public Guid PurchaseOrderSnapshotId { get; set; }

        public string PurchaseOrderExternalIdSnapshot { get; set; } = null!;

        public string EmployeeExternalIdSnapshot { get; set; } = null!;

        public string EmployeeFullNameSnapshot { get; set; } = null!;

        public string? PhoneNumberSnapshot { get; set; }

        public string? EmailSnapshot { get; set; }

        public string? SupplierExternalIdSnapshot { get; set; }
        public string? SupplierNameSnapshot { get; set; }
        public string? SupplierContactSnapshot { get; set; }
        public string? SupplierPhoneNumberSnapshot { get; set; }
        public string? SupplierAddressSnapshot { get; set; }

        public string? Note { get; set; }

        public decimal? TotalPrice { get; set; }

        public string? DeliveryAddress { get; set; }

        public string? PaymentTypes { get; set; }

        public int? Vat { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? RequestDeliveryDate { get; set; }

        public List<PdfPrinterPurchaseOrderDetail> Details { get; set; } = new List<PdfPrinterPurchaseOrderDetail>();   
    }
}
