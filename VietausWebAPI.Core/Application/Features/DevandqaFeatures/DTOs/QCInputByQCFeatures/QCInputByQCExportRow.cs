using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.QCInputByQCFeatures
{
    public class QCInputByQCExportRow
    {
        public string? MaterialExternalId { get; set; }   // Mã số
        public string? MaterialName { get; set; }         // Tên
        public string? ImportWarehouseText { get; set; }  // Nhập kho
        public string? SupplierName { get; set; }         // Nhà cung cấp
        public string? ResultText { get; set; }           // Kết quả
        public string? InspectorName { get; set; }        // Người kiểm
        public DateTime? InspectionDate { get; set; }     // Ngày kiểm
        public DateTime? CreatedDate { get; set; }        // Ngày tạo
        public string? LotNo { get; set; }                // Lot #
        public decimal? DeliveryQuantityKg { get; set; }  // Khối lượng giao hàng
        public decimal? ActualReceivedQuantityKg { get; set; } // Khối lượng thực nhận
        public string? Note { get; set; }                 // Ghi chú
    }
}
