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

public partial class MerchandiseOrderReportRepository
{
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

        var items = await EnrichMerchandiseOrderRowsAsync(orders, query, cancellationToken);

        return await BuildMerchandiseOrderHeaderSummaryAsync(items, query, viewerScope, cancellationToken);
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

        // ✅ THAY ĐỔI: Truyền thêm query parameter
        var items = await EnrichMerchandiseOrderRowsAsync(pageOrders, query, cancellationToken);

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
        MerchandiseOrderReportQuery query,
        CancellationToken cancellationToken)
    {
        if (orders.Count == 0)
        {
            return orders;
        }

        var orderIds = orders.Select(x => x.MerchandiseOrderId).Distinct().ToList();
        var customerIds = orders.Select(x => x.CustomerId).Distinct().ToList();
        var managerIds = orders.Select(x => x.ManagerById).Distinct().ToList();
        var fromDate = query.From?.Date;
        var toExclusive = query.To?.Date.AddDays(1);

        // Lấy thông tin order details
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

        // Tính tiến độ giao lũy kế của đơn.
        // Không lọc theo kỳ ở đây, vì RemainingQuantity phải dựa trên toàn bộ số đã giao
        // của đơn hàng. Bộ lọc ngày chỉ dùng để chọn tập đơn ở BuildMerchandiseOrderHeaderBaseQuery.
        var deliveryInfoQuery = 
            from dod in _context.DeliveryOrderDetails.AsNoTracking()
            join doo in _context.DeliveryOrders.AsNoTracking() on dod.DeliveryOrderId equals doo.Id
            where dod.IsActive
                  && !dod.IsAttach
                  && doo.IsActive
                  && dod.MerchandiseOrderDetailId.HasValue
            select new { dod, doo };

        var deliveryInfo =
            deliveryInfoQuery
            .GroupBy(x => x.dod.MerchandiseOrderDetailId!.Value)
            .Select(g => new
            {
                MerchandiseOrderDetailId = g.Key,
                DeliveredQuantityTotal = (decimal?)g.Sum(x => x.dod.Quantity),
                DeliveredQuantityInPeriod = g.Sum(x =>
                    (!fromDate.HasValue || x.doo.CreatedDate >= fromDate.Value)
                    && (!toExclusive.HasValue || x.doo.CreatedDate < toExclusive.Value)
                        ? (decimal?)x.dod.Quantity
                        : null),
                FirstDeliveryDate = g.Min(x => (DateTime?)x.doo.CreatedDate),
                LastDeliveryDate = g.Max(x => (DateTime?)x.doo.CreatedDate),
                FirstDeliveryDateInPeriod = g
                    .Where(x =>
                        (!fromDate.HasValue || x.doo.CreatedDate >= fromDate.Value)
                        && (!toExclusive.HasValue || x.doo.CreatedDate < toExclusive.Value))
                    .Min(x => (DateTime?)x.doo.CreatedDate),
                LastDeliveryDateInPeriod = g
                    .Where(x =>
                        (!fromDate.HasValue || x.doo.CreatedDate >= fromDate.Value)
                        && (!toExclusive.HasValue || x.doo.CreatedDate < toExclusive.Value))
                    .Max(x => (DateTime?)x.doo.CreatedDate),
                DeliveryCount = g.Select(x => x.doo.Id).Distinct().Count(),
                DeliveryCountInPeriod = g
                    .Where(x =>
                        (!fromDate.HasValue || x.doo.CreatedDate >= fromDate.Value)
                        && (!toExclusive.HasValue || x.doo.CreatedDate < toExclusive.Value))
                    .Select(x => x.doo.Id)
                    .Distinct()
                    .Count()
            });

        var deliveryAmounts = await (
            from mod in _context.MerchandiseOrderDetails.AsNoTracking()
            join di in deliveryInfo
                on mod.MerchandiseOrderDetailId equals di.MerchandiseOrderDetailId
            where mod.IsActive && orderIds.Contains(mod.MerchandiseOrderId)
            group new { mod, di } by mod.MerchandiseOrderId into g
            select new
            {
                MerchandiseOrderId = g.Key,
                DeliveredQuantity = g.Sum(x => (decimal?)x.di.DeliveredQuantityInPeriod) ?? 0m,
                ActualSoldAmount = g.Sum(x => (decimal?)(x.di.DeliveredQuantityInPeriod * x.mod.UnitPriceAgreed)) ?? 0m,
                FirstDeliveryDate = g.Min(x => x.di.FirstDeliveryDateInPeriod),
                LastActualDeliveryDate = g.Max(x => x.di.LastDeliveryDateInPeriod),
                DeliveryCount = g.Sum(x => x.di.DeliveryCountInPeriod)
            })
            .ToListAsync(cancellationToken);

        var remainingRows = await (
            from mod in _context.MerchandiseOrderDetails.AsNoTracking()
            join di in deliveryInfo
                on mod.MerchandiseOrderDetailId equals di.MerchandiseOrderDetailId into diLeft
            from di in diLeft.DefaultIfEmpty()
            where mod.IsActive && orderIds.Contains(mod.MerchandiseOrderId)
            select new
            {
                mod.MerchandiseOrderId,
                mod.ExpectedQuantity,
                mod.UnitPriceAgreed,
                DeliveredQuantity = di.DeliveredQuantityTotal ?? 0m
            })
            .ToListAsync(cancellationToken);

        var remainingMap = remainingRows
            .GroupBy(x => x.MerchandiseOrderId)
            .ToDictionary(
                g => g.Key,
                g => new
                {
                    RemainingQuantity = g.Sum(x =>
                    {
                        var remaining = x.ExpectedQuantity - x.DeliveredQuantity;
                        return remaining > 0m ? remaining : 0m;
                    }),
                    RemainingAmount = g.Sum(x =>
                    {
                        var remaining = x.ExpectedQuantity - x.DeliveredQuantity;
                        return remaining > 0m
                            ? remaining * x.UnitPriceAgreed
                            : 0m;
                    })
                });

        var assignmentRows = await _context.CustomerAssignments
            .AsNoTracking()
            .Where(x => x.IsActive && customerIds.Contains(x.CustomerId) && managerIds.Contains(x.EmployeeId))
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

            //var totalOrderAmount = orderAmount?.TotalOrderAmount ?? order.TotalOrderAmount;

            var totalOrderAmount = orderAmount?.TotalOrderAmount ?? order.TotalOrderAmount;

            totalOrderAmount = ApplyVat(
                totalOrderAmount,
                order.VatPercent,
                query.IncludeVat);

            //var actualSoldAmount = deliveryAmount?.ActualSoldAmount ?? 0m;

            var actualSoldAmount = deliveryAmount?.ActualSoldAmount ?? 0m;

            actualSoldAmount = ApplyVat(
                actualSoldAmount,
                order.VatPercent,
                query.IncludeVat);

            remainingMap.TryGetValue(order.MerchandiseOrderId, out var remainingInfo);

            var remainingQuantity = remainingInfo?.RemainingQuantity ?? 0m;
            var remainingAmount = remainingInfo?.RemainingAmount ?? 0m;

            remainingAmount = ApplyVat(
                remainingAmount,
                order.VatPercent,
                query.IncludeVat);

            var orderedQuantity = orderAmount?.OrderedQuantity ?? 0m;
            var deliveredQuantity = deliveryAmount?.DeliveredQuantity ?? 0m;

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
            order.RemainingAmount = remainingAmount;
            order.FulfillmentRate = MerchandiseOrderReportFormula.CalculateFulfillmentRate(deliveredQuantity, orderedQuantity);
            order.ActualSoldRate = MerchandiseOrderReportFormula.CalculateActualSoldRate(actualSoldAmount, totalOrderAmount);
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
    /// ✅ MỚI: Build summary với logic tính theo ngày giao hàng và so sánh kỳ trước.
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

        var fromDate = query.From?.Date;
        var toExclusive = query.To?.Date.AddDays(1);

        if (fromDate.HasValue || toExclusive.HasValue)
        {
            var deliveredOrderIds = BuildDeliveredOrderIdsQuery(fromDate, toExclusive);
            merchandiseOrders = merchandiseOrders.Where(x => deliveredOrderIds.Contains(x.MerchandiseOrderId));
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
                _context.MemberInGroups.Any(m =>
                    m.IsActive
                    && m.GroupId == groupId
                    && m.Profile.HasValue
                    && m.Profile.Value == mo.ManagerById));
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
            VatPercent = mo.Vat,
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

    private IQueryable<Guid> BuildDeliveredOrderIdsQuery(DateTime? fromDate, DateTime? toExclusive)
    {
        var query =
            from dod in _context.DeliveryOrderDetails.AsNoTracking()
            join doo in _context.DeliveryOrders.AsNoTracking()
                on dod.DeliveryOrderId equals doo.Id
            join mod in _context.MerchandiseOrderDetails.AsNoTracking()
                on dod.MerchandiseOrderDetailId equals mod.MerchandiseOrderDetailId
            where dod.IsActive
                  && !dod.IsAttach
                  && dod.MerchandiseOrderDetailId.HasValue
                  && doo.IsActive
                  && mod.IsActive
            select new
            {
                mod.MerchandiseOrderId,
                doo.CreatedDate
            };

        if (fromDate.HasValue)
        {
            query = query.Where(x => x.CreatedDate >= fromDate.Value);
        }

        if (toExclusive.HasValue)
        {
            query = query.Where(x => x.CreatedDate < toExclusive.Value);
        }

        return query
            .Select(x => x.MerchandiseOrderId)
            .Distinct();
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
}

