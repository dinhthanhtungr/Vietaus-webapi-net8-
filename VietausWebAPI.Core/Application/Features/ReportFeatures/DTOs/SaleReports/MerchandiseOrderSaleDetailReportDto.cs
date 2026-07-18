namespace VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports
{
    /// <summary>
    /// Represents aggregated sale performance by manager, including delivered revenue,
    /// ordered quantity, delivered quantity, remaining quantity, material cost, production cost and real profit.
    /// Cost is calculated from delivered merchandise lines that can be linked to a selected VA manufacturing formula.
    /// </summary>
    public class MerchandiseOrderSaleDetailReportDto
    {
        public Guid ManagerById { get; set; }
        public string ManagerCode { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;

        public Guid? GroupId { get; set; }
        public string GroupCode { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;

        public int OrderCount { get; set; }
        public int CustomerCount { get; set; }
        public int MissingFormulaLineCount { get; set; }
        public IReadOnlyList<MissingFormulaDeliveryOrderDto> MissingFormulaDeliveryOrders { get; set; }
            = new List<MissingFormulaDeliveryOrderDto>();

        public decimal OrderedQuantity { get; set; }
        public decimal DeliveredQuantity { get; set; }
        public decimal RemainingQuantity { get; set; }

        public decimal TotalOrderAmount { get; set; }
        public decimal ActualSoldAmount { get; set; }
        public decimal RemainingAmount { get; set; }

        public decimal MaterialCostAmount { get; set; }
        public decimal ProductionCostAmount { get; set; }
        public decimal TotalCostAmount { get; set; }
        public decimal MaterialOnlyProfitAmount { get; set; }
        public decimal MaterialOnlyProfitRate { get; set; }
        public decimal RealProfitAmount { get; set; }
        public decimal RealProfitRate { get; set; }

        public decimal AverageUnitSellingPrice { get; set; }
        public decimal AverageUnitCost { get; set; }
        public decimal AverageUnitProfit { get; set; }
    }

    /// <summary>
    /// Represents a delivery order line that could not be linked to a selected VA manufacturing formula.
    /// These lines are still included in delivered revenue, but their material and production costs may be incomplete.
    /// </summary>
    public class MissingFormulaDeliveryOrderDto
    {
        public Guid DeliveryOrderId { get; set; }
        public string DeliveryOrderCode { get; set; } = string.Empty;
        public DateTime DeliveryDate { get; set; }
        public Guid MerchandiseOrderId { get; set; }
        public string MerchandiseOrderCode { get; set; } = string.Empty;
        public Guid MerchandiseOrderDetailId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string LotNoList { get; set; } = string.Empty;
        public decimal DeliveredQuantity { get; set; }
        public MissingFormulaDeliveryOrderStatus Status { get; set; }
        public string StatusName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Defines why a delivered line could not be linked to a VA manufacturing formula.
    /// </summary>
    public enum MissingFormulaDeliveryOrderStatus
    {
        LegacyData = 1,
        MissingLotNo = 2,
        Other = 3
    }
}
