using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts
{
    public interface IWarehouseShelfStockRepository
    {
        IQueryable<WarehouseShelfStock> Query(bool track = true);
        Task AddAsync(WarehouseShelfStock warehouseShelfStock, CancellationToken ct = default);
    }
}
