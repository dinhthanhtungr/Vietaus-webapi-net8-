using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts.ReadRepositories;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Warehouses.ReadRepositories
{
    public class WarehouseVoucherDetailReadRepository : IWarehouseVoucherDetailReadRepository
    {
        private readonly ApplicationDbContext _context;
        public WarehouseVoucherDetailReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WarehouseVoucherDetail?> GetByIdAsync(long voucherDetailId, CancellationToken ct)
        {
            return await _context.WarehouseVoucherDetails
                .FirstOrDefaultAsync(x => x.VoucherDetailId == voucherDetailId, ct);
        }
    }
}
