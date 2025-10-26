using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Sales
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AttachmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task AddRangeAsync(IEnumerable<OrderAttachment> attachments, CancellationToken ct = default)
        {
            await _context.OrderAttachments.AddRangeAsync(attachments, ct);
        }

        public async Task<List<OrderAttachment>> GetByOrderAsync(Guid orderId, CancellationToken ct = default)
        {
             return await _context.OrderAttachments
                .Where(a => a.MerchandiseOrderId == orderId)
                .ToListAsync(ct);
        }

        public IQueryable<OrderAttachment> Query(bool track = false)
        {
            var db = _context.OrderAttachments.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
