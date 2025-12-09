using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Enums.Category
{
    public enum DocumentPrefix
    {
        PRQ,    // Phiếu yêu cầu (nhập/xuất kho)
        PXK,    // Phiếu xuất kho
        PNK,    // Phiếu nhập kho
        PCK,    // Phiếu chuyển kho / chuyển kho nội bộ

        MFG,    // Lệnh sản xuất
        VA,     // Công thức sản xuất (VA - formula)
        VU,     // Công thức phối màu (VU / Masterbatch color formula)

        DHG,    // Đơn hàng (Sales Order)
        PGH,    // Phiếu giao hàng / Delivery note
        DDH,    // Đơn đặt hàng (Purchase Order)

        PM,     // Phiếu bảo trì (maintenance request / maintenance ticket)
        IM,     // Phiếu cải tiến (improvement)
        IR,     // Phiếu sự cố (incident report)
        WO,     // Work Order / Phiếu lệnh sửa chữa
        PR,     // Purchase Request / Phiếu yêu cầu mua
        INV,    // Invoice (Hóa đơn)
        RCPT,   //  Biên nhận / Phiếu thu (nếu có)
    }
}
