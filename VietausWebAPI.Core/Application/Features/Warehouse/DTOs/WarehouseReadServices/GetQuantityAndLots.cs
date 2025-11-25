using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices
{
    public class GetQuantityAndLots
    {
        public string companyName { get; set; } = string.Empty;
        public decimal quantity { get; set; }
        public string lotcode { get; set; } = string.Empty;

        public StockType stockType { get; set; }
    }
}
