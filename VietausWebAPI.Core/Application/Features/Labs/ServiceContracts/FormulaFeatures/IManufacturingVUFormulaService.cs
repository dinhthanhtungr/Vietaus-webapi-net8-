using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ManufacturingVUFormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Queries.FormulaFeature;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.FormulaFeatures
{
    public interface IManufacturingVUFormulaService
    {
        // ======================================================================== Get ======================================================================== 

        Task<OperationResult<GetManufacturingVUFormula>> GetAsync(Guid id, CancellationToken ct = default);

        Task<OperationResult<PagedResult<GetSummaryManufacturingVUFormula>>> GetAllAsync(ManufacturingVUFormulaQuery req, CancellationToken ct = default);

        // ======================================================================== Post =======================================================================
        Task<OperationResult> CreateAsync(PostManufacturingVUFormula req, CancellationToken ct = default);

        // ====================================================================== Patch ======================================================================
        Task<OperationResult> UpdateAsync(PatchManufacturingVUFormula req, CancellationToken ct = default);

        // ====================================================================== Export PDF =====================================================================
        Task<byte[]> ExportToPdfAsync(Guid manufacturingVUFormulaId, CancellationToken ct = default);

    }
}
    