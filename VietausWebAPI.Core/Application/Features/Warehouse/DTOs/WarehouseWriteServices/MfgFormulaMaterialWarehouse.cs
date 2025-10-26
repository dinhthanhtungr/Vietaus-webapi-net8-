using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseWriteServices
{
    public class MfgFormulaMaterialWarehouse
    {
        public string? MaterialExternalIdSnapshot { get; set; }   // VARCHAR
        public decimal? TotalQuantity { get; set; }
    }
}
