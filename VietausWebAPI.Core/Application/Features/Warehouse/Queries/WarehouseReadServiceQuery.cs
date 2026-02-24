using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Application.Features.Warehouse.Queries
{
    public class WarehouseReadServiceQuery : PaginationQuery
    {
        public string? KeyWord { get; set; } = string.Empty;
        public StockType? StockTypes { get; set; }

        public decimal? AvailableMax { get; set; }   // ví dụ 0 để lấy <=0
        public bool OnlyAvailableLeZero { get; set; } = false; // shortcut
    }
}
