using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.SaleReports;
using VietausWebAPI.Core.Application.Features.Shared.DTO.Visibility;
using VietausWebAPI.Core.Domain.Enums.Category;
using VietausWebAPI.Core.Domain.Enums.Formulas;
using VietausWebAPI.Core.Domain.Enums.Merchadises;

public partial class MerchandiseOrderReportRepository
{
    private const string EndsWithCGroupCode = "ENDS_WITH_C";
    private const string EndsWithCGroupName = "Ma mau ket thuc bang C";
    private const string NotEndsWithCGroupCode = "NOT_ENDS_WITH_C";
    private const string NotEndsWithCGroupName = "Ma mau khong ket thuc bang C";

    /// <summary>
    /// Gets delivered revenue and profit analytics by actual product category.
    /// Category grouping uses Product.Category and does not reclassify products by colour code.
    /// Colour-code suffix C is returned as a separate ranking.
    /// </summary>
    public async Task<MerchandiseOrderProductCategoryAnalyticsDto> GetProductCategoryReportAsync(
        MerchandiseOrderReportQuery query,
        ViewerScope viewerScope,
        CancellationToken cancellationToken = default)
    {
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

        var productCategoryType = CategoryTypes.Product.ToString();

        var deliveryQuery =
            from dod in _context.DeliveryOrderDetails.AsNoTracking()
            join doo in _context.DeliveryOrders.AsNoTracking()
                on dod.DeliveryOrderId equals doo.Id
            join mod in _context.MerchandiseOrderDetails.AsNoTracking()
                on dod.MerchandiseOrderDetailId equals mod.MerchandiseOrderDetailId
            join mo in merchandiseOrders
                on mod.MerchandiseOrderId equals mo.MerchandiseOrderId
            join p in _context.Products.AsNoTracking()
                on mod.ProductId equals p.ProductId
            join c in _context.Categories.AsNoTracking()
                on p.CategoryId equals c.CategoryId into categoryLeft
            from c in categoryLeft.DefaultIfEmpty()
            where dod.IsActive
                  && !dod.IsAttach
                  && doo.IsActive
                  && doo.CreatedDate.HasValue
                  && mod.IsActive
                  && dod.MerchandiseOrderDetailId.HasValue
                  && p.IsActive
                  && (c == null || c.Types == productCategoryType)
            select new ProductCategoryDeliveryReportRow
            {
                DeliveryDate = doo.CreatedDate!.Value,
                MerchandiseOrderId = mo.MerchandiseOrderId,
                MerchandiseOrderDetailId = mod.MerchandiseOrderDetailId,
                CustomerId = mo.CustomerId,
                ManagerById = mo.ManagerById,
                ManagerCode = mo.ManagerExternalIdSnapshot ?? string.Empty,
                ManagerName = mo.ManagerByNameSnapshot ?? string.Empty,
                ProductId = mod.ProductId,
                ColourCode = p.ColourCode ?? string.Empty,
                CategoryId = c != null ? c.CategoryId : null,
                CategoryCode = c != null ? c.ExternalId ?? string.Empty : string.Empty,
                CategoryName = c != null ? c.Name ?? string.Empty : string.Empty,
                IsRecycle = p.IsRecycle,
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
            return new MerchandiseOrderProductCategoryAnalyticsDto();
        }

        var detailIds = deliveryRows
            .Select(x => x.MerchandiseOrderDetailId)
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

        formulaIds = formulaIds
            .Concat(lotFormulaRows.Select(x => x.ManufacturingFormulaId))
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

        var managerIds = deliveryRows
            .Select(x => x.ManagerById)
            .Distinct()
            .ToList();

        var assignmentRows = await _context.CustomerAssignments
            .AsNoTracking()
            .Where(x => x.IsActive && managerIds.Contains(x.EmployeeId))
            .OrderByDescending(x => x.CreatedDate)
            .Select(x => new ProductCategoryAssignmentReportRow
            {
                EmployeeId = x.EmployeeId,
                GroupId = (Guid?)x.GroupId,
                GroupCode = x.Group.ExternalId,
                GroupName = x.Group.Name
            })
            .ToListAsync(cancellationToken);

        var assignmentMap = assignmentRows
            .GroupBy(x => x.EmployeeId)
            .ToDictionary(x => x.Key, x => x.First());

        var costRows = deliveryRows
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

                var productionUnitCost = CalculateProductionAddOn(row.IsRecycle, row.ColourCode);
                var actualSoldAmount = ApplyVat(
                    row.DeliveredQuantity * row.UnitPrice,
                    row.VatPercent,
                    query.IncludeVat);

                return new ProductCategoryCostReportRow
                {
                    CategoryId = row.CategoryId,
                    CategoryCode = row.CategoryCode,
                    CategoryName = ResolveCategoryName(row.CategoryName),
                    EndsWithC = IsColourCodeEndsWithC(row.ColourCode),
                    ManagerById = row.ManagerById,
                    ManagerCode = row.ManagerCode,
                    ManagerName = row.ManagerName,
                    CustomerId = row.CustomerId,
                    MerchandiseOrderId = row.MerchandiseOrderId,
                    ProductId = row.ProductId,
                    DeliveredQuantity = row.DeliveredQuantity,
                    ActualSoldAmount = actualSoldAmount,
                    MaterialCostAmount = row.DeliveredQuantity * materialUnitCost,
                    ProductionCostAmount = row.DeliveredQuantity * productionUnitCost,
                    HasFormula = formulaResolution.FormulaId.HasValue
                };
            })
            .ToList();

        var analytics = BuildProductCategoryAnalytics(costRows, assignmentMap);

        if (!CanViewSaleProfitValues())
        {
            MaskProductCategoryProfitValues(analytics);
        }

        return analytics;
    }

    /// <summary>
    /// Builds the product category analytics response from priced delivered rows.
    /// </summary>
    private static MerchandiseOrderProductCategoryAnalyticsDto BuildProductCategoryAnalytics(
        IReadOnlyList<ProductCategoryCostReportRow> rows,
        IReadOnlyDictionary<Guid, ProductCategoryAssignmentReportRow> assignmentMap)
    {
        var totalDeliveredQuantity = rows.Sum(x => x.DeliveredQuantity);
        var totalActualSoldAmount = rows.Sum(x => x.ActualSoldAmount);
        var totalMaterialCostAmount = rows.Sum(x => x.MaterialCostAmount);
        var totalProductionCostAmount = rows.Sum(x => x.ProductionCostAmount);
        var totalCostAmount = totalMaterialCostAmount + totalProductionCostAmount;
        var materialOnlyProfitAmount = totalActualSoldAmount - totalMaterialCostAmount;
        var realProfitAmount = totalActualSoldAmount - totalCostAmount;

        var categories = rows
            .GroupBy(x => new
            {
                x.CategoryId,
                x.CategoryCode,
                x.CategoryName
            })
            .Select(g => BuildProductCategoryRow(g.ToList(), totalDeliveredQuantity, totalActualSoldAmount, assignmentMap))
            .OrderByDescending(x => x.ActualSoldAmount)
            .ThenBy(x => x.CategoryName)
            .ToList();

        for (var i = 0; i < categories.Count; i++)
        {
            categories[i].RevenueRank = i + 1;
        }

        return new MerchandiseOrderProductCategoryAnalyticsDto
        {
            DeliveredQuantity = totalDeliveredQuantity,
            ActualSoldAmount = totalActualSoldAmount,
            MaterialCostAmount = totalMaterialCostAmount,
            ProductionCostAmount = totalProductionCostAmount,
            TotalCostAmount = totalCostAmount,
            MaterialOnlyProfitAmount = materialOnlyProfitAmount,
            MaterialOnlyProfitRate = totalActualSoldAmount > 0m
                ? (materialOnlyProfitAmount / totalActualSoldAmount) * 100m
                : 0m,
            RealProfitAmount = realProfitAmount,
            RealProfitRate = totalActualSoldAmount > 0m
                ? (realProfitAmount / totalActualSoldAmount) * 100m
                : 0m,
            Categories = categories,
            Sales = BuildProductCategorySaleSummaries(rows, assignmentMap),
            ColourCodeSuffixGroups = BuildColourCodeSuffixGroups(rows, totalDeliveredQuantity, totalActualSoldAmount)
        };
    }

    /// <summary>
    /// Builds one actual product category row.
    /// </summary>
    private static MerchandiseOrderProductCategoryReportDto BuildProductCategoryRow(
        IReadOnlyList<ProductCategoryCostReportRow> rows,
        decimal totalDeliveredQuantity,
        decimal totalActualSoldAmount,
        IReadOnlyDictionary<Guid, ProductCategoryAssignmentReportRow> assignmentMap)
    {
        var first = rows.First();
        var deliveredQuantity = rows.Sum(x => x.DeliveredQuantity);
        var actualSoldAmount = rows.Sum(x => x.ActualSoldAmount);
        var materialCostAmount = rows.Sum(x => x.MaterialCostAmount);
        var productionCostAmount = rows.Sum(x => x.ProductionCostAmount);
        var totalCostAmount = materialCostAmount + productionCostAmount;
        var materialOnlyProfitAmount = actualSoldAmount - materialCostAmount;
        var realProfitAmount = actualSoldAmount - totalCostAmount;

        return new MerchandiseOrderProductCategoryReportDto
        {
            CategoryId = first.CategoryId,
            CategoryCode = first.CategoryCode,
            CategoryName = first.CategoryName,
            OrderCount = rows.Select(x => x.MerchandiseOrderId).Distinct().Count(),
            CustomerCount = rows.Select(x => x.CustomerId).Distinct().Count(),
            ProductCount = rows.Select(x => x.ProductId).Distinct().Count(),
            MissingFormulaLineCount = rows.Count(x => !x.HasFormula),
            DeliveredQuantity = deliveredQuantity,
            DeliveredQuantityRate = totalDeliveredQuantity > 0m
                ? (deliveredQuantity / totalDeliveredQuantity) * 100m
                : 0m,
            ActualSoldAmount = actualSoldAmount,
            ActualSoldAmountRate = totalActualSoldAmount > 0m
                ? (actualSoldAmount / totalActualSoldAmount) * 100m
                : 0m,
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
            AverageUnitMaterialCost = deliveredQuantity > 0m
                ? materialCostAmount / deliveredQuantity
                : 0m,
            AverageUnitTotalCost = deliveredQuantity > 0m
                ? totalCostAmount / deliveredQuantity
                : 0m,
            AverageUnitRealProfit = deliveredQuantity > 0m
                ? realProfitAmount / deliveredQuantity
                : 0m,
            Sales = BuildProductCategorySaleBreakdowns(rows, assignmentMap)
        };
    }

    /// <summary>
    /// Builds sale breakdown rows inside one product category.
    /// Percent fields are calculated against the category totals.
    /// </summary>
    private static IReadOnlyList<MerchandiseOrderProductCategorySaleBreakdownDto> BuildProductCategorySaleBreakdowns(
        IReadOnlyList<ProductCategoryCostReportRow> rows,
        IReadOnlyDictionary<Guid, ProductCategoryAssignmentReportRow> assignmentMap)
    {
        var categoryDeliveredQuantity = rows.Sum(x => x.DeliveredQuantity);
        var categoryActualSoldAmount = rows.Sum(x => x.ActualSoldAmount);

        return rows
            .GroupBy(x => x.ManagerById)
            .Select(g =>
            {
                assignmentMap.TryGetValue(g.Key, out var assignment);

                var deliveredQuantity = g.Sum(x => x.DeliveredQuantity);
                var actualSoldAmount = g.Sum(x => x.ActualSoldAmount);
                var materialCostAmount = g.Sum(x => x.MaterialCostAmount);
                var productionCostAmount = g.Sum(x => x.ProductionCostAmount);
                var totalCostAmount = materialCostAmount + productionCostAmount;
                var materialOnlyProfitAmount = actualSoldAmount - materialCostAmount;
                var realProfitAmount = actualSoldAmount - totalCostAmount;

                return new MerchandiseOrderProductCategorySaleBreakdownDto
                {
                    ManagerById = g.Key,
                    ManagerCode = g.Select(x => x.ManagerCode).FirstOrDefault() ?? string.Empty,
                    ManagerName = g.Select(x => x.ManagerName).FirstOrDefault() ?? string.Empty,
                    GroupId = assignment?.GroupId,
                    GroupCode = assignment?.GroupCode ?? string.Empty,
                    GroupName = assignment?.GroupName ?? string.Empty,
                    OrderCount = g.Select(x => x.MerchandiseOrderId).Distinct().Count(),
                    CustomerCount = g.Select(x => x.CustomerId).Distinct().Count(),
                    ProductCount = g.Select(x => x.ProductId).Distinct().Count(),
                    DeliveredQuantity = deliveredQuantity,
                    DeliveredQuantityRateInCategory = categoryDeliveredQuantity > 0m
                        ? (deliveredQuantity / categoryDeliveredQuantity) * 100m
                        : 0m,
                    ActualSoldAmount = actualSoldAmount,
                    ActualSoldAmountRateInCategory = categoryActualSoldAmount > 0m
                        ? (actualSoldAmount / categoryActualSoldAmount) * 100m
                        : 0m,
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
                        : 0m
                };
            })
            .OrderByDescending(x => x.ActualSoldAmount)
            .ThenBy(x => x.ManagerName)
            .ToList();
    }

    /// <summary>
    /// Builds sale summaries and each sale person's category mix.
    /// </summary>
    private static IReadOnlyList<MerchandiseOrderProductCategorySaleSummaryDto> BuildProductCategorySaleSummaries(
        IReadOnlyList<ProductCategoryCostReportRow> rows,
        IReadOnlyDictionary<Guid, ProductCategoryAssignmentReportRow> assignmentMap)
    {
        return rows
            .GroupBy(x => x.ManagerById)
            .Select(g =>
            {
                assignmentMap.TryGetValue(g.Key, out var assignment);

                var deliveredQuantity = g.Sum(x => x.DeliveredQuantity);
                var actualSoldAmount = g.Sum(x => x.ActualSoldAmount);
                var materialCostAmount = g.Sum(x => x.MaterialCostAmount);
                var totalCostAmount = materialCostAmount + g.Sum(x => x.ProductionCostAmount);
                var materialOnlyProfitAmount = actualSoldAmount - materialCostAmount;
                var realProfitAmount = actualSoldAmount - totalCostAmount;

                var mixes = g
                    .GroupBy(x => new
                    {
                        x.CategoryId,
                        x.CategoryCode,
                        x.CategoryName
                    })
                    .Select(cg =>
                    {
                        var categoryDeliveredQuantity = cg.Sum(x => x.DeliveredQuantity);
                        var categoryActualSoldAmount = cg.Sum(x => x.ActualSoldAmount);

                        return new MerchandiseOrderProductCategorySaleMixDto
                        {
                            CategoryId = cg.Key.CategoryId,
                            CategoryCode = cg.Key.CategoryCode,
                            CategoryName = cg.Key.CategoryName,
                            DeliveredQuantity = categoryDeliveredQuantity,
                            DeliveredQuantityRateInSale = deliveredQuantity > 0m
                                ? (categoryDeliveredQuantity / deliveredQuantity) * 100m
                                : 0m,
                            ActualSoldAmount = categoryActualSoldAmount,
                            ActualSoldAmountRateInSale = actualSoldAmount > 0m
                                ? (categoryActualSoldAmount / actualSoldAmount) * 100m
                                : 0m
                        };
                    })
                    .OrderByDescending(x => x.ActualSoldAmount)
                    .ThenBy(x => x.CategoryName)
                    .ToList();

                var bestCategory = mixes.FirstOrDefault();

                return new MerchandiseOrderProductCategorySaleSummaryDto
                {
                    ManagerById = g.Key,
                    ManagerCode = g.Select(x => x.ManagerCode).FirstOrDefault() ?? string.Empty,
                    ManagerName = g.Select(x => x.ManagerName).FirstOrDefault() ?? string.Empty,
                    GroupId = assignment?.GroupId,
                    GroupCode = assignment?.GroupCode ?? string.Empty,
                    GroupName = assignment?.GroupName ?? string.Empty,
                    DeliveredQuantity = deliveredQuantity,
                    ActualSoldAmount = actualSoldAmount,
                    MaterialOnlyProfitAmount = materialOnlyProfitAmount,
                    MaterialOnlyProfitRate = actualSoldAmount > 0m
                        ? (materialOnlyProfitAmount / actualSoldAmount) * 100m
                        : 0m,
                    RealProfitAmount = realProfitAmount,
                    RealProfitRate = actualSoldAmount > 0m
                        ? (realProfitAmount / actualSoldAmount) * 100m
                        : 0m,
                    BestCategoryId = bestCategory?.CategoryId,
                    BestCategoryCode = bestCategory?.CategoryCode ?? string.Empty,
                    BestCategoryName = bestCategory?.CategoryName ?? string.Empty,
                    BestCategoryActualSoldAmountRate = bestCategory?.ActualSoldAmountRateInSale ?? 0m,
                    BestCategoryDeliveredQuantityRate = bestCategory?.DeliveredQuantityRateInSale ?? 0m,
                    CategoryMixes = mixes
                };
            })
            .OrderByDescending(x => x.ActualSoldAmount)
            .ThenBy(x => x.ManagerName)
            .ToList();
    }

    /// <summary>
    /// Builds the separate ranking between colour codes ending with C and other colour codes.
    /// </summary>
    private static IReadOnlyList<MerchandiseOrderColourCodeSuffixGroupDto> BuildColourCodeSuffixGroups(
        IReadOnlyList<ProductCategoryCostReportRow> rows,
        decimal totalDeliveredQuantity,
        decimal totalActualSoldAmount)
    {
        return rows
            .GroupBy(x => x.EndsWithC)
            .Select(g =>
            {
                var deliveredQuantity = g.Sum(x => x.DeliveredQuantity);
                var actualSoldAmount = g.Sum(x => x.ActualSoldAmount);
                var materialCostAmount = g.Sum(x => x.MaterialCostAmount);
                var totalCostAmount = materialCostAmount + g.Sum(x => x.ProductionCostAmount);
                var materialOnlyProfitAmount = actualSoldAmount - materialCostAmount;
                var realProfitAmount = actualSoldAmount - totalCostAmount;

                return new MerchandiseOrderColourCodeSuffixGroupDto
                {
                    GroupCode = g.Key ? EndsWithCGroupCode : NotEndsWithCGroupCode,
                    GroupName = g.Key ? EndsWithCGroupName : NotEndsWithCGroupName,
                    ProductCount = g.Select(x => x.ProductId).Distinct().Count(),
                    DeliveredQuantity = deliveredQuantity,
                    DeliveredQuantityRate = totalDeliveredQuantity > 0m
                        ? (deliveredQuantity / totalDeliveredQuantity) * 100m
                        : 0m,
                    ActualSoldAmount = actualSoldAmount,
                    ActualSoldAmountRate = totalActualSoldAmount > 0m
                        ? (actualSoldAmount / totalActualSoldAmount) * 100m
                        : 0m,
                    MaterialOnlyProfitAmount = materialOnlyProfitAmount,
                    MaterialOnlyProfitRate = actualSoldAmount > 0m
                        ? (materialOnlyProfitAmount / actualSoldAmount) * 100m
                        : 0m,
                    RealProfitAmount = realProfitAmount,
                    RealProfitRate = actualSoldAmount > 0m
                        ? (realProfitAmount / actualSoldAmount) * 100m
                        : 0m
                };
            })
            .OrderByDescending(x => x.ActualSoldAmount)
            .ToList();
    }

    /// <summary>
    /// Masks profit values in product category analytics for users who cannot view profit metrics.
    /// </summary>
    private static void MaskProductCategoryProfitValues(MerchandiseOrderProductCategoryAnalyticsDto analytics)
    {
        analytics.MaterialOnlyProfitAmount = 0m;
        analytics.MaterialOnlyProfitRate = 0m;
        analytics.RealProfitAmount = 0m;
        analytics.RealProfitRate = 0m;

        foreach (var category in analytics.Categories)
        {
            category.MaterialOnlyProfitAmount = 0m;
            category.MaterialOnlyProfitRate = 0m;
            category.RealProfitAmount = 0m;
            category.RealProfitRate = 0m;
            category.AverageUnitRealProfit = 0m;

            foreach (var sale in category.Sales)
            {
                sale.MaterialOnlyProfitAmount = 0m;
                sale.MaterialOnlyProfitRate = 0m;
                sale.RealProfitAmount = 0m;
                sale.RealProfitRate = 0m;
            }
        }

        foreach (var sale in analytics.Sales)
        {
            sale.MaterialOnlyProfitAmount = 0m;
            sale.MaterialOnlyProfitRate = 0m;
            sale.RealProfitAmount = 0m;
            sale.RealProfitRate = 0m;
        }

        foreach (var group in analytics.ColourCodeSuffixGroups)
        {
            group.MaterialOnlyProfitAmount = 0m;
            group.MaterialOnlyProfitRate = 0m;
            group.RealProfitAmount = 0m;
            group.RealProfitRate = 0m;
        }
    }

    /// <summary>
    /// Determines whether the product colour code ends with C.
    /// </summary>
    private static bool IsColourCodeEndsWithC(string? colourCode)
    {
        return !string.IsNullOrWhiteSpace(colourCode)
               && colourCode.Trim().EndsWith("C", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Returns a safe category name when product category data is missing.
    /// </summary>
    private static string ResolveCategoryName(string? categoryName)
    {
        return string.IsNullOrWhiteSpace(categoryName)
            ? "Khong xac dinh"
            : categoryName;
    }

    private sealed class ProductCategoryDeliveryReportRow
    {
        public DateTime DeliveryDate { get; set; }
        public Guid MerchandiseOrderId { get; set; }
        public Guid MerchandiseOrderDetailId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ManagerById { get; set; }
        public string ManagerCode { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
        public string ColourCode { get; set; } = string.Empty;
        public Guid? CategoryId { get; set; }
        public string CategoryCode { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public bool IsRecycle { get; set; }
        public string LotNoList { get; set; } = string.Empty;
        public decimal? VatPercent { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    private sealed class ProductCategoryCostReportRow
    {
        public Guid? CategoryId { get; set; }
        public string CategoryCode { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public bool EndsWithC { get; set; }
        public Guid ManagerById { get; set; }
        public string ManagerCode { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public Guid MerchandiseOrderId { get; set; }
        public Guid ProductId { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public decimal ActualSoldAmount { get; set; }
        public decimal MaterialCostAmount { get; set; }
        public decimal ProductionCostAmount { get; set; }
        public bool HasFormula { get; set; }
    }

    private sealed class ProductCategoryAssignmentReportRow
    {
        public Guid EmployeeId { get; set; }
        public Guid? GroupId { get; set; }
        public string GroupCode { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
    }
}
