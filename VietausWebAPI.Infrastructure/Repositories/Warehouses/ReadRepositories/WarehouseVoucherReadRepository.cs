using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts.ReadRepositories;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.Warehouses.ReadRepositories
{
    public class WarehouseVoucherReadRepository : Repository<WarehouseVoucher>, IWarehouseVoucherReadRepository
    {
        private readonly ApplicationDbContext _context;
        public WarehouseVoucherReadRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<WarehouseVoucher?> GetByIdAsync(long voucherId, CancellationToken ct)
        {
            return await _context.WarehouseVouchers
                .FirstOrDefaultAsync(x => x.VoucherId == voucherId, ct);
        }


    }
}
