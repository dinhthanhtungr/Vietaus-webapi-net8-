using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Purchases
{
    public class PurchaseOrderDetailRepository : IPurchaseOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrderDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(PurchaseOrderDetail PurchaseOrderDetail, CancellationToken ct = default)
        {
            await _context.PurchaseOrderDetails.AddAsync(PurchaseOrderDetail, ct);
        }

        public IQueryable<PurchaseOrderDetail> Query(bool track = true)
        {
            var db = _context.PurchaseOrderDetails.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
