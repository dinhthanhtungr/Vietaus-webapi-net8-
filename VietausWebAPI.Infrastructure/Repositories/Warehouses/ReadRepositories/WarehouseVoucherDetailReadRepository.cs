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
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.Warehouses.ReadRepositories
{
    public class WarehouseVoucherDetailReadRepository : Repository<WarehouseVoucherDetail>, IWarehouseVoucherDetailReadRepository
    {
        private readonly ApplicationDbContext _context;
        public WarehouseVoucherDetailReadRepository(ApplicationDbContext context) : base(context)   
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
            CancellationToken ct,
            long? currentVoucherDetailId = null,
            QcDecision? currentDecision = null)
        {
            if (string.IsNullOrWhiteSpace(poCode) || string.IsNullOrWhiteSpace(materialCode))
                return 0m;

            poCode = poCode.Trim();
            materialCode = materialCode.Trim();

            var acceptedTypeValues = acceptedTypes.Select(x => (int)x).ToArray();

            var rows = await
            (
                from vd in _context.WarehouseVoucherDetails.AsNoTracking()
                join v in _context.WarehouseVouchers.AsNoTracking()
                    on vd.VoucherId equals v.VoucherId
                join wr in _context.WarehouseRequests.AsNoTracking()
                    on v.RequestId equals wr.RequestId
                join qc in _context.QCInputByQCs.AsNoTracking()
                    on vd.VoucherDetailId equals qc.VoucherDetailId into qcGroup
                from qc in qcGroup.DefaultIfEmpty()
                where v.RequestId != null
                      && wr.IsActive
                      && wr.codeFromRequest != null
                      && wr.codeFromRequest.Trim() == poCode
                      && vd.ProductCode != null
                      && vd.ProductCode.Trim() == materialCode
                select new
                {
                    vd.VoucherDetailId,
                    QtyKg = vd.QtyKg,
                    DbDecision = qc != null ? qc.ImportWarehouseType : null
                }
            ).ToListAsync(ct);

            decimal total = 0m;

            foreach (var row in rows)
            {
                QcDecision? effectiveDecision = row.DbDecision;

                // fallback cho dòng hiện tại đang create nhưng DB chưa có QC
                if (!effectiveDecision.HasValue
                    && currentVoucherDetailId.HasValue
                    && row.VoucherDetailId == currentVoucherDetailId.Value)
                {
                    effectiveDecision = currentDecision;
                }

                if (effectiveDecision.HasValue &&
                    acceptedTypeValues.Contains((int)effectiveDecision.Value))
                {
                    total += row.QtyKg;
                }
            }

            return total;
        }


        // ==================================================== Private helper methods (if needed)====================================================
    }
}
