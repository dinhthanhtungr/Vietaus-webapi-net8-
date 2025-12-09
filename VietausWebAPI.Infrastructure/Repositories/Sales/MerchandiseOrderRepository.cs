using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Sales
{
    public class MerchandiseOrderRepository : IMerchandiseOrderRepository
    {

        private readonly ApplicationDbContext _context;
        public MerchandiseOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MerchandiseOrder merchandiseOrder, CancellationToken ct = default)
        {
            await _context.MerchandiseOrders.AddAsync(merchandiseOrder, ct);
        }

        public async Task<string?> GetLatestExternalIdStartsWithAsync(string prefix)
        {
            return await _context.MerchandiseOrders
                .Where(e => e.ExternalId.StartsWith(prefix))
                .OrderByDescending(e => e.ExternalId.Length)   // dài hơn => số lớn hơn
                .ThenByDescending(e => e.ExternalId)           // cùng độ dài thì so chuỗi
                .Select(e => e.ExternalId)
                .FirstOrDefaultAsync();
        }
        public IQueryable<MerchandiseOrder> Query(bool track = false)
        {
            var db = _context.MerchandiseOrders.AsQueryable();
            return track ? db : db.AsNoTracking();
        }

        public IQueryable<MerchandiseOrderDetail> QueryDetail(bool track = false)
        {
            var db = _context.MerchandiseOrderDetails.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
