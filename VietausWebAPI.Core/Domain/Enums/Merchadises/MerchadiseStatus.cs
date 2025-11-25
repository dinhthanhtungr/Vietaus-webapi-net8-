using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Enums.Merchadises
{
    enum MerchadiseStatus
    {
        New = 0,          // Mới
        Approved = 1,     // Duyệt
        Pending = 2,      // Đang chờ
        Processing = 3,   // Đang xử lý
        Delivering = 4,   // Đang giao hàng
        Paused = 5,       // Tạm dừng
        Cancelled = 6,    // Hủy
        Completed = 7     // Hoàn thành

    }
}
