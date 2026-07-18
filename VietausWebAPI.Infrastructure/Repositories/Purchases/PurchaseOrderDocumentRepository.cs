using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Purchases
{
    public class PurchaseOrderDocumentRepository : IPurchaseOrderDocumentRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrderDocumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<PurchaseOrderDocument> Query(bool track = false)
        {
            var db = _context.PurchaseOrderDocuments.AsQueryable();
            return track ? db : db.AsNoTracking();
        }

        public async Task AddAsync(PurchaseOrderDocument entity, CancellationToken ct = default)
        {
            await _context.PurchaseOrderDocuments.AddAsync(entity, ct);
        }

        public async Task AddRangeAsync(IEnumerable<PurchaseOrderDocument> entities, CancellationToken ct = default)
        {
            await _context.PurchaseOrderDocuments.AddRangeAsync(entities, ct);
        }

        public void Remove(PurchaseOrderDocument entity)
        {
            _context.PurchaseOrderDocuments.Remove(entity);
        }
    }
}
