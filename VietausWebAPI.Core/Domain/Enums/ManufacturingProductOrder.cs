using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Enums
{
    public enum ManufacturingProductOrder
    {
        Unknown = 0,

        // ---- Khởi tạo & kiểm duyệt
        New = 1,                 // Mới tạo
        QCChecked = 2,           // Lab đã xác nhận công thức

        // ---- Thay đổi
        FormulaChangeRequested = 3, // Yêu cầu đổi công thức
        PLPUChecked = 4,         // Kế hoạch đã xác nhận công thức

        // ---- Lập lịch & chuẩn bị SX
        InProgress = 5,           // Đã lên lịch (Không ghi log)

        Stocked = 18, // Đã nhập kho nguyên vật liệu nghĩa là đã sản xuất xong rồi
        //Mixing = 5,              // Trộn 

        //Processing = 6,          // Đang chạy 
        //Produced = 7,            // Máy chạy xong / chốt sản lượng 

        // ---- Tạm dừng / Ngoại lệ
        //QCFail = 8,              // Dừng bởi QC
        Canceled = 19,           // Hủy LỆNH PHẢI CẬP NHẬT Ở HAI BẢN MfgProductOrder VÀ MfGSchechale
        //Error = 10,              // Lỗi

        // ---- Kết thúc
        //Completed = 11,          // Hoàn tất (đạt QC + quyết toán + đóng lệnh)

        QCFail = 22,

    // ====== Bước con (mới thêm, KHÔNG đè số cũ) ======
    //WaitingMaterials = 13,   // Chờ NVL
    //QueuedToMachine = 14,    // Đưa lên máy / xếp hàng chờ chạy
    //Weighing = 15,           // Cân
    //Regrind = 16,            // Tái sinh
    //WarehouseReceived = 17   // Đã nhập kho (sau Produced, trước Completed)
}

}
