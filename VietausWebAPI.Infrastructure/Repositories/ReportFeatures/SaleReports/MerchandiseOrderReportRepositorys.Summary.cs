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
    private async Task<MerchandiseOrderReportHeaderDto> BuildMerchandiseOrderHeaderSummaryAsync(
        IReadOnlyList<MerchandiseOrderReportRowDto> rows,
        MerchandiseOrderReportQuery query,
        ViewerScope viewerScope,
        CancellationToken cancellationToken)
    {
        var orderedQuantity = rows.Sum(x => x.OrderedQuantity);
        var deliveredQuantity = rows.Sum(x => x.DeliveredQuantity);
        var totalOrderAmount = rows.Sum(x => x.TotalOrderAmount);
        var actualSoldAmount = rows.Sum(x => x.ActualSoldAmount);

        var deliveredInPeriod = rows
            .Where(x => x.DeliveredQuantity > 0m && x.LastActualDeliveryDate.HasValue)
            .ToList();

        var deliveryDates = deliveredInPeriod
            .Select(x => x.LastActualDeliveryDate!.Value.Date)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        var deliveryDaysCount = deliveryDates.Count;
        var revenueInPeriod = deliveredInPeriod.Sum(x => x.ActualSoldAmount);

        decimal previousPeriodRevenue = 0m;
        if (query.From.HasValue && query.To.HasValue)
        {
            var currentFrom = query.From.Value.Date;
            var currentToExclusive = query.To.Value.Date.AddDays(1);
            var periodDays = (currentToExclusive - currentFrom).Days;

            var previousFrom = currentFrom.AddDays(-periodDays);
            var previousToExclusive = currentFrom;

            previousPeriodRevenue = await CalculatePreviousPeriodRevenueAsync(
                previousFrom,
                previousToExclusive,
                query,
                viewerScope,
                cancellationToken);
        }

        var revenueGrowthAmount = revenueInPeriod - previousPeriodRevenue;
        var revenueGrowthRate = previousPeriodRevenue > 0m
            ? (revenueGrowthAmount / previousPeriodRevenue) * 100m
            : 0m;

        var collectedAmount = rows.Where(x => x.IsPaid).Sum(x => x.ActualSoldAmount);
        var collectionRate = actualSoldAmount > 0m ? collectedAmount / actualSoldAmount : 0m;

        var createdFromDate = query.From?.Date;
        var createdToExclusive = query.To?.Date.AddDays(1);
        var ordersCreatedInPeriod = rows.Count(x =>
            (!createdFromDate.HasValue || x.OrderDate >= createdFromDate.Value) &&
            (!createdToExclusive.HasValue || x.OrderDate < createdToExclusive.Value));
        var expectedRevenueSummary = await CalculateExpectedRevenueSummaryAsync(
            query,
            viewerScope,
            cancellationToken);
        return new MerchandiseOrderReportHeaderDto
        {
            TotalOrderCount = rows.Count,
            OrdersCreatedInPeriod = ordersCreatedInPeriod,
            OrdersDeliveredInPeriod = deliveredInPeriod.Count,
            PaidOrderCount = rows.Count(x => x.IsPaid),
            UnpaidOrderCount = rows.Count(x => !x.IsPaid),
            OverdueOrderCount = rows.Count(x => x.IsOverdue),
            CompletedOrderCount = rows.Count(x => x.RemainingQuantity <= 0m && x.DeliveredQuantity > 0m),
            InProgressOrderCount = rows.Count(x => x.DeliveredQuantity > 0m && x.RemainingQuantity > 0m),
            NotDeliveredOrderCount = rows.Count(x => x.DeliveredQuantity <= 0m),

            OrderedQuantity = orderedQuantity,
            DeliveredQuantity = deliveredQuantity,
            RemainingQuantity = rows.Sum(x => x.RemainingQuantity),
            FulfillmentRate = MerchandiseOrderReportFormula.CalculateFulfillmentRate(deliveredQuantity, orderedQuantity),

            TotalOrderAmount = totalOrderAmount,
            ActualSoldAmount = actualSoldAmount,
            RemainingAmount = rows.Sum(x => x.RemainingAmount),
            UnpaidAmount = rows.Sum(x => x.UnpaidAmount),
            ActualSoldRate = MerchandiseOrderReportFormula.CalculateActualSoldRate(actualSoldAmount, totalOrderAmount),

            PreviousPeriodRevenue = previousPeriodRevenue,
            RevenueGrowthRate = revenueGrowthRate,
            RevenueGrowthAmount = revenueGrowthAmount,

            ActualSoldAmountInPeriod = expectedRevenueSummary.ActualSoldAmountInPeriod,
            ExpectedPendingAmountInPeriod = expectedRevenueSummary.ExpectedPendingAmountInPeriod,
            ExpectedRevenueInMonth = expectedRevenueSummary.ExpectedRevenueInMonth,

            AverageOrderValue = deliveredInPeriod.Count > 0 ? revenueInPeriod / deliveredInPeriod.Count : 0m,
            AverageDailyRevenue = deliveryDaysCount > 0 ? revenueInPeriod / deliveryDaysCount : 0m,
            DeliveryDaysCount = deliveryDaysCount,
            FirstDeliveryDate = deliveryDates.FirstOrDefault(),
            LastDeliveryDate = deliveryDates.LastOrDefault(),

            CollectedAmount = collectedAmount,
            CollectionRate = collectionRate,

            RevenueByMonth = BuildRevenueByMonthChart(deliveredInPeriod),
            RevenueByWeek = BuildRevenueByWeekChart(deliveredInPeriod),
            RevenueByManager = BuildRevenueByManagerChart(rows),
            RevenueByCustomer = BuildRevenueByCustomerChart(rows),
            QuantityByManager = BuildQuantityByManagerChart(rows),
            QuantityByCustomer = BuildQuantityByCustomerChart(rows)
        };
    }



    /// <summary>
    /// ✅ MỚI: Tính doanh thu kỳ trước để so sánh.
    /// </summary>
    private async Task<decimal> CalculatePreviousPeriodRevenueAsync(
        DateTime fromDate,
        DateTime toDate,
        MerchandiseOrderReportQuery query,
        ViewerScope viewerScope,
        CancellationToken cancellationToken)
    {
        var merchandiseOrders = ApplyReportVisibility(
            _context.MerchandiseOrders
                .AsNoTracking()
                .Where(x =>
                    x.IsActive
                    && x.OrderType == OrderType.Merchandise
                    && x.CustomerExternalIdSnapshot != "KH_VIETAUS"),
            viewerScope);

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

        var revenueQuery =
            from mo in merchandiseOrders
            join mod in _context.MerchandiseOrderDetails.AsNoTracking()
                on mo.MerchandiseOrderId equals mod.MerchandiseOrderId
            join dod in _context.DeliveryOrderDetails.AsNoTracking()
                on mod.MerchandiseOrderDetailId equals dod.MerchandiseOrderDetailId!.Value
            join doo in _context.DeliveryOrders.AsNoTracking()
                on dod.DeliveryOrderId equals doo.Id
            where mod.IsActive
                  && dod.IsActive
                  && !dod.IsAttach
                  && doo.IsActive
                  && doo.CreatedDate >= fromDate
                  && doo.CreatedDate < toDate
            select new
            {
                Quantity = dod.Quantity,
                UnitPrice = mod.UnitPriceAgreed,
                Vat = mo.Vat
            };

        var rows = await revenueQuery.ToListAsync(cancellationToken);

        return rows.Sum(x =>
        {
            var amount = x.Quantity * x.UnitPrice;
            return ApplyVat(amount, x.Vat, query.IncludeVat);
        });
    }

    /// <summary>
    /// ✅ MỚI: Build chart doanh thu theo tháng GIAO HÀNG.
    /// </summary>
    private static List<MerchandiseOrderReportChartPointDto> BuildRevenueByMonthChart(
        IReadOnlyList<MerchandiseOrderReportRowDto> rows)
    {
        return rows
            .Where(x => x.LastActualDeliveryDate.HasValue)
            .GroupBy(x => x.LastActualDeliveryDate!.Value.ToString("yyyy-MM"))
            .OrderBy(x => x.Key)
            .Select(g => new MerchandiseOrderReportChartPointDto
            {
                Label = g.Key,
                OrderCount = g.Select(x => x.MerchandiseOrderId).Distinct().Count(),
                DeliveryCount = g.Count(),
                OrderedQuantity = g.Sum(x => x.OrderedQuantity),
                DeliveredQuantity = g.Sum(x => x.DeliveredQuantity),
                TotalOrderAmount = g.Sum(x => x.TotalOrderAmount),
                ActualSoldAmount = g.Sum(x => x.ActualSoldAmount),
                RemainingAmount = g.Sum(x => x.RemainingAmount),
                AverageDeliveryAmount = g.Count() > 0 ? g.Sum(x => x.ActualSoldAmount) / g.Count() : 0m,
                PeriodStart = g.Min(x => x.LastActualDeliveryDate),
                PeriodEnd = g.Max(x => x.LastActualDeliveryDate)
            })
            .ToList();
    }

    /// <summary>
    /// ✅ MỚI: Build chart doanh thu theo tuần GIAO HÀNG.
    /// </summary>
    private static List<MerchandiseOrderReportChartPointDto> BuildRevenueByWeekChart(
        IReadOnlyList<MerchandiseOrderReportRowDto> rows)
    {
        return rows
            .Where(x => x.LastActualDeliveryDate.HasValue)
            .GroupBy(x =>
            {
                var date = x.LastActualDeliveryDate!.Value;
                var year = date.Year;
                var weekOfYear = System.Globalization.CultureInfo.CurrentCulture.Calendar
                    .GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                return $"{year}-W{weekOfYear:D2}";
            })
            .OrderBy(x => x.Key)
            .Select(g => new MerchandiseOrderReportChartPointDto
            {
                Label = g.Key,
                OrderCount = g.Select(x => x.MerchandiseOrderId).Distinct().Count(),
                DeliveryCount = g.Count(),
                OrderedQuantity = g.Sum(x => x.OrderedQuantity),
                DeliveredQuantity = g.Sum(x => x.DeliveredQuantity),
                TotalOrderAmount = g.Sum(x => x.TotalOrderAmount),
                ActualSoldAmount = g.Sum(x => x.ActualSoldAmount),
                RemainingAmount = g.Sum(x => x.RemainingAmount),
                AverageDeliveryAmount = g.Count() > 0 ? g.Sum(x => x.ActualSoldAmount) / g.Count() : 0m,
                PeriodStart = g.Min(x => x.LastActualDeliveryDate),
                PeriodEnd = g.Max(x => x.LastActualDeliveryDate)
            })
            .ToList();
    }

    /// <summary>
    /// ✅ MỚI: Build chart doanh thu theo sale/manager.
    /// </summary>
    private static List<MerchandiseOrderReportChartPointDto> BuildRevenueByManagerChart(
        IReadOnlyList<MerchandiseOrderReportRowDto> rows)
    {
        return rows
            .GroupBy(x => string.IsNullOrWhiteSpace(x.ManagerName) ? "Không xác định" : x.ManagerName)
            .OrderByDescending(x => x.Sum(row => row.ActualSoldAmount))
            .Take(10)
            .Select(g => new MerchandiseOrderReportChartPointDto
            {
                Label = g.Key,
                OrderCount = g.Count(),
                DeliveryCount = g.Count(x => x.DeliveredQuantity > 0),
                OrderedQuantity = g.Sum(x => x.OrderedQuantity),
                DeliveredQuantity = g.Sum(x => x.DeliveredQuantity),
                TotalOrderAmount = g.Sum(x => x.TotalOrderAmount),
                ActualSoldAmount = g.Sum(x => x.ActualSoldAmount),
                RemainingAmount = g.Sum(x => x.RemainingAmount),
                AverageDeliveryAmount = g.Count() > 0 ? g.Sum(x => x.ActualSoldAmount) / g.Count() : 0m
            })
            .ToList();
    }

    /// <summary>
    /// ✅ MỚI: Build chart doanh thu theo khách hàng.
    /// </summary>
    private static List<MerchandiseOrderReportChartPointDto> BuildRevenueByCustomerChart(
        IReadOnlyList<MerchandiseOrderReportRowDto> rows)
    {
        return rows
            .GroupBy(x => string.IsNullOrWhiteSpace(x.CustomerName) ? "Không xác định" : x.CustomerName)
            .OrderByDescending(x => x.Sum(row => row.ActualSoldAmount))
            .Take(10)
            .Select(g => new MerchandiseOrderReportChartPointDto
            {
                Label = g.Key,
                OrderCount = g.Count(),
                DeliveryCount = g.Count(x => x.DeliveredQuantity > 0),
                OrderedQuantity = g.Sum(x => x.OrderedQuantity),
                DeliveredQuantity = g.Sum(x => x.DeliveredQuantity),
                TotalOrderAmount = g.Sum(x => x.TotalOrderAmount),
                ActualSoldAmount = g.Sum(x => x.ActualSoldAmount),
                RemainingAmount = g.Sum(x => x.RemainingAmount),
                AverageDeliveryAmount = g.Count() > 0 ? g.Sum(x => x.ActualSoldAmount) / g.Count() : 0m
            })
            .ToList();
    }

    /// <summary>
    /// ✅ MỚI: Build chart số lượng giao theo sale/manager.
    /// </summary>
    private static List<MerchandiseOrderReportChartPointDto> BuildQuantityByManagerChart(
        IReadOnlyList<MerchandiseOrderReportRowDto> rows)
    {
        return rows
            .GroupBy(x => string.IsNullOrWhiteSpace(x.ManagerName) ? "Không xác định" : x.ManagerName)
            .OrderByDescending(x => x.Sum(row => row.DeliveredQuantity))
            .Take(10)
            .Select(g => new MerchandiseOrderReportChartPointDto
            {
                Label = g.Key,
                OrderCount = g.Count(),
                DeliveryCount = g.Count(x => x.DeliveredQuantity > 0),
                OrderedQuantity = g.Sum(x => x.OrderedQuantity),
                DeliveredQuantity = g.Sum(x => x.DeliveredQuantity),
                TotalOrderAmount = g.Sum(x => x.TotalOrderAmount),
                ActualSoldAmount = g.Sum(x => x.ActualSoldAmount),
                RemainingAmount = g.Sum(x => x.RemainingAmount)
            })
            .ToList();
    }

    /// <summary>
    /// ✅ MỚI: Build chart số lượng giao theo khách hàng.
    /// </summary>
    private static List<MerchandiseOrderReportChartPointDto> BuildQuantityByCustomerChart(
        IReadOnlyList<MerchandiseOrderReportRowDto> rows)
    {
        return rows
            .GroupBy(x => string.IsNullOrWhiteSpace(x.CustomerName) ? "Không xác định" : x.CustomerName)
            .OrderByDescending(x => x.Sum(row => row.DeliveredQuantity))
            .Take(10)
            .Select(g => new MerchandiseOrderReportChartPointDto
            {
                Label = g.Key,
                OrderCount = g.Count(),
                DeliveryCount = g.Count(x => x.DeliveredQuantity > 0),
                OrderedQuantity = g.Sum(x => x.OrderedQuantity),
                DeliveredQuantity = g.Sum(x => x.DeliveredQuantity),
                TotalOrderAmount = g.Sum(x => x.TotalOrderAmount),
                ActualSoldAmount = g.Sum(x => x.ActualSoldAmount),
                RemainingAmount = g.Sum(x => x.RemainingAmount)
            })
            .ToList();
    }

    /// <summary>
    /// Build query nền cho báo cáo kế hoạch giao hàng.
    /// Query này join MerchandiseOrder, MerchandiseOrderDetail,
    /// Customer và DeliveryOrder để tạo dữ liệu chi tiết giao hàng.
    /// </summary>

    private sealed class ExpectedRevenueSummary
    {
        public decimal ActualSoldAmountInPeriod { get; set; }
        public decimal ExpectedPendingAmountInPeriod { get; set; }
        public decimal ExpectedRevenueInMonth =>
            ActualSoldAmountInPeriod + ExpectedPendingAmountInPeriod;
    }
    private async Task<ExpectedRevenueSummary> CalculateExpectedRevenueSummaryAsync(
    MerchandiseOrderReportQuery query,
    ViewerScope viewerScope,
    CancellationToken cancellationToken)
    {
        var fromDate = query.From?.Date
            ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        var toExclusive = query.To?.Date.AddDays(1)
            ?? fromDate.AddMonths(1);

        var merchandiseOrders = ApplyReportVisibility(
            _context.MerchandiseOrders
                .AsNoTracking()
                .Where(x =>
                    x.IsActive
                    && x.Status != MerchadiseStatus.Cancelled.ToString()
                    && x.OrderType == OrderType.Merchandise
                    && x.CustomerExternalIdSnapshot != "KH_VIETAUS"),
            viewerScope);

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

        var actualSoldRows = await (
            from mo in merchandiseOrders
            join mod in _context.MerchandiseOrderDetails.AsNoTracking()
                on mo.MerchandiseOrderId equals mod.MerchandiseOrderId
            join dod in _context.DeliveryOrderDetails.AsNoTracking()
                on mod.MerchandiseOrderDetailId equals dod.MerchandiseOrderDetailId!.Value
            join doo in _context.DeliveryOrders.AsNoTracking()
                on dod.DeliveryOrderId equals doo.Id
            where mod.IsActive
                  && dod.IsActive
                  && !dod.IsAttach
                  && doo.IsActive
                  && doo.CreatedDate >= fromDate
                  && doo.CreatedDate < toExclusive
            select new
            {
                Amount = dod.Quantity * mod.UnitPriceAgreed,
                Vat = mo.Vat
            })
            .ToListAsync(cancellationToken);

        var actualSoldAmountInPeriod = actualSoldRows.Sum(x =>
            ApplyVat(x.Amount, x.Vat, query.IncludeVat));

        var deliveredQuantityByDetail = await (
            from dod in _context.DeliveryOrderDetails.AsNoTracking()
            join doo in _context.DeliveryOrders.AsNoTracking()
                on dod.DeliveryOrderId equals doo.Id
            where dod.IsActive
                  && !dod.IsAttach
                  && doo.IsActive
                  && dod.MerchandiseOrderDetailId.HasValue
            group dod by dod.MerchandiseOrderDetailId!.Value into g
            select new
            {
                MerchandiseOrderDetailId = g.Key,
                DeliveredQuantity = g.Sum(x => (decimal?)x.Quantity) ?? 0m
            })
            .ToListAsync(cancellationToken);

        var deliveredMap = deliveredQuantityByDetail.ToDictionary(
            x => x.MerchandiseOrderDetailId,
            x => x.DeliveredQuantity);

        var pendingRows = await (
            from mo in merchandiseOrders
            join mod in _context.MerchandiseOrderDetails.AsNoTracking()
                on mo.MerchandiseOrderId equals mod.MerchandiseOrderId
            where mod.IsActive
                  && mo.Status != MerchadiseStatus.Completed.ToString()
                  && mo.Status != MerchadiseStatus.Delivered.ToString()
                  && mo.Status != MerchadiseStatus.Paused.ToString()
                  && ((mod.ExpectedDeliveryDate ?? mod.DeliveryRequestDate) >= fromDate)
                  && ((mod.ExpectedDeliveryDate ?? mod.DeliveryRequestDate) < toExclusive)
            select new
            {
                mod.MerchandiseOrderDetailId,
                mod.ExpectedQuantity,
                mod.UnitPriceAgreed,
                Vat = mo.Vat
            })
            .ToListAsync(cancellationToken);

        var expectedPendingAmountInPeriod = pendingRows.Sum(x =>
        {
            deliveredMap.TryGetValue(x.MerchandiseOrderDetailId, out var deliveredQuantity);

            var remainingQuantity = x.ExpectedQuantity - deliveredQuantity;
            if (remainingQuantity <= 0m)
            {
                return 0m;
            }

            var amount = remainingQuantity * x.UnitPriceAgreed;
            return ApplyVat(amount, x.Vat, query.IncludeVat);
        });

        return new ExpectedRevenueSummary
        {
            ActualSoldAmountInPeriod = actualSoldAmountInPeriod,
            ExpectedPendingAmountInPeriod = expectedPendingAmountInPeriod
        };
    }
}

