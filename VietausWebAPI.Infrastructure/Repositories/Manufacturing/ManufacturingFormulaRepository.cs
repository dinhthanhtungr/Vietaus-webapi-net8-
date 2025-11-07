using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Manufacturing
{
    public class ManufacturingFormulaRepository : IManufacturingFormulaRepository
    {
        private readonly ApplicationDbContext _context;

        public ManufacturingFormulaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ManufacturingFormula sampleRequest, CancellationToken ct = default)
        {
            await _context.ManufacturingFormulas.AddAsync(sampleRequest, ct);
        }

        public async Task AddRangeAsync(IEnumerable<ManufacturingFormula> sampleRequests, CancellationToken ct = default)
        {
            await _context.AddRangeAsync(sampleRequests, ct);
        }

        public async Task<bool> ExistsAsync(Guid productId, CancellationToken ct)
        {
            return await _context.ManufacturingFormulas.AsNoTracking().AnyAsync(p => p.ManufacturingFormulaId == productId, ct);
        }

        public async Task<string?> GetLatestExternalIdStartsWithAsync(string prefix, Guid? id = null)
        {
            var query = _context.ManufacturingFormulas
                .AsNoTracking();


            if (id.HasValue)
            {
                query = query.Where(e => e.MfgProductionOrderId == id);
                query = query.Where(e => e.Name.StartsWith(prefix));
                var temp = await query
                                .OrderByDescending(e => e.Name)
                                .Select(e => e.Name)
                                .FirstOrDefaultAsync();

                return temp;
            }

            query = query.Where(e => e.ExternalId.StartsWith(prefix));
            var result = await query
                            .OrderByDescending(e => e.ExternalId)
                            .Select(e => e.ExternalId)
                            .FirstOrDefaultAsync();

            return result;
        }

        public IQueryable<ManufacturingFormula> Query(bool track = false)
        {
            var query = _context.ManufacturingFormulas.AsQueryable();
            return track ? query : query.AsNoTracking();
        }
    }
}
