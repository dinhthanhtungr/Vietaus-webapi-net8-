using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Warehouses
{
    public class WarehouseTempStockRepository : IWarehouseTempStockRepository
    {
        private readonly ApplicationDbContext _context;

        public WarehouseTempStockRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(WarehouseTempStock warehouseTempStock, CancellationToken ct = default)
        {
            await _context.WarehouseTempStocks.AddAsync(warehouseTempStock, ct);
        }

        public async Task AddRangeAsync(List<WarehouseTempStock> warehouseTempStock, CancellationToken ct = default)
        {
            await _context.WarehouseTempStocks.AddRangeAsync(warehouseTempStock, ct);
        }

        public IQueryable<WarehouseTempStock> Query(bool track = true)
        {
            var db = _context.WarehouseTempStocks.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
