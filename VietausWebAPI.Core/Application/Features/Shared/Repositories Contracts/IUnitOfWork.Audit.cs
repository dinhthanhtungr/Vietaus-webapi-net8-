using VietausWebAPI.Core.Application.Features.Audits.RepositoriesContracts;

namespace VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        IAuditLogRepository AuditLogRepository { get; }
    }
}