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
            .OrderByDescending(x => x.RemainingQuantity)
            .ThenBy(x => x.DeliveryRequestDate)
            .ThenBy(x => x.CustomerName)
            .ThenBy(x => x.ProductCode)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
         
        return (items, totalCount);
    }


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
    /// ĐÃ CẬP NHẬT: Tính doanh thu theo ngày giao hàng thực tế.
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
                    && x.Status != MerchadiseStatus.Cancelled.ToString()
                    && x.OrderType == OrderType.Merchandise
                    && x.CustomerExternalIdSnapshot != "KH_VIETAUS"),
            viewerScope);

        var monthFrom = query.From?.Date
            ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        var monthToExclusive = query.To?.Date.AddDays(1)
            ?? monthFrom.AddMonths(1);

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

                DeliveredQuantity = g.Sum(x => (decimal?)x.dod.Quantity),

                DeliveredQuantityInPeriod = g.Sum(x =>
                    x.doo.CreatedDate >= monthFrom
                    && x.doo.CreatedDate < monthToExclusive
                        ? (decimal?)x.dod.Quantity
                        : null),

                FirstDeliveryDate = g.Min(x => (DateTime?)x.doo.CreatedDate),
                LastDeliveryDate = g.Max(x => (DateTime?)x.doo.CreatedDate)
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

            let requestedQuantity = mod.ExpectedQuantity
            let deliveredQuantity = di.DeliveredQuantity ?? 0m
            let deliveredQuantityInPeriod = di.DeliveredQuantityInPeriod ?? 0m

            let remainingQuantity = requestedQuantity > deliveredQuantity
                ? requestedQuantity - deliveredQuantity
                : 0m


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

                RequestedQuantity = requestedQuantity,
                DeliveredQuantity = deliveredQuantity,
                RemainingQuantity = remainingQuantity,

                UnitPrice = mod.UnitPriceAgreed,
                TotalPrice = query.IncludeVat
                    ? mod.TotalPriceAgreed * (1 + ((mo.Vat ?? 0m) / 100m))
                    : mod.TotalPriceAgreed,

                ActualSoldAmount = query.IncludeVat
                    ? (deliveredQuantityInPeriod * mod.UnitPriceAgreed) * (1 + ((mo.Vat ?? 0m) / 100m))
                    : deliveredQuantityInPeriod * mod.UnitPriceAgreed,

                RemainingAmount = query.IncludeVat
                    ? (remainingQuantity * mod.UnitPriceAgreed) * (1 + ((mo.Vat ?? 0m) / 100m))
                    : remainingQuantity * mod.UnitPriceAgreed,

                Status =
                    mo.Status == MerchadiseStatus.New.ToString() ? "Mới" :
                    mo.Status == MerchadiseStatus.Approved.ToString() ? "Duyệt" :
                    mo.Status == MerchadiseStatus.Pending.ToString() ? "Đang chờ" :
                    mo.Status == MerchadiseStatus.Processing.ToString() ? "Đang xử lý" :
                    mo.Status == MerchadiseStatus.Delivering.ToString() ? "Đang giao hàng" :
                    mo.Status == MerchadiseStatus.Delivered.ToString() ? "Đã giao hàng" :
                    mo.Status == MerchadiseStatus.Paused.ToString() ? "Tạm dừng" :
                    mo.Status == MerchadiseStatus.Cancelled.ToString() ? "Hủy" :
                    mo.Status == MerchadiseStatus.Completed.ToString() ? "Hoàn thành" :
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
    /// Builds the base order query for the delivery shortage report.
    /// Date filters are applied to merchandise order detail delivery request dates, not delivery slip dates,
    /// so orders without delivery slips are still included when they are due in the filtered period.
    /// </summary>
}

