using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Sales
{
    public class TransferCustomerRepository : ITransferCustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public TransferCustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<CustomerTransferLog> Query()
        {
            // Chỉ đọc → NoTracking cho nhẹ
            return _context.CustomerTransferLogs.AsNoTracking();
        }

        public Task AddAsync(CustomerTransferLog log, CancellationToken ct = default)
        {
            return _context.CustomerTransferLogs.AddAsync(log, ct).AsTask();
        }
    }
}
