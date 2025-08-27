using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Share.SampleRequestFeature
{
    public class SampleRequestImageRepository : ISampleRequestImageRepository
    {

        private readonly ApplicationDbContext _context;

        public SampleRequestImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(SampleRequestImage entity, CancellationToken ct)
        {
            await _context.SampleRequestImages.AddAsync(entity, ct);
        }

        public async Task<List<SampleRequestImage>> ListAsync(Guid sampleRequestId, CancellationToken ct)
        {
            return await _context.SampleRequestImages
                .Where(img => img.SampleRequestId == sampleRequestId)
                .OrderBy(X => X.SortOrder)
                .ToListAsync(ct);
        }
    }
}
