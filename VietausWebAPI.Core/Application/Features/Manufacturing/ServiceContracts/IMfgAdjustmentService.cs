using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulaAdjustments;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgFormulaAdjustments;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts
{
    public interface IMfgAdjustmentService
    {
        Task<OperationResult<GetManufacturingFormulaAdjustment>> CreateAsync(
            PostManufacturingFormulaAdjustment request,
            CancellationToken ct = default);

        Task<OperationResult<GetManufacturingFormulaAdjustment>> PatchAsync(
            Guid id,
            PatchManufacturingFormulaAdjustment request,
            CancellationToken ct = default);

        Task<OperationResult<GetManufacturingFormulaAdjustment>> AddBatchAsync(
            Guid id,
            PostManufacturingFormulaAdjustmentBatch request,
            CancellationToken ct = default);

        Task<OperationResult<GetManufacturingFormulaAdjustment>> AddBatchRangeAsync(
            Guid id,
            PostManufacturingFormulaAdjustmentBatchRange request,
            CancellationToken ct = default);

        Task<OperationResult<GetManufacturingFormulaAdjustment>> PatchBatchAsync(
            Guid id,
            Guid batchId,
            PostManufacturingFormulaAdjustmentBatchRange request,
            CancellationToken ct = default);

        Task<OperationResult<GetManufacturingFormulaAdjustment>> DeleteBatchAsync(
            Guid id,
            Guid batchId,
            CancellationToken ct = default);

        Task<GetManufacturingFormulaAdjustment?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<GetManufacturingFormulaAdjustmentBatchGroups?> GetBatchGroupsAsync(
            Guid id,
            CancellationToken ct = default);

        Task<GetManufacturingFormulaAdjustmentBatchGroups?> GetBatchGroupsByProductionOrderIdAsync(
            Guid mfgProductionOrderId,
            CancellationToken ct = default);

        Task<List<GetManufacturingFormulaAdjustment>> GetAllAsync(
            ManufacturingFormulaAdjustmentQuery query,
            CancellationToken ct = default);

        Task<OperationResult> DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
