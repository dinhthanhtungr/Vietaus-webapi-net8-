using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.Share.SampleRequestFeature
{
    public class FormulaMaterialSnapshotRepository : Repository<FormulaMaterialSnapshot>, IFormulaMaterialSnapshotRepository
    {
        public FormulaMaterialSnapshotRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task AddSnapshotsFromFormulaAsync(Guid manufacturingVUFormulaId, Guid formulaId, CancellationToken ct = default)
        {
            var mats = await _context.Set<FormulaMaterial>()
                .AsNoTracking()
                .Where(x => x.FormulaId == formulaId && x.IsActive)
                .OrderBy(x => x.LineNo)
                .Select(x => new
                {
                    x.LineNo,
                    x.Quantity,
                    x.UnitPrice,
                    x.itemType,
                    x.MaterialNameSnapshot,
                    x.MaterialExternalIdSnapshot,
                    x.Unit,
                    x.CategoryId
                })
                .ToListAsync(ct);

            if (mats.Count == 0) return;

            var snaps = mats.Select(x => new FormulaMaterialSnapshot
            {
                FormulaMaterialSnapshotId = Guid.CreateVersion7(),
                ManufacturingVUFormulaId = manufacturingVUFormulaId,
                CategoryId = x.CategoryId,

                LineNo = x.LineNo,

                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                TotalPrice = x.Quantity * x.UnitPrice,
                itemType = x.itemType,
                MaterialNameSnapshot = x.MaterialNameSnapshot,
                MaterialExternalIdSnapshot = x.MaterialExternalIdSnapshot,
                Unit = x.Unit,
                IsActive = true
            });

            await AddRangeAsync(snaps, ct);
        }
    } 
}
