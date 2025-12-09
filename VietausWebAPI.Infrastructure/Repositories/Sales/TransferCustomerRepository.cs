using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Sales
{
    public class TransferCustomerRepository : ITransferCustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public TransferCustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<CustomerTransferLog> Query(bool track = false)
        {
            var db = _context.CustomerTransferLogs.AsQueryable();
            return track ? db : db.AsNoTracking();
        }


        public Task AddAsync(CustomerTransferLog log, CancellationToken ct = default)
        {
            return _context.CustomerTransferLogs.AddAsync(log, ct).AsTask();
        }
    }
}
