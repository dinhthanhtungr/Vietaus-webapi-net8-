using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Domain.Entities.AuditSchema;

namespace VietausWebAPI.Core.Application.Features.Audits.RepositoriesContracts
{
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
    }
}