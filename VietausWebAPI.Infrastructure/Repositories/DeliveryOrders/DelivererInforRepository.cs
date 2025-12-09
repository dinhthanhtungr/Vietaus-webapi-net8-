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
    public class DelivererInforRepository : IDelivererInforRepository
    {
        private readonly ApplicationDbContext _context;

        public DelivererInforRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(DelivererInfor delivererInfor, CancellationToken ct = default)
        {
            await _context.DelivererInfors.AddAsync(delivererInfor, ct);
        }

        public IQueryable<DelivererInfor> Query(bool track = true)
        {
            var db = _context.DelivererInfors.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
