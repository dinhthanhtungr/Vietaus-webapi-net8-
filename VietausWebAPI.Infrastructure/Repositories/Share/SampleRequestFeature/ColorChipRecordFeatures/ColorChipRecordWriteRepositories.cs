using Microsoft.AspNetCore.Server.Kestrel.Core.Features;
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
    public class ColorChipRecordWriteRepositories : IColorChipRecordWriteRepositories
    {
        private readonly ApplicationDbContext _context;
        public ColorChipRecordWriteRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ColorChipRecord> CreateAsync(ColorChipRecord entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.ColorChipRecords.AddAsync(entity, cancellationToken);

            return entity;
        }
    }
}
