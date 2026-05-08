using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.Devandqa;
using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.PLPUReports
{
    public class PLPUPurchaseOverviewReportDto
    {
        public PLPUPurchaseOverviewSummaryDto Summary { get; set; } = new();

        public PagedResult<PLPUPurchaseOrderHeaderReportDto> PurchaseOrders { get; set; } =
            new(new List<PLPUPurchaseOrderHeaderReportDto>(), 0, 1, 15);

        public List<PLPUPurchaseOrderHeaderReportDto> NeedActionPurchaseOrders { get; set; } = new();

        public List<PLPUSupplierPurchasePerformanceDto> SupplierPerformance { get; set; } = new();
    }

    public class PLPUPurchaseOverviewSummaryDto
    {
        public int TotalPO { get; set; }
        public int ActivePO { get; set; }
        public int CancelledPO { get; set; }
        public int ClosedPO { get; set; }
        public int CompletedPO { get; set; }
        public int NeedActionPO { get; set; }
        public int OverduePO { get; set; }
        public int QCFailPO { get; set; }
        public int PendingQCPO { get; set; }
        public int OverDeliveryPO { get; set; }

        public decimal TotalPurchaseValue { get; set; }

        public decimal OrderedQuantity { get; set; }
        public decimal WarehouseReceivedQuantity { get; set; }
        public decimal AcceptedQuantity { get; set; }
        public decimal PendingQcQuantity { get; set; }
        public decimal RejectedQuantity { get; set; }

        public decimal RemainingToReceive { get; set; }
        public decimal RemainingToAccept { get; set; }
        public decimal OverReceivedQuantity { get; set; }

        public decimal CompletionPercent { get; set; }
        public decimal WarehouseReceiptPercent { get; set; }

        public decimal OnTimeDeliveryRate { get; set; }
        public decimal OnTimeAcceptanceRate { get; set; }

        public decimal ShortageRate { get; set; }
        public decimal OverDeliveryRate { get; set; }
        public decimal QcFailRate { get; set; }
        public decimal QcAcceptanceRate { get; set; }
        public decimal PendingQcRate { get; set; }

        public decimal AverageDeliveryDelayDays { get; set; }
        public decimal AverageAcceptanceDelayDays { get; set; }
        public decimal AverageDelayDays { get; set; }

        public int MaxDeliveryDelayDays { get; set; }
        public int MaxAcceptanceDelayDays { get; set; }
        public int MaxDelayDays { get; set; }
    }

    public class PLPUPurchaseOrderHeaderDashboardDto : PLPUPurchaseOverviewSummaryDto
    {
        public IReadOnlyList<PLPUPurchaseReportChartPointDto> PurchaseValueByMonth { get; set; } =
            new List<PLPUPurchaseReportChartPointDto>();

        public IReadOnlyList<PLPUPurchaseReportChartPointDto> QuantityByMonth { get; set; } =
            new List<PLPUPurchaseReportChartPointDto>();

        public IReadOnlyList<PLPUPurchaseReportChartPointDto> PurchaseValueBySupplier { get; set; } =
            new List<PLPUPurchaseReportChartPointDto>();

        public IReadOnlyList<PLPUPurchaseReportChartPointDto> QuantityBySupplier { get; set; } =
            new List<PLPUPurchaseReportChartPointDto>();

        public IReadOnlyList<PLPUPurchaseReportChartPointDto> QuantityByMaterial { get; set; } =
            new List<PLPUPurchaseReportChartPointDto>();

        public IReadOnlyList<PLPUPurchaseReportChartPointDto> QcResultBySupplier { get; set; } =
            new List<PLPUPurchaseReportChartPointDto>();

        public IReadOnlyList<PLPUPurchaseReportChartPointDto> StatusDistribution { get; set; } =
            new List<PLPUPurchaseReportChartPointDto>();

        public IReadOnlyList<PLPUPurchaseReportChartPointDto> PriceVarianceByMaterial { get; set; } =
            new List<PLPUPurchaseReportChartPointDto>();
    }

    public class PLPUPurchaseReportChartPointDto
    {
        public string Label { get; set; } = string.Empty;
        public Guid? Id { get; set; }

        public int PurchaseOrderCount { get; set; }
        public int LineCount { get; set; }

        public decimal OrderedQuantity { get; set; }
        public decimal WarehouseReceivedQuantity { get; set; }
        public decimal AcceptedQuantity { get; set; }
        public decimal PendingQcQuantity { get; set; }
        public decimal RejectedQuantity { get; set; }

        public decimal TotalPurchaseValue { get; set; }
        public decimal CompletionPercent { get; set; }
        public decimal WarehouseReceiptPercent { get; set; }

        public decimal QcFailRate { get; set; }
        public decimal QcAcceptanceRate { get; set; }

        public decimal AverageDelayDays { get; set; }
        public decimal AverageDeliveryDelayDays { get; set; }
        public decimal AverageAcceptanceDelayDays { get; set; }

        public decimal PriceVarianceAmount { get; set; }

        public DateTime? PeriodStart { get; set; }
        public DateTime? PeriodEnd { get; set; }
    }

    public class PLPUPurchaseOrderHeaderReportDto
    {
        public Guid PurchaseOrderId { get; set; }
        public string POExternalId { get; set; } = string.Empty;
        public string OrderType { get; set; } = string.Empty;

        public Guid? SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;

        public Guid? CompanyId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }

        public DateTime? RequestDeliveryDate { get; set; }
        public DateTime? RealDeliveryDate { get; set; }

        public string POStatus { get; set; } = string.Empty;

        // Primary status dùng để group chart.
        public string ReportStatus { get; set; } = string.Empty;

        // Alert flags dùng để biết PO có nhiều vấn đề cùng lúc.
        // Ví dụ: "Overdue,HasQCFail,WaitingQC"
        public string AlertFlags { get; set; } = string.Empty;

        public int TotalLines { get; set; }

        public decimal OrderedQuantity { get; set; }
        public decimal WarehouseReceivedQuantity { get; set; }
        public decimal AcceptedQuantity { get; set; }
        public decimal PendingQcQuantity { get; set; }
        public decimal RejectedQuantity { get; set; }

        public decimal RemainingToReceive { get; set; }
        public decimal RemainingToAccept { get; set; }
        public decimal OverReceivedQuantity { get; set; }

        public decimal CompletionPercent { get; set; }
        public decimal WarehouseReceiptPercent { get; set; }

        public decimal TotalPurchaseValue { get; set; }

        public bool IsOverdue { get; set; }
        public int DelayDays { get; set; }
        public int DeliveryDelayDays { get; set; }
        public int AcceptanceDelayDays { get; set; }

        public bool HasQCFail { get; set; }
        public bool HasPendingQC { get; set; }
        public bool HasShortage { get; set; }
        public bool HasOpenBalance { get; set; }
        public bool HasOverDelivery { get; set; }

        public bool NeedBuyerAction { get; set; }
        public string BuyerAction { get; set; } = string.Empty;

        public string PLPUComment { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
    }

    public class PLPUPurchaseOrderRowDto
    {
        public Guid PurchaseOrderId { get; set; }
        public string POExternalId { get; set; } = string.Empty;
        public string OrderType { get; set; } = string.Empty;

        public Guid? SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;

        public DateTime? CreatedDate { get; set; }
        public DateTime? RequestDeliveryDate { get; set; }
        public DateTime? RealDeliveryDate { get; set; }

        public string POStatus { get; set; } = string.Empty;
        public string ReportStatus { get; set; } = string.Empty;
        public string AlertFlags { get; set; } = string.Empty;

        public int TotalLines { get; set; }

        public decimal OrderedQuantity { get; set; }
        public decimal WarehouseReceivedQuantity { get; set; }
        public decimal AcceptedQuantity { get; set; }
        public decimal PendingQcQuantity { get; set; }
        public decimal RejectedQuantity { get; set; }

        public decimal RemainingToAccept { get; set; }
        public decimal CompletionPercent { get; set; }
        public decimal TotalPurchaseValue { get; set; }

        public bool IsOverdue { get; set; }
        public int DelayDays { get; set; }

        public bool NeedBuyerAction { get; set; }
        public string BuyerAction { get; set; } = string.Empty;

        public string PLPUComment { get; set; } = string.Empty;
    }

    public class PLPUPurchaseOrderDetailDashboardDto
    {
        public PLPUPurchaseOrderHeaderReportDto Header { get; set; } = new();

        public IReadOnlyList<PLPUPurchaseOrderDetailReportDto> Lines { get; set; } =
            new List<PLPUPurchaseOrderDetailReportDto>();

        public IReadOnlyList<PLPUPurchaseReceiptReportDto> Receipts { get; set; } =
            new List<PLPUPurchaseReceiptReportDto>();

        public IReadOnlyList<PLPUPurchaseQcReportDto> QcRows { get; set; } =
            new List<PLPUPurchaseQcReportDto>();
    }

    public class PLPUPurchaseOrderDetailReportDto
    {
        public Guid PurchaseOrderDetailId { get; set; }
        public Guid PurchaseOrderId { get; set; }

        public string POExternalId { get; set; } = string.Empty;

        public int LineNo { get; set; }

        public Guid MaterialId { get; set; }
        public string MaterialCode { get; set; } = string.Empty;
        public string MaterialName { get; set; } = string.Empty;
        public string Package { get; set; } = string.Empty;

        public decimal OrderedQuantity { get; set; }
        public decimal WarehouseReceivedQuantity { get; set; }
        public decimal AcceptedQuantity { get; set; }
        public decimal PendingQcQuantity { get; set; }
        public decimal RejectedQuantity { get; set; }

        public decimal RemainingToReceive { get; set; }
        public decimal RemainingToAccept { get; set; }
        public decimal OverReceivedQuantity { get; set; }

        public DateTime? DeliveryDate { get; set; }
        public DateTime? RequestDeliveryDate { get; set; }

        public DateTime? FirstReceiptDate { get; set; }
        public DateTime? LastReceiptDate { get; set; }

        public DateTime? DateWhenReceivedEnough { get; set; }
        public DateTime? DateWhenAcceptedEnough { get; set; }

        public int DelayDays { get; set; }
        public int DeliveryDelayDays { get; set; }
        public int AcceptanceDelayDays { get; set; }

        // Primary status
        public string LineReportStatus { get; set; } = string.Empty;

        // Multi flags
        public string AlertFlags { get; set; } = string.Empty;

        public bool NeedBuyerAction { get; set; }
        public string BuyerAction { get; set; } = string.Empty;

        public decimal? BaseCostSnapshot { get; set; }
        public DateTime? BaseDateSnapshot { get; set; }

        public decimal? UnitPriceAgreed { get; set; }
        public decimal? TotalPriceAgreed { get; set; }

        public decimal? PriceVariance { get; set; }
        public decimal? PriceVariancePercent { get; set; }
        public decimal? PriceImpactAmount { get; set; }

        public string Note { get; set; } = string.Empty;
    }

    public class PLPUPurchaseReceiptReportDto
    {
        public int? WarehouseRequestId { get; set; }
        public string CodeFromRequest { get; set; } = string.Empty;

        public long WarehouseVoucherId { get; set; }
        public string WarehouseVoucherCode { get; set; } = string.Empty;

        public long WarehouseVoucherDetailId { get; set; }
        public int VoucherType { get; set; }

        public DateTime ReceiptDate { get; set; }

        public Guid PurchaseOrderId { get; set; }
        public Guid? PurchaseOrderDetailId { get; set; }

        public string POExternalId { get; set; } = string.Empty;
        public int LineNo { get; set; }

        public Guid? SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;

        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string LotNumber { get; set; } = string.Empty;

        public decimal QtyKg { get; set; }

        public QcDecision? QCResult { get; set; }
        public DateTime? QCCreatedDate { get; set; }

        public string QCGroup { get; set; } = string.Empty;

        public VoucherDetailType VoucherDetailType { get; set; }

        public decimal AcceptedQty { get; set; }
        public decimal PendingQcQty { get; set; }
        public decimal RejectedQty { get; set; }
    }

    public class PLPUPurchaseQcReportDto
    {
        public Guid? QCInputByQCId { get; set; }

        public long VoucherDetailId { get; set; }

        public Guid PurchaseOrderId { get; set; }
        public Guid? PurchaseOrderDetailId { get; set; }

        public string POExternalId { get; set; } = string.Empty;
        public int LineNo { get; set; }

        public Guid? SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;

        public string MaterialCode { get; set; } = string.Empty;
        public string MaterialName { get; set; } = string.Empty;
        public string LotNumber { get; set; } = string.Empty;

        public decimal QtyKg { get; set; }

        public QcDecision? QCResult { get; set; }
        public string QCGroup { get; set; } = string.Empty;

        public DateTime ReceiptDate { get; set; }
        public DateTime? QCCreatedDate { get; set; }

        public int QCAgeDays { get; set; }

        public string BuyerAction { get; set; } = string.Empty;
    }

    public class PLPUSupplierPurchasePerformanceDto
    {
        public Guid? SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;

        public int TotalPO { get; set; }
        public int TotalLines { get; set; }

        public decimal TotalPurchaseValue { get; set; }

        public decimal OrderedQuantity { get; set; }
        public decimal WarehouseReceivedQuantity { get; set; }
        public decimal AcceptedQuantity { get; set; }
        public decimal PendingQcQuantity { get; set; }
        public decimal RejectedQuantity { get; set; }

        // Giữ field cũ, hiểu là OTD giao hàng vật lý.
        public decimal SupplierOTD { get; set; }

        public decimal SupplierDeliveryOTD { get; set; }
        public decimal SupplierAcceptanceOTD { get; set; }

        public decimal SupplierFillRate { get; set; }
        public decimal SupplierAcceptanceRate { get; set; }
        public decimal SupplierQcFailRate { get; set; }

        public decimal AverageDeliveryDelayDays { get; set; }
        public decimal AverageAcceptanceDelayDays { get; set; }
        public decimal AverageDelayDays { get; set; }

        public decimal AverageUnitPrice { get; set; }
        public decimal PriceVarianceAmount { get; set; }

        public decimal RiskScore { get; set; }
    }

    public class PLPUPurchaseReportRawData
    {
        public List<PLPUPurchaseOrderLineRaw> OrderLines { get; set; } = new();

        // Raw receipts chưa gắn chắc vào từng PurchaseOrderDetail.
        // Service sẽ allocate lại để tránh double count.
        public List<PLPUPurchaseReceiptReportDto> Receipts { get; set; } = new();

        public List<PLPUPurchaseQcReportDto> QcRows { get; set; } = new();
    }

    public class PLPUPurchaseOrderLineRaw
    {
        public Guid PurchaseOrderId { get; set; }
        public string POExternalId { get; set; } = string.Empty;
        public string OrderType { get; set; } = string.Empty;

        public Guid? SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;

        public Guid? CompanyId { get; set; }

        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }

        public DateTime? HeaderRequestDeliveryDate { get; set; }
        public DateTime? RealDeliveryDate { get; set; }

        public string POStatus { get; set; } = string.Empty;
        public string PLPUComment { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;

        public bool POIsActive { get; set; }

        public Guid PurchaseOrderDetailId { get; set; }
        public int LineNo { get; set; }

        public Guid MaterialId { get; set; }
        public string MaterialCode { get; set; } = string.Empty;
        public string MaterialName { get; set; } = string.Empty;
        public string Package { get; set; } = string.Empty;

        public decimal OrderedQuantity { get; set; }

        public decimal? BaseCostSnapshot { get; set; }
        public DateTime? BaseDateSnapshot { get; set; }

        public decimal? UnitPriceAgreed { get; set; }
        public decimal? TotalPriceAgreed { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public string Note { get; set; } = string.Empty;
    }
}