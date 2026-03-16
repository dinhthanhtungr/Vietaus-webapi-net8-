using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices;

namespace VietausWebAPI.Core.Application.Features.Warehouse.Helpers
{
    public interface IStockAvailableExcel
    {
        byte[] Render(IReadOnlyList<StockAvailableExportRow> rows);
    }
}
