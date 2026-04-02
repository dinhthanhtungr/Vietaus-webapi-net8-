using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.DTOs.EndOfShiftReportFeatureDTOs.EndOfShiftReportReadRepositories;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.RepositoriesContracts.EndOfShiftReportFeatures;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.ShiftReportFeatures.EndOfShiftReportRepositories
{
    public class EndOfShiftReportReadRepositories : IEndOfShiftReportReadRepositories
    {
        private readonly ApplicationDbContext _context;

        public EndOfShiftReportReadRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MfgFormulaQuantityRow>> GetFormulaQuantitiesAsync(
            List<string> mfgProductionOrderExternalIds,
            List<string> formulaExternalIds,
            CancellationToken ct = default)
        {
            if (formulaExternalIds == null || formulaExternalIds.Count == 0)
                return new();

            var headers = _context.EndOfShiftReportForAlls.AsNoTracking();
            var details = _context.EndOfShiftReportDetailForAlls.AsNoTracking();

            return await (
                from h in headers
                join d in details on h.ShiftReportForAllId equals d.ShiftReportForAllId
                where h.ExternalId != null
                      && d.ExternalId != null
                      && formulaExternalIds.Contains(h.ExternalId)
                      && formulaExternalIds.Contains(d.ExternalId)
                group new { h, d } by h.ExternalId into g
                select new MfgFormulaQuantityRow
                {
                    // lúc này không còn meaning MPO nữa
                    MfgProductionOrderExternalId = string.Empty,
                    MfgFormulaExternalId = g.Key!,
                    TotalQuantity = g.Sum(x => x.d.WeightStockedKg ?? x.d.NetWeight ?? 0),
                    GoodQuantity = g.Sum(x =>
                        x.h.ProductStatus == "Reported" || x.d.ProductType == "Thanh pham"
                            ? (x.d.WeightStockedKg ?? x.d.NetWeight ?? 0)
                            : 0),
                    ErrorQuantity = g.Sum(x =>
                        x.h.ProductStatus == "Error" || x.h.ProductStatus == "NG"
                            ? (x.d.WeightStockedKg ?? x.d.NetWeight ?? 0)
                            : 0)
                }
            ).ToListAsync(ct);
        }
    }
}
