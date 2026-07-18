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
    public async Task<(IReadOnlyList<InactiveCustomerReportDto> Items, int TotalCount)> GetInactiveCustomersReportAsync(
        MerchandiseOrderReportQuery query,
        ViewerScope viewerScope,
        CancellationToken cancellationToken = default)
    {
        var today = DateTime.Now.Date;

        var pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
        var pageSize = Math.Min(query.PageSize <= 0 ? 10 : query.PageSize, 50);

        var fromDate = query.From?.Date;
        var toExclusive = query.To?.Date.AddDays(1);

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
                (mo.CustomerExternalIdSnapshot ?? "").ToLower().Contains(keyword) ||
                (mo.CustomerNameSnapshot ?? "").ToLower().Contains(keyword) ||
                (mo.ManagerByNameSnapshot ?? "").ToLower().Contains(keyword));
        }

        var customerQuery = merchandiseOrders
            .GroupBy(x => new
            {
                x.CustomerId,
                CustomerCode = x.CustomerExternalIdSnapshot,
                CustomerName = x.CustomerNameSnapshot
            })
            .Select(g => new
            {
                g.Key.CustomerId,
                CustomerCode = g.Key.CustomerCode ?? string.Empty,
                CustomerName = g.Key.CustomerName ?? string.Empty,

                ManagerById = g
                    .OrderByDescending(x => x.CreateDate)
                    .Select(x => x.ManagerById)
                    .FirstOrDefault(),

                ManagerName = g
                    .OrderByDescending(x => x.CreateDate)
                    .Select(x => x.ManagerByNameSnapshot)
                    .FirstOrDefault() ?? string.Empty,

                LastOrderDate = g.Max(x => (DateTime?)x.CreateDate),

                TotalOrderCount = g.Count(),
                TotalOrderAmount = g.Sum(x => x.TotalPrice ?? 0m),

                OrderCountInPeriod = g.Count(x =>
                    (!fromDate.HasValue || x.CreateDate >= fromDate.Value) &&
                    (!toExclusive.HasValue || x.CreateDate < toExclusive.Value)),

                OrderAmountInPeriod = g
                    .Where(x =>
                        (!fromDate.HasValue || x.CreateDate >= fromDate.Value) &&
                        (!toExclusive.HasValue || x.CreateDate < toExclusive.Value))
                    .Sum(x => x.TotalPrice ?? 0m)
            });

        var totalCount = await customerQuery.CountAsync(cancellationToken);

        var pageRows = await customerQuery
            .OrderBy(x => x.LastOrderDate)
            .ThenBy(x => x.CustomerName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = pageRows
            .Select(x =>
            {
                var daysSinceLastOrder = x.LastOrderDate.HasValue
                    ? Math.Max(0, (today - x.LastOrderDate.Value.Date).Days)
                    : 0;

                return new InactiveCustomerReportDto
                {
                    CustomerId = x.CustomerId,
                    CustomerCode = x.CustomerCode,
                    CustomerName = x.CustomerName,

                    ManagerById = x.ManagerById,
                    ManagerName = x.ManagerName,

                    LastOrderDate = x.LastOrderDate,
                    DaysSinceLastOrder = daysSinceLastOrder,

                    TotalOrderCount = x.TotalOrderCount,
                    TotalOrderAmount = x.TotalOrderAmount,

                    OrderCountInPeriod = x.OrderCountInPeriod,
                    OrderAmountInPeriod = x.OrderAmountInPeriod,

                    RiskLevel =
                        daysSinceLastOrder >= 180 ? "Rất lâu chưa mua" :
                        daysSinceLastOrder >= 90 ? "Lâu chưa mua" :
                        daysSinceLastOrder >= 30 ? "Ít mua gần đây" :
                        "Bình thường"
                };
            })
            .ToList();

        return (items, totalCount);
    }
}
