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

    public class WarehouseRequestDetailRepository : IWarehouseRequestDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public WarehouseRequestDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(WarehouseRequestDetail warehouseRequestDetail, CancellationToken ct = default)
        {
            await _context.WarehouseRequestDetails.AddAsync(warehouseRequestDetail, ct);
        }

        public IQueryable<WarehouseRequestDetail> Query(bool track = true)
        {
            var db = _context.WarehouseRequestDetails.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
