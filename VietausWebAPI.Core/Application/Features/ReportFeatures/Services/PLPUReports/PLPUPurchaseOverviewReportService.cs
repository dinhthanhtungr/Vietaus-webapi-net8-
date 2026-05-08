using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.ServiceContracts.PLPUReports;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.Services.PLPUReports
{
    public class PLPUPurchaseOverviewReportService : IPLPUPurchaseOverviewReportService
    {
        private const string StatusCancelled = "Cancelled";
        private const string StatusCanceled = "Canceled";
        private const string StatusClosed = "Closed";


        private const decimal CompletionTolerancePercent = 2m;
        private const decimal OverDeliveryTolerancePercent = 2m;

        private readonly IPLPUPurchaseOverviewReportRepository _repository;

        public PLPUPurchaseOverviewReportService(IPLPUPurchaseOverviewReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<PLPUPurchaseOverviewReportDto> GetOverviewAsync(
            PLPUPurchaseOverviewQuery query,
            CancellationToken ct = default)
        {
            query ??= new PLPUPurchaseOverviewQuery();

            var rawData = await _repository.GetRawDataAsync(query, ct);

            var allocatedReceipts = AllocateReceiptsToOrderLines(rawData.OrderLines, rawData.Receipts);
            var details = BuildDetailRows(rawData.OrderLines, allocatedReceipts);
            var headers = BuildHeaderRows(rawData.OrderLines, details);

            if (query.OnlyNeedAction)
                headers = headers.Where(x => x.NeedBuyerAction).ToList();

            var pageNumber = NormalizePageNumber(query.PageNumber);
            var pageSize = NormalizePageSize(query.PageSize);

            var pagedHeaders = headers
                .OrderByDescending(x => x.NeedBuyerAction)
                .ThenByDescending(x => x.IsOverdue)
                .ThenByDescending(x => x.HasQCFail)
                .ThenByDescending(x => x.HasPendingQC)
                .ThenBy(x => x.RequestDeliveryDate ?? DateTime.MaxValue)
                .ThenByDescending(x => x.CreatedDate)
                .ToList();

            var summary = BuildSummary(headers, details);

            var localizedPagedHeaders = LocalizeHeaders(pagedHeaders);

            var localizedNeedActionHeaders = LocalizeHeaders(
                headers
                    .Where(x => x.NeedBuyerAction)
                    .OrderByDescending(x => x.IsOverdue)
                    .ThenByDescending(x => x.HasQCFail)
                    .ThenByDescending(x => x.HasPendingQC)
                    .ThenByDescending(x => x.HasOverDelivery)
                    .ThenBy(x => x.RequestDeliveryDate ?? DateTime.MaxValue)
                    .Take(20)
            );

            return new PLPUPurchaseOverviewReportDto
            {
                Summary = summary,
                PurchaseOrders = ToPagedResult(localizedPagedHeaders, pageNumber, pageSize),
                NeedActionPurchaseOrders = localizedNeedActionHeaders,
                SupplierPerformance = BuildSupplierPerformance(rawData.OrderLines, details, headers)
                    .OrderByDescending(x => x.RiskScore)
                    .ThenBy(x => x.SupplierName)
                    .ToList()
            };
        }

        public async Task<PLPUPurchaseOrderHeaderDashboardDto> GetHeaderReportAsync(
            PLPUPurchaseOverviewQuery query,
            CancellationToken ct = default)
        {
            query ??= new PLPUPurchaseOverviewQuery();

            var rawData = await _repository.GetRawDataAsync(query, ct);

            var allocatedReceipts = AllocateReceiptsToOrderLines(rawData.OrderLines, rawData.Receipts);
            var details = BuildDetailRows(rawData.OrderLines, allocatedReceipts);
            var headers = BuildHeaderRows(rawData.OrderLines, details);

            return BuildHeaderDashboard(rawData.OrderLines, details, headers);
        }

        public async Task<PagedResult<PLPUPurchaseOrderRowDto>> GetRowsAsync(
            PLPUPurchaseOverviewQuery query,
            CancellationToken ct = default)
        {
            query ??= new PLPUPurchaseOverviewQuery();

            var rawData = await _repository.GetRawDataAsync(query, ct);

            var allocatedReceipts = AllocateReceiptsToOrderLines(rawData.OrderLines, rawData.Receipts);
            var details = BuildDetailRows(rawData.OrderLines, allocatedReceipts);
            var headers = BuildHeaderRows(rawData.OrderLines, details);

            if (query.OnlyNeedAction)
                headers = headers.Where(x => x.NeedBuyerAction).ToList();

            var rows = headers
                .OrderByDescending(x => x.NeedBuyerAction)
                .ThenByDescending(x => x.IsOverdue)
                .ThenByDescending(x => x.HasQCFail)
                .ThenBy(x => x.RequestDeliveryDate ?? DateTime.MaxValue)
                .ThenByDescending(x => x.CreatedDate)
                .Select(x => new PLPUPurchaseOrderRowDto
                {
                    PurchaseOrderId = x.PurchaseOrderId,
                    POExternalId = x.POExternalId,
                    OrderType = x.OrderType,

                    SupplierId = x.SupplierId,
                    SupplierName = x.SupplierName,

                    CreatedDate = x.CreatedDate,
                    RequestDeliveryDate = x.RequestDeliveryDate,
                    RealDeliveryDate = x.RealDeliveryDate,

                    POStatus = x.POStatus,
                    ReportStatus = ToVietnameseStatus(x.ReportStatus),
                    AlertFlags = ToVietnameseFlags(x.AlertFlags),

                    TotalLines = x.TotalLines,

                    OrderedQuantity = x.OrderedQuantity,
                    WarehouseReceivedQuantity = x.WarehouseReceivedQuantity,
                    AcceptedQuantity = x.AcceptedQuantity,
                    PendingQcQuantity = x.PendingQcQuantity,
                    RejectedQuantity = x.RejectedQuantity,

                    RemainingToAccept = x.RemainingToAccept,
                    CompletionPercent = x.CompletionPercent,
                    TotalPurchaseValue = x.TotalPurchaseValue,

                    IsOverdue = x.IsOverdue,
                    DelayDays = x.DelayDays,

                    NeedBuyerAction = x.NeedBuyerAction,
                    BuyerAction = ToVietnameseBuyerAction(x.BuyerAction),

                    PLPUComment = x.PLPUComment
                })
                .ToList();

            return ToPagedResult(rows, NormalizePageNumber(query.PageNumber), NormalizePageSize(query.PageSize));
        }

        public async Task<PLPUPurchaseOrderDetailDashboardDto> GetOrderDetailAsync(
            Guid purchaseOrderId,
            CancellationToken ct = default)
        {
            var query = new PLPUPurchaseOverviewQuery
            {
                PurchaseOrderId = purchaseOrderId,
                IncludeInactive = true
            };

            var rawData = await _repository.GetRawDataAsync(query, ct);

            var allocatedReceipts = AllocateReceiptsToOrderLines(rawData.OrderLines, rawData.Receipts);
            var details = BuildDetailRows(rawData.OrderLines, allocatedReceipts);
            var headers = BuildHeaderRows(rawData.OrderLines, details);
            var qcRows = BuildQcRowsFromAllocatedReceipts(allocatedReceipts);

            return new PLPUPurchaseOrderDetailDashboardDto
            {
                Header = LocalizeHeaders(headers).FirstOrDefault() ?? new PLPUPurchaseOrderHeaderReportDto(),

                Lines = LocalizeDetails(
                    details
                        .OrderBy(x => x.LineNo)
                        .ThenBy(x => x.MaterialCode)
                ),

                Receipts = LocalizeReceipts(
                    allocatedReceipts
                        .OrderByDescending(x => x.ReceiptDate)
                        .ThenBy(x => x.LineNo)
                        .ThenBy(x => x.MaterialCode())
                ),

                QcRows = LocalizeQcRows(
                    qcRows
                        .OrderByDescending(x => x.QCGroup == "Rejected")
                        .ThenByDescending(x => x.QCGroup == "Pending")
                        .ThenByDescending(x => x.ReceiptDate)
                )
            };
        }

        public async Task<PagedResult<PLPUPurchaseOrderDetailReportDto>> GetDetailLinesAsync(
            PLPUPurchaseOverviewQuery query,
            CancellationToken ct = default)
        {
            query ??= new PLPUPurchaseOverviewQuery();

            var rawData = await _repository.GetRawDataAsync(query, ct);

            var allocatedReceipts = AllocateReceiptsToOrderLines(rawData.OrderLines, rawData.Receipts);

            var details = BuildDetailRows(rawData.OrderLines, allocatedReceipts)
                .OrderByDescending(x => x.NeedBuyerAction)
                .ThenBy(x => x.RequestDeliveryDate ?? DateTime.MaxValue)
                .ThenBy(x => x.POExternalId)
                .ThenBy(x => x.LineNo)
                .ToList();

            if (query.OnlyNeedAction)
                details = details.Where(x => x.NeedBuyerAction).ToList();

            return ToPagedResult(
                LocalizeDetails(details),
                NormalizePageNumber(query.PageNumber),
                NormalizePageSize(query.PageSize));
        }

        public async Task<PagedResult<PLPUPurchaseReceiptReportDto>> GetReceiptsAsync(
            PLPUPurchaseOverviewQuery query,
            CancellationToken ct = default)
        {
            query ??= new PLPUPurchaseOverviewQuery();

            var rawData = await _repository.GetRawDataAsync(query, ct);

            var allocatedReceipts = AllocateReceiptsToOrderLines(rawData.OrderLines, rawData.Receipts)
                .OrderByDescending(x => x.ReceiptDate)
                .ThenByDescending(x => x.WarehouseVoucherDetailId)
                .ToList();

            return ToPagedResult(
                 LocalizeReceipts(allocatedReceipts),
                 NormalizePageNumber(query.PageNumber),
                 NormalizePageSize(query.PageSize));
        }

        public async Task<PagedResult<PLPUPurchaseQcReportDto>> GetQcAsync(
            PLPUPurchaseOverviewQuery query,
            CancellationToken ct = default)
        {
            query ??= new PLPUPurchaseOverviewQuery();

            var rawData = await _repository.GetRawDataAsync(query, ct);

            var allocatedReceipts = AllocateReceiptsToOrderLines(rawData.OrderLines, rawData.Receipts);
            var qcRows = BuildQcRowsFromAllocatedReceipts(allocatedReceipts)
                .OrderByDescending(x => x.QCGroup == "Rejected")
                .ThenByDescending(x => x.QCGroup == "Pending")
                .ThenByDescending(x => x.ReceiptDate)
                .ToList();

            return ToPagedResult(
                LocalizeQcRows(qcRows),
                NormalizePageNumber(query.PageNumber),
                NormalizePageSize(query.PageSize));
        }

        public async Task<List<PLPUSupplierPurchasePerformanceDto>> GetSupplierPerformanceAsync(
            PLPUPurchaseOverviewQuery query,
            CancellationToken ct = default)
        {
            query ??= new PLPUPurchaseOverviewQuery();

            var rawData = await _repository.GetRawDataAsync(query, ct);

            var allocatedReceipts = AllocateReceiptsToOrderLines(rawData.OrderLines, rawData.Receipts);
            var details = BuildDetailRows(rawData.OrderLines, allocatedReceipts);
            var headers = BuildHeaderRows(rawData.OrderLines, details);

            return BuildSupplierPerformance(rawData.OrderLines, details, headers)
                .OrderByDescending(x => x.RiskScore)
                .ThenBy(x => x.SupplierName)
                .ToList();
        }

        private static List<PLPUPurchaseReceiptReportDto> AllocateReceiptsToOrderLines(
            List<PLPUPurchaseOrderLineRaw> orderLines,
            List<PLPUPurchaseReceiptReportDto> receipts)
        {
            var result = new List<PLPUPurchaseReceiptReportDto>();

            if (orderLines.Count == 0 || receipts.Count == 0)
                return result;

            var linesByPoAndMaterial = orderLines
                .GroupBy(x => ReceiptMatchKey(x.POExternalId, x.MaterialCode))
                .ToDictionary(
                    x => x.Key,
                    x => x
                        .OrderBy(r => r.LineNo)
                        .ThenBy(r => r.PurchaseOrderDetailId)
                        .ToList()
                );

            var allocatedPhysicalQtyByLine = orderLines.ToDictionary(
                x => x.PurchaseOrderDetailId,
                _ => 0m
            );

            foreach (var receiptGroup in receipts
                         .OrderBy(x => x.ReceiptDate)
                         .ThenBy(x => x.WarehouseVoucherDetailId)
                         .GroupBy(x => ReceiptMatchKey(x.POExternalId, x.ProductCode)))
            {
                if (!linesByPoAndMaterial.TryGetValue(receiptGroup.Key, out var candidateLines) ||
                    candidateLines.Count == 0)
                {
                    // Không tìm được line tương ứng thì vẫn giữ receipt để debug,
                    // nhưng PurchaseOrderDetailId = null.
                    result.AddRange(receiptGroup);
                    continue;
                }

                foreach (var receipt in receiptGroup)
                {
                    var remainingReceiptQty = receipt.QtyKg;

                    if (remainingReceiptQty <= 0)
                        continue;

                    while (remainingReceiptQty > 0)
                    {
                        var targetLine = candidateLines.FirstOrDefault(line =>
                        {
                            var allocated = allocatedPhysicalQtyByLine[line.PurchaseOrderDetailId];
                            return allocated < line.OrderedQuantity;
                        });

                        targetLine ??= candidateLines.Last();

                        var targetAllocated = allocatedPhysicalQtyByLine[targetLine.PurchaseOrderDetailId];
                        var targetRemaining = Positive(targetLine.OrderedQuantity - targetAllocated);

                        var allocatedQty = targetRemaining > 0
                            ? Math.Min(remainingReceiptQty, targetRemaining)
                            : remainingReceiptQty;

                        if (allocatedQty <= 0)
                            allocatedQty = remainingReceiptQty;

                        result.Add(CloneReceiptForLine(receipt, targetLine, allocatedQty));

                        allocatedPhysicalQtyByLine[targetLine.PurchaseOrderDetailId] += allocatedQty;
                        remainingReceiptQty -= allocatedQty;
                    }
                }
            }

            return result;
        }

        private static PLPUPurchaseReceiptReportDto CloneReceiptForLine(
            PLPUPurchaseReceiptReportDto source,
            PLPUPurchaseOrderLineRaw line,
            decimal allocatedQty)
        {
            var ratio = source.QtyKg == 0 ? 0 : allocatedQty / source.QtyKg;

            return new PLPUPurchaseReceiptReportDto
            {
                WarehouseRequestId = source.WarehouseRequestId,
                CodeFromRequest = source.CodeFromRequest,

                WarehouseVoucherId = source.WarehouseVoucherId,
                WarehouseVoucherCode = source.WarehouseVoucherCode,
                WarehouseVoucherDetailId = source.WarehouseVoucherDetailId,
                VoucherType = source.VoucherType,

                ReceiptDate = source.ReceiptDate,

                PurchaseOrderId = line.PurchaseOrderId,
                PurchaseOrderDetailId = line.PurchaseOrderDetailId,

                POExternalId = line.POExternalId,
                LineNo = line.LineNo,

                SupplierId = line.SupplierId,
                SupplierName = line.SupplierName,

                ProductCode = source.ProductCode,
                ProductName = string.IsNullOrWhiteSpace(source.ProductName) ? line.MaterialName : source.ProductName,
                LotNumber = source.LotNumber,

                QtyKg = allocatedQty,

                QCResult = source.QCResult,
                QCCreatedDate = source.QCCreatedDate,
                QCGroup = source.QCGroup,

                VoucherDetailType = source.VoucherDetailType,

                AcceptedQty = Math.Round(source.AcceptedQty * ratio, 6),
                PendingQcQty = Math.Round(source.PendingQcQty * ratio, 6),
                RejectedQty = Math.Round(source.RejectedQty * ratio, 6)
            };
        }

        private static List<PLPUPurchaseOrderDetailReportDto> BuildDetailRows(
            List<PLPUPurchaseOrderLineRaw> orderLines,
            List<PLPUPurchaseReceiptReportDto> allocatedReceipts)
        {
            var receiptsByDetailId = allocatedReceipts
                .Where(x => x.PurchaseOrderDetailId.HasValue)
                .GroupBy(x => x.PurchaseOrderDetailId!.Value)
                .ToDictionary(x => x.Key, x => x.OrderBy(r => r.ReceiptDate).ToList());

            return orderLines
                .Select(line =>
                {
                    receiptsByDetailId.TryGetValue(line.PurchaseOrderDetailId, out var receipts);
                    receipts ??= new List<PLPUPurchaseReceiptReportDto>();

                    var received = receipts.Sum(x => x.QtyKg);
                    var accepted = receipts.Sum(x => x.AcceptedQty);
                    var pendingQc = receipts.Sum(x => x.PendingQcQty);
                    var rejected = receipts.Sum(x => x.RejectedQty);

                    var dueDate = line.DeliveryDate ?? line.HeaderRequestDeliveryDate;

                    var firstReceipt = receipts.Count > 0 ? receipts.Min(x => x.ReceiptDate) : (DateTime?)null;
                    var lastReceipt = receipts.Count > 0 ? receipts.Max(x => x.ReceiptDate) : (DateTime?)null;

                    var dateWhenReceivedEnough = GetDateWhenEnough(
                        receipts.OrderBy(x => x.ReceiptDate),
                        line.OrderedQuantity,
                        x => x.QtyKg);

                    var dateWhenAcceptedEnough = GetDateWhenEnough(
                        receipts.OrderBy(x => x.ReceiptDate),
                        line.OrderedQuantity,
                        x => x.AcceptedQty);

                    var deliveryDelayDays = CalculateDelayDays(
                        dueDate,
                        dateWhenReceivedEnough,
                        received,
                        line.OrderedQuantity);

                    var acceptanceDelayDays = CalculateDelayDays(
                        dueDate,
                        dateWhenAcceptedEnough,
                        accepted,
                        line.OrderedQuantity);

                    var priceVariance = line.UnitPriceAgreed.HasValue && line.BaseCostSnapshot.HasValue
                        ? line.UnitPriceAgreed.Value - line.BaseCostSnapshot.Value
                        : (decimal?)null;

                    var priceVariancePercent = priceVariance.HasValue &&
                                               line.BaseCostSnapshot.HasValue &&
                                               line.BaseCostSnapshot.Value != 0
                        ? priceVariance.Value / line.BaseCostSnapshot.Value * 100
                        : (decimal?)null;

                    var flags = ResolveLineAlertFlags(
                        ordered: line.OrderedQuantity,
                        received: received,
                        accepted: accepted,
                        pendingQc: pendingQc,
                        rejected: rejected,
                        dueDate: dueDate);

                    var status = ResolveLinePrimaryStatus(
                        isActive: line.POIsActive,
                        poStatus: line.POStatus,
                        ordered: line.OrderedQuantity,
                        received: received,
                        accepted: accepted,
                        pendingQc: pendingQc,
                        rejected: rejected,
                        dueDate: dueDate);

                    var needAction = IsNeedActionLine(flags);

                    return new PLPUPurchaseOrderDetailReportDto
                    {
                        PurchaseOrderDetailId = line.PurchaseOrderDetailId,
                        PurchaseOrderId = line.PurchaseOrderId,
                        POExternalId = line.POExternalId,

                        LineNo = line.LineNo,

                        MaterialId = line.MaterialId,
                        MaterialCode = line.MaterialCode,
                        MaterialName = line.MaterialName,
                        Package = line.Package,

                        OrderedQuantity = line.OrderedQuantity,
                        WarehouseReceivedQuantity = received,
                        AcceptedQuantity = accepted,
                        PendingQcQuantity = pendingQc,
                        RejectedQuantity = rejected,

                        RemainingToReceive = RemainingWithTolerance(line.OrderedQuantity, received),
                        RemainingToAccept = RemainingWithTolerance(line.OrderedQuantity, accepted),
                        OverReceivedQuantity = OverReceivedWithTolerance(received, line.OrderedQuantity),

                        DeliveryDate = line.DeliveryDate,
                        RequestDeliveryDate = dueDate,

                        FirstReceiptDate = firstReceipt,
                        LastReceiptDate = lastReceipt,
                        DateWhenReceivedEnough = dateWhenReceivedEnough,
                        DateWhenAcceptedEnough = dateWhenAcceptedEnough,

                        DeliveryDelayDays = deliveryDelayDays,
                        AcceptanceDelayDays = acceptanceDelayDays,
                        DelayDays = acceptanceDelayDays,

                        LineReportStatus = status,
                        AlertFlags = string.Join(",", flags),

                        NeedBuyerAction = needAction,
                        BuyerAction = ResolveBuyerAction(flags),

                        BaseCostSnapshot = line.BaseCostSnapshot,
                        BaseDateSnapshot = line.BaseDateSnapshot,
                        UnitPriceAgreed = line.UnitPriceAgreed,
                        TotalPriceAgreed = line.TotalPriceAgreed,

                        PriceVariance = priceVariance,
                        PriceVariancePercent = priceVariancePercent,
                        PriceImpactAmount = priceVariance.HasValue
                            ? priceVariance.Value * line.OrderedQuantity
                            : null,

                        Note = line.Note
                    };
                })
                .ToList();
        }

        private static List<PLPUPurchaseOrderHeaderReportDto> BuildHeaderRows(
            List<PLPUPurchaseOrderLineRaw> orderLines,
            List<PLPUPurchaseOrderDetailReportDto> details)
        {
            var detailsByPo = details
                .GroupBy(x => x.PurchaseOrderId)
                .ToDictionary(x => x.Key, x => x.ToList());

            return orderLines
                .GroupBy(x => x.PurchaseOrderId)
                .Select(g =>
                {
                    var first = g.First();

                    detailsByPo.TryGetValue(first.PurchaseOrderId, out var poDetails);
                    poDetails ??= new List<PLPUPurchaseOrderDetailReportDto>();

                    var ordered = poDetails.Sum(x => x.OrderedQuantity);
                    var received = poDetails.Sum(x => x.WarehouseReceivedQuantity);
                    var accepted = poDetails.Sum(x => x.AcceptedQuantity);
                    var pendingQc = poDetails.Sum(x => x.PendingQcQuantity);
                    var rejected = poDetails.Sum(x => x.RejectedQuantity);

                    var totalValue = g.Sum(x =>
                        x.TotalPriceAgreed ??
                        x.OrderedQuantity * (x.UnitPriceAgreed ?? 0));

                    var flags = ResolveHeaderAlertFlags(poDetails);

                    var reportStatus = ResolveHeaderPrimaryStatus(
                        first.POIsActive,
                        first.POStatus,
                        poDetails,
                        ordered,
                        received,
                        accepted,
                        pendingQc,
                        rejected);

                    var isOverdue = flags.Contains("Overdue");
                    var hasQcFail = flags.Contains("HasQCFail");
                    var hasPendingQc = flags.Contains("WaitingQC");
                    var hasShortage = flags.Contains("Shortage");
                    var hasOpenBalance = !IsQuantityEnough(accepted, ordered);
                    var hasOverDelivery = flags.Contains("OverDelivered");

                    var needAction = IsNeedActionHeader(flags, reportStatus);

                    var deliveryDelayDays = poDetails.Count > 0
                        ? poDetails.Max(x => x.DeliveryDelayDays)
                        : 0;

                    var acceptanceDelayDays = poDetails.Count > 0
                        ? poDetails.Max(x => x.AcceptanceDelayDays)
                        : 0;

                    return new PLPUPurchaseOrderHeaderReportDto
                    {
                        PurchaseOrderId = first.PurchaseOrderId,
                        POExternalId = first.POExternalId,
                        OrderType = first.OrderType,

                        SupplierId = first.SupplierId,
                        SupplierName = first.SupplierName,

                        CompanyId = first.CompanyId,
                        CreatedDate = first.CreatedDate,
                        CreatedBy = first.CreatedBy,

                        RequestDeliveryDate = first.HeaderRequestDeliveryDate,
                        RealDeliveryDate = first.RealDeliveryDate,

                        POStatus = first.POStatus,
                        ReportStatus = reportStatus,
                        AlertFlags = string.Join(",", flags),

                        TotalLines = poDetails.Count,

                        OrderedQuantity = ordered,
                        WarehouseReceivedQuantity = received,
                        AcceptedQuantity = accepted,
                        PendingQcQuantity = pendingQc,
                        RejectedQuantity = rejected,

                        RemainingToReceive = RemainingWithTolerance(ordered, received),
                        RemainingToAccept = RemainingWithTolerance(ordered, accepted),
                        OverReceivedQuantity = OverReceivedWithTolerance(received, ordered),

                        CompletionPercent = Percent(accepted, ordered),
                        WarehouseReceiptPercent = Percent(received, ordered),

                        TotalPurchaseValue = totalValue,

                        IsOverdue = isOverdue,
                        DeliveryDelayDays = deliveryDelayDays,
                        AcceptanceDelayDays = acceptanceDelayDays,
                        DelayDays = acceptanceDelayDays,

                        HasQCFail = hasQcFail,
                        HasPendingQC = hasPendingQc,
                        HasShortage = hasShortage,
                        HasOpenBalance = hasOpenBalance,
                        HasOverDelivery = hasOverDelivery,

                        NeedBuyerAction = needAction,
                        BuyerAction = ResolveBuyerAction(flags),

                        PLPUComment = first.PLPUComment,
                        Comment = first.Comment
                    };
                })
                .ToList();
        }

        private static PLPUPurchaseOverviewSummaryDto BuildSummary(
            List<PLPUPurchaseOrderHeaderReportDto> headers,
            List<PLPUPurchaseOrderDetailReportDto> details)
        {
            var ordered = details.Sum(x => x.OrderedQuantity);
            var received = details.Sum(x => x.WarehouseReceivedQuantity);
            var accepted = details.Sum(x => x.AcceptedQuantity);
            var pendingQc = details.Sum(x => x.PendingQcQuantity);
            var rejected = details.Sum(x => x.RejectedQuantity);

            var deliveryEvaluableDetails = details
                .Where(IsDeliveryOtdEvaluable)
                .ToList();

            var acceptanceEvaluableDetails = details
                .Where(IsAcceptanceOtdEvaluable)
                .ToList();

            return new PLPUPurchaseOverviewSummaryDto
            {
                TotalPO = headers.Count,
                ActivePO = headers.Count(x => !IsCancelledOrInactiveStatus(x.POStatus)),
                CancelledPO = headers.Count(x => x.ReportStatus == "Cancelled"),
                ClosedPO = headers.Count(x => x.ReportStatus == "Closed" || x.ReportStatus == "ClosedWithShortage"),
                CompletedPO = headers.Count(x => x.ReportStatus == "Completed"),

                NeedActionPO = headers.Count(x => x.NeedBuyerAction),
                OverduePO = headers.Count(x => x.IsOverdue),
                QCFailPO = headers.Count(x => x.HasQCFail),
                PendingQCPO = headers.Count(x => x.HasPendingQC),
                OverDeliveryPO = headers.Count(x => x.HasOverDelivery),

                TotalPurchaseValue = headers.Sum(x => x.TotalPurchaseValue),

                OrderedQuantity = ordered,
                WarehouseReceivedQuantity = received,
                AcceptedQuantity = accepted,
                PendingQcQuantity = pendingQc,
                RejectedQuantity = rejected,

                RemainingToReceive = RemainingWithTolerance(ordered, received),
                RemainingToAccept = RemainingWithTolerance(ordered, accepted),
                OverReceivedQuantity = OverReceivedWithTolerance(received, ordered),

                CompletionPercent = Percent(accepted, ordered),
                WarehouseReceiptPercent = Percent(received, ordered),

                OnTimeDeliveryRate = Percent(
                    deliveryEvaluableDetails.Count(IsDeliveryOnTime),
                    deliveryEvaluableDetails.Count),

                OnTimeAcceptanceRate = Percent(
                    acceptanceEvaluableDetails.Count(IsAcceptanceOnTime),
                    acceptanceEvaluableDetails.Count),

                ShortageRate = Percent(
                    details.Where(x => HasFlag(x.AlertFlags, "Shortage")).Sum(x => x.RemainingToAccept),
                    ordered),

                OverDeliveryRate = Percent(details.Sum(x => x.OverReceivedQuantity), ordered),

                QcFailRate = Percent(rejected, received),
                QcAcceptanceRate = Percent(accepted, received),
                PendingQcRate = Percent(pendingQc, received),

                AverageDeliveryDelayDays = details.Count > 0
                    ? Math.Round((decimal)details.Average(x => x.DeliveryDelayDays), 2)
                    : 0,

                AverageAcceptanceDelayDays = details.Count > 0
                    ? Math.Round((decimal)details.Average(x => x.AcceptanceDelayDays), 2)
                    : 0,

                AverageDelayDays = details.Count > 0
                    ? Math.Round((decimal)details.Average(x => x.DelayDays), 2)
                    : 0,

                MaxDeliveryDelayDays = details.Count > 0 ? details.Max(x => x.DeliveryDelayDays) : 0,
                MaxAcceptanceDelayDays = details.Count > 0 ? details.Max(x => x.AcceptanceDelayDays) : 0,
                MaxDelayDays = details.Count > 0 ? details.Max(x => x.DelayDays) : 0
            };
        }

        private static PLPUPurchaseOrderHeaderDashboardDto BuildHeaderDashboard(
            List<PLPUPurchaseOrderLineRaw> orderLines,
            List<PLPUPurchaseOrderDetailReportDto> details,
            List<PLPUPurchaseOrderHeaderReportDto> headers)
        {
            var summary = BuildSummary(headers, details);
            var linesByDetailId = orderLines.ToDictionary(x => x.PurchaseOrderDetailId, x => x);

            return new PLPUPurchaseOrderHeaderDashboardDto
            {
                TotalPO = summary.TotalPO,
                ActivePO = summary.ActivePO,
                CancelledPO = summary.CancelledPO,
                ClosedPO = summary.ClosedPO,
                CompletedPO = summary.CompletedPO,
                NeedActionPO = summary.NeedActionPO,
                OverduePO = summary.OverduePO,
                QCFailPO = summary.QCFailPO,
                PendingQCPO = summary.PendingQCPO,
                OverDeliveryPO = summary.OverDeliveryPO,

                TotalPurchaseValue = summary.TotalPurchaseValue,

                OrderedQuantity = summary.OrderedQuantity,
                WarehouseReceivedQuantity = summary.WarehouseReceivedQuantity,
                AcceptedQuantity = summary.AcceptedQuantity,
                PendingQcQuantity = summary.PendingQcQuantity,
                RejectedQuantity = summary.RejectedQuantity,

                RemainingToReceive = summary.RemainingToReceive,
                RemainingToAccept = summary.RemainingToAccept,
                OverReceivedQuantity = summary.OverReceivedQuantity,

                CompletionPercent = summary.CompletionPercent,
                WarehouseReceiptPercent = summary.WarehouseReceiptPercent,

                OnTimeDeliveryRate = summary.OnTimeDeliveryRate,
                OnTimeAcceptanceRate = summary.OnTimeAcceptanceRate,

                ShortageRate = summary.ShortageRate,
                OverDeliveryRate = summary.OverDeliveryRate,
                QcFailRate = summary.QcFailRate,
                QcAcceptanceRate = summary.QcAcceptanceRate,
                PendingQcRate = summary.PendingQcRate,

                AverageDeliveryDelayDays = summary.AverageDeliveryDelayDays,
                AverageAcceptanceDelayDays = summary.AverageAcceptanceDelayDays,
                AverageDelayDays = summary.AverageDelayDays,

                MaxDeliveryDelayDays = summary.MaxDeliveryDelayDays,
                MaxAcceptanceDelayDays = summary.MaxAcceptanceDelayDays,
                MaxDelayDays = summary.MaxDelayDays,

                PurchaseValueByMonth = BuildMonthChart(headers),
                QuantityByMonth = BuildMonthQuantityChart(headers),
                PurchaseValueBySupplier = BuildSupplierChart(headers),
                QuantityBySupplier = BuildSupplierQuantityChart(headers),
                QuantityByMaterial = BuildMaterialQuantityChart(details, linesByDetailId),
                QcResultBySupplier = BuildSupplierQcChart(headers),
                StatusDistribution = BuildStatusChart(headers),
                PriceVarianceByMaterial = BuildMaterialPriceVarianceChart(details, linesByDetailId)
            };
        }

        private static IReadOnlyList<PLPUPurchaseReportChartPointDto> BuildMonthChart(
            List<PLPUPurchaseOrderHeaderReportDto> headers)
        {
            return headers
                .Where(x => x.CreatedDate.HasValue)
                .GroupBy(x => new { x.CreatedDate!.Value.Year, x.CreatedDate!.Value.Month })
                .OrderBy(x => x.Key.Year)
                .ThenBy(x => x.Key.Month)
                .Select(g =>
                {
                    var start = new DateTime(g.Key.Year, g.Key.Month, 1);
                    var ordered = g.Sum(x => x.OrderedQuantity);
                    var received = g.Sum(x => x.WarehouseReceivedQuantity);
                    var accepted = g.Sum(x => x.AcceptedQuantity);

                    return new PLPUPurchaseReportChartPointDto
                    {
                        Label = start.ToString("yyyy-MM"),
                        PurchaseOrderCount = g.Count(),
                        OrderedQuantity = ordered,
                        WarehouseReceivedQuantity = received,
                        AcceptedQuantity = accepted,
                        PendingQcQuantity = g.Sum(x => x.PendingQcQuantity),
                        RejectedQuantity = g.Sum(x => x.RejectedQuantity),
                        TotalPurchaseValue = g.Sum(x => x.TotalPurchaseValue),
                        CompletionPercent = Percent(accepted, ordered),
                        WarehouseReceiptPercent = Percent(received, ordered),
                        PeriodStart = start,
                        PeriodEnd = start.AddMonths(1).AddDays(-1)
                    };
                })
                .ToList();
        }

        private static IReadOnlyList<PLPUPurchaseReportChartPointDto> BuildMonthQuantityChart(
            List<PLPUPurchaseOrderHeaderReportDto> headers)
        {
            return BuildMonthChart(headers)
                .Select(x =>
                {
                    x.TotalPurchaseValue = 0;
                    return x;
                })
                .ToList();
        }

        private static IReadOnlyList<PLPUPurchaseReportChartPointDto> BuildSupplierChart(
            List<PLPUPurchaseOrderHeaderReportDto> headers)
        {
            return headers
                .GroupBy(x => new { x.SupplierId, x.SupplierName })
                .OrderByDescending(x => x.Sum(r => r.TotalPurchaseValue))
                .Take(15)
                .Select(g =>
                {
                    var ordered = g.Sum(x => x.OrderedQuantity);
                    var received = g.Sum(x => x.WarehouseReceivedQuantity);
                    var accepted = g.Sum(x => x.AcceptedQuantity);

                    return new PLPUPurchaseReportChartPointDto
                    {
                        Id = g.Key.SupplierId,
                        Label = string.IsNullOrWhiteSpace(g.Key.SupplierName)
                            ? "Unknown supplier"
                            : g.Key.SupplierName,
                        PurchaseOrderCount = g.Count(),
                        OrderedQuantity = ordered,
                        WarehouseReceivedQuantity = received,
                        AcceptedQuantity = accepted,
                        PendingQcQuantity = g.Sum(x => x.PendingQcQuantity),
                        RejectedQuantity = g.Sum(x => x.RejectedQuantity),
                        TotalPurchaseValue = g.Sum(x => x.TotalPurchaseValue),
                        CompletionPercent = Percent(accepted, ordered),
                        WarehouseReceiptPercent = Percent(received, ordered),
                        QcFailRate = Percent(g.Sum(x => x.RejectedQuantity), received),
                        QcAcceptanceRate = Percent(accepted, received),
                        AverageDelayDays = g.Any()
                            ? Math.Round((decimal)g.Average(x => x.DelayDays), 2)
                            : 0,
                        AverageDeliveryDelayDays = g.Any()
                            ? Math.Round((decimal)g.Average(x => x.DeliveryDelayDays), 2)
                            : 0,
                        AverageAcceptanceDelayDays = g.Any()
                            ? Math.Round((decimal)g.Average(x => x.AcceptanceDelayDays), 2)
                            : 0
                    };
                })
                .ToList();
        }

        private static IReadOnlyList<PLPUPurchaseReportChartPointDto> BuildSupplierQuantityChart(
            List<PLPUPurchaseOrderHeaderReportDto> headers)
        {
            return BuildSupplierChart(headers)
                .OrderByDescending(x => x.OrderedQuantity)
                .ToList();
        }

        private static IReadOnlyList<PLPUPurchaseReportChartPointDto> BuildSupplierQcChart(
            List<PLPUPurchaseOrderHeaderReportDto> headers)
        {
            return BuildSupplierChart(headers)
                .Where(x => x.WarehouseReceivedQuantity > 0 ||
                            x.PendingQcQuantity > 0 ||
                            x.RejectedQuantity > 0)
                .OrderByDescending(x => x.QcFailRate)
                .ThenByDescending(x => x.RejectedQuantity)
                .ToList();
        }

        private static IReadOnlyList<PLPUPurchaseReportChartPointDto> BuildStatusChart(
            List<PLPUPurchaseOrderHeaderReportDto> headers)
        {
            return headers
                .GroupBy(x => x.ReportStatus)
                .OrderByDescending(x => x.Count())
                .Select(g =>
                {
                    var ordered = g.Sum(x => x.OrderedQuantity);
                    var received = g.Sum(x => x.WarehouseReceivedQuantity);
                    var accepted = g.Sum(x => x.AcceptedQuantity);

                    return new PLPUPurchaseReportChartPointDto
                    {
                        Label = ToVietnameseStatus(g.Key),
                        PurchaseOrderCount = g.Count(),
                        OrderedQuantity = ordered,
                        WarehouseReceivedQuantity = received,
                        AcceptedQuantity = accepted,
                        PendingQcQuantity = g.Sum(x => x.PendingQcQuantity),
                        RejectedQuantity = g.Sum(x => x.RejectedQuantity),
                        TotalPurchaseValue = g.Sum(x => x.TotalPurchaseValue),
                        CompletionPercent = Percent(accepted, ordered),
                        WarehouseReceiptPercent = Percent(received, ordered)
                    };
                })
                .ToList();
        }

        private static IReadOnlyList<PLPUPurchaseReportChartPointDto> BuildMaterialQuantityChart(
            List<PLPUPurchaseOrderDetailReportDto> details,
            Dictionary<Guid, PLPUPurchaseOrderLineRaw> linesByDetailId)
        {
            return details
                .Where(x => linesByDetailId.ContainsKey(x.PurchaseOrderDetailId))
                .GroupBy(x => new { x.MaterialId, x.MaterialCode, x.MaterialName })
                .OrderByDescending(x => x.Sum(r => r.OrderedQuantity))
                .Take(20)
                .Select(g =>
                {
                    var ordered = g.Sum(x => x.OrderedQuantity);
                    var received = g.Sum(x => x.WarehouseReceivedQuantity);
                    var accepted = g.Sum(x => x.AcceptedQuantity);

                    return new PLPUPurchaseReportChartPointDto
                    {
                        Id = g.Key.MaterialId,
                        Label = string.IsNullOrWhiteSpace(g.Key.MaterialCode)
                            ? g.Key.MaterialName
                            : $"{g.Key.MaterialCode} - {g.Key.MaterialName}",
                        LineCount = g.Count(),
                        PurchaseOrderCount = g.Select(x => x.PurchaseOrderId).Distinct().Count(),
                        OrderedQuantity = ordered,
                        WarehouseReceivedQuantity = received,
                        AcceptedQuantity = accepted,
                        PendingQcQuantity = g.Sum(x => x.PendingQcQuantity),
                        RejectedQuantity = g.Sum(x => x.RejectedQuantity),
                        TotalPurchaseValue = g.Sum(x =>
                            x.TotalPriceAgreed ??
                            x.OrderedQuantity * (x.UnitPriceAgreed ?? 0)),
                        CompletionPercent = Percent(accepted, ordered),
                        WarehouseReceiptPercent = Percent(received, ordered),
                        QcFailRate = Percent(g.Sum(x => x.RejectedQuantity), received),
                        QcAcceptanceRate = Percent(accepted, received)
                    };
                })
                .ToList();
        }

        private static IReadOnlyList<PLPUPurchaseReportChartPointDto> BuildMaterialPriceVarianceChart(
            List<PLPUPurchaseOrderDetailReportDto> details,
            Dictionary<Guid, PLPUPurchaseOrderLineRaw> linesByDetailId)
        {
            return details
                .Where(x => linesByDetailId.ContainsKey(x.PurchaseOrderDetailId) && x.PriceVariance.HasValue)
                .GroupBy(x => new { x.MaterialId, x.MaterialCode, x.MaterialName })
                .Select(g => new PLPUPurchaseReportChartPointDto
                {
                    Id = g.Key.MaterialId,
                    Label = string.IsNullOrWhiteSpace(g.Key.MaterialCode)
                        ? g.Key.MaterialName
                        : $"{g.Key.MaterialCode} - {g.Key.MaterialName}",
                    LineCount = g.Count(),
                    OrderedQuantity = g.Sum(x => x.OrderedQuantity),
                    TotalPurchaseValue = g.Sum(x =>
                        x.TotalPriceAgreed ??
                        x.OrderedQuantity * (x.UnitPriceAgreed ?? 0)),
                    PriceVarianceAmount = g.Sum(x => x.PriceImpactAmount ?? 0)
                })
                .OrderByDescending(x => Math.Abs(x.PriceVarianceAmount))
                .Take(20)
                .ToList();
        }

        private static List<PLPUSupplierPurchasePerformanceDto> BuildSupplierPerformance(
            List<PLPUPurchaseOrderLineRaw> orderLines,
            List<PLPUPurchaseOrderDetailReportDto> details,
            List<PLPUPurchaseOrderHeaderReportDto> headers)
        {
            var lineById = orderLines.ToDictionary(x => x.PurchaseOrderDetailId, x => x);
            var headerByPo = headers.ToDictionary(x => x.PurchaseOrderId, x => x);

            return details
                .Where(x => lineById.ContainsKey(x.PurchaseOrderDetailId))
                .GroupBy(x => lineById[x.PurchaseOrderDetailId].SupplierId)
                .Select(g =>
                {
                    var firstLine = lineById[g.First().PurchaseOrderDetailId];

                    var poIds = g.Select(x => x.PurchaseOrderId).Distinct().ToList();

                    var ordered = g.Sum(x => x.OrderedQuantity);
                    var received = g.Sum(x => x.WarehouseReceivedQuantity);
                    var accepted = g.Sum(x => x.AcceptedQuantity);
                    var rejected = g.Sum(x => x.RejectedQuantity);
                    var pending = g.Sum(x => x.PendingQcQuantity);

                    var totalValue = g.Sum(x =>
                        x.TotalPriceAgreed ??
                        x.OrderedQuantity * (x.UnitPriceAgreed ?? 0));

                    var priceVarianceAmount = g.Sum(x => x.PriceImpactAmount ?? 0);

                    var deliveryEvaluable = g.Where(IsDeliveryOtdEvaluable).ToList();
                    var acceptanceEvaluable = g.Where(IsAcceptanceOtdEvaluable).ToList();

                    var overdueCount = poIds.Count(id =>
                        headerByPo.TryGetValue(id, out var h) && h.IsOverdue);

                    var qcFailCount = g.Count(x => x.RejectedQuantity > 0);
                    var shortageCount = g.Count(x => HasFlag(x.AlertFlags, "Shortage"));
                    var pendingCount = g.Count(x => x.PendingQcQuantity > 0);
                    var overDeliveryCount = g.Count(x => x.OverReceivedQuantity > 0);

                    var supplierDeliveryOtd = Percent(
                        deliveryEvaluable.Count(IsDeliveryOnTime),
                        deliveryEvaluable.Count);

                    var supplierAcceptanceOtd = Percent(
                        acceptanceEvaluable.Count(IsAcceptanceOnTime),
                        acceptanceEvaluable.Count);

                    return new PLPUSupplierPurchasePerformanceDto
                    {
                        SupplierId = firstLine.SupplierId,
                        SupplierName = firstLine.SupplierName,

                        TotalPO = poIds.Count,
                        TotalLines = g.Count(),

                        TotalPurchaseValue = totalValue,

                        OrderedQuantity = ordered,
                        WarehouseReceivedQuantity = received,
                        AcceptedQuantity = accepted,
                        PendingQcQuantity = pending,
                        RejectedQuantity = rejected,

                        SupplierOTD = supplierDeliveryOtd,
                        SupplierDeliveryOTD = supplierDeliveryOtd,
                        SupplierAcceptanceOTD = supplierAcceptanceOtd,

                        SupplierFillRate = Percent(g.Sum(x => Math.Min(x.WarehouseReceivedQuantity, x.OrderedQuantity)), ordered),
                        SupplierAcceptanceRate = Percent(accepted, received),
                        SupplierQcFailRate = Percent(rejected, received),

                        AverageDeliveryDelayDays = g.Any()
                            ? Math.Round((decimal)g.Average(x => x.DeliveryDelayDays), 2)
                            : 0,

                        AverageAcceptanceDelayDays = g.Any()
                            ? Math.Round((decimal)g.Average(x => x.AcceptanceDelayDays), 2)
                            : 0,

                        AverageDelayDays = g.Any()
                            ? Math.Round((decimal)g.Average(x => x.DelayDays), 2)
                            : 0,

                        AverageUnitPrice = ordered == 0
                            ? 0
                            : Math.Round(totalValue / ordered, 4),

                        PriceVarianceAmount = priceVarianceAmount,

                        RiskScore =
                            overdueCount * 30 +
                            qcFailCount * 25 +
                            shortageCount * 20 +
                            pendingCount * 10 +
                            overDeliveryCount * 5
                    };
                })
                .ToList();
        }

        private static List<PLPUPurchaseQcReportDto> BuildQcRowsFromAllocatedReceipts(
            List<PLPUPurchaseReceiptReportDto> receipts)
        {
            return receipts
                .Select(receipt =>
                {
                    var qcGroup = receipt.QCResult is VietausWebAPI.Core.Domain.Enums.Devandqa.QcDecision.QCPass
                        or VietausWebAPI.Core.Domain.Enums.Devandqa.QcDecision.Special
                            ? "Accepted"
                            : receipt.QCResult == VietausWebAPI.Core.Domain.Enums.Devandqa.QcDecision.QCFail
                                ? "Rejected"
                                : "Pending";

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
                        QCGroup = qcGroup,

                        ReceiptDate = receipt.ReceiptDate,
                        QCCreatedDate = receipt.QCCreatedDate,

                        QCAgeDays = qcGroup == "Pending"
                            ? Math.Max((DateTime.Today - receipt.ReceiptDate.Date).Days, 0)
                            : 0,

                        BuyerAction = qcGroup == "Rejected" ? "Handle QC fail with supplier"
                            : qcGroup == "Pending" ? "Follow up QC decision"
                            : string.Empty
                    };
                })
                .ToList();
        }

        private static DateTime? GetDateWhenEnough(
            IEnumerable<PLPUPurchaseReceiptReportDto> receipts,
            decimal orderedQuantity,
            Func<PLPUPurchaseReceiptReportDto, decimal> quantitySelector)
        {
            if (orderedQuantity <= 0)
                return null;

            decimal running = 0;

            foreach (var receipt in receipts.OrderBy(x => x.ReceiptDate))
            {
                running += quantitySelector(receipt);

                if (running >= orderedQuantity)
                    return receipt.ReceiptDate;
            }

            return null;
        }

        private static int CalculateDelayDays(
            DateTime? dueDate,
            DateTime? actualEnoughDate,
            decimal currentQuantity,
            decimal orderedQuantity)
        {
            if (!dueDate.HasValue || orderedQuantity <= 0)
                return 0;

            var comparisonDate = IsQuantityEnough(currentQuantity, orderedQuantity) && actualEnoughDate.HasValue
                ? actualEnoughDate.Value.Date
                : DateTime.Today;

            return Math.Max((comparisonDate - dueDate.Value.Date).Days, 0);
        }

        private static string ResolveLinePrimaryStatus(
            bool isActive,
            string poStatus,
            decimal ordered,
            decimal received,
            decimal accepted,
            decimal pendingQc,
            decimal rejected,
            DateTime? dueDate)
        {
            if (!isActive || IsCancelledOrInactiveStatus(poStatus))
                return "Cancelled";

            if (IsClosedStatus(poStatus))
            {
                if (ordered > 0 && !IsQuantityEnough(accepted, ordered))
                    return "ClosedWithShortage";

                return "Closed";
            }

            if (ordered <= 0)
                return "NoQuantity";

            if (IsQuantityEnough(accepted, ordered))
                return "AcceptedEnough";

            if (IsQuantityEnough(received, ordered) && pendingQc > 0)
                return "DeliveredEnoughWaitingQC";

            if (IsQuantityEnough(received, ordered))
                return "DeliveredEnough";

            if (accepted > 0)
                return "PartiallyAccepted";

            if (received > 0)
                return "PartiallyDelivered";

            if (dueDate.HasValue && DateTime.Today > dueDate.Value.Date)
                return "OverdueNotDelivered";

            return "NotDelivered";
        }

        private static List<string> ResolveLineAlertFlags(
            decimal ordered,
            decimal received,
            decimal accepted,
            decimal pendingQc,
            decimal rejected,
            DateTime? dueDate)
        {
            var flags = new List<string>();

            if (ordered <= 0)
                return flags;

            if (dueDate.HasValue && DateTime.Today > dueDate.Value.Date && !IsQuantityEnough(accepted, ordered))
            {
                flags.Add("Overdue");
                flags.Add("Shortage");
            }

            if (rejected > 0)
                flags.Add("HasQCFail");

            if (pendingQc > 0)
                flags.Add("WaitingQC");

            if (IsOverDelivered(received, ordered))
                flags.Add("OverDelivered");

            return flags.Distinct().ToList();
        }

        private static List<string> ResolveHeaderAlertFlags(
            List<PLPUPurchaseOrderDetailReportDto> details)
        {
            return details
                .SelectMany(x => SplitFlags(x.AlertFlags))
                .Distinct()
                .ToList();
        }

        private static string ResolveHeaderPrimaryStatus(
            bool isActive,
            string poStatus,
            List<PLPUPurchaseOrderDetailReportDto> details,
            decimal ordered,
            decimal received,
            decimal accepted,
            decimal pendingQc,
            decimal rejected)
        {
            if (!isActive || IsCancelledOrInactiveStatus(poStatus))
                return "Cancelled";

            if (IsClosedStatus(poStatus))
            {
                if (ordered > 0 && !IsQuantityEnough(accepted, ordered))
                    return "ClosedWithShortage";

                return "Closed";
            }

            if (ordered <= 0 || details.Count == 0)
                return "NoQuantity";

            if (IsQuantityEnough(accepted, ordered))
                return "Completed";

            if (details.Any(x => HasFlag(x.AlertFlags, "Overdue")))
                return "OverdueWithShortage";

            if (rejected > 0)
                return "HasQCFail";

            if (pendingQc > 0 && IsQuantityEnough(received, ordered))
                return "DeliveredEnoughWaitingQC";

            if (pendingQc > 0)
                return "WaitingQC";

            if (IsQuantityEnough(received, ordered))
                return "DeliveredEnough";

            if (accepted > 0)
                return "PartiallyAccepted";

            if (received > 0)
                return "PartiallyDelivered";

            return "NotDelivered";
        }

        private static bool IsNeedActionHeader(List<string> flags, string reportStatus)
        {
            if (reportStatus is "Cancelled" or "Closed" or "Completed")
                return false;

            return flags.Contains("Overdue") ||
                   flags.Contains("Shortage") ||
                   flags.Contains("HasQCFail") ||
                   flags.Contains("WaitingQC") ||
                   flags.Contains("OverDelivered") ||
                   reportStatus == "ClosedWithShortage";
        }

        private static bool IsNeedActionLine(List<string> flags)
        {
            return flags.Contains("Overdue") ||
                   flags.Contains("Shortage") ||
                   flags.Contains("HasQCFail") ||
                   flags.Contains("WaitingQC") ||
                   flags.Contains("OverDelivered");
        }

        private static string ResolveBuyerAction(IEnumerable<string> flags)
        {
            var flagSet = flags.ToHashSet(StringComparer.OrdinalIgnoreCase);

            var actions = new List<string>();

            if (flagSet.Contains("Overdue") || flagSet.Contains("Shortage"))
                actions.Add("Follow up supplier for overdue shortage");

            if (flagSet.Contains("HasQCFail"))
                actions.Add("Handle QC fail with supplier");

            if (flagSet.Contains("WaitingQC"))
                actions.Add("Follow up QC decision");

            if (flagSet.Contains("OverDelivered"))
                actions.Add("Review over delivery");

            return string.Join("; ", actions);
        }

        private static bool IsDeliveryOtdEvaluable(PLPUPurchaseOrderDetailReportDto detail)
        {
            if (detail.OrderedQuantity <= 0 || !detail.RequestDeliveryDate.HasValue)
                return false;

            return detail.DateWhenReceivedEnough.HasValue ||
                   DateTime.Today > detail.RequestDeliveryDate.Value.Date;
        }

        private static bool IsAcceptanceOtdEvaluable(PLPUPurchaseOrderDetailReportDto detail)
        {
            if (detail.OrderedQuantity <= 0 || !detail.RequestDeliveryDate.HasValue)
                return false;

            return detail.DateWhenAcceptedEnough.HasValue ||
                   DateTime.Today > detail.RequestDeliveryDate.Value.Date;
        }

        private static bool IsDeliveryOnTime(PLPUPurchaseOrderDetailReportDto detail)
        {
            return detail.DateWhenReceivedEnough.HasValue &&
                   detail.RequestDeliveryDate.HasValue &&
                   detail.DateWhenReceivedEnough.Value.Date <= detail.RequestDeliveryDate.Value.Date;
        }

        private static bool IsAcceptanceOnTime(PLPUPurchaseOrderDetailReportDto detail)
        {
            return detail.DateWhenAcceptedEnough.HasValue &&
                   detail.RequestDeliveryDate.HasValue &&
                   detail.DateWhenAcceptedEnough.Value.Date <= detail.RequestDeliveryDate.Value.Date;
        }

        private static bool IsOverDelivered(decimal received, decimal ordered)
        {
            if (ordered <= 0)
                return received > 0;

            var toleranceQty = ordered * OverDeliveryTolerancePercent / 100m;

            return received > ordered + toleranceQty;
        }

        private static bool IsCancelledOrInactiveStatus(string? status)
        {
            status ??= string.Empty;

            return status.Equals(StatusCancelled, StringComparison.OrdinalIgnoreCase) ||
                   status.Equals(StatusCanceled, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsClosedStatus(string? status)
        {
            status ??= string.Empty;

            return status.Equals(StatusClosed, StringComparison.OrdinalIgnoreCase);
        }

        private static bool HasFlag(string flags, string flag)
        {
            return SplitFlags(flags).Contains(flag, StringComparer.OrdinalIgnoreCase);
        }

        private static IEnumerable<string> SplitFlags(string flags)
        {
            if (string.IsNullOrWhiteSpace(flags))
                return Enumerable.Empty<string>();

            return flags
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        private static string ReceiptMatchKey(string poExternalId, string materialCode)
        {
            return $"{NormalizeKey(poExternalId)}|{NormalizeKey(materialCode)}";
        }

        private static string NormalizeKey(string? value)
        {
            return (value ?? string.Empty).Trim().ToUpperInvariant();
        }

        private static decimal Positive(decimal value)
        {
            return value > 0 ? value : 0;
        }

        private static decimal Percent(decimal numerator, decimal denominator)
        {
            return denominator == 0 ? 0 : Math.Round(numerator / denominator * 100, 2);
        }

        private static decimal Percent(int numerator, int denominator)
        {
            return denominator == 0 ? 0 : Math.Round((decimal)numerator / denominator * 100, 2);
        }

        private static int NormalizePageNumber(int pageNumber)
        {
            return pageNumber <= 0 ? 1 : pageNumber;
        }

        private static int NormalizePageSize(int pageSize)
        {
            return pageSize <= 0 ? 15 : pageSize;
        }

        private static PagedResult<T> ToPagedResult<T>(List<T> items, int pageNumber, int pageSize)
        {
            var pagedItems = items
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<T>(pagedItems, items.Count, pageNumber, pageSize);
        }


        private static decimal LowerCompletionBound(decimal ordered)
        {
            if (ordered <= 0)
                return 0;

            return ordered * (100m - CompletionTolerancePercent) / 100m;
        }

        private static decimal UpperCompletionBound(decimal ordered)
        {
            if (ordered <= 0)
                return 0;

            return ordered * (100m + CompletionTolerancePercent) / 100m;
        }

        private static bool IsQuantityEnough(decimal actual, decimal ordered)
        {
            if (ordered <= 0)
                return actual <= 0;

            return actual >= LowerCompletionBound(ordered);
        }

        private static decimal RemainingWithTolerance(decimal ordered, decimal actual)
        {
            return IsQuantityEnough(actual, ordered)
                ? 0
                : Positive(ordered - actual);
        }

        private static decimal OverReceivedWithTolerance(decimal received, decimal ordered)
        {
            return IsOverDelivered(received, ordered)
                ? Positive(received - ordered)
                : 0;
        }

        private static string ToVietnameseStatus(string? status)
        {
            return status switch
            {
                "Cancelled" => "Đã hủy",
                "Closed" => "Đã đóng",
                "ClosedWithShortage" => "Đã đóng còn thiếu",
                "NoQuantity" => "Không có số lượng",
                "Completed" => "Hoàn thành",
                "AcceptedEnough" => "QC đạt đủ",
                "DeliveredEnoughWaitingQC" => "Đã giao đủ, chờ QC",
                "DeliveredEnough" => "Đã giao đủ",
                "OverdueWithShortage" => "Quá hạn còn thiếu",
                "HasQCFail" => "Có hàng lỗi QC",
                "WaitingQC" => "Chờ QC",
                "PartiallyAccepted" => "QC đạt một phần",
                "PartiallyDelivered" => "Giao một phần",
                "OverdueNotDelivered" => "Quá hạn chưa giao",
                "NotDelivered" => "Chưa giao",
                _ => status ?? string.Empty
            };
        }

        private static string ToVietnameseFlag(string? flag)
        {
            return flag switch
            {
                "Overdue" => "Quá hạn",
                "Shortage" => "Thiếu hàng",
                "HasQCFail" => "Có hàng lỗi QC",
                "WaitingQC" => "Chờ QC",
                "OverDelivered" => "Giao vượt",
                _ => flag ?? string.Empty
            };
        }

        private static string ToVietnameseFlags(string? flags)
        {
            if (string.IsNullOrWhiteSpace(flags))
                return string.Empty;

            return string.Join(", ",
                flags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                     .Select(ToVietnameseFlag));
        }

        private static string ToVietnameseBuyerAction(string? action)
        {
            if (string.IsNullOrWhiteSpace(action))
                return string.Empty;

            var parts = action.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var translated = parts.Select(x => x switch
            {
                "Follow up supplier for overdue shortage" => "Theo dõi NCC do quá hạn/thiếu hàng",
                "Handle QC fail with supplier" => "Xử lý hàng lỗi QC với NCC",
                "Follow up QC decision" => "Theo dõi kết quả QC",
                "Review over delivery" => "Kiểm tra giao vượt",
                _ => x
            });

            return string.Join("; ", translated);
        }

        private static string ToVietnameseQcGroup(string? qcGroup)
        {
            return qcGroup switch
            {
                "Accepted" => "Đạt QC",
                "Rejected" => "Không đạt QC",
                "Pending" => "Chờ QC",
                _ => qcGroup ?? string.Empty
            };
        }

        private static List<PLPUPurchaseOrderHeaderReportDto> LocalizeHeaders(
            IEnumerable<PLPUPurchaseOrderHeaderReportDto> headers)
        {
            return headers.Select(x =>
            {
                x.ReportStatus = ToVietnameseStatus(x.ReportStatus);
                x.AlertFlags = ToVietnameseFlags(x.AlertFlags);
                x.BuyerAction = ToVietnameseBuyerAction(x.BuyerAction);
                return x;
            }).ToList();
        }

        private static List<PLPUPurchaseOrderDetailReportDto> LocalizeDetails(
            IEnumerable<PLPUPurchaseOrderDetailReportDto> details)
        {
            return details.Select(x =>
            {
                x.LineReportStatus = ToVietnameseStatus(x.LineReportStatus);
                x.AlertFlags = ToVietnameseFlags(x.AlertFlags);
                x.BuyerAction = ToVietnameseBuyerAction(x.BuyerAction);
                return x;
            }).ToList();
        }

        private static List<PLPUPurchaseReceiptReportDto> LocalizeReceipts(
            IEnumerable<PLPUPurchaseReceiptReportDto> receipts)
        {
            return receipts.Select(x =>
            {
                x.QCGroup = ToVietnameseQcGroup(x.QCGroup);
                return x;
            }).ToList();
        }

        private static List<PLPUPurchaseQcReportDto> LocalizeQcRows(
            IEnumerable<PLPUPurchaseQcReportDto> rows)
        {
            return rows.Select(x =>
            {
                x.QCGroup = ToVietnameseQcGroup(x.QCGroup);
                x.BuyerAction = ToVietnameseBuyerAction(x.BuyerAction);
                return x;
            }).ToList();
        }
    }

    internal static class PLPUPurchaseReceiptReportDtoExtensions
    {
        public static string MaterialCode(this PLPUPurchaseReceiptReportDto row)
        {
            return row.ProductCode;
        }
    }


}