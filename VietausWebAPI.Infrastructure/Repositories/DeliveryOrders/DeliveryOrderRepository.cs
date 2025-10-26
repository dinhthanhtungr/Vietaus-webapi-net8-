using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.DeliveryOrders
{
    public class DeliveryOrderRepository : IDeliveryOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public DeliveryOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(DeliveryOrder deliveryOrder, CancellationToken ct = default)
        {
            await _context.DeliveryOrders.AddAsync(deliveryOrder, ct);
        }

        public IQueryable<DeliveryOrder> Query(bool track = true)
        {
            var db = _context.DeliveryOrders.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
