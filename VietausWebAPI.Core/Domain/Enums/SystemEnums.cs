namespace Shared.Enums
{
    // ===== STATUS của mã hàng =====
    public static class ManufacturingProductOrder
    {
        public const string Unknown = "Unknown";
        public const string New = "New";                    // Khi tạo mới đơn hàng
        public const string FormulaRequested = "FormulaRequested"; // Khi gửi yêu cầu công thức
        public const string FormulaSuccess = "FormulaSuccess";   // Khi công thức được duyệt

        public const string Scheduling = "Scheduling";      // Khi nhấn tạo ct ghi vào SchedulMfg với trạng thái mặc định Scheduling
        public const string Scheduled = "Scheduled";        // Khi kéo vào máy
        public const string Waiting = "Waiting";            // Chờ NVL
        public const string Unassign = "Unassign";          // chỉ ghi log cập nhật SchedulMfg là Scheduling
        public const string Getback = "Getback";            // chỉ ghi log cập nhật SchedulMfg là Scheduling

        // QA/QC ghi vào QCStatus
        public const string QCinprogress = "QCinprogress";  // Khi bắt đầu QC
        public const string QCPassed = "QCPassed";          // Khi QC ok
        public const string QCFail = "QCFail";              // cập nhật thêm vào MFG

        // QLSX_CT Status của BTPStatus 
        public const string BTPNew = "BTPNew";              // khi kế hoạch phân công đoạn có qua cân trộn
        public const string Weighting = "Weighting";        // Đang cân nguyên liệu
        public const string Weighted = "Weighted";          // Đã cân xong nguyên liệu
        public const string Mixing = "Mixing";              // Đang trộn
        public const string Mixed = "Mixed";                // Đã trộn xong
        // QLSX MD
        public const string Started = "Started";            // Khi nhấn bắt đầu sx bên máy đùn
        public const string Canceled = "Canceled";          // Hủy đơn sản xuất
        public const string Running = "Running";            // Đang sản xuất
        public const string Finished = "Finished";          // Khi nhấn kết thúc sx
        // Trạng thái phụ chỉ xem được trong Log không cập nhật vaog MFG
        public const string Change = "Valuable";            // Biến chỉ log(vd: máy, chuyển vị trí từ 1 - 2) ghi tự do chỉ để ghi log
        public const string Unassignfrommd = "Valuable";    // Biến chỉ log (vd: gỡ lịch từ trạng thái x và máy y về khung chờ) ghi tự do chỉ đẻ ghi log
        public const string Recored = "Recored";            // Chỉ ghi log và tempendofshift không cập nhật SchedulMfg
        public const string Reported = "Reported";          // Chỉ ghi log và endofshift không cập nhật SchedulMfg
        // Đã tạo yêu cầu nhập kho
        public const string Done = "Done";   // log, cập nhật MFG, SchedulMfg
        // Đã nhập kho
        public const string Stocked = "Stocked";            // log, cập nhật MFG, SchedulMfg
    }

    // ===== MerchadiseStatus → string =====
    public static class MerchadiseStatus
    {
        public const string New = "New";         // (was 0)
        public const string Approved = "Approved";    // (was 1)
        public const string Pending = "Pending";     // (was 2)
        public const string Processing = "Processing";  // (was 3)
        public const string Delivering = "Delivering";  // (was 4)
        public const string Paused = "Paused";      // (was 5)
        public const string Cancelled = "Cancelled";   // (was 6)
        public const string Completed = "Completed";   // (was 7)
    }
    public static class EndOfShiftStatus // trạng thái báo cáo cuối ca của đơn hàng
    {

    }

    public static class SupplyRequestStatus     // trạng thái cho phiếu mua vật tư đề xuất
    {
        public const string New = "New";            // Mới
        public const string FirstSuccess = "FirstSuccess";     // Xác nhận lần đầu
        public const string SecondSuccess = "SecondSuccess";     // Xác nhận lần hai
        public const string Buying = "Buying";     // Đang mua
        public const string Puncharted = "Puncharted";     // Đã mua
        public const string Cancelled = "Cancelled";      // Hủy
        public const string Completed = "Completed";       // Hoàn thành
    }
    // ===== DeliveryOrderStatus → string =====
    public static class DeliveryOrderStatus // trạng thái giao hàng
    {
        public const string Pending = "Pending";
        public const string InProgress = "InProgress";
        public const string Completed = "Completed";
        public const string Canceled = "Canceled";
    }

    public static class PurchaseOrderStatus // trạng thái đặt hàng
    {
        public const string Pending = "Pending";
        public const string InProgress = "InProgress";
        public const string Completed = "Completed";
        public const string Canceled = "Canceled";
    }

    // ===== WarehouseRequestStatus → string =====
    public static class WarehouseRequestStatus // trạng thái phiếu
    {
        public const string Pending = "Pending";   // (was 0)
        public const string Approved = "Approved";  // (was 1)
        public const string Cancelled = "Cancelled"; // (was 2)
        public const string Completed = "Completed"; // (was 3)
    }

    // ===== ReserveStatus → string =====
    public static class ReserveStatus
    {
        public const string Open = "Open";      // (was 1) mở dữ chỗ
        public const string Consumed = "Consumed";  // (was 2) đã hoàn thành.
        public const string Cancelled = "Cancelled"; // (was 3) hủy
    }
    public enum Area // Cho biết máy móc thiết bị thuộc khu vực nào
    {
        None = 0,

        DA = 1, // TP
        TP = 2, // DA
    }


    public enum RequestType // Các loại phiếu nhập/xuất kho
    {

        // WareHouse (ví dụ)
        ImportFinishedGoods = 1,   // Nhập kho thành phẩm
        ImportDefectiveGoods = 3,   // Nhập kho hàng lỗi
        ImportFromSupplier = 5,   // Nhập kho mua hàng (NCC)
        ImportInternalTransfer = 7,   // Nhập kho điều chuyển nội bộ
        ImportReturnedGoods = 9,   // Nhập kho hàng trả về
        ImportAdjustmentIncrease = 11,  // Nhập điều chỉnh (tăng)
        ImportPlasticCut = 13,  // Nhập kho nhựa cắt rửa
        ImportOther = 15,  // Khác

        ExportForProduction = 2,   // Xuất cho sản xuất (NVL)
        ExportForSales = 4,   // Xuất bán (thành phẩm)
        ExportInternalTransfer = 6,   // Xuất điều chuyển nội bộ
        ExportReturnToSupplier = 8,   // Xuất trả lại NCC
        ExportForRecycle = 10,  // Xuất tái chế/phế liệu
        ExportForLab = 12,  // Xuất cho Lab, R&D
        ExportForAuditLoss = 14,  // Xuất kiểm kê/hao hụt
        ExportForCleaning = 16,  // Xuất vật tư/nhựa rửa máy
        ExportOther = 18, // khác
    }

    public enum EventType : int
    {
        ManufacturingProductOrder = 1,
        DeliveryOrderStatus = 2,
        ManufacturingProductOrderFormula = 3,
        MerchadiseStatus = 4,
        SamplerStatus = 5,
        SupplyRequestStatus = 6,
    }

    public enum StockType : int     // loại phiếu nhập kho
    {
        FinishedGood = 1, // Hàng thành phẩm
        DefectiveFinishedGood = 2, // Hàng thành phẩm lỗi
        RawMaterial = 3, // Nguyên vật liệu
        DefectiveRawMaterial = 4  // Nguyên vật liệu lỗi
    }

    public enum ManufacturingProductOrderFormula : int
    {
        UnKnow = 0,
        New = 1,  // Mới
        Checking = 2,  // Đang kiểm tra
        IsSelect = 3,  // Duyệt công thức
        Processing = 4,  // Đang xử lý
        Cancelled = 5,  // Hủy
        Error = 6,  // Lỗi
        Completed = 7   // Hoàn thành
    }
    // công đoạn sx
    public enum StepOfProduct : int // công đoạn sx. các công đoạn sx để ra thành phẩm (kế hoạch sẽ chọn khi xếp lịch để biết hàng đó qua các công đoạn nào)
    {
        Forsb = 0, // sang bao => thành phẩm
        MD_GH = 1, // tạo hạt => thành phẩm
        CT_MD_GH = 2, // nghiền, trộn, tạo hạt => thành phẩm
        CT_MD_HT_GH = 3, // trộn, tạo hạt, trộn recolor => thành phẩm
        CT_GH = 4, // trộn => thành phẩm
        CT_BN_MD_GH = 5, // nghiền/băm, trộn, tạo hạt => thành phẩm
        CT_BN_MD_HT_GH = 6, // nghiền/băm, trộn, tạo hạt, trộn recolor => thành phẩm
        OTHER = 7 // các hình thức Khác
    }

    public static class GroupType // Cho phân quyền người dùng 
    {
        public const string HCHR = "HCHR";            // Hành chính nhân sự
        public const string IMS = "IMS";     // Quản lý hệ thống
        public const string Fin_Acc = "ACC";     // Kế toán, tài chính

        public const string CMR = "CMR";     // Sales Marketing
        public const string CMR_G1 = "CMR.G1";     // Sales Marketing nhóm 1
        public const string CMR_G2 = "CMR.G2";     // Sales Marketing nhóm 2
        public const string WH = "WH";     // Kho
        public const string PLPU = "PLPU";     // Kế hoạch thu mua - chung
        public const string PLPU_PL = "PLPU.PL";     // Kế hoạch thu mua - lên lệnh sx
        public const string PLPU_PR = "PLPU.PR";     // Kế hoạch thu mua - mua nguyên liệu
        public const string PLPU_LG = "PLPU.LG";     // Kế hoạch thu mua - logictic
        public const string PLPU_MT = "PLPU.MT";     // Kế hoạch thu mua - vật tư
        public const string QLSX = "QLSX";      // Quản lý sx
        public const string QLSX_KTSX = "QLSX.KTSX";      // Quản lý sx
        public const string QLSX_CT = "QLSX.CT";      // Quản lý sx
        public const string QLSX_MD = "QLSX.MD";      // Quản lý sx
        public const string QLSX_HT = "QLSX.HT";      // Quản lý sx
        public const string QLSX_TSBN = "QLSX.TSBN";      // Quản lý sx
        public const string QAQC = "QAQC";       // quản lý chất lượng
        public const string QAQC_RD = "QAQC.RD";       // quản lý chất lượng
        public const string QAQC_PM = "QAQC.PM";       // quản lý chất lượng
        public const string QAQC_LAB = "QAQC.LAB";       // quản lý chất lượng
        public const string QAQC_QC = "QAQC.QC";       // quản lý chất lượng
        public const string TECH = "TECH";       // Kĩ thuật và công nghệ
        public const string TECH_IT = "TECH.IT";       // Kĩ thuật và công nghệ
        public const string TECH_EV = "TECH.EV";       // Kĩ thuật và công nghệ
        public const string TECH_MC = "TECH.MC";       // Kĩ thuật và công nghệ

        // Nếu phân thêm nữa cho từng tổ thì quy tắc là: tên phòng ban '.' tổ/bộ phận thuộc phòng ban đó. (VD: QLSX.MD.G1...)
    }
    public enum DocumentPrefix
    {
        PRQ,    // Phiếu yêu cầu (nhập/xuất kho)
        PXK,    // Phiếu xuất kho
        PNK,    // Phiếu nhập kho
        PCK,    // Phiếu chuyển kho / chuyển kho nội bộ

        MFG,    // Lệnh sản xuất
        VA,     // Công thức sản xuất (VA - formula)
        VU,     // Công thức phối màu (Vụ / Masterbatch color formula)

        DHG,    // Đơn hàng (Sales Order)
        PGH,    // Phiếu giao hàng / Delivery note
        DDH,    // Đơn đặt hàng (Purchase Order)

        PM,     // Phiếu bảo trì (maintenance request / maintenance ticket)
        IM,     // Phiếu cải tiến (improvement)
        IR,     // Phiếu sự cố (incident report)
        WO,     // Work Order / Phiếu lệnh sửa chữa
        PR,     // PO mua vật tư
        PO,     // PO mua NVL
        INV,    // Invoice (Hóa đơn)
        RCPT,   //  Biên nhận / Phiếu thu (nếu có)
                // ===== Kế toán: AP (Mua hàng - Công nợ phải trả) =====
        APINV,      // AP Invoice: Hóa đơn mua (purchase.ap_docs doc_kind=INVOICE/BOTH)
        APDN,       // AP Delivery note / Receipt: Phiếu nhận hàng mua (doc_kind=DELIVERY)
        APRET,      // AP Return: Trả hàng NCC (doc_kind=RETURN)
        APADJ,      // AP Adjust: Điều chỉnh công nợ NCC (doc_kind=ADJUST)
        APPAY,      // AP Payment: Phiếu chi NCC (purchase.ap_payments)

        // ===== Kế toán: AR (Bán hàng - Công nợ phải thu) =====
        ARINV,      // AR Invoice: Hóa đơn bán (sales.ar_docs doc_kind=INVOICE/BOTH)
        ARDN,       // AR Delivery: Phiếu giao hàng / xuất bán (doc_kind=DELIVERY)
        ARRET,      // AR Return: Hàng bán bị trả lại (doc_kind=RETURN)
        ARADJ,      // AR Adjust: Điều chỉnh công nợ KH (doc_kind=ADJUST)
        ARRCPT,     // AR Receipt: Phiếu thu khách hàng (sales.ar_receipts)

        // ===== Kế toán: Sổ cái (GL / Journal) =====
        JE,         // Journal Entry: Bút toán tổng hợp (nếu/ khi có bảng acct.gl_journal)
        GLPOST,     // Batch post / Workbench posting batch (nếu cần gom theo đợt)

        // ===== Kế toán: Kỳ kế toán =====
        PER,        // Period (kỳ kế toán)
        CLP,        // Close Period chứng từ khóa kỳ
        OPN,        // Opening (số dư đầu kỳ)
        // Chý ý
        //... Riêng phiếu báo cáo sự cố, đề xuất cải tiến và đề xuất vật tư thì prefix là tên code của phòng ban tạo phiếu + "." + enum + "." + mã số gọi từ API)
        // VD: phiếu báo cáo sự cố thuộc phòng IMS sẽ là: IMS.IR.251200001)
    }
    public enum CustomerRequestStatus // trạng thái yêu cầu của khách hàng
    {
        New = 0,
        InReview = 1,
        Quoted = 2,
        ClosedWon = 3,
        ClosedLost = 4,
        Cancelled = 9
    }

    public enum SalesQuoteStatus // trạng thái báo giá
    {
        Draft = 0, // nháp
        Sent = 1, // đã gởi
        Accepted = 2, // đã chấp nhận
        Rejected = 3, // 
        Expired = 4, // 
        Cancelled = 9 // hủy
    }
}
