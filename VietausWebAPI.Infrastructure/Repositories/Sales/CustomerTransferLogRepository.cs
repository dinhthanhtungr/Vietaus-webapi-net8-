using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Sales
{
    public class CustomerTransferLogRepository : ICustomerTransferLogRepository
    {
        private readonly ApplicationDbContext _context;
        public CustomerTransferLogRepository(ApplicationDbContext db) { _context = db; }

        public IQueryable<CustomerTransferLog> Query()
        {
            // Chỉ đọc → NoTracking cho nhẹ. Service sẽ tự Select → DTO.
            return _context.CustomerTransferLogs.AsNoTracking();
        }

        public Task AddAsync(CustomerTransferLog log, CancellationToken ct = default)
        {
            return _context.CustomerTransferLogs.AddAsync(log, ct).AsTask();
        }
    }
}
