using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Manufacturing
{
    public class MfgProductionOrderRepository : IMfgProductionOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public MfgProductionOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        

        public async Task AddAsync(MfgProductionOrder mfgProductionOrder, CancellationToken ct = default)
        {
            await _context.MfgProductionOrders.AddAsync(mfgProductionOrder, ct);   
        }

        public async Task AddRangeAsync(IEnumerable<MfgProductionOrder> mfgProductionOrders, CancellationToken ct = default)
        {
            await _context.MfgProductionOrders.AddRangeAsync(mfgProductionOrders, ct);
        }

        public async Task<string?> GetLatestExternalIdStartsWithAsync(string prefix)
        {
            return await _context.MfgProductionOrders
                .Where(e => e.ExternalId.StartsWith(prefix))
                .OrderByDescending(e => e.ExternalId.Length)   // dài hơn => số lớn hơn
                .ThenByDescending(e => e.ExternalId)           // cùng độ dài thì so chuỗi
                .Select(e => e.ExternalId)
                .FirstOrDefaultAsync();
        }

        public IQueryable<MfgProductionOrder> Query(bool track = false)
        {
            var db = _context.MfgProductionOrders.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
 