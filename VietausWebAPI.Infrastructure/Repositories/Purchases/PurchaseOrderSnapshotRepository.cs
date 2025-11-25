using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Purchases
{
    public class PurchaseOrderSnapshotRepository : IPurchaseOrderSnapshotRepository
    {
        private readonly ApplicationDbContext _context;
        public PurchaseOrderSnapshotRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(PurchaseOrderSnapshot PurchaseOrderSnapshot, CancellationToken ct = default)
        {
            await _context.PurchaseOrderSnapshots.AddAsync(PurchaseOrderSnapshot, ct);
        }

        public IQueryable<PurchaseOrderSnapshot> Query(bool track = true)
        {
            var db = _context.PurchaseOrderSnapshots.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
