using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Enums.WareHouses
{
    public enum StockType
    {
        FinishedGood = 1,           // Hàng đạt
        DefectiveFinishedGood = 2,  // Hàng lỗi
        RawMaterial = 3,            // Hàng đạt
        DefectiveRawMaterial = 4,   // Hàng lỗi
        Other = 5,                  // Khác
        Waiter = 6,                  // Chưa QC
        Material = 7                  // Vật tư, bao bì
    }
}
