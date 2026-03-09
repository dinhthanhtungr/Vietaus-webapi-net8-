using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices
{
    public class StockDetailAvaiable
    {
        public string? LotNo { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public decimal OnHandKg { get; set; }
    }
}
