using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.TimelineFeature.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

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
