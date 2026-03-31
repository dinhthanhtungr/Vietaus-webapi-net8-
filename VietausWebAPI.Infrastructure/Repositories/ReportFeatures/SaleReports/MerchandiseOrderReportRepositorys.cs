using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.SaleReports;
using VietausWebAPI.Core.Application.Features.Shared.DTO.Visibility;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;
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

    private IQueryable<SummaryMOReportDto> BuildDeliveryPlanReportQuery(
     MerchandiseOrderReportQuery query,
     ViewerScope viewerScope)
    {
        var EmployeeId = _currentUser.EmployeeId; // cần có trong query DTO

        Guid groupId = _context.MemberInGroups
            .AsNoTracking()
            .Where(x => x.Profile == EmployeeId && x.IsActive)
            .Select(x => x.GroupId)
            .FirstOrDefault();

        var merchandiseOrders = _context.MerchandiseOrders
            .AsNoTracking()
            .Where(x => x.IsActive);

        var merchandiseOrderDetails = _context.MerchandiseOrderDetails
            .AsNoTracking()
            .Where(x => x.IsActive);

        var customers = _context.Customers
            .AsNoTracking();

        var customerAssignments = _context.CustomerAssignments
            .AsNoTracking()
            .Where(x => x.IsActive);

        var deliveryOrderDetails = _context.DeliveryOrderDetails
            .AsNoTracking()
            .Where(x => x.IsActive && !x.IsAttach && x.MerchandiseOrderDetailId != null);

        var deliveryOrders = _context.DeliveryOrders
            .AsNoTracking()
            .Where(x => x.IsActive);

        var targetCustomers =
            from ca in customerAssignments
            where ca.GroupId == groupId
            select ca.CustomerId;

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
            join tc in targetCustomers.Distinct()
                on mo.CustomerId equals tc
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

        if (query.From.HasValue)
        {
            var fromDate = query.From.Value.Date;
            baseQuery = baseQuery.Where(x => x.OrderDate >= fromDate);
        }

        if (query.To.HasValue)
        {
            var toExclusive = query.To.Value.Date.AddDays(1);
            baseQuery = baseQuery.Where(x => x.OrderDate < toExclusive);
        }

        return baseQuery;
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