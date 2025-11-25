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
            var mf = _context.ManufacturingFormulas.AsNoTracking();

            if (id.HasValue)
            {
                // Prefilter bảng select để join gọn hơn
                var sels = _context.ProductionSelectVersions
                    .AsNoTracking()
                    .Where(s => s.MfgProductionOrderId == id.Value);

                return await mf
                    .Where(f => f.Name.StartsWith(prefix))
                    .Join(
                        sels,
                        f => f.ManufacturingFormulaId,
                        s => s.ManufacturingFormulaId,
                        (f, s) => f)
                    .OrderByDescending(f => f.Name)   // nhớ zero-pad để sort đúng theo số
                    .Select(f => f.Name)
                    .FirstOrDefaultAsync();
            }

            // Toàn cục (hoặc thêm filter CompanyId nếu bạn muốn series theo công ty)
            return await mf
                .Where(f => f.Name.StartsWith(prefix))
                .OrderByDescending(f => f.Name)
                .Select(f => f.Name)
                .FirstOrDefaultAsync();
        }

        public IQueryable<ManufacturingFormula> Query(bool track = false)
        {
            var query = _context.ManufacturingFormulas.AsQueryable();
            return track ? query : query.AsNoTracking();
        }
    }
}
