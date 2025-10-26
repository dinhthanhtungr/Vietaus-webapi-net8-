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
    public class DeliveryOrderDetailRepository : IDeliveryOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public DeliveryOrderDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(DeliveryOrderDetail deliveryOrderDetail, CancellationToken ct = default)
        {
            await _context.DeliveryOrderDetails.AddAsync(deliveryOrderDetail, ct);
        }

        public IQueryable<DeliveryOrderDetail> Query(bool track = true)
        {
            var db = _context.DeliveryOrderDetails.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
