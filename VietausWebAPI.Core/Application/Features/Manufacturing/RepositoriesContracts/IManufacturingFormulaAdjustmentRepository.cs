using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts
{
    public interface IManufacturingFormulaAdjustmentRepository
    {
        IQueryable<ManufacturingFormulaAdjustment> Query(bool track = false);
        Task<ManufacturingFormulaAdjustment?> GetTrackedAsync(Guid id, Guid companyId, CancellationToken ct = default);
        Task<ManufacturingFormulaAdjustment?> GetTrackedWithBatchesOnlyAsync(Guid id, Guid companyId, CancellationToken ct = default);
        Task<ManufacturingFormulaAdjustment?> GetTrackedWithBatchesAsync(Guid id, Guid companyId, CancellationToken ct = default);
        Task AddAsync(ManufacturingFormulaAdjustment entity, CancellationToken ct = default);
        Task AddBatchAsync(ManufacturingFormulaAdjustmentBatch batch, CancellationToken ct = default);
        Task DeleteItemsByBatchIdAsync(Guid batchId, CancellationToken ct = default);
    }
}
