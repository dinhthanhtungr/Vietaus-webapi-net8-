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
    /// Gets merchandise orders that still have undelivered quantity.
    /// Only orders with delivery request dates from 2026-03-01 onward are included.
    /// Undelivered quantity is calculated as ordered quantity minus total delivered quantity,
    /// including orders without delivery slips and orders that have only been partially delivered.
    /// </summary>
    public async Task<(IReadOnlyList<MerchandiseOrderReportRowDto> Items, int TotalCount)> GetDeliveryShortageReportAsync(
        MerchandiseOrderReportQuery query,
        ViewerScope viewerScope,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
        var pageSize = query.PageSize <= 0 ? 20 : query.PageSize;

        var reportQuery = BuildDeliveryShortageOrderBaseQuery(query, viewerScope);

        var orders = await reportQuery
            .OrderByDescending(x => x.OrderDate)
            .ThenBy(x => x.CustomerName)
            .ThenBy(x => x.MerchandiseOrderCode)
            .ToListAsync(cancellationToken);

        var enrichedRows = await EnrichMerchandiseOrderRowsAsync(orders, query, cancellationToken);

        var shortageRows = enrichedRows
            .Where(x => x.RemainingQuantity > 0m)
            .OrderByDescending(x => x.RemainingQuantity)
            .ThenBy(x => x.FirstDeliveryRequestDate ?? DateTime.MaxValue)
            .ThenBy(x => x.CustomerName)
            .ThenBy(x => x.MerchandiseOrderCode)
            .ToList();

        var totalCount = shortageRows.Count;

        var items = shortageRows
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (items, totalCount);
    }

    /// <summary>
    /// Lấy toàn bộ dữ liệu kế hoạch giao hàng để export Excel.
    /// Không phân trang, dùng chung query với màn hình báo cáo kế hoạch giao hàng.
    /// </summary>

    private IQueryable<MerchandiseOrderReportRowDto> BuildDeliveryShortageOrderBaseQuery(
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

        var requestedFromDate = query.From?.Date;
        var fromDate = requestedFromDate.HasValue && requestedFromDate.Value > DeliveryShortageStartDate
            ? requestedFromDate.Value
            : DeliveryShortageStartDate;

        var toExclusive = query.To?.Date.AddDays(1);

        merchandiseOrders = merchandiseOrders.Where(mo =>
            mo.MerchandiseOrderDetails.Any(d =>
                d.IsActive
                && d.DeliveryRequestDate >= fromDate
                && (!toExclusive.HasValue || d.DeliveryRequestDate < toExclusive.Value)));

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

    /// <summary>
    /// Build query nền cho dashboard đơn hàng bán.
    /// Query này chỉ lấy thông tin chính của đơn hàng trước,
    /// sau đó dữ liệu giao hàng và tiền sẽ được bổ sung ở hàm EnrichMerchandiseOrderRowsAsync.
    /// </summary>
}

