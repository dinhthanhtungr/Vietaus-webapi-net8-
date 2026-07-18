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
        public int TotalPO { get; set; }              // Tổng số PO.
        public int ActivePO { get; set; }             // PO chưa bị hủy.
        public int CancelledPO { get; set; }          // PO đã hủy.
        public int ClosedPO { get; set; }             // PO đã đóng.
        public int CompletedPO { get; set; }          // PO đã hoàn thành, QC đạt đủ.

        public int NeedActionPO { get; set; } // Số PO cần buyer xử lý.
                                              // Được tính bằng:
                                              // headers.Count(x => x.NeedBuyerAction)
                                              //
                                              // Một PO được xem là cần xử lý nếu:
                                              // - Có quá hạn (Overdue)
                                              // - Có thiếu hàng (Shortage)
                                              // - Có hàng QC fail (HasQCFail)
                                              // - Có hàng chờ QC (WaitingQC)
                                              // - Có giao vượt (OverDelivered)
                                              // - Hoặc trạng thái = ClosedWithShortage
                                              //
                                              // Nhưng sẽ KHÔNG tính nếu PO:
                                              // - Đã hủy (Cancelled)
                                              // - Đã đóng hoàn toàn (Closed)
                                              // - Đã hoàn thành (Completed)

        public int OverduePO { get; set; }            // Số PO quá hạn.
        public int QCFailPO { get; set; }             // Số PO có hàng QC fail.
        public int PendingQCPO { get; set; }          // Số PO có hàng đang chờ QC.
        public int OverDeliveryPO { get; set; }       // Số PO có giao vượt.

        public decimal TotalPurchaseValue { get; set; } // Tổng giá trị mua hàng theo số lượng đặt.
        public decimal ReceivedPurchaseValue { get; set; } // Tổng giá trị theo số lượng đã nhập kho.

        public decimal OrderedQuantity { get; set; }            // Tổng số lượng đặt.
        public decimal WarehouseReceivedQuantity { get; set; }  // Tổng số lượng kho đã nhập.
        public decimal AcceptedQuantity { get; set; }           // Tổng số lượng QC đạt.
        public decimal PendingQcQuantity { get; set; }          // Tổng số lượng chờ QC.
        public decimal RejectedQuantity { get; set; }           // Tổng số lượng QC fail.

        public decimal RemainingToReceive { get; set; } // Còn thiếu để nhập kho đủ, có tolerance 2%.
        public decimal RemainingToAccept { get; set; }  // Còn thiếu để QC đạt đủ, có tolerance 2%.
        public decimal OverReceivedQuantity { get; set; } // Số lượng giao vượt quá mức cho phép.

        public decimal CompletionPercent { get; set; }       // AcceptedQuantity / OrderedQuantity * 100.
        public decimal WarehouseReceiptPercent { get; set; } // WarehouseReceivedQuantity / OrderedQuantity * 100.

        public decimal OnTimeDeliveryRate { get; set; }    // Tỷ lệ dòng nhập kho đủ đúng hạn.
        public decimal OnTimeAcceptanceRate { get; set; }  // Tỷ lệ dòng QC đạt đủ đúng hạn.

        public decimal ShortageRate { get; set; }       // Tỷ lệ thiếu hàng.
        public decimal OverDeliveryRate { get; set; }   // Tỷ lệ giao vượt.
        public decimal QcFailRate { get; set; }         // RejectedQuantity / WarehouseReceivedQuantity * 100.
        public decimal QcAcceptanceRate { get; set; }   // AcceptedQuantity / WarehouseReceivedQuantity * 100.
        public decimal PendingQcRate { get; set; }      // PendingQcQuantity / WarehouseReceivedQuantity * 100.

        public decimal AverageDeliveryDelayDays { get; set; }    // Số ngày trễ nhập kho trung bình.
        public decimal AverageAcceptanceDelayDays { get; set; }  // Số ngày trễ QC đạt trung bình.
        public decimal AverageDelayDays { get; set; }            // Số ngày trễ chính, hiện lấy theo QC đạt.

        public int MaxDeliveryDelayDays { get; set; }    // Số ngày trễ nhập kho lớn nhất.
        public int MaxAcceptanceDelayDays { get; set; }  // Số ngày trễ QC đạt lớn nhất.
        public int MaxDelayDays { get; set; }            // Số ngày trễ lớn nhất.
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
        public decimal ReceivedPurchaseValue { get; set; }
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
        public Guid PurchaseOrderId { get; set; }       // Id PO.
        public string POExternalId { get; set; } = string.Empty; // Mã PO.
        public string OrderType { get; set; } = string.Empty;    // Loại đơn mua.

        public Guid? SupplierId { get; set; }           // Id nhà cung cấp.
        public string SupplierName { get; set; } = string.Empty; // Tên nhà cung cấp.

        public Guid? CompanyId { get; set; }            // Id công ty.
        public DateTime? CreatedDate { get; set; }      // Ngày tạo PO.
        public Guid? CreatedBy { get; set; }            // Người tạo PO.

        public DateTime? RequestDeliveryDate { get; set; } // Ngày yêu cầu giao ở header PO.
        public DateTime? RealDeliveryDate { get; set; }    // Ngày giao thực tế nếu PO có lưu.
        public DateTime? LastReceiptDate { get; set; }     // Ngày nhập kho cuối cùng theo ledger.

        public string POStatus { get; set; } = string.Empty;     // Trạng thái gốc của PO.
        public string ReportStatus { get; set; } = string.Empty; // Trạng thái báo cáo do service tự tính.
        public string AlertFlags { get; set; } = string.Empty;   // Cảnh báo: Overdue, Shortage, WaitingQC...

        public int TotalLines { get; set; }              // Số dòng vật tư trong PO.

        public decimal OrderedQuantity { get; set; }             // Tổng số lượng đặt.
        public decimal WarehouseReceivedQuantity { get; set; }   // Tổng số lượng đã nhập kho.
        public decimal AcceptedQuantity { get; set; }            // Tổng số lượng QC đạt.
        public decimal PendingQcQuantity { get; set; }           // Tổng số lượng chờ QC.
        public decimal RejectedQuantity { get; set; }            // Tổng số lượng QC fail.

        public decimal RemainingToReceive { get; set; }  // Còn thiếu để nhập kho đủ.
        public decimal RemainingToAccept { get; set; }   // Còn thiếu để QC đạt đủ: OrderedQuantity - AcceptedQuantity.
        public decimal OverReceivedQuantity { get; set; } // Tổng số lượng giao vượt.

        public decimal CompletionPercent { get; set; }       // AcceptedQuantity / OrderedQuantity * 100.
        public decimal WarehouseReceiptPercent { get; set; } // Tỷ lệ nhập kho.

        public decimal TotalPurchaseValue { get; set; }  // Tổng giá trị PO theo số lượng đặt.
        public decimal ReceivedPurchaseValue { get; set; } // Tổng giá trị PO theo số lượng đã nhập kho.

        public bool IsOverdue { get; set; }              // PO trễ nhập kho theo ngày yêu cầu giao.
        public int DelayDays { get; set; }               // Số ngày trễ nhập kho lớn nhất của PO.
        public int DeliveryDelayDays { get; set; }       // Số ngày trễ nhập kho.
        public int AcceptanceDelayDays { get; set; }     // Số ngày trễ QC đạt.

        public bool HasQCFail { get; set; }              // Có hàng QC fail.
        public bool HasPendingQC { get; set; }           // Có hàng chờ QC.
        public bool HasShortage { get; set; }            // Có thiếu hàng.
        public bool HasOpenBalance { get; set; }         // Chưa QC đạt đủ.
        public bool HasOverDelivery { get; set; }        // Có giao vượt.

        public bool NeedBuyerAction { get; set; }        // Buyer cần xử lý hay không.
        public string BuyerAction { get; set; } = string.Empty; // Gợi ý việc buyer cần làm.

        public string PLPUComment { get; set; } = string.Empty; // Ghi chú PLPU.
        public string Comment { get; set; } = string.Empty;     // Ghi chú PO.
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
        public DateTime? LastReceiptDate { get; set; }

        public string POStatus { get; set; } = string.Empty;
        public string ReportStatus { get; set; } = string.Empty;
        public string AlertFlags { get; set; } = string.Empty;

        public int TotalLines { get; set; }

        public decimal OrderedQuantity { get; set; }
        public decimal WarehouseReceivedQuantity { get; set; }
        public decimal AcceptedQuantity { get; set; }
        public decimal PendingQcQuantity { get; set; }
        public decimal RejectedQuantity { get; set; }

        public decimal RemainingToAccept { get; set; } // Còn thiếu để QC đạt đủ.
        public decimal CompletionPercent { get; set; } // AcceptedQuantity / OrderedQuantity * 100.
        public decimal TotalPurchaseValue { get; set; }
        public decimal ReceivedPurchaseValue { get; set; }

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
        public Guid PurchaseOrderDetailId { get; set; } // Id dòng PO.
        public Guid PurchaseOrderId { get; set; }       // Id PO cha.

        public string POExternalId { get; set; } = string.Empty; // Mã PO.
        public int LineNo { get; set; }                 // Số thứ tự dòng.

        public Guid MaterialId { get; set; }            // Id vật tư.
        public string MaterialCode { get; set; } = string.Empty; // Mã vật tư.
        public string MaterialName { get; set; } = string.Empty; // Tên vật tư.
        public string Package { get; set; } = string.Empty;      // Quy cách/bao bì.

        public decimal OrderedQuantity { get; set; }            // Số lượng đặt.
        public decimal WarehouseReceivedQuantity { get; set; }  // Số lượng nhập kho.
        public decimal AcceptedQuantity { get; set; }           // Số lượng QC đạt.
        public decimal PendingQcQuantity { get; set; }          // Số lượng chờ QC.
        public decimal RejectedQuantity { get; set; }           // Số lượng QC fail.

        public decimal RemainingToReceive { get; set; } // Còn thiếu để nhập kho đủ.
        public decimal RemainingToAccept { get; set; }  // Còn thiếu để QC đạt đủ.
        public decimal OverReceivedQuantity { get; set; } // Số lượng giao vượt.

        public DateTime? DeliveryDate { get; set; }        // Ngày giao yêu cầu ở dòng PO.
        public DateTime? RequestDeliveryDate { get; set; } // Ngày dùng để tính hạn giao.

        public DateTime? FirstReceiptDate { get; set; }    // Ngày nhập kho đầu tiên.
        public DateTime? LastReceiptDate { get; set; }     // Ngày nhập kho cuối cùng.

        public DateTime? DateWhenReceivedEnough { get; set; } // Ngày nhập kho cộng dồn đủ.
        public DateTime? DateWhenAcceptedEnough { get; set; } // Ngày QC đạt cộng dồn đủ.

        public int DelayDays { get; set; }              // Số ngày trễ chính, hiện lấy theo QC đạt.
        public int DeliveryDelayDays { get; set; }      // Số ngày trễ nhập kho.
        public int AcceptanceDelayDays { get; set; }    // Số ngày trễ QC đạt.

        public string LineReportStatus { get; set; } = string.Empty; // Trạng thái báo cáo của dòng.
        public string AlertFlags { get; set; } = string.Empty;       // Các cảnh báo của dòng.

        public bool NeedBuyerAction { get; set; }        // Dòng này cần buyer xử lý không.
        public string BuyerAction { get; set; } = string.Empty; // Gợi ý hành động buyer.

        public decimal? BaseCostSnapshot { get; set; }   // Giá gốc tại thời điểm tạo PO.
        public DateTime? BaseDateSnapshot { get; set; }  // Ngày giá gốc.

        public decimal? UnitPriceAgreed { get; set; }    // Đơn giá đã chốt.
        public decimal? TotalPriceAgreed { get; set; }   // Thành tiền đã chốt.
        public decimal ReceivedPurchaseValue { get; set; } // Thành tiền theo số lượng đã nhập kho.

        public decimal? PriceVariance { get; set; }        // Chênh lệch đơn giá.
        public decimal? PriceVariancePercent { get; set; } // % chênh lệch giá.
        public decimal? PriceImpactAmount { get; set; }    // Ảnh hưởng tiền do lệch giá.

        public string Note { get; set; } = string.Empty; // Ghi chú dòng PO.
    }

    public class PLPUPurchaseReceiptReportDto
    {
        public int? WarehouseRequestId { get; set; }     // Id yêu cầu kho.
        public string CodeFromRequest { get; set; } = string.Empty; // Mã yêu cầu kho.

        public long WarehouseVoucherId { get; set; }     // Id phiếu kho.
        public string WarehouseVoucherCode { get; set; } = string.Empty; // Mã phiếu kho.

        public long WarehouseVoucherDetailId { get; set; } // Id dòng phiếu kho.
        public int VoucherType { get; set; }             // Loại phiếu kho.

        public DateTime ReceiptDate { get; set; }        // Ngày nhập kho.

        public Guid PurchaseOrderId { get; set; }        // Id PO được match.
        public Guid? PurchaseOrderDetailId { get; set; } // Id dòng PO được allocate.

        public string POExternalId { get; set; } = string.Empty; // Mã PO.
        public int LineNo { get; set; }                  // Dòng PO được allocate.

        public Guid? SupplierId { get; set; }            // Id nhà cung cấp.
        public string SupplierName { get; set; } = string.Empty; // Tên nhà cung cấp.

        public string ProductCode { get; set; } = string.Empty; // Mã vật tư trên phiếu kho.
        public string ProductName { get; set; } = string.Empty; // Tên vật tư.
        public string LotNumber { get; set; } = string.Empty;   // Số lot.

        public decimal QtyKg { get; set; }               // Số lượng nhập kho.

        public QcDecision? QCResult { get; set; }        // Kết quả QC gốc.
        public DateTime? QCCreatedDate { get; set; }     // Ngày tạo kết quả QC.

        public string QCGroup { get; set; } = string.Empty; // Nhóm QC: Accepted/Rejected/Pending.

        public VoucherDetailType VoucherDetailType { get; set; } // Loại dòng phiếu kho.

        public decimal AcceptedQty { get; set; }         // Số lượng QC đạt.
        public decimal PendingQcQty { get; set; }        // Số lượng chờ QC.
        public decimal RejectedQty { get; set; }         // Số lượng QC fail.
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
        public Guid? SupplierId { get; set; }            // Id nhà cung cấp.
        public string SupplierName { get; set; } = string.Empty; // Tên nhà cung cấp.

        public int TotalPO { get; set; }                 // Tổng số PO của NCC.
        public int TotalLines { get; set; }              // Tổng số dòng PO của NCC.

        public decimal TotalPurchaseValue { get; set; }  // Tổng giá trị mua từ NCC theo số lượng đặt.
        public decimal ReceivedPurchaseValue { get; set; } // Tổng giá trị từ NCC theo số lượng đã nhập kho.

        public decimal OrderedQuantity { get; set; }           // Tổng số lượng đặt.
        public decimal WarehouseReceivedQuantity { get; set; } // Tổng số lượng nhập kho.
        public decimal AcceptedQuantity { get; set; }          // Tổng số lượng QC đạt.
        public decimal PendingQcQuantity { get; set; }         // Tổng số lượng chờ QC.
        public decimal RejectedQuantity { get; set; }          // Tổng số lượng QC fail.

        public decimal SupplierOTD { get; set; }          // Field cũ, hiểu là SupplierDeliveryOTD.
        public decimal SupplierDeliveryOTD { get; set; }  // Tỷ lệ NCC giao đủ đúng hạn.
        public decimal SupplierAcceptanceOTD { get; set; } // Tỷ lệ hàng QC đạt đủ đúng hạn.

        public decimal SupplierFillRate { get; set; }       // Tỷ lệ NCC giao đủ hàng.
        public decimal SupplierAcceptanceRate { get; set; } // AcceptedQuantity / WarehouseReceivedQuantity * 100.
        public decimal SupplierQcFailRate { get; set; }     // RejectedQuantity / WarehouseReceivedQuantity * 100.

        public decimal AverageDeliveryDelayDays { get; set; }   // Trễ nhập kho trung bình.
        public decimal AverageAcceptanceDelayDays { get; set; } // Trễ QC đạt trung bình.
        public decimal AverageDelayDays { get; set; }           // Trễ trung bình chính.

        public decimal AverageUnitPrice { get; set; }     // TotalPurchaseValue / OrderedQuantity.
        public decimal PriceVarianceAmount { get; set; }  // Tổng ảnh hưởng tiền do lệch giá.

        public decimal RiskScore { get; set; }            // Điểm rủi ro: overdue, QC fail, thiếu, chờ QC, giao vượt.
        public List<PLPUSupplierRiskReasonDto> RiskReasons { get; set; } = new(); // Giải trình điểm rủi ro.
    }

    public class PLPUSupplierRiskReasonDto
    {
        public Guid PurchaseOrderId { get; set; }
        public string POExternalId { get; set; } = string.Empty;
        public Guid? PurchaseOrderDetailId { get; set; }
        public int? LineNo { get; set; }

        public string MaterialCode { get; set; } = string.Empty;
        public string MaterialName { get; set; } = string.Empty;

        public string ReasonCode { get; set; } = string.Empty;
        public string ReasonText { get; set; } = string.Empty;
        public decimal ImpactScore { get; set; }

        public decimal OrderedQuantity { get; set; }
        public decimal WarehouseReceivedQuantity { get; set; }
        public decimal AcceptedQuantity { get; set; }
        public decimal PendingQcQuantity { get; set; }
        public decimal RejectedQuantity { get; set; }
        public int DelayDays { get; set; }
        public DateTime? RequestDeliveryDate { get; set; }
        public DateTime? ActualDate { get; set; }
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
        public Guid? CategoryId { get; set; } // Category của vật tư, dùng để nhận diện bao bì.
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
