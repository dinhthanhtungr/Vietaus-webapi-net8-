using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Enums.Manufacturings
{
    public enum ManufacturingProductOrderFormula
    {
        UnKnow = 0,
        New = 1,            // Mới
        Checking = 2,       // Đang kiểm tra
        IsSelect = 3,       // Duyệt công thức
        Cancelled = 4,      // Hủy
        Error = 5,          // Lỗi
        Completed = 6       // Hoàn thành
    }
}
