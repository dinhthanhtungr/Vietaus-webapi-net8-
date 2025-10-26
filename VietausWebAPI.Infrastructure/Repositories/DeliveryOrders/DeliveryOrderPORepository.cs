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
    public class DeliveryOrderPORepository : IDeliveryOrderPORepository
    {
        private readonly ApplicationDbContext _context;

        public DeliveryOrderPORepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DeliveryOrderPO deliveryOrderPO, CancellationToken ct = default)
        {
            await _context.DeliveryOrderPOs.AddAsync(deliveryOrderPO, ct);
        }

        public async Task AddRangeAsync(IEnumerable<DeliveryOrderPO> deliveryOrderPOs, CancellationToken ct = default)
        {
            await _context.DeliveryOrderPOs.AddRangeAsync(deliveryOrderPOs, ct);
        }

        public IQueryable<DeliveryOrderPO> Query(bool track = true)
        {
            return track ? _context.DeliveryOrderPOs.AsTracking() : _context.DeliveryOrderPOs.AsNoTracking();
        }
    }
}
