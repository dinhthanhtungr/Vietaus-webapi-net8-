using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Warehouses
{
    public class WarehouseShelfStockRepository : IWarehouseShelfStockRepository
    {
        private readonly ApplicationDbContext _context;

        public WarehouseShelfStockRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(WarehouseShelfStock warehouseShelfStock, CancellationToken ct = default)
        {
            await _context.WarehouseShelfStocks.AddAsync(warehouseShelfStock, ct);
        }

        public IQueryable<WarehouseShelfStock> Query(bool track = true)
        {
            var db = _context.WarehouseShelfStocks.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
