using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.UpsertInformationDtos;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts.MFGProductionOrderFeatures
{
    public interface IMfgUpsertInformationService
    {
        Task<OperationResult> ExportFromStockAsync(PatchStockMfgProductionOrderRequest req, CancellationToken ct = default);
        Task<OperationResult> SaveAndCreateFormulaAsync(PostMfgFormulaAndUpdateMpoRequest req, CancellationToken ct = default);
    }
}
