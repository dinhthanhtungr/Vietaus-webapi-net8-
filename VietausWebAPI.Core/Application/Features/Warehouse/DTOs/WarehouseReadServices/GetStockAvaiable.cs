using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices
{
    public class GetStockAvaiable
    {
        public int ShelfStockId { get; set; }
        public string Code { get; set; } = "";
        public StockType StockType { get; set; }
        public string CodeName { get; set; } = "";
        public string CategoryName { get; set; } = "";

        public decimal TotalOnHandKg { get; set; }      // tổng tồn
        public decimal? ReservedOpenAllKg { get; set; } // tổng giữ chỗ theo code
        public decimal? AvailableKg { get; set; }       // tổng khả dụng theo code

        public List<StockDetailAvaiable> StockDetailAvaiables { get; set; } = new();
    }
}
