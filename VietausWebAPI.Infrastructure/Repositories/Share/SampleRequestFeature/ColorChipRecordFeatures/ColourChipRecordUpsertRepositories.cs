using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature.ColorChipRecordFeatures;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Share.SampleRequestFeature.ColorChipRecordFeatures
{
    public class ColourChipRecordUpsertRepositories : IColorChipRecordUpsertRepositories
    {
        private readonly ApplicationDbContext _context;

        public ColourChipRecordUpsertRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ColorChipRecord?> GetByProductIdAsync(
            Guid productId,
            CancellationToken cancellationToken = default)
        {
            return await _context.ColorChipRecords
                .Include(x => x.DevelopmentFormulas)
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.IsActive, cancellationToken);
        }
    }
}
