using Microsoft.AspNetCore.Server.Kestrel.Core.Features;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature.ColorChipRecordFeatures;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.Share.SampleRequestFeature.ColorChipRecordFeatures
{
    public class ColourChipRecordWriteRepositories : Repository<ColorChipRecord>, IColorChipRecordWriteRepositories
    {
        public ColourChipRecordWriteRepositories(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ColorChipRecordDevelopmentFormula> ColorChipRecordDevelopmentFormulaCreateAsync(ColorChipRecordDevelopmentFormula entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.ColorChipRecordDevelopmentFormulas.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<ColorChipRecord> CreateAsync(
            ColorChipRecord entity,
            CancellationToken cancellationToken = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.ColorChipRecords.AddAsync(entity, cancellationToken);
            return entity;
        }

        public IQueryable<ColorChipRecordDevelopmentFormula> QueryDetail(bool track = false)
        {
            var db = _context.ColorChipRecordDevelopmentFormulas.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
