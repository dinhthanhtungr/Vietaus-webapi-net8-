using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.Audits.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.AuditSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Audits
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<AuditLog> Query(bool track = false)
        {
            var query = _context.AuditLogs.AsQueryable();
            return track ? query : query.AsNoTracking();
        }

        public async Task AddAsync(AuditLog entity, CancellationToken ct = default)
        {
            await _context.AuditLogs.AddAsync(entity, ct);
        }

        public async Task AddRangeAsync(IEnumerable<AuditLog> entities, CancellationToken ct = default)
        {
            await _context.AuditLogs.AddRangeAsync(entities, ct);
        }

        public void Remove(AuditLog entity)
        {
            _context.AuditLogs.Remove(entity);
        }
    }
}