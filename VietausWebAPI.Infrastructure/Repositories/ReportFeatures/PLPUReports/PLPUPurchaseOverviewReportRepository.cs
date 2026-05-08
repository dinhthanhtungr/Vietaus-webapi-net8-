using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.PLPUReports;
using VietausWebAPI.Core.Domain.Enums.Devandqa;
using VietausWebAPI.Core.Domain.Enums.WareHouses;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.ReportFeatures.PLPUReports
{
    public class PLPUPurchaseOverviewReportRepository : IPLPUPurchaseOverviewReportRepository
    {
        private static readonly int[] PurchaseReceiptVoucherTypes =
        {
            (int)WareHouseRequestType.ImportRawMaterial,
            (int)WareHouseRequestType.ImportOther,
            (int)WareHouseRequestType.ImportMaterial
        };

        private readonly ApplicationDbContext _context;

        public PLPUPurchaseOverviewReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PLPUPurchaseReportRawData> GetRawDataAsync(
            PLPUPurchaseOverviewQuery query,
            CancellationToken ct)
        {
            query ??= new PLPUPurchaseOverviewQuery();

            var orderLines = await BuildOrderLineQuery(query).ToListAsync(ct);

            var rawReceipts = await BuildReceiptQuery(query).ToListAsync(ct);

            var receipts = NormalizeReceiptRows(rawReceipts)
                .Select(ApplyReceiptQuantities)
                .ToList();

            var qcRows = receipts
                .Select(ToQcRow)
                .Select(ApplyQcGroup)
                .ToList();

            return new PLPUPurchaseReportRawData
            {
                OrderLines = orderLines,
                Receipts = receipts,
                QcRows = qcRows
            };
        }

        private IQueryable<PLPUPurchaseOrderLineRaw> BuildOrderLineQuery(PLPUPurchaseOverviewQuery query)
        {
            var fromDate = query.From?.Date;
            var toDate = query.To?.Date.AddDays(1);
            var keyword = query.Keyword?.Trim();

            var q =
                from po in _context.PurchaseOrders.AsNoTracking()
                join pod in _context.PurchaseOrderDetails.AsNoTracking()
                    on po.PurchaseOrderId equals pod.PurchaseOrderId
                join s in _context.Suppliers.AsNoTracking()
                    on po.SupplierId equals s.SupplierId into sJoin
                from s in sJoin.DefaultIfEmpty()
                where pod.IsActive
                select new { po, pod, s };

            q = q.Where(x =>
                string.IsNullOrEmpty(x.po.ExternalId) ||
                !EF.Functions.ILike(x.po.ExternalId, "PO%"));

            if (!query.IncludeInactive)
                q = q.Where(x => x.po.IsActive == true);

            if (fromDate.HasValue)
                q = q.Where(x => x.po.CreateDate >= fromDate.Value);

            if (toDate.HasValue)
                q = q.Where(x => x.po.CreateDate < toDate.Value);

            if (query.CompanyId.HasValue)
                q = q.Where(x => x.po.CompanyId == query.CompanyId.Value);

            if (query.PurchaseOrderId.HasValue)
                q = q.Where(x => x.po.PurchaseOrderId == query.PurchaseOrderId.Value);

            if (query.SupplierId.HasValue)
                q = q.Where(x => x.po.SupplierId == query.SupplierId.Value);

            if (query.MaterialId.HasValue)
                q = q.Where(x => x.pod.MaterialId == query.MaterialId.Value);

            if (!string.IsNullOrWhiteSpace(query.OrderType))
                q = q.Where(x => (x.po.OrderType ?? "") == query.OrderType);

            if (!string.IsNullOrWhiteSpace(query.Status))
                q = q.Where(x => (x.po.Status ?? "") == query.Status);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                q = q.Where(x =>
                    (x.po.ExternalId ?? "").Contains(keyword!) ||
                    (x.pod.MaterialExternalIDSnapshot ?? "").Contains(keyword!) ||
                    (x.pod.MaterialNameSnapshot ?? "").Contains(keyword!) ||
                    (x.s != null && (x.s.SupplierName ?? "").Contains(keyword!)) ||
                    (x.s != null && (x.s.ExternalId ?? "").Contains(keyword!))
                );
            }

            return q.Select(x => new PLPUPurchaseOrderLineRaw
            {
                PurchaseOrderId = x.po.PurchaseOrderId,
                POExternalId = x.po.ExternalId ?? string.Empty,
                OrderType = x.po.OrderType ?? string.Empty,

                SupplierId = x.po.SupplierId,
                SupplierName = x.s != null ? (x.s.SupplierName ?? string.Empty) : string.Empty,

                CompanyId = x.po.CompanyId,

                CreatedDate = x.po.CreateDate,
                CreatedBy = x.po.CreatedBy,

                HeaderRequestDeliveryDate = x.po.RequestDeliveryDate,
                RealDeliveryDate = x.po.RealDeliveryDate,

                POStatus = x.po.Status ?? string.Empty,
                PLPUComment = x.po.PLPUComment ?? string.Empty,
                Comment = x.po.Comment ?? string.Empty,
                POIsActive = x.po.IsActive == true,

                PurchaseOrderDetailId = x.pod.PurchaseOrderDetailId,
                LineNo = x.pod.LineNo,

                MaterialId = x.pod.MaterialId,
                MaterialCode = x.pod.MaterialExternalIDSnapshot ?? string.Empty,
                MaterialName = x.pod.MaterialNameSnapshot ?? string.Empty,
                Package = x.pod.Package ?? string.Empty,

                OrderedQuantity = x.pod.RequestQuantity ?? 0,

                BaseCostSnapshot = x.pod.BaseCostSnapshot,
                BaseDateSnapshot = x.pod.BaseDateSnapshot,
                UnitPriceAgreed = x.pod.UnitPriceAgreed,
                TotalPriceAgreed = x.pod.TotalPriceAgreed,

                DeliveryDate = x.pod.DeliveryDate,

                Note = x.pod.Note ?? string.Empty
            });
        }

        private IQueryable<PLPUPurchaseReceiptReportDto> BuildReceiptQuery(PLPUPurchaseOverviewQuery query)
        {
            var fromDate = query.From?.Date;
            var toDate = query.To?.Date.AddDays(1);
            var keyword = query.Keyword?.Trim();

            var q =
                from po in _context.PurchaseOrders.AsNoTracking()
                join s in _context.Suppliers.AsNoTracking()
                    on po.SupplierId equals s.SupplierId into sJoin
                from s in sJoin.DefaultIfEmpty()
                join wr in _context.WarehouseRequests.AsNoTracking()
                    on po.ExternalId equals wr.codeFromRequest
                join v in _context.WarehouseVouchers.AsNoTracking()
                    on wr.RequestId equals v.RequestId
                join vd in _context.WarehouseVoucherDetails.AsNoTracking()
                    on v.VoucherId equals vd.VoucherId
                join ledger in _context.WarehouseShelfLedgers.AsNoTracking()
                    on vd.VoucherDetailId equals ledger.VoucherDetailId
                where PurchaseReceiptVoucherTypes.Contains(v.VoucherType)
                      && ledger.DeltaKg > 0
                let qcLatest = _context.QCInputByQCs.AsNoTracking()
                    .Where(qc => qc.VoucherDetailId == vd.VoucherDetailId)
                    .OrderByDescending(qc => qc.CreatedDate)
                    .Select(qc => new
                    {
                        qc.QCInputByQCId,
                        qc.ImportWarehouseType,
                        qc.CreatedDate
                    })
                    .FirstOrDefault()
                select new { po, s, wr, v, vd, ledger, qcLatest };

            q = q.Where(x =>
                string.IsNullOrEmpty(x.po.ExternalId) ||
                !EF.Functions.ILike(x.po.ExternalId, "PO%"));

            if (!query.IncludeInactive)
                q = q.Where(x => x.po.IsActive == true && x.wr.IsActive);

            if (fromDate.HasValue)
                q = q.Where(x => x.po.CreateDate >= fromDate.Value);

            if (toDate.HasValue)
                q = q.Where(x => x.po.CreateDate < toDate.Value);

            if (query.CompanyId.HasValue)
                q = q.Where(x => x.po.CompanyId == query.CompanyId.Value);

            if (query.PurchaseOrderId.HasValue)
                q = q.Where(x => x.po.PurchaseOrderId == query.PurchaseOrderId.Value);

            if (query.SupplierId.HasValue)
                q = q.Where(x => x.po.SupplierId == query.SupplierId.Value);

            if (query.MaterialId.HasValue)
            {
                q = q.Where(x =>
                    _context.PurchaseOrderDetails.AsNoTracking().Any(pod =>
                        pod.PurchaseOrderId == x.po.PurchaseOrderId &&
                        pod.IsActive &&
                        pod.MaterialId == query.MaterialId.Value &&
                        (pod.MaterialExternalIDSnapshot ?? "") ==
                        (x.ledger.ProductCode ?? x.vd.ProductCode ?? "")
                    )
                );
            }

            if (!string.IsNullOrWhiteSpace(query.OrderType))
                q = q.Where(x => (x.po.OrderType ?? "") == query.OrderType);

            if (!string.IsNullOrWhiteSpace(query.Status))
                q = q.Where(x => (x.po.Status ?? "") == query.Status);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                q = q.Where(x =>
                    (x.po.ExternalId ?? "").Contains(keyword!) ||
                    (x.ledger.ProductCode ?? x.vd.ProductCode ?? "").Contains(keyword!) ||
                    (x.vd.ProductName ?? "").Contains(keyword!) ||
                    (x.ledger.LotNumber ?? x.vd.LotNumber ?? "").Contains(keyword!) ||
                    (x.s != null && (x.s.SupplierName ?? "").Contains(keyword!)) ||
                    (x.s != null && (x.s.ExternalId ?? "").Contains(keyword!))
                );
            }

            return q.Select(x => new PLPUPurchaseReceiptReportDto
            {
                WarehouseRequestId = x.wr.RequestId,
                CodeFromRequest = x.wr.codeFromRequest ?? string.Empty,

                WarehouseVoucherId = x.v.VoucherId,
                WarehouseVoucherCode = x.v.VoucherCode ?? string.Empty,

                WarehouseVoucherDetailId = x.ledger.VoucherDetailId ?? x.vd.VoucherDetailId,
                VoucherType = x.v.VoucherType,

                ReceiptDate = x.ledger.CreatedAt,

                PurchaseOrderId = x.po.PurchaseOrderId,
                PurchaseOrderDetailId = null,

                POExternalId = x.po.ExternalId ?? string.Empty,
                LineNo = 0,

                SupplierId = x.po.SupplierId,
                SupplierName = x.s != null ? (x.s.SupplierName ?? string.Empty) : string.Empty,

                ProductCode = x.ledger.ProductCode ?? x.vd.ProductCode ?? string.Empty,
                ProductName = x.vd.ProductName ?? string.Empty,
                LotNumber = x.ledger.LotNumber ?? x.vd.LotNumber ?? string.Empty,

                QtyKg = x.ledger.DeltaKg,

                QCResult = x.qcLatest != null ? x.qcLatest.ImportWarehouseType : null,
                QCCreatedDate = x.qcLatest != null ? x.qcLatest.CreatedDate : null,

                VoucherDetailType = x.vd.VoucherType
            });
        }

        private static List<PLPUPurchaseReceiptReportDto> NormalizeReceiptRows(
            IEnumerable<PLPUPurchaseReceiptReportDto> rows)
        {
            return rows
                .GroupBy(x => new
                {
                    x.PurchaseOrderId,
                    POExternalId = NormalizeKey(x.POExternalId),
                    x.SupplierId,
                    x.SupplierName,
                    x.WarehouseRequestId,
                    CodeFromRequest = NormalizeKey(x.CodeFromRequest),
                    x.WarehouseVoucherId,
                    x.WarehouseVoucherCode,
                    x.WarehouseVoucherDetailId,
                    x.VoucherType,
                    ProductCode = NormalizeKey(x.ProductCode),
                    ProductName = NormalizeKey(x.ProductName),
                    LotNumber = NormalizeKey(x.LotNumber),
                    x.QCResult,
                    x.QCCreatedDate,
                    x.VoucherDetailType
                })
                .Select(g =>
                {
                    var first = g.First();

                    return new PLPUPurchaseReceiptReportDto
                    {
                        WarehouseRequestId = g.Key.WarehouseRequestId,
                        CodeFromRequest = first.CodeFromRequest,

                        WarehouseVoucherId = g.Key.WarehouseVoucherId,
                        WarehouseVoucherCode = first.WarehouseVoucherCode,

                        WarehouseVoucherDetailId = g.Key.WarehouseVoucherDetailId,
                        VoucherType = g.Key.VoucherType,

                        ReceiptDate = g.Min(x => x.ReceiptDate),

                        PurchaseOrderId = g.Key.PurchaseOrderId,
                        PurchaseOrderDetailId = null,

                        POExternalId = first.POExternalId,
                        LineNo = 0,

                        SupplierId = g.Key.SupplierId,
                        SupplierName = first.SupplierName,

                        ProductCode = first.ProductCode,
                        ProductName = first.ProductName,
                        LotNumber = first.LotNumber,

                        QtyKg = g.Sum(x => x.QtyKg),

                        QCResult = g.Key.QCResult,
                        QCCreatedDate = g.Key.QCCreatedDate,

                        VoucherDetailType = g.Key.VoucherDetailType
                    };
                })
                .ToList();
        }

        private static PLPUPurchaseReceiptReportDto ApplyReceiptQuantities(PLPUPurchaseReceiptReportDto receipt)
        {
            var decision = receipt.QCResult;

            if (!decision.HasValue)
            {
                decision = receipt.VoucherDetailType == VoucherDetailType.QCPass ? QcDecision.QCPass
                    : receipt.VoucherDetailType == VoucherDetailType.Special ? QcDecision.Special
                    : receipt.VoucherDetailType == VoucherDetailType.QCFail ? QcDecision.QCFail
                    : receipt.VoucherDetailType == VoucherDetailType.Waiter ? QcDecision.Waiter
                    : null;
            }

            receipt.QCResult = decision;

            receipt.AcceptedQty = decision is QcDecision.QCPass or QcDecision.Special
                ? receipt.QtyKg
                : 0;

            receipt.PendingQcQty = !decision.HasValue || decision == QcDecision.Waiter
                ? receipt.QtyKg
                : 0;

            receipt.RejectedQty = decision == QcDecision.QCFail
                ? receipt.QtyKg
                : 0;

            receipt.QCGroup = decision is QcDecision.QCPass or QcDecision.Special ? "Accepted"
                : decision == QcDecision.QCFail ? "Rejected"
                : "Pending";

            return receipt;
        }

        private static PLPUPurchaseQcReportDto ToQcRow(PLPUPurchaseReceiptReportDto receipt)
        {
            return new PLPUPurchaseQcReportDto
            {
                QCInputByQCId = null,
                VoucherDetailId = receipt.WarehouseVoucherDetailId,

                PurchaseOrderId = receipt.PurchaseOrderId,
                PurchaseOrderDetailId = receipt.PurchaseOrderDetailId,

                POExternalId = receipt.POExternalId,
                LineNo = receipt.LineNo,

                SupplierId = receipt.SupplierId,
                SupplierName = receipt.SupplierName,

                MaterialCode = receipt.ProductCode,
                MaterialName = receipt.ProductName,
                LotNumber = receipt.LotNumber,

                QtyKg = receipt.QtyKg,

                QCResult = receipt.QCResult,
                QCGroup = receipt.QCGroup,

                ReceiptDate = receipt.ReceiptDate,
                QCCreatedDate = receipt.QCCreatedDate
            };
        }

        private static PLPUPurchaseQcReportDto ApplyQcGroup(PLPUPurchaseQcReportDto row)
        {
            row.QCGroup = row.QCResult is QcDecision.QCPass or QcDecision.Special ? "Accepted"
                : row.QCResult == QcDecision.QCFail ? "Rejected"
                : "Pending";

            row.QCAgeDays = row.QCGroup == "Pending"
                ? Math.Max((DateTime.Today - row.ReceiptDate.Date).Days, 0)
                : 0;

            row.BuyerAction = row.QCGroup == "Rejected" ? "Handle QC fail with supplier"
                : row.QCGroup == "Pending" ? "Follow up QC decision"
                : string.Empty;

            return row;
        }

        private static string NormalizeKey(string? value)
        {
            return (value ?? string.Empty).Trim().ToUpperInvariant();
        }
    }
}