namespace VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports
{
    /// <summary>
    /// Represents the product category analytics payload for delivered sale revenue and profit.
    /// It contains overall totals, category ranking, sale mix by category, and colour-code suffix ranking.
    /// </summary>
    public class MerchandiseOrderProductCategoryAnalyticsDto
    {
        public decimal DeliveredQuantity { get; set; }
        public decimal ActualSoldAmount { get; set; }
        public decimal MaterialCostAmount { get; set; }
        public decimal ProductionCostAmount { get; set; }
        public decimal TotalCostAmount { get; set; }
        public decimal MaterialOnlyProfitAmount { get; set; }
        public decimal MaterialOnlyProfitRate { get; set; }
        public decimal RealProfitAmount { get; set; }
        public decimal RealProfitRate { get; set; }

        public IReadOnlyList<MerchandiseOrderProductCategoryReportDto> Categories { get; set; }
            = new List<MerchandiseOrderProductCategoryReportDto>();

        public IReadOnlyList<MerchandiseOrderProductCategorySaleSummaryDto> Sales { get; set; }
            = new List<MerchandiseOrderProductCategorySaleSummaryDto>();

        public IReadOnlyList<MerchandiseOrderColourCodeSuffixGroupDto> ColourCodeSuffixGroups { get; set; }
            = new List<MerchandiseOrderColourCodeSuffixGroupDto>();
    }

    /// <summary>
    /// Represents delivered sale performance grouped by the actual Product.Category.
    /// Products are not reclassified by colour code here; colour-code suffix C is reported separately.
    /// </summary>
    public class MerchandiseOrderProductCategoryReportDto
    {
        public int RevenueRank { get; set; }
        public Guid? CategoryId { get; set; }
        public string CategoryCode { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;

        public int OrderCount { get; set; }
        public int CustomerCount { get; set; }
        public int ProductCount { get; set; }
        public int MissingFormulaLineCount { get; set; }

        public decimal DeliveredQuantity { get; set; }
        public decimal DeliveredQuantityRate { get; set; }
        public decimal ActualSoldAmount { get; set; }
        public decimal ActualSoldAmountRate { get; set; }

        public decimal MaterialCostAmount { get; set; }
        public decimal ProductionCostAmount { get; set; }
        public decimal TotalCostAmount { get; set; }
        public decimal MaterialOnlyProfitAmount { get; set; }
        public decimal MaterialOnlyProfitRate { get; set; }
        public decimal RealProfitAmount { get; set; }
        public decimal RealProfitRate { get; set; }

        public decimal AverageUnitSellingPrice { get; set; }
        public decimal AverageUnitMaterialCost { get; set; }
        public decimal AverageUnitTotalCost { get; set; }
        public decimal AverageUnitRealProfit { get; set; }

        public IReadOnlyList<MerchandiseOrderProductCategorySaleBreakdownDto> Sales { get; set; }
            = new List<MerchandiseOrderProductCategorySaleBreakdownDto>();
    }

    /// <summary>
    /// Represents one sale person's contribution inside one product category.
    /// Percent fields are calculated against the category total.
    /// </summary>
    public class MerchandiseOrderProductCategorySaleBreakdownDto
    {
        public Guid ManagerById { get; set; }
        public string ManagerCode { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;

        public Guid? GroupId { get; set; }
        public string GroupCode { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;

        public int OrderCount { get; set; }
        public int CustomerCount { get; set; }
        public int ProductCount { get; set; }

        public decimal DeliveredQuantity { get; set; }
        public decimal DeliveredQuantityRateInCategory { get; set; }
        public decimal ActualSoldAmount { get; set; }
        public decimal ActualSoldAmountRateInCategory { get; set; }

        public decimal MaterialCostAmount { get; set; }
        public decimal ProductionCostAmount { get; set; }
        public decimal TotalCostAmount { get; set; }
        public decimal MaterialOnlyProfitAmount { get; set; }
        public decimal MaterialOnlyProfitRate { get; set; }
        public decimal RealProfitAmount { get; set; }
        public decimal RealProfitRate { get; set; }
    }

    /// <summary>
    /// Represents one sale person's total performance and their product category mix in the period.
    /// BestCategory fields identify which product category contributes the largest revenue for that sale.
    /// </summary>
    public class MerchandiseOrderProductCategorySaleSummaryDto
    {
        public Guid ManagerById { get; set; }
        public string ManagerCode { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;

        public Guid? GroupId { get; set; }
        public string GroupCode { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;

        public decimal DeliveredQuantity { get; set; }
        public decimal ActualSoldAmount { get; set; }
        public decimal MaterialOnlyProfitAmount { get; set; }
        public decimal MaterialOnlyProfitRate { get; set; }
        public decimal RealProfitAmount { get; set; }
        public decimal RealProfitRate { get; set; }

        public Guid? BestCategoryId { get; set; }
        public string BestCategoryCode { get; set; } = string.Empty;
        public string BestCategoryName { get; set; } = string.Empty;
        public decimal BestCategoryActualSoldAmountRate { get; set; }
        public decimal BestCategoryDeliveredQuantityRate { get; set; }

        public IReadOnlyList<MerchandiseOrderProductCategorySaleMixDto> CategoryMixes { get; set; }
            = new List<MerchandiseOrderProductCategorySaleMixDto>();
    }

    /// <summary>
    /// Represents how much one product category contributes to one sale person's delivered amount.
    /// </summary>
    public class MerchandiseOrderProductCategorySaleMixDto
    {
        public Guid? CategoryId { get; set; }
        public string CategoryCode { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal DeliveredQuantity { get; set; }
        public decimal DeliveredQuantityRateInSale { get; set; }
        public decimal ActualSoldAmount { get; set; }
        public decimal ActualSoldAmountRateInSale { get; set; }
    }

    /// <summary>
    /// Represents the separate ranking between products whose colour code ends with C and products that do not.
    /// This does not change the product category grouping.
    /// </summary>
    public class MerchandiseOrderColourCodeSuffixGroupDto
    {
        public string GroupCode { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public int ProductCount { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public decimal DeliveredQuantityRate { get; set; }
        public decimal ActualSoldAmount { get; set; }
        public decimal ActualSoldAmountRate { get; set; }
        public decimal MaterialOnlyProfitAmount { get; set; }
        public decimal MaterialOnlyProfitRate { get; set; }
        public decimal RealProfitAmount { get; set; }
        public decimal RealProfitRate { get; set; }
    }
}
