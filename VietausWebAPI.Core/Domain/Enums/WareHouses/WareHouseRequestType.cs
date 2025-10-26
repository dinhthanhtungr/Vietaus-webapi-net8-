using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Enums.WareHouses
{
    public enum WareHouseRequestType
    {
        // === Nhập kho (số lẻ) ===
        ImportFinishedGoods = 1,          // Nhập kho thành phẩm
        ImportDefectiveGoods = 3,         // Nhập kho hàng lỗi
        ImportFromSupplier = 5,           // Nhập kho mua hàng (NCC)
        ImportInternalTransfer = 7,       // Nhập kho điều chuyển nội bộ
        ImportReturnedGoods = 9,          // Nhập kho hàng trả về
        ImportAdjustmentIncrease = 11,    // Nhập điều chỉnh (tăng)
        ImportPlasticCut = 13,            // Nhập kho nhựa cắt rửa
        ImportOther = 15,                 // Khác

        // === Xuất kho (số chẵn) ===
        ExportForProduction = 2,          // Xuất cho sản xuất (NVL)
        ExportForSales = 4,               // Xuất bán (thành phẩm)
        ExportInternalTransfer = 6,       // Xuất điều chuyển nội bộ
        ExportReturnToSupplier = 8,       // Xuất trả lại NCC
        ExportForRecycle = 10,            // Xuất tái chế/phế liệu
        ExportForLab = 12,                // Xuất cho Lab, R&D
        ExportForAuditLoss = 14,          // Xuất kiểm kê/hao hụt
        ExportForCleaning = 16,           // Xuất vật tư/nhựa rửa máy
        ExportOther = 18                  // Khác
    }
}
