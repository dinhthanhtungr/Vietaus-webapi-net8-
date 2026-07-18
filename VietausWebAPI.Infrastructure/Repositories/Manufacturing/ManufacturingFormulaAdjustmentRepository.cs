using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Manufacturing
{
    public class ManufacturingFormulaAdjustmentRepository : IManufacturingFormulaAdjustmentRepository
    {
        private readonly ApplicationDbContext _context;

        public ManufacturingFormulaAdjustmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<ManufacturingFormulaAdjustment> Query(bool track = false)
        {
            var query = _context.ManufacturingFormulaAdjustments.AsQueryable();
            return track ? query : query.AsNoTracking();
        }

        public async Task<ManufacturingFormulaAdjustment?> GetTrackedAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default)
        {
            return await _context.ManufacturingFormulaAdjustments
                .FirstOrDefaultAsync(x =>
                    x.ManufacturingFormulaAdjustmentId == id &&
                    x.CompanyId == companyId &&
                    x.IsActive, ct);
        }

        public async Task<ManufacturingFormulaAdjustment?> GetTrackedWithBatchesAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default)
        {
            return await _context.ManufacturingFormulaAdjustments
                .Include(x => x.Batches)
                    .ThenInclude(x => x.Items)
                .FirstOrDefaultAsync(x =>
                    x.ManufacturingFormulaAdjustmentId == id &&
                    x.CompanyId == companyId &&
                    x.IsActive, ct);
        }

        public async Task<ManufacturingFormulaAdjustment?> GetTrackedWithBatchesOnlyAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default)
        {
            return await _context.ManufacturingFormulaAdjustments
                .Include(x => x.Batches)
                .FirstOrDefaultAsync(x =>
                    x.ManufacturingFormulaAdjustmentId == id &&
                    x.CompanyId == companyId &&
                    x.IsActive, ct);
        }

        public async Task AddAsync(ManufacturingFormulaAdjustment entity, CancellationToken ct = default)
        {
            await _context.ManufacturingFormulaAdjustments.AddAsync(entity, ct);
        }

        public async Task AddBatchAsync(ManufacturingFormulaAdjustmentBatch batch, CancellationToken ct = default)
        {
            await _context.ManufacturingFormulaAdjustmentBatches.AddAsync(batch, ct);
        }

        public async Task DeleteItemsByBatchIdAsync(Guid batchId, CancellationToken ct = default)
        {
            await _context.ManufacturingFormulaAdjustmentItems
                .Where(x => x.ManufacturingFormulaAdjustmentBatchId == batchId)
                .ExecuteDeleteAsync(ct);

            var trackedItems = _context.ChangeTracker
                .Entries<ManufacturingFormulaAdjustmentItem>()
                .Where(x => x.Entity.ManufacturingFormulaAdjustmentBatchId == batchId)
                .ToList();

            foreach (var entry in trackedItems)
                entry.State = EntityState.Detached;
        }

    }
}
