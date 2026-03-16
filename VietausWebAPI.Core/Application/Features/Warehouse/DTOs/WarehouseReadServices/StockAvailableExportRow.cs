using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices
{
    public class StockAvailableExportRow
    {
        public string Code { get; set; } = string.Empty;
        public string CodeName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string StockType { get; set; } = string.Empty;

        public decimal TotalOnHandKg { get; set; }

        public string ShelfStockCode { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string? LotNo { get; set; }
        public decimal OnHandKg { get; set; }
    }
}
