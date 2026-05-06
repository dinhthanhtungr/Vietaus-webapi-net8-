using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.SaleReports;
using VietausWebAPI.Core.Application.Features.Shared.DTO.Visibility;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;
using VietausWebAPI.Core.Domain.Enums.Merchadises;
using VietausWebAPI.Core.Domain.Enums.Visibilitys;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

public class MerchandiseOrderReportRepository : IMerchandiseOrderReportRepositorys
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;

    public MerchandiseOrderReportRepository(ApplicationDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Lấy báo cáo kế hoạch giao hàng dạng phân trang.
    /// Dữ liệu trả về theo từng dòng chi tiết đơn hàng, có thông tin khách hàng, sản phẩm,
    /// số lượng đặt, số lượng đã giao, còn lại, ngày yêu cầu giao và trạng thái.
    /// </summary>
    public async Task<(IReadOnlyList<SummaryMOReportDto> Items, int TotalCount)> GetDeliveryPlanReportAsync(
        MerchandiseOrderReportQuery query,
        ViewerScope viewerScope,
        CancellationToken cancellationToken = default)
    {
        var reportQuery = BuildDeliveryPlanReportQuery(query, viewerScope);

        var totalCount = await reportQuery.CountAsync(cancellationToken);

        var pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
        var pageSize = query.PageSize <= 0 ? 20 : query.PageSize;

        var items = await reportQuery
            .OrderByDescending(x => x.OrderDate)
            .ThenBy(x => x.DeliveryRequestDate)
            .ThenBy(x => x.CustomerName)
            .ThenBy(x => x.ProductCode)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    /// <summary>
    /// Lấy toàn bộ dữ liệu kế hoạch giao hàng để export Excel.
    /// Không phân trang, dùng chung query với màn hình báo cáo kế hoạch giao hàng.
    /// </summary>
    public async Task<List<SummaryMOReportDto>> GetDeliveryPlanReportForExportAsync(
        MerchandiseOrderReportQuery query,
        ViewerScope viewerScope,
        CancellationToken cancellationToken = default)
    {
        var reportQuery = BuildDeliveryPlanReportQuery(query, viewerScope);

        return await reportQuery
            .ToListAsync(cancellationToken);

        //return await Task.FromResult(new List<SummaryMOReportDto>());
    }

    /// <summary>
    /// Lấy số liệu tổng quan theo tiêu chí lọc để vẽ biểu đồ và dashboard báo cáo đơn hàng hàng hóa.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="viewerScope"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<MerchandiseOrderReportHeaderDto> GetMerchandiseOrderHeaderReportAsync(
        MerchandiseOrderReportQuery query,
        ViewerScope viewerScope,
        CancellationToken cancellationToken = default)
    {
        var reportQuery = BuildMerchandiseOrderHeaderBaseQuery(query, viewerScope, includeKeyword: false);

        var orders = await reportQuery
            .OrderByDescending(x => x.OrderDate)
            .ThenBy(x => x.CustomerName)
            .ThenBy(x => x.MerchandiseOrderCode)
            .ToListAsync(cancellationToken);

        var items = await EnrichMerchandiseOrderRowsAsync(orders, cancellationToken);

        return BuildMerchandiseOrderHeaderSummary(items);
    }

    /// <summary>
    /// Lấy danh sách đơn hàng bán dạng phân trang.
    /// Dữ liệu dùng cho bảng danh sách đơn hàng ở dashboard.
    /// Có áp dụng filter ngày, sale, group và keyword nếu có.
    /// </summary>
    public async Task<(IReadOnlyList<MerchandiseOrderReportRowDto> Items, int TotalCount)> GetMerchandiseOrderRowsAsync(
        MerchandiseOrderReportQuery query,
        ViewerScope viewerScope,
        CancellationToken cancellationToken = default)
    {
        var reportQuery = BuildMerchandiseOrderHeaderBaseQuery(query, viewerScope);

        var totalCount = await reportQuery.CountAsync(cancellationToken);

        var pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
        var pageSize = Math.Min(query.PageSize <= 0 ? 15 : query.PageSize, 15);

        var pageOrders = await reportQuery
            .OrderByDescending(x => x.OrderDate)
            .ThenBy(x => x.CustomerName)
            .ThenBy(x => x.MerchandiseOrderCode)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        if (pageOrders.Count == 0)
        {
            return (pageOrders, totalCount);
        }

        var items = await EnrichMerchandiseOrderRowsAsync(pageOrders, cancellationToken);

        return (items, totalCount);
    }


    /// <summary>
    /// Bổ sung số liệu tính toán cho danh sách đơn hàng.
    /// Hàm này lấy thêm tổng số lượng đặt, tổng tiền đặt,
    /// số lượng đã giao, doanh thu thực bán, số còn lại,
    /// tình trạng quá hạn, group sale và health status.
    /// </summary>
    private async Task<IReadOnlyList<MerchandiseOrderReportRowDto>> EnrichMerchandiseOrderRowsAsync(
        IReadOnlyList<MerchandiseOrderReportRowDto> orders,
        CancellationToken cancellationToken)
    {
        if (orders.Count == 0)
        {
            return orders;
        }

        var orderIds = orders
            .Select(x => x.MerchandiseOrderId)
            .Distinct()
            .ToList();

        var customerIds = orders
            .Select(x => x.CustomerId)
            .Distinct()
            .ToList();

        var managerIds = orders
            .Select(x => x.ManagerById)
            .Distinct()
            .ToList();

        var orderAmounts = await _context.MerchandiseOrderDetails
            .AsNoTracking()
            .Where(x => x.IsActive && orderIds.Contains(x.MerchandiseOrderId))
            .GroupBy(x => x.MerchandiseOrderId)
            .Select(g => new
            {
                MerchandiseOrderId = g.Key,
                FirstDeliveryRequestDate = g.Min(x => (DateTime?)x.DeliveryRequestDate),
                LastDeliveryRequestDate = g.Max(x => (DateTime?)x.DeliveryRequestDate),
                OrderedQuantity = g.Sum(x => (decimal?)x.ExpectedQuantity) ?? 0m,
                TotalOrderAmount = g.Sum(x => (decimal?)x.TotalPriceAgreed) ?? 0m,
                DetailCount = g.Count()
            })
            .ToListAsync(cancellationToken);

        var deliveryInfo =
            from dod in _context.DeliveryOrderDetails.AsNoTracking()
            join doo in _context.DeliveryOrders.AsNoTracking() on dod.DeliveryOrderId equals doo.Id
            where dod.IsActive
                  && !dod.IsAttach
                  && doo.IsActive
                  && dod.MerchandiseOrderDetailId.HasValue
            group new { dod, doo } by dod.MerchandiseOrderDetailId.Value into g
            select new
            {
                MerchandiseOrderDetailId = g.Key,
                DeliveredQuantity = (decimal?)g.Sum(x => x.dod.Quantity),
                LastDeliveryDate = g.Max(x => (DateTime?)x.doo.CreatedDate)
            };

        var deliveryAmounts = await (
            from mod in _context.MerchandiseOrderDetails.AsNoTracking()
            join di in deliveryInfo
                on mod.MerchandiseOrderDetailId equals di.MerchandiseOrderDetailId
            where mod.IsActive && orderIds.Contains(mod.MerchandiseOrderId)
            group new { mod, di } by mod.MerchandiseOrderId into g
            select new
            {
                MerchandiseOrderId = g.Key,
                DeliveredQuantity = g.Sum(x => (decimal?)x.di.DeliveredQuantity) ?? 0m,
                ActualSoldAmount = g.Sum(x => (decimal?)(x.di.DeliveredQuantity * x.mod.UnitPriceAgreed)) ?? 0m,
                LastActualDeliveryDate = g.Max(x => x.di.LastDeliveryDate)
            })
            .ToListAsync(cancellationToken);

        var assignmentRows = await _context.CustomerAssignments
            .AsNoTracking()
            .Where(x =>
                x.IsActive
                && customerIds.Contains(x.CustomerId)
                && managerIds.Contains(x.EmployeeId))
            .OrderByDescending(x => x.CreatedDate)
            .Select(x => new
            {
                x.CustomerId,
                x.EmployeeId,
                GroupId = (Guid?)x.GroupId,
                GroupCode = x.Group.ExternalId,
                GroupName = x.Group.Name
            })
            .ToListAsync(cancellationToken);

        var orderAmountMap = orderAmounts.ToDictionary(x => x.MerchandiseOrderId);
        var deliveryAmountMap = deliveryAmounts.ToDictionary(x => x.MerchandiseOrderId);
        var assignmentMap = assignmentRows
            .GroupBy(x => new { x.CustomerId, x.EmployeeId })
            .ToDictionary(x => x.Key, x => x.First());

        var items = orders.Select(order =>
        {
            orderAmountMap.TryGetValue(order.MerchandiseOrderId, out var orderAmount);
            deliveryAmountMap.TryGetValue(order.MerchandiseOrderId, out var deliveryAmount);
            assignmentMap.TryGetValue(new { order.CustomerId, EmployeeId = order.ManagerById }, out var assignment);

            var totalOrderAmount = orderAmount?.TotalOrderAmount ?? order.TotalOrderAmount;
            var actualSoldAmount = deliveryAmount?.ActualSoldAmount ?? 0m;
            var orderedQuantity = orderAmount?.OrderedQuantity ?? 0m;
            var deliveredQuantity = deliveryAmount?.DeliveredQuantity ?? 0m;
            var remainingQuantity = orderedQuantity - deliveredQuantity;
            var today = DateTime.Now.Date;
            var isOverdue = remainingQuantity > 0m
                            && orderAmount?.FirstDeliveryRequestDate is DateTime firstRequestDate
                            && firstRequestDate.Date < today;

            order.GroupId = assignment?.GroupId;
            order.GroupCode = assignment?.GroupCode ?? string.Empty;
            order.GroupName = assignment?.GroupName ?? string.Empty;
            order.FirstDeliveryRequestDate = orderAmount?.FirstDeliveryRequestDate;
            order.LastDeliveryRequestDate = orderAmount?.LastDeliveryRequestDate;
            order.LastActualDeliveryDate = deliveryAmount?.LastActualDeliveryDate;
            order.OrderedQuantity = orderedQuantity;
            order.DeliveredQuantity = deliveredQuantity;
            order.RemainingQuantity = remainingQuantity;
            order.TotalOrderAmount = totalOrderAmount;
            order.ActualSoldAmount = actualSoldAmount;
            order.RemainingAmount = totalOrderAmount - actualSoldAmount;
            order.FulfillmentRate =
                MerchandiseOrderReportFormula.CalculateFulfillmentRate(deliveredQuantity, orderedQuantity);

            order.ActualSoldRate =
                MerchandiseOrderReportFormula.CalculateActualSoldRate(actualSoldAmount, totalOrderAmount);
            order.UnpaidAmount = order.IsPaid ? 0m : actualSoldAmount;
            order.OrderAgeDays = Math.Max(0, (today - order.OrderDate.Date).Days);
            order.IsOverdue = isOverdue;
            order.OverdueDays = isOverdue && orderAmount?.FirstDeliveryRequestDate is DateTime overdueDate
                ? (today - overdueDate.Date).Days
                : 0;
            order.HealthStatus =
                isOverdue ? "Quá hạn giao" :
                remainingQuantity <= 0m && actualSoldAmount > 0m && !order.IsPaid ? "Đã giao đủ chưa thanh toán" :
                remainingQuantity <= 0m ? "Đã giao đủ" :
                actualSoldAmount > 0m && !order.IsPaid ? "Đã giao một phần chưa thanh toán" :
                deliveredQuantity > 0m ? "Đang giao" :
                "Chưa giao";
            order.DetailCount = orderAmount?.DetailCount ?? 0;

            return order;
        }).ToList();

        return items;
    }

    /// <summary>
    /// Gom danh sách đơn hàng đã enrich thành dữ liệu tổng quan dashboard.
    /// Tính tổng đơn, tổng tiền, tổng số lượng, tỉ lệ giao hàng,
    /// tỉ lệ thực bán và build dữ liệu biểu đồ.
    /// </summary>
    private static MerchandiseOrderReportHeaderDto BuildMerchandiseOrderHeaderSummary(
        IReadOnlyList<MerchandiseOrderReportRowDto> rows)   
    {
        var orderedQuantity = rows.Sum(x => x.OrderedQuantity);
        var deliveredQuantity = rows.Sum(x => x.DeliveredQuantity);
        var totalOrderAmount = rows.Sum(x => x.TotalOrderAmount);
        var actualSoldAmount = rows.Sum(x => x.ActualSoldAmount);

        return new MerchandiseOrderReportHeaderDto
        {
            TotalOrderCount = rows.Count,
            PaidOrderCount = rows.Count(x => x.IsPaid),
            UnpaidOrderCount = rows.Count(x => !x.IsPaid),
            OverdueOrderCount = rows.Count(x => x.IsOverdue),
            CompletedOrderCount = rows.Count(x => x.RemainingQuantity <= 0m && x.DeliveredQuantity > 0m),
            InProgressOrderCount = rows.Count(x => x.DeliveredQuantity > 0m && x.RemainingQuantity > 0m),
            NotDeliveredOrderCount = rows.Count(x => x.DeliveredQuantity <= 0m),

            OrderedQuantity = orderedQuantity,
            DeliveredQuantity = deliveredQuantity,
            RemainingQuantity = rows.Sum(x => x.RemainingQuantity),
            TotalOrderAmount = totalOrderAmount,
            ActualSoldAmount = actualSoldAmount,
            RemainingAmount = rows.Sum(x => x.RemainingAmount),
            UnpaidAmount = rows.Sum(x => x.UnpaidAmount),
            FulfillmentRate =
    MerchandiseOrderReportFormula.CalculateFulfillmentRate(deliveredQuantity, orderedQuantity),

            ActualSoldRate =
    MerchandiseOrderReportFormula.CalculateActualSoldRate(actualSoldAmount, totalOrderAmount),

            RevenueByMonth = rows
                .GroupBy(x => x.OrderDate.ToString("yyyy-MM"))
                .OrderBy(x => x.Key)
                .Select(ToChartPoint)
                .ToList(),
            RevenueByManager = rows
                .GroupBy(x => string.IsNullOrWhiteSpace(x.ManagerName) ? "Không xác định" : x.ManagerName)
                .OrderByDescending(x => x.Sum(row => row.ActualSoldAmount))
                .Take(10)
                .Select(ToChartPoint)
                .ToList(),
            RevenueByCustomer = rows
                .GroupBy(x => string.IsNullOrWhiteSpace(x.CustomerName) ? "Không xác định" : x.CustomerName)
                .OrderByDescending(x => x.Sum(row => row.ActualSoldAmount))
                .Take(10)
                .Select(ToChartPoint)
                .ToList(),
            QuantityByManager = rows
                .GroupBy(x => string.IsNullOrWhiteSpace(x.ManagerName) ? "Không xác định" : x.ManagerName)
                .OrderByDescending(x => x.Sum(row => row.DeliveredQuantity))
                .Take(10)
                .Select(ToChartPoint)
                .ToList(),
            QuantityByCustomer = rows
                .GroupBy(x => string.IsNullOrWhiteSpace(x.CustomerName) ? "Không xác định" : x.CustomerName)
                .OrderByDescending(x => x.Sum(row => row.DeliveredQuantity))
                .Take(10)
                .Select(ToChartPoint)
                .ToList()
        };
    }

    /// <summary>
    /// Chuyển một nhóm dữ liệu đơn hàng thành một điểm dữ liệu cho chart.
    /// Dùng cho chart theo tháng, theo sale hoặc theo khách hàng.
    /// </summary>
    private static MerchandiseOrderReportChartPointDto ToChartPoint(
        IGrouping<string, MerchandiseOrderReportRowDto> group)
    {
        return new MerchandiseOrderReportChartPointDto
        {
            Label = group.Key,
            OrderCount = group.Count(),
            OrderedQuantity = group.Sum(x => x.OrderedQuantity),
            DeliveredQuantity = group.Sum(x => x.DeliveredQuantity),
            TotalOrderAmount = group.Sum(x => x.TotalOrderAmount),
            ActualSoldAmount = group.Sum(x => x.ActualSoldAmount),
            RemainingAmount = group.Sum(x => x.RemainingAmount)
        };
    }

    /// <summary>
    /// Lấy chi tiết của một đơn hàng bán.
    /// Trả về từng dòng sản phẩm trong đơn, gồm số lượng đặt,
    /// đã giao, còn lại, đơn giá, thành tiền, thực bán,
    /// ngày giao và trạng thái dòng hàng.
    /// </summary>
    public async Task<IReadOnlyList<MerchandiseOrderReportDetailDto>> GetMerchandiseOrderDetailReportAsync(
        Guid merchandiseOrderId,
        ViewerScope viewerScope,
        CancellationToken cancellationToken = default)
    {
        var merchandiseOrders = ApplyReportVisibility(
            _context.MerchandiseOrders
                .AsNoTracking()
                .Where(x =>
                    x.IsActive
                    && x.OrderType == OrderType.Merchandise
                    && x.CustomerExternalIdSnapshot != "KH_VIETAUS"),
            viewerScope);

        var order = await merchandiseOrders
            .Where(x => EF.Property<Guid?>(x, nameof(MerchandiseOrder.MerchandiseOrderId)) == merchandiseOrderId)
            .Select(x => new
            {
                MerchandiseOrderId = EF.Property<Guid?>(x, nameof(MerchandiseOrder.MerchandiseOrderId)),
                CustomerId = EF.Property<Guid?>(x, nameof(MerchandiseOrder.CustomerId)),
                MerchandiseOrderCode = EF.Property<string?>(x, nameof(MerchandiseOrder.ExternalId)) ?? string.Empty
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (order?.MerchandiseOrderId is not Guid orderId)
        {
            return Array.Empty<MerchandiseOrderReportDetailDto>();
        }

        var details = await _context.MerchandiseOrderDetails
            .AsNoTracking()
            .Where(x =>
                EF.Property<bool?>(x, nameof(MerchandiseOrderDetail.IsActive)) == true
                && EF.Property<Guid?>(x, nameof(MerchandiseOrderDetail.MerchandiseOrderId)) == orderId)
            .Select(x => new
            {
                MerchandiseOrderDetailId = EF.Property<Guid?>(x, nameof(MerchandiseOrderDetail.MerchandiseOrderDetailId)),
                ProductId = EF.Property<Guid?>(x, nameof(MerchandiseOrderDetail.ProductId)),
                ProductCode = EF.Property<string?>(x, nameof(MerchandiseOrderDetail.ProductExternalIdSnapshot)) ?? string.Empty,
                ProductName = EF.Property<string?>(x, nameof(MerchandiseOrderDetail.ProductNameSnapshot)) ?? string.Empty,
                FormulaCode = EF.Property<string?>(x, nameof(MerchandiseOrderDetail.FormulaExternalIdSnapshot)) ?? string.Empty,
                DeliveryRequestDate = EF.Property<DateTime?>(x, nameof(MerchandiseOrderDetail.DeliveryRequestDate)),
                ExpectedDeliveryDate = EF.Property<DateTime?>(x, nameof(MerchandiseOrderDetail.ExpectedDeliveryDate)),
                DeliveryActualDate = EF.Property<DateTime?>(x, nameof(MerchandiseOrderDetail.DeliveryActualDate)),
                RequestedQuantity = EF.Property<decimal?>(x, nameof(MerchandiseOrderDetail.ExpectedQuantity)) ?? 0m,
                UnitPrice = EF.Property<decimal?>(x, nameof(MerchandiseOrderDetail.UnitPriceAgreed)) ?? 0m,
                TotalPrice = EF.Property<decimal?>(x, nameof(MerchandiseOrderDetail.TotalPriceAgreed)) ?? 0m,
                BagType = EF.Property<string?>(x, nameof(MerchandiseOrderDetail.BagType)) ?? string.Empty,
                PackageWeight = EF.Property<string?>(x, nameof(MerchandiseOrderDetail.PackageWeight)) ?? string.Empty,
                Status = EF.Property<string?>(x, nameof(MerchandiseOrderDetail.Status)) ?? string.Empty,
                Comment = EF.Property<string?>(x, nameof(MerchandiseOrderDetail.Comment)) ?? string.Empty
            })
            .ToListAsync(cancellationToken);

        if (details.Count == 0)
        {
            return Array.Empty<MerchandiseOrderReportDetailDto>();
        }

        var detailIds = details
            .Select(x => x.MerchandiseOrderDetailId)
            .Where(x => x.HasValue)
            .Select(x => x!.Value)
            .Distinct()
            .ToList();

        var productIds = details
            .Select(x => x.ProductId)
            .Where(x => x.HasValue)
            .Select(x => x!.Value)
            .Distinct()
            .ToList();

        var deliveryRows = await (
            from dod in _context.DeliveryOrderDetails.AsNoTracking()
            join doo in _context.DeliveryOrders.AsNoTracking()
                on EF.Property<Guid?>(dod, nameof(DeliveryOrderDetail.DeliveryOrderId)) equals EF.Property<Guid?>(doo, nameof(DeliveryOrder.Id))
            where EF.Property<bool?>(dod, nameof(DeliveryOrderDetail.IsActive)) == true
                  && EF.Property<bool?>(dod, nameof(DeliveryOrderDetail.IsAttach)) != true
                  && EF.Property<bool?>(doo, nameof(DeliveryOrder.IsActive)) == true
                  && EF.Property<Guid?>(dod, nameof(DeliveryOrderDetail.MerchandiseOrderDetailId)).HasValue
                  && detailIds.Contains(EF.Property<Guid?>(dod, nameof(DeliveryOrderDetail.MerchandiseOrderDetailId))!.Value)
            group new { dod, doo } by EF.Property<Guid?>(dod, nameof(DeliveryOrderDetail.MerchandiseOrderDetailId)) into g
            select new
            {
                MerchandiseOrderDetailId = g.Key,
                DeliveredQuantity = g.Sum(x => EF.Property<decimal?>(x.dod, nameof(DeliveryOrderDetail.Quantity))) ?? 0m,
                LastDeliveryDate = g.Max(x => EF.Property<DateTime?>(x.doo, nameof(DeliveryOrder.CreatedDate)))
            })
            .ToListAsync(cancellationToken);

        var productRows = await _context.Products
            .AsNoTracking()
            .Where(x => productIds.Contains(EF.Property<Guid?>(x, "ProductId")!.Value))
            .Select(x => new
            {
                ProductId = EF.Property<Guid?>(x, "ProductId"),
                ColourCode = EF.Property<string?>(x, "ColourCode") ?? string.Empty
            })
            .ToListAsync(cancellationToken);

        var sampleRows = await _context.SampleRequests
            .AsNoTracking()
            .Where(x =>
                order.CustomerId.HasValue
                && EF.Property<bool?>(x, "IsActive") == true
                && EF.Property<Guid?>(x, "CustomerId") == order.CustomerId
                && EF.Property<Guid?>(x, "ProductId").HasValue
                && productIds.Contains(EF.Property<Guid?>(x, "ProductId")!.Value))
            .OrderByDescending(x => EF.Property<DateTime?>(x, "CreatedDate"))
            .Select(x => new
            {
                ProductId = EF.Property<Guid?>(x, "ProductId"),
                ExternalId = EF.Property<string?>(x, "ExternalId") ?? string.Empty
            })
            .ToListAsync(cancellationToken);

        var deliveryMap = deliveryRows
            .Where(x => x.MerchandiseOrderDetailId.HasValue)
            .ToDictionary(x => x.MerchandiseOrderDetailId!.Value);
        var productMap = productRows
            .Where(x => x.ProductId.HasValue)
            .GroupBy(x => x.ProductId!.Value)
            .ToDictionary(x => x.Key, x => x.First().ColourCode);
        var sampleMap = sampleRows
            .Where(x => x.ProductId.HasValue)
            .GroupBy(x => x.ProductId!.Value)
            .ToDictionary(x => x.Key, x => x.First().ExternalId);

        return details
            .OrderBy(x => x.DeliveryRequestDate)
            .ThenBy(x => x.ProductCode)
            .Select(x =>
            {
                var detailId = x.MerchandiseOrderDetailId ?? Guid.Empty;
                var deliveredQuantity = detailId != Guid.Empty && deliveryMap.TryGetValue(detailId, out var delivery)
                    ? delivery.DeliveredQuantity
                    : 0m;
                var actualDeliveryDate = x.DeliveryActualDate
                                         ?? (detailId != Guid.Empty && deliveryMap.TryGetValue(detailId, out var deliveryDate)
                                             ? deliveryDate.LastDeliveryDate
                                             : null);
                var actualSoldAmount = deliveredQuantity * x.UnitPrice;

                return new MerchandiseOrderReportDetailDto
                {
                    MerchandiseOrderId = orderId,
                    MerchandiseOrderDetailId = detailId,
                    MerchandiseOrderCode = order.MerchandiseOrderCode,
                    SampleRequestExternalId = x.ProductId.HasValue && sampleMap.TryGetValue(x.ProductId.Value, out var sampleExternalId)
                        ? sampleExternalId
                        : string.Empty,
                    ColourCode = x.ProductId.HasValue && productMap.TryGetValue(x.ProductId.Value, out var colourCode)
                        ? colourCode
                        : string.Empty,
                    ProductCode = x.ProductCode,
                    ProductName = x.ProductName,
                    FormulaCode = x.FormulaCode,
                    DeliveryRequestDate = x.DeliveryRequestDate,
                    ExpectedDeliveryDate = x.ExpectedDeliveryDate,
                    ActualDeliveryDate = actualDeliveryDate,
                    RequestedQuantity = x.RequestedQuantity,
                    DeliveredQuantity = deliveredQuantity,
                    RemainingQuantity = x.RequestedQuantity - deliveredQuantity,
                    UnitPrice = x.UnitPrice,
                    TotalPrice = x.TotalPrice,
                    ActualSoldAmount = actualSoldAmount,
                    RemainingAmount = x.TotalPrice - actualSoldAmount,
                    BagType = x.BagType,
                    PackageWeight = x.PackageWeight,
                    Status = x.Status,
                    Comment = x.Comment
                };
            })
            .ToList();
    }

    /// <summary>
    /// Build query nền cho báo cáo kế hoạch giao hàng.
    /// Query này join MerchandiseOrder, MerchandiseOrderDetail,
    /// Customer và DeliveryOrder để tạo dữ liệu chi tiết giao hàng.
    /// </summary>
    private IQueryable<SummaryMOReportDto> BuildDeliveryPlanReportQuery(
     MerchandiseOrderReportQuery query,
     ViewerScope viewerScope)
    {
        var merchandiseOrders = ApplyReportVisibility(
            _context.MerchandiseOrders
                .AsNoTracking()
                .Where(x =>
                    x.IsActive
                    && x.OrderType == OrderType.Merchandise
                    && x.CustomerExternalIdSnapshot != "KH_VIETAUS"),
            viewerScope);

        if (query.From.HasValue)
        {
            var fromDate = query.From.Value.Date;
            merchandiseOrders = merchandiseOrders.Where(x => x.CreateDate >= fromDate);
        }

        if (query.To.HasValue)
        {
            var toExclusive = query.To.Value.Date.AddDays(1);
            merchandiseOrders = merchandiseOrders.Where(x => x.CreateDate < toExclusive);
        }

        if (query.EmployeeId.HasValue)
        {
            var employeeId = query.EmployeeId.Value;
            merchandiseOrders = merchandiseOrders.Where(x => x.ManagerById == employeeId);
        }

        if (query.GroupId.HasValue)
        {
            var groupId = query.GroupId.Value;
            merchandiseOrders = merchandiseOrders.Where(mo =>
                _context.CustomerAssignments.Any(ca =>
                    ca.IsActive
                    && ca.CustomerId == mo.CustomerId
                    && ca.GroupId == groupId));
        }

        var merchandiseOrderDetails = _context.MerchandiseOrderDetails
            .AsNoTracking()
            .Where(x => x.IsActive);

        var customers = _context.Customers
            .AsNoTracking();

        var deliveryOrderDetails = _context.DeliveryOrderDetails
            .AsNoTracking()
            .Where(x => x.IsActive && !x.IsAttach && x.MerchandiseOrderDetailId != null);

        var deliveryOrders = _context.DeliveryOrders
            .AsNoTracking()
            .Where(x => x.IsActive);

        var deliveryInfo =
            from dod in deliveryOrderDetails
            join doo in deliveryOrders on dod.DeliveryOrderId equals doo.Id
            where dod.MerchandiseOrderDetailId.HasValue
            group new { dod, doo } by dod.MerchandiseOrderDetailId.Value into g
            select new
            {
                MerchandiseOrderDetailId = g.Key,
                DeliveredQuantity = g.Sum(x => (decimal?)x.dod.Quantity) ?? 0m,
                FirstDeliveryDate = g.Min(x => x.doo.CreatedDate),
                LastDeliveryDate = g.Max(x => x.doo.CreatedDate)
            };

        var baseQuery =
            from mo in merchandiseOrders
            join mod in merchandiseOrderDetails
                on mo.MerchandiseOrderId equals mod.MerchandiseOrderId
            join c in customers
                on mo.CustomerId equals c.CustomerId
            join di in deliveryInfo
                on mod.MerchandiseOrderDetailId equals di.MerchandiseOrderDetailId into diLeft
            from di in diLeft.DefaultIfEmpty()
            select new SummaryMOReportDto
            {
                MerchandiseOrderId = mo.MerchandiseOrderId,
                MerchandiseOrderDetailId = mod.MerchandiseOrderDetailId,

                MerchandiseOrderCode = mo.ExternalId ?? string.Empty,
                CustomerCode = c.ExternalId ?? string.Empty,
                CustomerName = c.CustomerName ?? string.Empty,
                ProductCode = mod.ProductExternalIdSnapshot ?? string.Empty,
                ProductName = mod.ProductNameSnapshot ?? string.Empty,

                OrderDate = mo.CreateDate,
                DeliveryRequestDate = mod.DeliveryRequestDate,
                ExpectedDeliveryDate = mod.ExpectedDeliveryDate,

                ActualDeliveryDate = mod.DeliveryActualDate ?? (di != null ? di.LastDeliveryDate : null),

                RequestedQuantity = mod.ExpectedQuantity,
                DeliveredQuantity = di != null ? di.DeliveredQuantity : 0m,
                RemainingQuantity = (mod.ExpectedQuantity) - (di != null ? di.DeliveredQuantity : 0m),

                UnitPrice = mod.UnitPriceAgreed,
                TotalPrice = mod.TotalPriceAgreed,
                ActualSoldAmount = (di != null ? di.DeliveredQuantity : 0m) * mod.UnitPriceAgreed,
                RemainingAmount = mod.TotalPriceAgreed - ((di != null ? di.DeliveredQuantity : 0m) * mod.UnitPriceAgreed),

                Status =
                    mo.Status == "New" ? "Mới" :
                    mo.Status == "Approved" ? "Duyệt" :
                    mo.Status == "Pending" ? "Đang chờ" :
                    mo.Status == "Processing" ? "Đang xử lý" :
                    mo.Status == "Delivering" ? "Đang giao hàng" :
                    mo.Status == "Delivered" ? "Đã giao hàng" :
                    mo.Status == "Paused" ? "Tạm dừng" :
                    mo.Status == "Cancelled" ? "Hủy" :
                    mo.Status == "Completed" ? "Hoàn thành" :
                    "Không xác định"
            };

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            var keyword = query.Keyword.Trim().ToLower();

            baseQuery = baseQuery.Where(x =>
                (x.MerchandiseOrderCode ?? "").ToLower().Contains(keyword) ||
                (x.CustomerCode ?? "").ToLower().Contains(keyword) ||
                (x.CustomerName ?? "").ToLower().Contains(keyword) ||
                (x.ProductCode ?? "").ToLower().Contains(keyword) ||
                (x.ProductName ?? "").ToLower().Contains(keyword));
        }

        return baseQuery;
    }

    /// <summary>
    /// Build query nền cho dashboard đơn hàng bán.
    /// Query này chỉ lấy thông tin chính của đơn hàng trước,
    /// sau đó dữ liệu giao hàng và tiền sẽ được bổ sung ở hàm EnrichMerchandiseOrderRowsAsync.
    /// </summary>
    private IQueryable<MerchandiseOrderReportRowDto> BuildMerchandiseOrderHeaderBaseQuery(
        MerchandiseOrderReportQuery query,
        ViewerScope viewerScope,
        bool includeKeyword = true)
    {
        var merchandiseOrders = ApplyReportVisibility(
            _context.MerchandiseOrders
                .AsNoTracking()
                .Where(x =>
                    x.IsActive
                    && x.OrderType == OrderType.Merchandise
                    && x.CustomerExternalIdSnapshot != "KH_VIETAUS"),
            viewerScope);

        if (query.From.HasValue)
        {
            var fromDate = query.From.Value.Date;
            merchandiseOrders = merchandiseOrders.Where(x => x.CreateDate >= fromDate);
        }

        if (query.To.HasValue)
        {
            var toExclusive = query.To.Value.Date.AddDays(1);
            merchandiseOrders = merchandiseOrders.Where(x => x.CreateDate < toExclusive);
        }

        if (query.EmployeeId.HasValue)
        {
            var employeeId = query.EmployeeId.Value;
            merchandiseOrders = merchandiseOrders.Where(x => x.ManagerById == employeeId);
        }

        if (query.GroupId.HasValue)
        {
            var groupId = query.GroupId.Value;
            merchandiseOrders = merchandiseOrders.Where(mo =>
                _context.CustomerAssignments.Any(ca =>
                    ca.IsActive
                    && ca.CustomerId == mo.CustomerId
                    && ca.GroupId == groupId));
        }

        if (includeKeyword && !string.IsNullOrWhiteSpace(query.Keyword))
        {
            var keyword = query.Keyword.Trim().ToLower();

            merchandiseOrders = merchandiseOrders.Where(mo =>
                (mo.ExternalId ?? "").ToLower().Contains(keyword) ||
                (mo.PONo ?? "").ToLower().Contains(keyword) ||
                (mo.CustomerExternalIdSnapshot ?? "").ToLower().Contains(keyword) ||
                (mo.CustomerNameSnapshot ?? "").ToLower().Contains(keyword) ||
                (mo.ManagerByNameSnapshot ?? "").ToLower().Contains(keyword) ||
                mo.MerchandiseOrderDetails.Any(d =>
                    d.IsActive &&
                    ((d.ProductExternalIdSnapshot ?? "").ToLower().Contains(keyword) ||
                     (d.ProductNameSnapshot ?? "").ToLower().Contains(keyword))));
        }

        return merchandiseOrders.Select(mo => new MerchandiseOrderReportRowDto
        {
            MerchandiseOrderId = mo.MerchandiseOrderId,
            MerchandiseOrderCode = mo.ExternalId ?? string.Empty,
            PONo = mo.PONo ?? string.Empty,
            CustomerId = mo.CustomerId,
            CustomerCode = mo.CustomerExternalIdSnapshot ?? string.Empty,
            CustomerName = mo.CustomerNameSnapshot ?? string.Empty,
            ManagerById = mo.ManagerById,
            ManagerCode = mo.ManagerExternalIdSnapshot ?? string.Empty,
            ManagerName = mo.ManagerByNameSnapshot ?? string.Empty,
            OrderDate = mo.CreateDate,
            TotalOrderAmount = mo.TotalPrice ?? 0m,
            PaymentType = mo.PaymentType ?? string.Empty,
            IsPaid = mo.IsPaid,
            PaymentDate = mo.PaymentDate,
            Currency = mo.Currency ?? string.Empty,
            Status =
                mo.Status == "New" ? "Mới" :
                mo.Status == "Approved" ? "Duyệt" :
                mo.Status == "Pending" ? "Đang chờ" :
                mo.Status == "Processing" ? "Đang xử lý" :
                mo.Status == "Delivering" ? "Đang giao hàng" :
                mo.Status == "Delivered" ? "Đã giao hàng" :
                mo.Status == "Paused" ? "Tạm dừng" :
                mo.Status == "Cancelled" ? "Hủy" :
                mo.Status == "Completed" ? "Hoàn thành" :
                "Không xác định"
        });
    }

    /// <summary>
    /// Build query đầy đủ cho báo cáo đơn hàng bán, có join sẵn số lượng đặt,
    /// số lượng giao, doanh thu thực bán và thông tin group.
    /// Hiện tại có vẻ không còn được dùng nếu flow đang dùng BuildMerchandiseOrderHeaderBaseQuery + Enrich.
    /// </summary>
    private IQueryable<MerchandiseOrderReportRowDto> BuildMerchandiseOrderHeaderReportQuery(
        MerchandiseOrderReportQuery query,
        ViewerScope viewerScope)
    {
        var merchandiseOrders = ApplyReportVisibility(
            _context.MerchandiseOrders
                .AsNoTracking()
                .Where(x =>
                    x.IsActive
                    && x.OrderType == OrderType.Merchandise
                    && x.CustomerExternalIdSnapshot != "KH_VIETAUS"),
            viewerScope);

        if (query.From.HasValue)
        {
            var fromDate = query.From.Value.Date;
            merchandiseOrders = merchandiseOrders.Where(x => x.CreateDate >= fromDate);
        }

        if (query.To.HasValue)
        {
            var toExclusive = query.To.Value.Date.AddDays(1);
            merchandiseOrders = merchandiseOrders.Where(x => x.CreateDate < toExclusive);
        }

        if (query.EmployeeId.HasValue)
        {
            var employeeId = query.EmployeeId.Value;
            merchandiseOrders = merchandiseOrders.Where(x => x.ManagerById == employeeId);
        }

        if (query.GroupId.HasValue)
        {
            var groupId = query.GroupId.Value;
            merchandiseOrders = merchandiseOrders.Where(mo =>
                _context.CustomerAssignments.Any(ca =>
                    ca.IsActive
                    && ca.CustomerId == mo.CustomerId
                    && ca.GroupId == groupId));
        }

        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            var keyword = query.Keyword.Trim().ToLower();

            merchandiseOrders = merchandiseOrders.Where(mo =>
                (mo.ExternalId ?? "").ToLower().Contains(keyword) ||
                (mo.PONo ?? "").ToLower().Contains(keyword) ||
                (mo.CustomerExternalIdSnapshot ?? "").ToLower().Contains(keyword) ||
                (mo.CustomerNameSnapshot ?? "").ToLower().Contains(keyword) ||
                (mo.ManagerByNameSnapshot ?? "").ToLower().Contains(keyword) ||
                mo.MerchandiseOrderDetails.Any(d =>
                    d.IsActive &&
                    ((d.ProductExternalIdSnapshot ?? "").ToLower().Contains(keyword) ||
                     (d.ProductNameSnapshot ?? "").ToLower().Contains(keyword))));
        }

        var deliveryInfo =
            from dod in _context.DeliveryOrderDetails.AsNoTracking()
            join doo in _context.DeliveryOrders.AsNoTracking() on dod.DeliveryOrderId equals doo.Id
            where dod.IsActive
                  && !dod.IsAttach
                  && doo.IsActive
                  && dod.MerchandiseOrderDetailId.HasValue
            group new { dod, doo } by dod.MerchandiseOrderDetailId.Value into g
            select new
            {
                MerchandiseOrderDetailId = g.Key,
                DeliveredQuantity = g.Sum(x => (decimal?)x.dod.Quantity) ?? 0m,
                LastDeliveryDate = g.Max(x => x.doo.CreatedDate)
            };

        var orderAmounts =
            from mod in _context.MerchandiseOrderDetails.AsNoTracking().Where(x => x.IsActive)
            group mod by mod.MerchandiseOrderId into g
            select new
            {
                MerchandiseOrderId = g.Key,
                FirstDeliveryRequestDate = g.Min(x => (DateTime?)x.DeliveryRequestDate),
                LastDeliveryRequestDate = g.Max(x => (DateTime?)x.DeliveryRequestDate),
                OrderedQuantity = g.Sum(x => (decimal?)x.ExpectedQuantity),
                TotalOrderAmount = g.Sum(x => (decimal?)x.TotalPriceAgreed),
                DetailCount = (int?)g.Count()
            };

        var deliveryAmounts =
            from mod in _context.MerchandiseOrderDetails.AsNoTracking().Where(x => x.IsActive)
            join di in deliveryInfo
                on mod.MerchandiseOrderDetailId equals di.MerchandiseOrderDetailId
            group new { mod, di } by mod.MerchandiseOrderId into g
            select new
            {
                MerchandiseOrderId = g.Key,
                DeliveredQuantity = g.Sum(x => (decimal?)x.di.DeliveredQuantity),
                ActualSoldAmount = g.Sum(x => (decimal?)(x.di.DeliveredQuantity * x.mod.UnitPriceAgreed)),
                LastActualDeliveryDate = g.Max(x => x.di.LastDeliveryDate)
            };

        var assignments =
            from ca in _context.CustomerAssignments.AsNoTracking()
            where ca.IsActive
            group ca by new { ca.CustomerId, ca.EmployeeId } into g
            select new
            {
                g.Key.CustomerId,
                g.Key.EmployeeId,
                GroupId = g.OrderByDescending(x => x.CreatedDate).Select(x => (Guid?)x.GroupId).FirstOrDefault(),
                GroupCode = g.OrderByDescending(x => x.CreatedDate).Select(x => x.Group.ExternalId).FirstOrDefault(),
                GroupName = g.OrderByDescending(x => x.CreatedDate).Select(x => x.Group.Name).FirstOrDefault()
            };

        return
            from mo in merchandiseOrders
            join amount in orderAmounts
                on mo.MerchandiseOrderId equals amount.MerchandiseOrderId into amountLeft
            from amount in amountLeft.DefaultIfEmpty()
            join deliveryAmount in deliveryAmounts
                on mo.MerchandiseOrderId equals deliveryAmount.MerchandiseOrderId into deliveryAmountLeft
            from deliveryAmount in deliveryAmountLeft.DefaultIfEmpty()
            join assignment in assignments
                on new { mo.CustomerId, EmployeeId = mo.ManagerById }
                equals new { assignment.CustomerId, assignment.EmployeeId } into assignmentLeft
            from assignment in assignmentLeft.DefaultIfEmpty()
            let totalOrderAmount = ((amount != null ? amount.TotalOrderAmount : null) ?? mo.TotalPrice) ?? 0m
            let actualSoldAmount = (deliveryAmount != null ? deliveryAmount.ActualSoldAmount : null) ?? 0m
            let orderedQuantity = (amount != null ? amount.OrderedQuantity : null) ?? 0m
            let deliveredQuantity = (deliveryAmount != null ? deliveryAmount.DeliveredQuantity : null) ?? 0m
            select new MerchandiseOrderReportRowDto
            {
                MerchandiseOrderId = mo.MerchandiseOrderId,
                MerchandiseOrderCode = mo.ExternalId ?? string.Empty,
                PONo = mo.PONo ?? string.Empty,
                CustomerId = mo.CustomerId,
                CustomerCode = mo.CustomerExternalIdSnapshot ?? string.Empty,
                CustomerName = mo.CustomerNameSnapshot ?? string.Empty,
                ManagerById = mo.ManagerById,
                ManagerCode = mo.ManagerExternalIdSnapshot ?? string.Empty,
                ManagerName = mo.ManagerByNameSnapshot ?? string.Empty,
                GroupId = assignment != null ? assignment.GroupId : null,
                GroupCode = assignment != null ? assignment.GroupCode ?? string.Empty : string.Empty,
                GroupName = assignment != null ? assignment.GroupName ?? string.Empty : string.Empty,
                OrderDate = mo.CreateDate,
                FirstDeliveryRequestDate = amount != null ? amount.FirstDeliveryRequestDate : null,
                LastDeliveryRequestDate = amount != null ? amount.LastDeliveryRequestDate : null,
                LastActualDeliveryDate = deliveryAmount != null ? deliveryAmount.LastActualDeliveryDate : null,
                OrderedQuantity = orderedQuantity,
                DeliveredQuantity = deliveredQuantity,
                RemainingQuantity = orderedQuantity - deliveredQuantity,
                TotalOrderAmount = totalOrderAmount,
                ActualSoldAmount = actualSoldAmount,
                RemainingAmount = totalOrderAmount - actualSoldAmount,
                Currency = mo.Currency ?? string.Empty,
                Status =
                    mo.Status == "New" ? "Mới" :
                    mo.Status == "Approved" ? "Duyệt" :
                    mo.Status == "Pending" ? "Đang chờ" :
                    mo.Status == "Processing" ? "Đang xử lý" :
                    mo.Status == "Delivering" ? "Đang giao hàng" :
                    mo.Status == "Delivered" ? "Đã giao hàng" :
                    mo.Status == "Paused" ? "Tạm dừng" :
                    mo.Status == "Cancelled" ? "Hủy" :
                    mo.Status == "Completed" ? "Hoàn thành" :
                    "Không xác định",
                DetailCount = (amount != null ? amount.DetailCount : null) ?? 0
            };
    }


    /// <summary>
    /// Áp dụng quyền xem dữ liệu theo người dùng hiện tại.
    /// Admin/LabFull được xem toàn bộ trong công ty.
    /// Sale thường chỉ xem khách hàng được assign hoặc claim.
    /// Leader xem dữ liệu theo group của mình.
    /// </summary>
    private IQueryable<MerchandiseOrder> ApplyReportVisibility(
        IQueryable<MerchandiseOrder> query,
        ViewerScope viewerScope)
    {
        query = query.Where(x => x.CompanyId == viewerScope.CompanyId);

        if (viewerScope.ScopeType is ViewerScopeType.AdminFull or ViewerScopeType.LabFull)
        {
            return query;
        }

        var now = viewerScope.Now;

        return query.Where(mo =>
            _context.Customers.Any(c =>
                c.IsActive == true
                && c.CustomerId == mo.CustomerId
                && (
                    (c.IsLead && (
                        !_context.CustomerClaims.Any(cl =>
                            cl.IsActive
                            && cl.Type == ClaimType.Work
                            && cl.CustomerId == c.CustomerId
                            && cl.ExpiresAt > now)
                        || _context.CustomerClaims.Any(cl =>
                            cl.IsActive
                            && cl.Type == ClaimType.Work
                            && cl.CustomerId == c.CustomerId
                            && cl.ExpiresAt > now
                            && cl.EmployeeId == viewerScope.EmployeeId)
                        || (viewerScope.IsLeader && viewerScope.GroupId != null && _context.CustomerClaims.Any(cl =>
                            cl.IsActive
                            && cl.Type == ClaimType.Work
                            && cl.CustomerId == c.CustomerId
                            && cl.ExpiresAt > now
                            && cl.GroupId == viewerScope.GroupId))))
                    || (!c.IsLead && (
                        (!viewerScope.IsLeader && (
                            _context.CustomerAssignments.Any(a =>
                                a.IsActive
                                && a.CustomerId == c.CustomerId
                                && a.EmployeeId == viewerScope.EmployeeId)
                            || _context.CustomerClaims.Any(cl =>
                                cl.IsActive
                                && cl.Type == ClaimType.Work
                                && cl.CustomerId == c.CustomerId
                                && cl.ExpiresAt > now
                                && cl.EmployeeId == viewerScope.EmployeeId)))
                        || (viewerScope.IsLeader && viewerScope.GroupId != null && (
                            _context.CustomerAssignments.Any(a =>
                                a.IsActive
                                && a.CustomerId == c.CustomerId
                                && a.GroupId == viewerScope.GroupId)
                            || _context.CustomerClaims.Any(cl =>
                                cl.IsActive
                                && cl.Type == ClaimType.Work
                                && cl.CustomerId == c.CustomerId
                                && cl.ExpiresAt > now
                                && cl.GroupId == viewerScope.GroupId))))))));
    }

    // Ví dụ nếu cần scope:
    // private IQueryable<MerchandiseOrder> ApplyViewerScope(
    //     IQueryable<MerchandiseOrder> query,
    //     ViewerScope viewerScope)
    // {
    //     // tùy hệ thống của bạn
    //     return query;
    // }
}
