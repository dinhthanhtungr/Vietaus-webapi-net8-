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
    public class WarehouseRequestRepository : IWarehouseRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public WarehouseRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(WarehouseRequest warehouseRequest, CancellationToken ct = default)
        {
            await _context.WarehouseRequests.AddAsync(warehouseRequest, ct);
        }

        public IQueryable<WarehouseRequest> Query(bool track = true)
        {
            var db = _context.WarehouseRequests.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
