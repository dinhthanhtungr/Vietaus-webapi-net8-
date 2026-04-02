using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders.GetRepositories;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts.GetRepositories;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Manufacturing.ProductionSelectVersionRepositories
{
    public class ProductionSelectVersionReadRepository : IProductionSelectVersionReadRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductionSelectVersionReadRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<MfgFormulaHistoryRow>> GetFormulaHistoriesByProductionOrderIdsAsync(
            List<Guid> mfgProductionOrderIds,
            CancellationToken ct = default)
        {
            if (mfgProductionOrderIds == null || mfgProductionOrderIds.Count == 0)
                return new();

            var vers = _context.ProductionSelectVersions.AsNoTracking();
            var mfs = _context.ManufacturingFormulas.AsNoTracking();
            var mpos = _context.MfgProductionOrders.AsNoTracking();

            return await (
                from v in vers
                join mf in mfs on v.ManufacturingFormulaId equals mf.ManufacturingFormulaId
                join mpo in mpos on v.MfgProductionOrderId equals mpo.MfgProductionOrderId
                where mfgProductionOrderIds.Contains(v.MfgProductionOrderId)
                select new MfgFormulaHistoryRow
                {
                    MfgProductionOrderId = v.MfgProductionOrderId,
                    MfgProductionOrderExternalId = mpo.ExternalId,
                    MfgFormulaExternalId = mf.ExternalId,
                    CreatedDate = v.ValidFrom ?? mf.CreatedDate
                }
            ).ToListAsync(ct);
        }
    }
}
