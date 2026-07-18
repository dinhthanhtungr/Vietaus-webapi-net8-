using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.SaleReports;
using VietausWebAPI.Core.Application.Features.Shared.DTO.Visibility;
using VietausWebAPI.Core.Domain.Enums.Formulas;
using VietausWebAPI.Core.Domain.Enums.Merchadises;

public partial class MerchandiseOrderReportRepository
{
    /// <summary>
    /// Lấy báo cáo chi tiết theo sale.
    /// Doanh thu thực được tính từ các dòng giao hàng trong kỳ lọc.
    /// Giá vốn được tính từ VA manufacturing formula đang được chọn cho dòng đơn hàng,
    /// gồm giá NVL mới nhất và phụ phí sản xuất theo logic của MaterialsSupplierPriceProvider.
    /// </summary>
    public async Task<(IReadOnlyList<MerchandiseOrderSaleDetailReportDto> Items, int TotalCount)> GetSaleDetailReportAsync(
        MerchandiseOrderReportQuery query,
        ViewerScope viewerScope,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
        var pageSize = Math.Min(query.PageSize <= 0 ? 20 : query.PageSize, 100);
        var fromDate = query.From?.Date;
        var toExclusive = query.To?.Date.AddDays(1);

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

        var deliveryQuery =
            from dod in _context.DeliveryOrderDetails.AsNoTracking()
            join doo in _context.DeliveryOrders.AsNoTracking()
                on dod.DeliveryOrderId equals doo.Id
            join mod in _context.MerchandiseOrderDetails.AsNoTracking()
                on dod.MerchandiseOrderDetailId equals mod.MerchandiseOrderDetailId
            join mo in merchandiseOrders
                on mod.MerchandiseOrderId equals mo.MerchandiseOrderId
            where dod.IsActive
                  && !dod.IsAttach
                  && doo.IsActive
                  && doo.CreatedDate.HasValue
                  && mod.IsActive
                  && dod.MerchandiseOrderDetailId.HasValue
            select new SaleDeliveryReportRow
            {
                DeliveryOrderId = doo.Id,
                DeliveryDate = doo.CreatedDate!.Value,
                MerchandiseOrderId = mo.MerchandiseOrderId,
                MerchandiseOrderCode = mo.ExternalId ?? string.Empty,
                MerchandiseOrderDetailId = mod.MerchandiseOrderDetailId,
                CustomerId = mo.CustomerId,
                ManagerById = mo.ManagerById,
                ManagerCode = mo.ManagerExternalIdSnapshot ?? string.Empty,
                ManagerName = mo.ManagerByNameSnapshot ?? string.Empty,
                ProductId = mod.ProductId,
                ProductCode = dod.ProductExternalIdSnapShot ?? mod.ProductExternalIdSnapshot,
                ProductName = dod.ProductNameSnapShot ?? mod.ProductNameSnapshot,
                DeliveryOrderCode = doo.ExternalId ?? string.Empty,
                LotNoList = dod.LotNoList ?? string.Empty,
                VatPercent = mo.Vat,
                DeliveredQuantity = dod.Quantity,
                UnitPrice = mod.UnitPriceAgreed
            };

        if (fromDate.HasValue)
        {
            deliveryQuery = deliveryQuery.Where(x => x.DeliveryDate >= fromDate.Value);
        }

        if (toExclusive.HasValue)
        {
            deliveryQuery = deliveryQuery.Where(x => x.DeliveryDate < toExclusive.Value);
        }

        var deliveryRows = await deliveryQuery.ToListAsync(cancellationToken);

        if (deliveryRows.Count == 0)
        {
            return (Array.Empty<MerchandiseOrderSaleDetailReportDto>(), 0);
        }

        var orderIds = deliveryRows
            .Select(x => x.MerchandiseOrderId)
            .Distinct()
            .ToList();

        var detailIds = deliveryRows
            .Select(x => x.MerchandiseOrderDetailId)
            .Distinct()
            .ToList();

        var productIds = deliveryRows
            .Select(x => x.ProductId)
            .Distinct()
            .ToList();

        var formulaVersionRows = await (
            from mop in _context.MfgOrderPOs.AsNoTracking()
            join psv in _context.ProductionSelectVersions.AsNoTracking()
                on mop.MfgProductionOrderId equals psv.MfgProductionOrderId
            where mop.IsActive
                  && detailIds.Contains(mop.MerchandiseOrderDetailId)
                  && psv.ManufacturingFormulaId.HasValue
            select new FormulaVersionReportRow
            {
                MerchandiseOrderDetailId = mop.MerchandiseOrderDetailId,
                ManufacturingFormulaId = psv.ManufacturingFormulaId!.Value,
                ValidFrom = psv.ValidFrom,
                ValidTo = psv.ValidTo
            })
            .ToListAsync(cancellationToken);

        var formulaIds = formulaVersionRows
            .Select(x => x.ManufacturingFormulaId)
            .Distinct()
            .ToList();

        var lotCodes = deliveryRows
            .SelectMany(x => SplitLotCodes(x.LotNoList))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var lotFormulaRows = await _context.ManufacturingFormulas
            .AsNoTracking()
            .Where(x => x.IsActive && lotCodes.Contains(x.ExternalId))
            .Select(x => new
            {
                x.ManufacturingFormulaId,
                x.ExternalId
            })
            .ToListAsync(cancellationToken);

        var lotFormulaMap = lotFormulaRows
            .GroupBy(x => x.ExternalId, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                g => g.Key,
                g => g
                    .OrderByDescending(x => x.ExternalId)
                    .Select(x => x.ManufacturingFormulaId)
                    .First(),
                StringComparer.OrdinalIgnoreCase);

        var lotFormulaIds = lotFormulaRows
            .Select(x => x.ManufacturingFormulaId)
            .Distinct()
            .ToList();

        formulaIds = formulaIds
            .Concat(lotFormulaIds)
            .Distinct()
            .ToList();

        var formulaMaterialRows = await _context.ManufacturingFormulaMaterials
            .AsNoTracking()
            .Where(x =>
                x.IsActive
                && x.itemType == ItemType.Material
                && x.MaterialId.HasValue
                && formulaIds.Contains(x.ManufacturingFormulaId))
            .Select(x => new
            {
                x.ManufacturingFormulaId,
                MaterialId = x.MaterialId!.Value,
                x.Quantity,
                x.UnitPrice
            })
            .ToListAsync(cancellationToken);

        var formulaSnapshotMaterialUnitCostMap = formulaMaterialRows
            .GroupBy(x => x.ManufacturingFormulaId)
            .ToDictionary(
                g => g.Key,
                g => g.Sum(x => x.Quantity * x.UnitPrice));

        var deliveryDates = deliveryRows
            .Select(x => x.DeliveryDate.Date)
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        var formulaHistoricalMaterialUnitCostMap = await BuildHistoricalMaterialUnitCostMapAsync(
            formulaMaterialRows
                .Select(x => new FormulaMaterialCostRow
                {
                    ManufacturingFormulaId = x.ManufacturingFormulaId,
                    MaterialId = x.MaterialId,
                    Quantity = x.Quantity
                })
                .ToList(),
            deliveryDates,
            cancellationToken);

        var productRows = await _context.Products
            .AsNoTracking()
            .Where(x => productIds.Contains(x.ProductId) && x.IsActive)
            .Select(x => new
            {
                x.ProductId,
                x.IsRecycle,
                x.ColourCode
            })
            .ToListAsync(cancellationToken);

        var productAddOnMap = productRows.ToDictionary(
            x => x.ProductId,
            x => CalculateProductionAddOn(x.IsRecycle, x.ColourCode));

        var allDeliveredQuantityRows = await (
            from dod in _context.DeliveryOrderDetails.AsNoTracking()
            join doo in _context.DeliveryOrders.AsNoTracking()
                on dod.DeliveryOrderId equals doo.Id
            where dod.IsActive
                  && !dod.IsAttach
                  && doo.IsActive
                  && dod.MerchandiseOrderDetailId.HasValue
                  && detailIds.Contains(dod.MerchandiseOrderDetailId.Value)
            group dod by dod.MerchandiseOrderDetailId!.Value into g
            select new
            {
                MerchandiseOrderDetailId = g.Key,
                DeliveredQuantity = g.Sum(x => (decimal?)x.Quantity) ?? 0m
            })
            .ToListAsync(cancellationToken);

        var allDeliveredQuantityMap = allDeliveredQuantityRows.ToDictionary(
            x => x.MerchandiseOrderDetailId,
            x => x.DeliveredQuantity);

        var orderDetailRows = await (
            from mod in _context.MerchandiseOrderDetails.AsNoTracking()
            join mo in merchandiseOrders
                on mod.MerchandiseOrderId equals mo.MerchandiseOrderId
            where mod.IsActive
                  && orderIds.Contains(mod.MerchandiseOrderId)
            select new
            {
                mo.ManagerById,
                mod.MerchandiseOrderId,
                mod.MerchandiseOrderDetailId,
                mod.ExpectedQuantity,
                mod.UnitPriceAgreed,
                mod.TotalPriceAgreed,
                VatPercent = mo.Vat
            })
            .ToListAsync(cancellationToken);

        var orderMetricMap = orderDetailRows
            .GroupBy(x => x.ManagerById)
            .ToDictionary(
                g => g.Key,
                g => new
                {
                    OrderedQuantity = g.Sum(x => x.ExpectedQuantity),
                    TotalOrderAmount = g.Sum(x => ApplyVat(x.TotalPriceAgreed, x.VatPercent, query.IncludeVat)),
                    RemainingQuantity = g.Sum(x =>
                    {
                        allDeliveredQuantityMap.TryGetValue(x.MerchandiseOrderDetailId, out var deliveredQuantity);
                        var remainingQuantity = x.ExpectedQuantity - deliveredQuantity;
                        return remainingQuantity > 0m ? remainingQuantity : 0m;
                    }),
                    RemainingAmount = g.Sum(x =>
                    {
                        allDeliveredQuantityMap.TryGetValue(x.MerchandiseOrderDetailId, out var deliveredQuantity);
                        var remainingQuantity = x.ExpectedQuantity - deliveredQuantity;
                        return remainingQuantity > 0m
                            ? ApplyVat(remainingQuantity * x.UnitPriceAgreed, x.VatPercent, query.IncludeVat)
                            : 0m;
                    })
                });

        var managerIds = deliveryRows
            .Select(x => x.ManagerById)
            .Distinct()
            .ToList();

        var assignmentRows = await _context.CustomerAssignments
            .AsNoTracking()
            .Where(x => x.IsActive && managerIds.Contains(x.EmployeeId))
            .OrderByDescending(x => x.CreatedDate)
            .Select(x => new
            {
                x.EmployeeId,
                GroupId = (Guid?)x.GroupId,
                GroupCode = x.Group.ExternalId,
                GroupName = x.Group.Name
            })
            .ToListAsync(cancellationToken);

        var assignmentMap = assignmentRows
            .GroupBy(x => x.EmployeeId)
            .ToDictionary(x => x.Key, x => x.First());

        var saleRows = deliveryRows
            .Select(row =>
            {
                var formulaResolution = ResolveFormula(
                    formulaVersionRows,
                    lotFormulaMap,
                    row.MerchandiseOrderDetailId,
                    row.LotNoList,
                    row.DeliveryDate);

                var materialUnitCost = formulaResolution.FormulaId.HasValue
                    ? ResolveMaterialUnitCost(
                        formulaResolution.FormulaId.Value,
                        row.DeliveryDate.Date,
                        formulaSnapshotMaterialUnitCostMap,
                        formulaHistoricalMaterialUnitCostMap)
                    : 0m;

                var productionUnitCost = productAddOnMap.TryGetValue(row.ProductId, out var addOn)
                    ? addOn
                    : 0m;

                var actualSoldAmount = ApplyVat(
                    row.DeliveredQuantity * row.UnitPrice,
                    row.VatPercent,
                    query.IncludeVat);

                return new
                {
                    row.ManagerById,
                    row.ManagerCode,
                    row.ManagerName,
                    row.CustomerId,
                    row.MerchandiseOrderId,
                    row.DeliveryOrderId,
                    row.DeliveryOrderCode,
                    row.DeliveryDate,
                    row.MerchandiseOrderCode,
                    row.MerchandiseOrderDetailId,
                    row.ProductCode,
                    row.ProductName,
                    row.LotNoList,
                    row.DeliveredQuantity,
                    ActualSoldAmount = actualSoldAmount,
                    MaterialCostAmount = row.DeliveredQuantity * materialUnitCost,
                    ProductionCostAmount = row.DeliveredQuantity * productionUnitCost,
                    HasFormula = formulaResolution.FormulaId.HasValue,
                    MissingStatus = formulaResolution.MissingStatus,
                    MissingStatusName = GetMissingFormulaStatusName(formulaResolution.MissingStatus)
                };
            })
            .GroupBy(x => x.ManagerById)
            .Select(g =>
            {
                orderMetricMap.TryGetValue(g.Key, out var orderMetric);
                assignmentMap.TryGetValue(g.Key, out var assignment);

                var actualSoldAmount = g.Sum(x => x.ActualSoldAmount);
                var materialCostAmount = g.Sum(x => x.MaterialCostAmount);
                var productionCostAmount = g.Sum(x => x.ProductionCostAmount);
                var totalCostAmount = materialCostAmount + productionCostAmount;
                var deliveredQuantity = g.Sum(x => x.DeliveredQuantity);
                var materialOnlyProfitAmount = actualSoldAmount - materialCostAmount;
                var realProfitAmount = actualSoldAmount - totalCostAmount;

                return new MerchandiseOrderSaleDetailReportDto
                {
                    ManagerById = g.Key,
                    ManagerCode = g.Select(x => x.ManagerCode).FirstOrDefault() ?? string.Empty,
                    ManagerName = g.Select(x => x.ManagerName).FirstOrDefault() ?? string.Empty,
                    GroupId = assignment?.GroupId,
                    GroupCode = assignment?.GroupCode ?? string.Empty,
                    GroupName = assignment?.GroupName ?? string.Empty,
                    OrderCount = g.Select(x => x.MerchandiseOrderId).Distinct().Count(),
                    CustomerCount = g.Select(x => x.CustomerId).Distinct().Count(),
                    MissingFormulaLineCount = g.Count(x => !x.HasFormula),
                    MissingFormulaDeliveryOrders = g
                        .Where(x => !x.HasFormula)
                        .GroupBy(x => new
                        {
                            x.DeliveryOrderId,
                            x.DeliveryOrderCode,
                            x.DeliveryDate,
                            x.MerchandiseOrderId,
                            x.MerchandiseOrderCode,
                            x.MerchandiseOrderDetailId,
                            x.ProductCode,
                            x.ProductName,
                            x.LotNoList
                        })
                        .Select(x => new MissingFormulaDeliveryOrderDto
                        {
                            DeliveryOrderId = x.Key.DeliveryOrderId,
                            DeliveryOrderCode = x.Key.DeliveryOrderCode,
                            DeliveryDate = x.Key.DeliveryDate,
                            MerchandiseOrderId = x.Key.MerchandiseOrderId,
                            MerchandiseOrderCode = x.Key.MerchandiseOrderCode,
                            MerchandiseOrderDetailId = x.Key.MerchandiseOrderDetailId,
                            ProductCode = x.Key.ProductCode,
                            ProductName = x.Key.ProductName,
                            LotNoList = x.Key.LotNoList,
                            DeliveredQuantity = x.Sum(row => row.DeliveredQuantity),
                            Status = x.Select(row => row.MissingStatus).FirstOrDefault(),
                            StatusName = x.Select(row => row.MissingStatusName).FirstOrDefault() ?? string.Empty
                        })
                        .OrderBy(x => x.DeliveryDate)
                        .ThenBy(x => x.DeliveryOrderCode)
                        .ToList(),
                    OrderedQuantity = orderMetric?.OrderedQuantity ?? 0m,
                    DeliveredQuantity = deliveredQuantity,
                    RemainingQuantity = orderMetric?.RemainingQuantity ?? 0m,
                    TotalOrderAmount = orderMetric?.TotalOrderAmount ?? 0m,
                    ActualSoldAmount = actualSoldAmount,
                    RemainingAmount = orderMetric?.RemainingAmount ?? 0m,
                    MaterialCostAmount = materialCostAmount,
                    ProductionCostAmount = productionCostAmount,
                    TotalCostAmount = totalCostAmount,
                    MaterialOnlyProfitAmount = materialOnlyProfitAmount,
                    MaterialOnlyProfitRate = actualSoldAmount > 0m
                        ? (materialOnlyProfitAmount / actualSoldAmount) * 100m
                        : 0m,
                    RealProfitAmount = realProfitAmount,
                    RealProfitRate = actualSoldAmount > 0m
                        ? (realProfitAmount / actualSoldAmount) * 100m
                        : 0m,
                    AverageUnitSellingPrice = deliveredQuantity > 0m
                        ? actualSoldAmount / deliveredQuantity
                        : 0m,
                    AverageUnitCost = deliveredQuantity > 0m
                        ? totalCostAmount / deliveredQuantity
                        : 0m,
                    AverageUnitProfit = deliveredQuantity > 0m
                        ? realProfitAmount / deliveredQuantity
                        : 0m
                };
            })
            .ToList();

        if (!CanViewSaleProfitValues())
        {
            MaskSaleProfitValues(saleRows);
        }

        saleRows = ApplySaleDetailSorting(saleRows, query);

        var totalCount = saleRows.Count;
        var items = saleRows
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (items, totalCount);
    }

    /// <summary>
    /// Masks sale profit values for users who are not allowed to see profit metrics.
    /// This runs before sorting so profit-based ordering does not leak hidden values.
    /// </summary>
    private static void MaskSaleProfitValues(IEnumerable<MerchandiseOrderSaleDetailReportDto> rows)
    {
        foreach (var item in rows)
        {
            item.MaterialOnlyProfitAmount = 0m;
            item.MaterialOnlyProfitRate = 0m;
            item.RealProfitAmount = 0m;
            item.RealProfitRate = 0m;
        }
    }

    /// <summary>
    /// Applies user requested sorting to sale detail report rows.
    /// Unknown sort fields fall back to ActualSoldAmount descending.
    /// </summary>
    private static List<MerchandiseOrderSaleDetailReportDto> ApplySaleDetailSorting(
        IReadOnlyList<MerchandiseOrderSaleDetailReportDto> rows,
        MerchandiseOrderReportQuery query)
    {
        var sortBy = NormalizeSortKey(query.SortBy);
        var descending = IsDescendingSort(query.SortDirection);

        IOrderedEnumerable<MerchandiseOrderSaleDetailReportDto> orderedRows = sortBy switch
        {
            "managercode" => OrderRows(rows, x => x.ManagerCode, descending),
            "managername" => OrderRows(rows, x => x.ManagerName, descending),
            "groupcode" => OrderRows(rows, x => x.GroupCode, descending),
            "groupname" => OrderRows(rows, x => x.GroupName, descending),
            "ordercount" => OrderRows(rows, x => x.OrderCount, descending),
            "customercount" => OrderRows(rows, x => x.CustomerCount, descending),
            "missingformulalinecount" => OrderRows(rows, x => x.MissingFormulaLineCount, descending),
            "orderedquantity" => OrderRows(rows, x => x.OrderedQuantity, descending),
            "deliveredquantity" => OrderRows(rows, x => x.DeliveredQuantity, descending),
            "remainingquantity" => OrderRows(rows, x => x.RemainingQuantity, descending),
            "totalorderamount" => OrderRows(rows, x => x.TotalOrderAmount, descending),
            "actualsoldamount" => OrderRows(rows, x => x.ActualSoldAmount, descending),
            "remainingamount" => OrderRows(rows, x => x.RemainingAmount, descending),
            "materialcostamount" => OrderRows(rows, x => x.MaterialCostAmount, descending),
            "productioncostamount" => OrderRows(rows, x => x.ProductionCostAmount, descending),
            "totalcostamount" => OrderRows(rows, x => x.TotalCostAmount, descending),
            "materialonlyprofitamount" => OrderRows(rows, x => x.MaterialOnlyProfitAmount, descending),
            "materialonlyprofitrate" => OrderRows(rows, x => x.MaterialOnlyProfitRate, descending),
            "realprofitamount" => OrderRows(rows, x => x.RealProfitAmount, descending),
            "realprofitrate" => OrderRows(rows, x => x.RealProfitRate, descending),
            "averageunitsellingprice" => OrderRows(rows, x => x.AverageUnitSellingPrice, descending),
            "averageunitcost" => OrderRows(rows, x => x.AverageUnitCost, descending),
            "averageunitprofit" => OrderRows(rows, x => x.AverageUnitProfit, descending),
            _ => rows
                .OrderByDescending(x => x.ActualSoldAmount)
                .ThenBy(x => x.ManagerName)
        };

        return orderedRows
            .ThenBy(x => x.ManagerName)
            .ThenBy(x => x.ManagerCode)
            .ToList();
    }

    /// <summary>
    /// Orders sale detail rows by a typed key selector.
    /// </summary>
    private static IOrderedEnumerable<MerchandiseOrderSaleDetailReportDto> OrderRows<TKey>(
        IReadOnlyList<MerchandiseOrderSaleDetailReportDto> rows,
        Func<MerchandiseOrderSaleDetailReportDto, TKey> keySelector,
        bool descending)
    {
        return descending
            ? rows.OrderByDescending(keySelector)
            : rows.OrderBy(keySelector);
    }

    /// <summary>
    /// Normalizes a query sort field into a case-insensitive key without common separators.
    /// </summary>
    private static string NormalizeSortKey(string? sortBy)
    {
        return string.IsNullOrWhiteSpace(sortBy)
            ? string.Empty
            : sortBy.Trim()
                .Replace("_", string.Empty)
                .Replace("-", string.Empty)
                .ToLowerInvariant();
    }

    /// <summary>
    /// Determines whether the requested sort direction is descending.
    /// Descending is the default because report users usually expect highest values first.
    /// </summary>
    private static bool IsDescendingSort(string? sortDirection)
    {
        if (string.IsNullOrWhiteSpace(sortDirection))
        {
            return true;
        }

        return sortDirection.Trim().Equals("desc", StringComparison.OrdinalIgnoreCase)
               || sortDirection.Trim().Equals("descending", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Determines whether the current user can see sale profit values.
    /// Only President and Developer roles can see material-only and real profit metrics.
    /// </summary>
    private bool CanViewSaleProfitValues()
    {
        return _currentUser.IsInRole("President")
               || _currentUser.IsInRole("Developer");
    }

    /// <summary>
    /// Calculates the production add-on used by MaterialsSupplierPriceProvider.
    /// Recycle products use 5,000; colour codes ending with C use 10,000; all other products use 20,000.
    /// </summary>
    private static decimal CalculateProductionAddOn(bool isRecycle, string? colourCode)
    {
        if (isRecycle)
        {
            return 5_000m;
        }

        if (!string.IsNullOrWhiteSpace(colourCode)
            && colourCode.EndsWith("C", StringComparison.OrdinalIgnoreCase))
        {
            return 10_000m;
        }

        return 20_000m;
    }

    /// <summary>
    /// Resolves the selected VA manufacturing formula for a delivered line.
    /// The selected production version is preferred; if it is missing, LotNoList is used
    /// as a fallback by matching VA code with ManufacturingFormula.ExternalId.
    /// </summary>
    private static FormulaResolution ResolveFormula(
        IReadOnlyList<FormulaVersionReportRow> formulaVersions,
        IReadOnlyDictionary<string, Guid> lotFormulaMap,
        Guid merchandiseOrderDetailId,
        string? lotNoList,
        DateTime deliveryDate)
    {
        var matchingVersions = formulaVersions
            .Where(x => x.MerchandiseOrderDetailId == merchandiseOrderDetailId)
            .ToList();

        var activeVersion = matchingVersions
            .Where(x =>
                (!x.ValidFrom.HasValue || x.ValidFrom.Value <= deliveryDate)
                && (!x.ValidTo.HasValue || x.ValidTo.Value >= deliveryDate))
            .OrderByDescending(x => x.ValidFrom ?? DateTime.MinValue)
            .FirstOrDefault();

        if (activeVersion != null)
        {
            return new FormulaResolution
            {
                FormulaId = activeVersion.ManufacturingFormulaId
            };
        }

        var latestSelectedVersion = matchingVersions
            .OrderByDescending(x => x.ValidFrom ?? DateTime.MinValue)
            .Select(x => (Guid?)x.ManufacturingFormulaId)
            .FirstOrDefault();

        if (latestSelectedVersion.HasValue)
        {
            return new FormulaResolution
            {
                FormulaId = latestSelectedVersion.Value
            };
        }

        var lotCodes = SplitLotCodes(lotNoList).ToList();

        foreach (var lotCode in lotCodes)
        {
            if (lotFormulaMap.TryGetValue(lotCode, out var formulaId))
            {
                return new FormulaResolution
                {
                    FormulaId = formulaId
                };
            }
        }

        return new FormulaResolution
        {
            FormulaId = null,
            MissingStatus = ResolveMissingFormulaStatus(deliveryDate, lotCodes)
        };
    }

    /// <summary>
    /// Splits a delivery LotNoList value into normalized lot codes.
    /// Delivery data may contain multiple lot codes separated by comma, semicolon, pipe or whitespace.
    /// </summary>
    private static IEnumerable<string> SplitLotCodes(string? lotNoList)
    {
        if (string.IsNullOrWhiteSpace(lotNoList))
        {
            return Array.Empty<string>();
        }

        return lotNoList
            .Split(new[] { ',', ';', '|', '\n', '\r', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Classifies why the report could not resolve a VA formula for a delivered line.
    /// </summary>
    private static MissingFormulaDeliveryOrderStatus ResolveMissingFormulaStatus(
        DateTime deliveryDate,
        IReadOnlyList<string> lotCodes)
    {
        if (deliveryDate.Date < DeliveryShortageStartDate)
        {
            return MissingFormulaDeliveryOrderStatus.LegacyData;
        }

        if (lotCodes.Count == 0)
        {
            return MissingFormulaDeliveryOrderStatus.MissingLotNo;
        }

        return MissingFormulaDeliveryOrderStatus.Other;
    }

    /// <summary>
    /// Returns the Vietnamese display name for a missing formula status.
    /// </summary>
    private static string GetMissingFormulaStatusName(MissingFormulaDeliveryOrderStatus status)
    {
        return status switch
        {
            MissingFormulaDeliveryOrderStatus.LegacyData => "Dữ liệu cũ",
            MissingFormulaDeliveryOrderStatus.MissingLotNo => "Sót số Lot khi giao hàng",
            _ => "Thông tin khác"
        };
    }

    /// <summary>
    /// Builds material unit cost by formula and delivery date using MaterialsSupplier and PriceHistory.
    /// This is used only when the VA formula material snapshot price is missing.
    /// For a target date, the price before the next price change is taken from PriceHistory.OldPrice;
    /// if no later change exists, the current supplier price is used.
    /// </summary>
    private async Task<Dictionary<(Guid FormulaId, DateTime PriceDate), decimal>> BuildHistoricalMaterialUnitCostMapAsync(
        IReadOnlyList<FormulaMaterialCostRow> formulaMaterials,
        IReadOnlyList<DateTime> priceDates,
        CancellationToken cancellationToken)
    {
        if (formulaMaterials.Count == 0 || priceDates.Count == 0)
        {
            return new Dictionary<(Guid FormulaId, DateTime PriceDate), decimal>();
        }

        var materialIds = formulaMaterials
            .Select(x => x.MaterialId)
            .Distinct()
            .ToList();

        var supplierRows = await _context.MaterialsSuppliers
            .AsNoTracking()
            .Where(x => materialIds.Contains(x.MaterialId) && (x.IsActive ?? true))
            .Select(x => new MaterialSupplierPriceRow
            {
                MaterialsSuppliersId = x.MaterialsSuppliersId,
                MaterialId = x.MaterialId,
                CurrentPrice = x.CurrentPrice ?? 0m,
                IsPreferred = x.IsPreferred ?? false,
                Stamp = x.UpdatedDate ?? x.CreateDate
            })
            .ToListAsync(cancellationToken);

        if (supplierRows.Count == 0)
        {
            return new Dictionary<(Guid FormulaId, DateTime PriceDate), decimal>();
        }

        var supplierIds = supplierRows
            .Select(x => x.MaterialsSuppliersId)
            .Distinct()
            .ToList();

        var priceHistoryRows = await _context.PriceHistories
            .AsNoTracking()
            .Where(x =>
                supplierIds.Contains(x.MaterialsSuppliersId)
                && x.CreateDate.HasValue
                && x.OldPrice.HasValue)
            .Select(x => new MaterialPriceHistoryRow
            {
                MaterialsSuppliersId = x.MaterialsSuppliersId,
                OldPrice = x.OldPrice!.Value,
                CreateDate = x.CreateDate!.Value
            })
            .ToListAsync(cancellationToken);

        var historyMap = priceHistoryRows
            .GroupBy(x => x.MaterialsSuppliersId)
            .ToDictionary(
                g => g.Key,
                g => g.OrderBy(x => x.CreateDate).ToList());

        var supplierMap = supplierRows
            .GroupBy(x => x.MaterialId)
            .ToDictionary(
                g => g.Key,
                g => g
                    .OrderByDescending(x => x.IsPreferred)
                    .ThenByDescending(x => x.Stamp ?? DateTime.MinValue)
                    .ToList());

        var result = new Dictionary<(Guid FormulaId, DateTime PriceDate), decimal>();

        foreach (var priceDate in priceDates)
        {
            var materialPriceMap = new Dictionary<Guid, decimal>();

            foreach (var materialId in materialIds)
            {
                if (!supplierMap.TryGetValue(materialId, out var suppliers))
                {
                    materialPriceMap[materialId] = 0m;
                    continue;
                }

                var supplierPrices = suppliers
                    .Select(supplier => new
                    {
                        Supplier = supplier,
                        Price = ResolveHistoricalSupplierPrice(supplier, historyMap, priceDate)
                    })
                    .ToList();

                materialPriceMap[materialId] = supplierPrices
                    .OrderByDescending(x => x.Supplier.IsPreferred)
                    .ThenByDescending(x => x.Supplier.Stamp ?? DateTime.MinValue)
                    .Select(x => x.Price)
                    .FirstOrDefault();
            }

            foreach (var formulaGroup in formulaMaterials.GroupBy(x => x.ManufacturingFormulaId))
            {
                result[(formulaGroup.Key, priceDate)] = formulaGroup.Sum(x =>
                {
                    materialPriceMap.TryGetValue(x.MaterialId, out var unitPrice);
                    return x.Quantity * unitPrice;
                });
            }
        }

        return result;
    }

    /// <summary>
    /// Resolves material unit cost for one VA formula.
    /// The VA snapshot cost is preferred because it reflects the price saved with the released formula.
    /// Historical supplier price is used only when the snapshot cost is zero.
    /// </summary>
    private static decimal ResolveMaterialUnitCost(
        Guid formulaId,
        DateTime priceDate,
        IReadOnlyDictionary<Guid, decimal> snapshotCostMap,
        IReadOnlyDictionary<(Guid FormulaId, DateTime PriceDate), decimal> historicalCostMap)
    {
        if (snapshotCostMap.TryGetValue(formulaId, out var snapshotCost) && snapshotCost > 0m)
        {
            return snapshotCost;
        }

        return historicalCostMap.TryGetValue((formulaId, priceDate), out var historicalCost)
            ? historicalCost
            : 0m;
    }

    /// <summary>
    /// Resolves supplier price as of a target date from PriceHistory and current price.
    /// PriceHistory stores OldPrice at the time of change, so the first change after the target date
    /// gives the price that was valid on the target date. If no later change exists, current price is used.
    /// </summary>
    private static decimal ResolveHistoricalSupplierPrice(
        MaterialSupplierPriceRow supplier,
        IReadOnlyDictionary<Guid, List<MaterialPriceHistoryRow>> historyMap,
        DateTime targetDate)
    {
        if (!historyMap.TryGetValue(supplier.MaterialsSuppliersId, out var histories))
        {
            return supplier.CurrentPrice;
        }

        var nextChange = histories
            .Where(x => x.CreateDate.Date > targetDate.Date)
            .OrderBy(x => x.CreateDate)
            .FirstOrDefault();

        return nextChange?.OldPrice ?? supplier.CurrentPrice;
    }

    private sealed class SaleDeliveryReportRow
    {
        public Guid DeliveryOrderId { get; set; }
        public string DeliveryOrderCode { get; set; } = string.Empty;
        public DateTime DeliveryDate { get; set; }
        public Guid MerchandiseOrderId { get; set; }
        public string MerchandiseOrderCode { get; set; } = string.Empty;
        public Guid MerchandiseOrderDetailId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ManagerById { get; set; }
        public string ManagerCode { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string LotNoList { get; set; } = string.Empty;
        public decimal? VatPercent { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    private sealed class FormulaVersionReportRow
    {
        public Guid MerchandiseOrderDetailId { get; set; }
        public Guid ManufacturingFormulaId { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }

    private sealed class FormulaResolution
    {
        public Guid? FormulaId { get; set; }
        public MissingFormulaDeliveryOrderStatus MissingStatus { get; set; }
    }

    private sealed class FormulaMaterialCostRow
    {
        public Guid ManufacturingFormulaId { get; set; }
        public Guid MaterialId { get; set; }
        public decimal Quantity { get; set; }
    }

    private sealed class MaterialSupplierPriceRow
    {
        public Guid MaterialsSuppliersId { get; set; }
        public Guid MaterialId { get; set; }
        public decimal CurrentPrice { get; set; }
        public bool IsPreferred { get; set; }
        public DateTime? Stamp { get; set; }
    }

    private sealed class MaterialPriceHistoryRow
    {
        public Guid MaterialsSuppliersId { get; set; }
        public decimal OldPrice { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
