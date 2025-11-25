
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;
using VietausWebAPI.Core.Application.Features.TimelineFeature.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.AuditSchema;
using Microsoft.EntityFrameworkCore;

namespace VietausWebAPI.Infrastructure.Repositories.Audits
{
    public class EventLogRepository : IEventLogRepository
    {
        private readonly ApplicationDbContext _context;

        public EventLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(EventLog EventLogs, CancellationToken ct = default)
        {
            await _context.EventLogs.AddAsync(EventLogs, ct);
        }

        public async Task AddRangeAsync(IEnumerable<EventLog> EventLogs, CancellationToken ct = default)
        {
            await _context.EventLogs.AddRangeAsync(EventLogs, ct);
        }

        public IQueryable<EventLog> Query(bool track = true)
        {
            var db = _context.EventLogs.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
