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
        public int ShelfStockId { get; set; }                 // PK (identity/serial)
        public string Code { get; set; } = "";                // mã NVL
        public StockType StockType { get; set; }
        public string CodeName { get; set; } = "";            // Tên NVL
        public string CategoryName { get; set; } = "";          


        // Lấy dữ liệu tồn kho
        public decimal OnHandKg { get; set; }
        public decimal? ReservedOpenAllKg { get; set; }
        public decimal? AvailableKg { get; set; }
    }
}
