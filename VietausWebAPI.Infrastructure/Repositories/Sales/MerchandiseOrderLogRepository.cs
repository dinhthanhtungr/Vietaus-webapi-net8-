using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Sales
{
    public class MerchandiseOrderLogRepository : IMerchandiseOrderLogRepository
    {
        private readonly ApplicationDbContext _context;

        public MerchandiseOrderLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<MerchandiseOrderLog> logs, CancellationToken ct = default)
        {
            await _context.MerchandiseOrderLogs.AddRangeAsync(logs, ct);
        }

        public IQueryable<MerchandiseOrderLog> Query(bool track = false)
        {
            var db = _context.MerchandiseOrderLogs.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
