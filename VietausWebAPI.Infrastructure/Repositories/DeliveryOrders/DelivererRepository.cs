using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.DeliveryOrders
{
    public class DelivererRepository : IDelivererRepository
    {
        private readonly ApplicationDbContext _context;

        public DelivererRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Deliverer deliverer, CancellationToken ct = default)
        {
            await _context.Deliverers.AddAsync(deliverer, ct);
        }

        public IQueryable<Deliverer> Query(bool track = true)
        {
            var db = _context.Deliverers.AsQueryable();
            return track ? db : db.AsNoTracking();
        }

        public Task RemoveAsync(Deliverer deliverer, CancellationToken ct = default)
        {
            _context.Deliverers.Remove(deliverer);
            return Task.CompletedTask;
        }

    }
}
