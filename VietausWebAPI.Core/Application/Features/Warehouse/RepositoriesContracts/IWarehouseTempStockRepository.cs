using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;

namespace VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts
{
    public interface IWarehouseTempStockRepository
    {
        IQueryable<WarehouseTempStock> Query(bool track = true);
        Task AddAsync(WarehouseTempStock warehouseTempStock, CancellationToken ct = default);
        Task AddRangeAsync(List<WarehouseTempStock> warehouseTempStock, CancellationToken ct = default);
    }
}
