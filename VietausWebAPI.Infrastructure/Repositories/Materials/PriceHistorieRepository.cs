using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Materials
{
    public class PriceHistorieRepository : IPriceHistorieRepository
    {
        private readonly ApplicationDbContext _context;
        public PriceHistorieRepository(ApplicationDbContext context)
        {
            _context = context; 
        }

        public IQueryable<PriceHistory> Query(bool track = false)
        {
            var db = _context.PriceHistories.AsQueryable();
            return track ? db : db.AsNoTracking();
        }

        /// <summary>
        /// Thêm một giá mới
        /// </summary>
        /// <param name="sampleRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task AddAsync(PriceHistory newPrice, CancellationToken ct = default)
        {
            await _context.PriceHistories.AddAsync(newPrice, ct);
        }
    }
}
