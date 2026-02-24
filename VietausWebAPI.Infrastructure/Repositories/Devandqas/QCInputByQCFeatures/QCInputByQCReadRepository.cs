using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.Queries.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.RepositoriesContracts.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.DevandqaSchema;
using VietausWebAPI.Core.Domain.Entities.HrSchema;
using VietausWebAPI.Core.Domain.Enums.Devandqa;
using VietausWebAPI.Core.Domain.Enums.WareHouses;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Devandqas.QCInputByQCFeatures
{
    public class QCInputByQCReadRepository : IQCInputByQCReadRepository
    {
        private readonly ApplicationDbContext _context;
        public QCInputByQCReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<GetSummaryQCInput>> GetPagedSummaryAsync(QCInputQuery query, CancellationToken ct)
        {
            var pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
            var pageSize = query.PageSize <= 0 ? 20 : query.PageSize;

            var hasKeyword = !string.IsNullOrWhiteSpace(query.Keyword);
            var kw = hasKeyword ? query.Keyword!.Trim() : "";

            // Nếu keyword của bạn CHỈ search ProductCode/ProductName/Lot thì để false (nhẹ).
            // Nếu keyword cần search cả QCEmployeeName/CSName/CSExternalId thì bật true.
            var keywordIncludesQcFields = false;

            var needQcForFilter =
                query.QCInputByQCId.HasValue ||
                query.HasQC.HasValue ||
                (hasKeyword && keywordIncludesQcFields);

            // =========================
            // PHASE 1: base query để lọc + count + lấy pageIds
            // =========================
            var q1 =
                from vd in _context.WarehouseVoucherDetails.AsNoTracking()
                join v in _context.WarehouseVouchers.AsNoTracking()
                    on vd.VoucherId equals v.VoucherId
                select new { vd, v };

            if (query.VoucherDetailId > 0)
                q1 = q1.Where(x => x.vd.VoucherDetailId == query.VoucherDetailId);

            if (query.VoucherDetailType.HasValue)
                q1 = q1.Where(x => x.v.VoucherType == (int)query.VoucherDetailType.Value);

            if (query.FromDate.HasValue)
                q1 = q1.Where(x => x.v.CreatedDate >= query.FromDate.Value);

            if (query.ToDate.HasValue)
                q1 = q1.Where(x => x.v.CreatedDate <= query.ToDate.Value);

            // Keyword phần vd (nhẹ)
            if (hasKeyword)
            {
                q1 = q1.Where(x =>
                    x.vd.ProductCode.Contains(kw) ||
                    x.vd.ProductName.Contains(kw) ||
                    (x.vd.LotNumber ?? "").Contains(kw)
                );
            }

            IQueryable<long> idsQuery;

            if (!needQcForFilter)
            {
                idsQuery = q1
                    .OrderByDescending(x => x.vd.VoucherDetailId)
                    .Select(x => x.vd.VoucherDetailId);
            }
            else
            {
                // Phase 1 có QC filter (nhưng vẫn chỉ lấy id)
                var q1WithQc =
                    from x in q1
                    let qcLatest = _context.QCInputByQCs.AsNoTracking()
                        .Where(qc => qc.VoucherDetailId == x.vd.VoucherDetailId)
                        .OrderByDescending(qc => qc.CreatedDate)
                        .Select(qc => new
                        {
                            qc.QCInputByQCId,
                            qc.CSName,
                            qc.CSExternalId,
                            qc.CreatedDate,
                            qc.CreatedBy
                        })
                        .FirstOrDefault()
                    join eQc in _context.Employees.AsNoTracking()
                        on qcLatest.CreatedBy equals eQc.EmployeeId into eJoin
                    from eQc in eJoin.DefaultIfEmpty()
                    select new
                    {
                        x.vd,
                        x.v,
                        qc = qcLatest,
                        QCEmployeeName = eQc != null ? eQc.FullName : null
                    };

                if (query.QCInputByQCId.HasValue)
                    q1WithQc = q1WithQc.Where(x => x.qc != null && x.qc.QCInputByQCId == query.QCInputByQCId.Value);

                if (query.HasQC.HasValue)
                    q1WithQc = query.HasQC.Value
                        ? q1WithQc.Where(x => x.qc != null)
                        : q1WithQc.Where(x => x.qc == null);

                // Nếu bạn bật keywordIncludesQcFields = true thì nhớ dùng OR (đừng ép qc != null)
                if (hasKeyword && keywordIncludesQcFields)
                {
                    q1WithQc = q1WithQc.Where(x =>
                        (x.qc != null && (
                            (x.qc.CSName ?? "").Contains(kw) ||
                            (x.qc.CSExternalId ?? "").Contains(kw) ||
                            (x.QCEmployeeName ?? "").Contains(kw)
                        ))
                    );
                }

                idsQuery = q1WithQc
                    .OrderByDescending(x => x.vd.VoucherDetailId)
                    .Select(x => x.vd.VoucherDetailId);
            }

            var total = await idsQuery.CountAsync(ct);

            var pageIds = await idsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            if (pageIds.Count == 0)
                return new PagedResult<GetSummaryQCInput>(new List<GetSummaryQCInput>(), total, pageNumber, pageSize);

            // =========================
            // PHASE 2: lấy đúng page rows rồi mới lấy qcLatest + employee
            // =========================
            var itemsQuery =
                from vd in _context.WarehouseVoucherDetails.AsNoTracking()
                join v in _context.WarehouseVouchers.AsNoTracking()
                    on vd.VoucherId equals v.VoucherId
                where pageIds.Contains(vd.VoucherDetailId)

                let qcLatest = _context.QCInputByQCs.AsNoTracking()
                    .Where(qc => qc.VoucherDetailId == vd.VoucherDetailId)
                    .OrderByDescending(qc => qc.CreatedDate)
                    .Select(qc => new
                    {
                        qc.QCInputByQCId,
                        qc.CreatedDate,
                        qc.CreatedBy
                    })
                    .FirstOrDefault()

                join eQc in _context.Employees.AsNoTracking()
                    on qcLatest.CreatedBy equals eQc.EmployeeId into eJoin
                from eQc in eJoin.DefaultIfEmpty()

                select new GetSummaryQCInput
                {
                    VoucherDetailId = vd.VoucherDetailId,
                    ExternalId = vd.ProductCode,
                    Name = vd.ProductName,
                    LotNumber = vd.LotNumber ?? "",
                    QtyKg = vd.QtyKg,
                    Unit = "kg",
                    VoucherType = vd.VoucherType,

                    HasQC = qcLatest != null,
                    QCInputByQCId = qcLatest != null ? qcLatest.QCInputByQCId : null,
                    QCCreatedDate = qcLatest != null ? qcLatest.CreatedDate : null,
                    QCEmployeeName = (qcLatest != null && eQc != null) ? (eQc.FullName ?? "") : ""
                };

            var items = await itemsQuery
                .OrderByDescending(x => x.VoucherDetailId)
                .ToListAsync(ct);

            return new PagedResult<GetSummaryQCInput>(items, total, pageNumber, pageSize);
        }
        public async Task<QCInputByQC?> GetLatestByVoucherDetailIdAsync(long voucherDetailId, CancellationToken ct)
        {
            return await _context.QCInputByQCs
                .AsNoTracking()
                .Where(x => x.VoucherDetailId == voucherDetailId)
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync(ct);
        }
        public async Task<GetQCInputByQC?> GetDetailByVoucherDetailIdAsync(long voucherDetailId, CancellationToken ct)
        {
            var q =
                from vd in _context.WarehouseVoucherDetails.AsNoTracking()
                join v in _context.WarehouseVouchers.AsNoTracking()
                    on vd.VoucherId equals v.VoucherId
                where vd.VoucherDetailId == voucherDetailId

                // Voucher -> Request
                join r0 in _context.WarehouseRequests.AsNoTracking()
                    on v.RequestId equals r0.RequestId into rJoin
                from r in rJoin.DefaultIfEmpty()

                    // Latest QC
                let qcLatest = _context.QCInputByQCs.AsNoTracking()
                    .Where(qc => qc.VoucherDetailId == vd.VoucherDetailId)
                    .OrderByDescending(qc => qc.CreatedDate)
                    .Select(qc => new
                    {
                        qc.QCInputByQCId,
                        qc.CSName,
                        qc.CSExternalId,
                        qc.InspectionMethod,
                        qc.IsCOAProvided,
                        qc.IsMSDSTDSProvided,
                        qc.IsMetalDetectionRequired,
                        qc.AttachmentCollectionId,
                        qc.ImportWarehouseType,
                        qc.Note,
                        qc.CreatedDate,
                        qc.CreatedBy
                    })
                    .FirstOrDefault()

                // ✅ EmployeeName (safe)
                let qcEmployeeName = qcLatest == null ? null
                    : _context.Employees.AsNoTracking()
                        .Where(emp => emp.EmployeeId == qcLatest.CreatedBy)
                        .Select(emp => emp.FullName)
                        .FirstOrDefault()

                // ✅ OUTER APPLY: resolve PO theo codeFromRequest khi là ImportFromSupplier
                from po in _context.PurchaseOrders.AsNoTracking()
                    .Where(po =>
                        r != null &&
                        r.ReqType == WareHouseRequestType.ImportFromSupplier &&
                        po.ExternalId != null &&
                        po.ExternalId == r.codeFromRequest &&
                        po.IsActive == true
                    )
                    .OrderByDescending(po => po.CreateDate)   // phòng trường hợp ExternalId trùng
                    .Take(1)
                    .DefaultIfEmpty()

                select new GetQCInputByQC
                {
                    // context - material
                    VoucherDetailId = vd.VoucherDetailId,
                    MaterialExternalId = vd.ProductCode,
                    MaterialName = vd.ProductName,
                    LotNumber = vd.LotNumber,
                    QtyKg = vd.QtyKg,
                    Unit = "kg",

                    // ✅ Supplier có trước QC (từ PO)
                    SupplierId = po != null ? po.SupplierId : null,
                    SupplierName = (po != null && po.Supplier != null) ? po.Supplier.SupplierName : null,
                    SupplierExternalId = (po != null && po.Supplier != null) ? po.Supplier.ExternalId : null,

                    // qc (nullable)
                    QCInputByQCId = qcLatest != null ? qcLatest.QCInputByQCId : null,
                    InspectionMethod = qcLatest != null ? qcLatest.InspectionMethod : null,
                    IsCOAProvided = qcLatest != null ? qcLatest.IsCOAProvided : null,
                    IsMSDSTDSProvided = qcLatest != null ? qcLatest.IsMSDSTDSProvided : null,
                    IsMetalDetectionRequired = qcLatest != null ? qcLatest.IsMetalDetectionRequired : null,
                    AttachmentCollectionId = qcLatest != null ? qcLatest.AttachmentCollectionId : null,
                    ImportWarehouseType = qcLatest != null ? qcLatest.ImportWarehouseType : null,
                    Note = qcLatest != null ? qcLatest.Note : null,

                    // qc audit
                    QCCreatedDate = qcLatest != null ? qcLatest.CreatedDate : null,
                    QCCreatedBy = qcLatest != null ? qcLatest.CreatedBy : null,
                    QCEmployeeName = qcEmployeeName
                };

            return await q.FirstOrDefaultAsync(ct);
        }




    }
}
