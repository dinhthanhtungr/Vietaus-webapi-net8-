using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.DTOs.EndOfShiftReportFeatureDTOs.EndOfShiftReportReadRepositories;

namespace VietausWebAPI.Core.Application.Features.ShiftReportFeatures.RepositoriesContracts.EndOfShiftReportFeatures
{
    public interface IEndOfShiftReportReadRepositories
    {
        Task<List<MfgFormulaQuantityRow>> GetFormulaQuantitiesAsync(
            List<string> mfgProductionOrderExternalIds,
            List<string> formulaExternalIds,
            CancellationToken ct = default);
    }
}
