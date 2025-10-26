using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Enums.WareHouses
{
    public enum StockType
    {
        FinishedGood = 1, // TP
        DefectiveFinishedGood = 2, // TPL
        RawMaterial = 3, // NVL
        DefectiveRawMaterial = 4  // NVLL
    }
}
