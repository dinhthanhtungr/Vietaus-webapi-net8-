using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.AuditSchema;

namespace VietausWebAPI.Core.Application.Features.TimelineFeature.RepositoriesContracts
{
    public interface IEventLogRepository
    {
        IQueryable<EventLog> Query(bool track = true);
        Task AddAsync(EventLog EventLog, CancellationToken ct = default);
        Task AddRangeAsync(IEnumerable<EventLog> EventLogs, CancellationToken ct = default);
    }
}
