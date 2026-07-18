using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Enums.Attachment
{
    public enum AttachmentSlot
    {
        //===========================================================
        Contract = 0,        // Hợp đồng
        PurchaseOrder = 1,   // Đơn đặt hàng
        DeliveryNote = 2,    // Phiếu giao hàng
        Invoice = 3,         // Hóa đơn
        Photo = 4,           // Ảnh hiện trường / sản phẩm
        Specification = 5,   // Spec/tiêu chuẩn kỹ thuật
        Other = 6,           // Khác
        SampleRequest = 7,   // Ảnh mẫu
        ColouredChip = 8,    // Mẫu màu
        QcReport = 9,        // Báo cáo QC
        TDS_MSDS = 10,  // TDS/MSDS
        COA = 11,             // Chứng nhận COA
        Acceptance = 12, // Biên bản nghiệm thu
    } 

}
