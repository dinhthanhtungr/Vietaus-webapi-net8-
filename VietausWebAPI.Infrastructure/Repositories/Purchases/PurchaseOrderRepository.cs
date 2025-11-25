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
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public PurchaseOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PurchaseOrder PurchaseOrder, CancellationToken ct = default)
        {
            await _context.PurchaseOrders.AddAsync(PurchaseOrder, ct);
        }

        public IQueryable<PurchaseOrder> Query(bool track = true)
        {
            var db = _context.PurchaseOrders.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
