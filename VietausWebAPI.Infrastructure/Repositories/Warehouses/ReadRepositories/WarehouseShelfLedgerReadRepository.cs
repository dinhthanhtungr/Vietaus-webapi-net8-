using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts.ReadRepositories;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.Warehouses.ReadRepositories
{
    public class WarehouseShelfLedgerReadRepository : Repository<WarehouseShelfLedger>, IWarehouseShelfLedgerReadRepository
    {
        public WarehouseShelfLedgerReadRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
