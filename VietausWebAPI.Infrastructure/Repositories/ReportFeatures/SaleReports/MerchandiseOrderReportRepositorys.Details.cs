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
    /// Lấy chi tiết của một đơn hàng bán.
    /// Trả về từng dòng sản phẩm trong đơn, gồm số lượng đặt,
    /// đã giao, còn lại, đơn giá, thành tiền, thực bán,
    /// ngày giao và trạng thái dòng hàng.
    /// </summary>
    public async Task<IReadOnlyList<MerchandiseOrderReportDetailDto>> GetMerchandiseOrderDetailReportAsync(
        Guid merchandiseOrderId,
        MerchandiseOrderReportQuery query,
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
                VatPercent = EF.Property<decimal?>(x, nameof(MerchandiseOrder.Vat)),
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

                var remainingQuantity = x.RequestedQuantity - deliveredQuantity;
                if (remainingQuantity < 0m)
                {
                    remainingQuantity = 0m;
                }

                var totalPrice = ApplyVat(x.TotalPrice, order.VatPercent, query.IncludeVat);
                var actualSoldAmount = ApplyVat(deliveredQuantity * x.UnitPrice, order.VatPercent, query.IncludeVat);
                var remainingAmount = ApplyVat(remainingQuantity * x.UnitPrice, order.VatPercent, query.IncludeVat);

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
                    RemainingQuantity = remainingQuantity,
                    UnitPrice = x.UnitPrice,
                    TotalPrice = totalPrice,
                    ActualSoldAmount = actualSoldAmount,
                    RemainingAmount = remainingAmount,
                    BagType = x.BagType,
                    PackageWeight = x.PackageWeight,
                    Status = x.Status,
                    Comment = x.Comment
                };
            })
            .Where(x => x.RemainingQuantity > 0m)
            .OrderByDescending(x => x.RemainingQuantity)
            .ThenBy(x => x.DeliveryRequestDate ?? DateTime.MaxValue)
            .ThenBy(x => x.ProductCode)
            .ToList();
    }
}
