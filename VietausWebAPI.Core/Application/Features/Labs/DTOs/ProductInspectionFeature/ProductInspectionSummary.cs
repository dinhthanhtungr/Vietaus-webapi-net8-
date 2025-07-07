using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductInspectionFeature
{
    public class ProductInspectionSummary
    {
        public Guid? Id { get; set; }               // ID để View / Clone / Delete
        public string? ColourCode { get; set; }       // Mã số (VD: LP71034C)
        public string? ProductName { get; set; }        // Tên sản phẩm (VD: HẠT COMPOUND...)
        public string? BatchNumber { get; set; }      // Batch # (VD: VA250600335)
        public string? Status { get; set; }           // Trạng thái (VD: Trộn hàng)

        public bool? Result { get; set; }           // Kết quả (VD: Giao hàng)
        public DateTime CreateDate { get; set; }  // Ngày tạo
        public string? CreatedBy { get; set; }        // Người kiểm (VD: Mai Thị Hằng)
    }
}
