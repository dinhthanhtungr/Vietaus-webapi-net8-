using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts.ReadRepositories;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;
using VietausWebAPI.Core.Domain.Enums.Devandqa;
using VietausWebAPI.Core.Domain.Enums.WareHouses;
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

        public async Task<decimal> SumAcceptedQtyByPoCodeAndMaterialCodeAsync(
            string poCode,
            string materialCode,
            QcDecision[] acceptedTypes,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(poCode) || string.IsNullOrWhiteSpace(materialCode))
                return 0m;

            var acceptedTypeValues = acceptedTypes.Select(x => (int)x).ToArray();

            return await
            (
                from vd in _context.WarehouseVoucherDetails.AsNoTracking()
                join v in _context.WarehouseVouchers.AsNoTracking()
                    on vd.VoucherId equals v.VoucherId
                join wr in _context.WarehouseRequests.AsNoTracking()
                    on v.RequestId equals wr.RequestId
                join qc in _context.QCInputByQCs.AsNoTracking()
                    on vd.VoucherDetailId equals qc.VoucherDetailId
                where v.RequestId != null
                      && wr.IsActive
                      && wr.codeFromRequest == poCode
                      && vd.ProductCode == materialCode
                      && qc.ImportWarehouseType.HasValue
                      && acceptedTypeValues.Contains((int)qc.ImportWarehouseType.Value)
                select (decimal?)vd.QtyKg
            ).SumAsync(ct) ?? 0m;
        }


        // ==================================================== Private helper methods (if needed)====================================================
    }
}
