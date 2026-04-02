using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders.GetRepositories;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts.GetRepositories
{
    public interface IProductionSelectVersionReadRepository
    {
        Task<List<MfgFormulaHistoryRow>> GetFormulaHistoriesByProductionOrderIdsAsync(
            List<Guid> mfgProductionOrderIds,
            CancellationToken ct = default);
    }
}
