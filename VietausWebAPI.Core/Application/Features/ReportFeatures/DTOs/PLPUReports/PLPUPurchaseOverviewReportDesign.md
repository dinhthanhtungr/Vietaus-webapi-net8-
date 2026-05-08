# PLPU Purchase Overview Report Design

## 1. Business design

Báo cáo này dùng cho phòng PLPU theo dõi toàn bộ vòng đời mua hàng từ lúc tạo PO, NCC giao hàng, kho ghi nhận nhập, QC kiểm tra, đến khi đơn được xem là hoàn thành hoặc cần xử lý.

Nguyên tắc thiết kế:

- Không gộp nhầm "hàng đã về kho" với "hàng QC đạt". Đây là hai mốc nghiệp vụ khác nhau.
- Header PO trả lời câu hỏi: PO này đang ở trạng thái nào, có rủi ro gì, tổng tiền bao nhiêu, NCC có giao đúng không.
- Detail line trả lời câu hỏi: vật tư/NVL nào đã đặt, đã về bao nhiêu, QC đạt bao nhiêu, còn thiếu bao nhiêu, có fail/vượt/chậm không.
- Voucher/QC trả lời câu hỏi: từng lần nhập hàng thực tế diễn ra lúc nào, lot nào, số lượng nào, kết quả QC nào.
- Supplier analytics trả lời câu hỏi: NCC nào giao đúng hạn, giao đủ, chất lượng ổn định, giá tốt hoặc biến động mạnh.

Phạm vi áp dụng:

- Mua nguyên vật liệu.
- Mua vật tư.
- Theo dõi tiến độ PO từ tạo đơn đến nhập kho/QC.
- Phát hiện PO chậm, thiếu hàng, chờ QC, QC fail, giao thiếu, giao vượt.
- Đánh giá NCC theo tiến độ, chất lượng, giá mua.

## 2. Report sections

### KPI summary

Hiển thị nhanh tình hình mua hàng trong kỳ:

- Tổng số PO.
- Tổng giá trị mua theo giá đã thỏa thuận.
- Tổng số lượng đặt mua.
- Tổng số lượng đã về kho.
- Tổng số lượng QC đạt.
- Tổng số lượng chờ QC.
- Tổng số lượng QC fail.
- Tỷ lệ hoàn thành PO.
- Tỷ lệ giao đúng hạn.
- Tỷ lệ thiếu hàng.
- Tỷ lệ giao vượt.
- Tỷ lệ QC fail.
- Top NCC có rủi ro cao.
- Top vật tư/NVL có biến động giá mạnh.

### Danh sách PO tổng hợp

Mỗi dòng là một PO header, có tổng hợp số lượng, giá trị, trạng thái mua hàng, trạng thái giao hàng, trạng thái QC, tình trạng quá hạn và mức độ cần xử lý.

### Chi tiết từng dòng vật tư/NVL

Mỗi dòng là một PurchaseOrderDetail, dùng để xem vật tư/NVL nào đang thiếu, đã nhận, QC đạt, QC fail, vượt số lượng, trễ giao hoặc có giá mua bất thường.

### Theo dõi nhập kho

Mỗi dòng là một lần nhập kho hoặc một voucher detail. Phần này giúp truy vết WarehouseRequest, WarehouseVoucher, WarehouseVoucherDetail, lot, số lượng nhập và thời điểm nhập.

### Theo dõi QC

Mỗi dòng là kết quả QC theo VoucherDetailId. Phần này tách rõ QCPass, Special, QCFail, Waiter để biết hàng nào được chấp nhận, hàng nào đang treo, hàng nào bị lỗi.

### Theo dõi thiếu hàng / chậm giao

Danh sách các PO hoặc line cần xử lý ngay:

- Quá RequestDeliveryDate nhưng chưa nhận đủ.
- Đã nhận kho nhưng QC chưa đạt đủ.
- Có QC fail.
- Có Waiter quá lâu.
- Giao thiếu hoặc giao vượt ngưỡng cho phép.

### Đánh giá supplier

Phân tích NCC theo số PO, giá trị mua, tỷ lệ giao đúng hạn, tỷ lệ giao đủ, tỷ lệ QC đạt, tỷ lệ QC fail, số ngày trễ trung bình và mức biến động giá.

### Phân tích giá mua

Theo dõi UnitPriceAgreed, BaseCostSnapshot, chênh lệch giá, giá trung bình theo vật tư/NVL, theo NCC, theo tháng, phát hiện vật tư/NVL tăng giá mạnh hoặc NCC có giá cao hơn mặt bằng.

## 3. KPI definitions

| KPI | Ý nghĩa | Công thức gợi ý |
| --- | --- | --- |
| TotalPO | Tổng số PO trong kỳ | Count distinct PurchaseOrderId |
| ActivePO | Số PO còn hiệu lực | Count PO where IsActive = true |
| CancelledPO | Số PO bị hủy/không còn hiệu lực | Count PO where IsActive = false hoặc Status = Cancelled |
| TotalPurchaseValue | Tổng giá trị mua | Sum PurchaseOrderDetail.TotalPriceAgreed hoặc RequestQuantity * UnitPriceAgreed |
| OrderedQuantity | Tổng số lượng đặt | Sum PurchaseOrderDetail.RequestQuantity |
| WarehouseReceivedQuantity | Tổng số lượng đã về kho | Sum WarehouseVoucherDetail.QtyKg theo PO line |
| AcceptedQuantity | Tổng số lượng QC đạt | Sum QtyKg where QC result in QCPass, Special |
| PendingQcQuantity | Tổng số lượng chờ QC | Sum QtyKg where chưa có QC hoặc QC result = Waiter |
| RejectedQuantity | Tổng số lượng QC fail | Sum QtyKg where QC result = QCFail |
| RemainingToReceive | Số lượng còn thiếu so với đặt mua | Max(OrderedQuantity - WarehouseReceivedQuantity, 0) |
| RemainingToAccept | Số lượng còn thiếu hợp lệ sau QC | Max(OrderedQuantity - AcceptedQuantity, 0) |
| OverReceivedQuantity | Số lượng giao vượt | Max(WarehouseReceivedQuantity - OrderedQuantity, 0) |
| CompletionPercent | Mức hoàn thành theo QC đạt | Min(AcceptedQuantity / OrderedQuantity, 1) * 100 |
| WarehouseReceiptPercent | Mức đã về kho | Min(WarehouseReceivedQuantity / OrderedQuantity, 1) * 100 |
| OnTimeDeliveryRate | Tỷ lệ giao đúng hạn | Count line/PO on time / Count line/PO có yêu cầu giao |
| ShortageRate | Tỷ lệ thiếu hàng | Sum RemainingToAccept / Sum OrderedQuantity |
| OverDeliveryRate | Tỷ lệ giao vượt | Sum OverReceivedQuantity / Sum OrderedQuantity |
| QcFailRate | Tỷ lệ QC fail | RejectedQuantity / WarehouseReceivedQuantity |
| QcAcceptanceRate | Tỷ lệ QC đạt | AcceptedQuantity / WarehouseReceivedQuantity |
| AverageDelayDays | Số ngày trễ trung bình | Avg Max(ActualReceiptDate - RequestDeliveryDate, 0) |
| MaxDelayDays | Số ngày trễ lớn nhất | Max DelayDays |
| WaitingQcAgeDays | Số ngày hàng chờ QC | Today - WarehouseVoucherDetail.CreatedDate với hàng Waiter/chưa QC |
| SupplierOTD | Tỷ lệ giao đúng hạn của NCC | On-time accepted or received lines / total supplier lines |
| SupplierFillRate | Tỷ lệ giao đủ của NCC | Sum Min(ReceivedQuantity, OrderedQuantity) / Sum OrderedQuantity |
| SupplierAcceptanceRate | Tỷ lệ hàng đạt QC của NCC | AcceptedQuantity / WarehouseReceivedQuantity |
| PriceVariance | Chênh lệch giá mua | UnitPriceAgreed - BaseCostSnapshot |
| PriceVariancePercent | Tỷ lệ chênh lệch giá | (UnitPriceAgreed - BaseCostSnapshot) / BaseCostSnapshot * 100 |
| AverageUnitPriceByMaterial | Giá mua TB theo vật tư/NVL | Sum TotalPriceAgreed / Sum OrderedQuantity |
| SupplierPriceIndex | So sánh giá NCC với bình quân | Supplier avg price / market/internal avg price |

## 4. Status matrix

### Detail line status

| Status | Điều kiện dữ liệu | Ý nghĩa |
| --- | --- | --- |
| NotDelivered | WarehouseReceivedQuantity = 0 và chưa quá hạn | Chưa có hàng về |
| PartiallyDelivered | WarehouseReceivedQuantity > 0 và WarehouseReceivedQuantity < OrderedQuantity | NCC đã giao một phần |
| DeliveredEnough | WarehouseReceivedQuantity >= OrderedQuantity và OverReceivedQuantity = 0 | Kho đã nhận đủ theo đặt mua |
| OverDelivered | WarehouseReceivedQuantity > OrderedQuantity | NCC giao vượt số lượng |
| WaitingQC | WarehouseReceivedQuantity > 0 và PendingQcQuantity > 0 | Có hàng đã về nhưng chưa có kết luận QC |
| PartiallyAccepted | AcceptedQuantity > 0 và AcceptedQuantity < OrderedQuantity | QC đạt một phần, chưa đủ hoàn thành |
| AcceptedEnough | AcceptedQuantity >= OrderedQuantity | Hàng hợp lệ đủ theo PO line |
| HasQCFail | RejectedQuantity > 0 | Có hàng lỗi hoặc không đạt QC |
| Overdue | Today > RequestDeliveryDate và AcceptedQuantity < OrderedQuantity | Quá hạn nhưng chưa đủ hàng hợp lệ |
| Closed | Status = Closed hoặc nghiệp vụ đóng PO | Không tiếp tục theo dõi nhận hàng |
| Cancelled | IsActive = false hoặc Status = Cancelled | PO line bị hủy/không hiệu lực |

### Header PO status

Header nên tổng hợp từ line status theo thứ tự ưu tiên:

| Header status | Điều kiện dữ liệu | Ý nghĩa |
| --- | --- | --- |
| Cancelled | PO IsActive = false hoặc Status = Cancelled | PO bị hủy |
| Closed | PO Status = Closed | PO đã đóng |
| OverdueWithShortage | Có ít nhất một line Overdue | PO quá hạn và còn thiếu hàng hợp lệ |
| HasQCFail | Có ít nhất một line HasQCFail | PO có rủi ro chất lượng |
| WaitingQC | Không thiếu giao nghiêm trọng nhưng có PendingQcQuantity > 0 | Đang chờ QC |
| NotDelivered | Tất cả line WarehouseReceivedQuantity = 0 | Chưa giao |
| PartiallyDelivered | Có line đã nhận nhưng chưa đủ accepted toàn PO | Giao/đạt một phần |
| OverDelivered | Có line giao vượt | Cần xử lý vượt giao |
| Completed | Tất cả active line AcceptedQuantity >= OrderedQuantity | Hoàn thành theo hàng QC đạt |

Nên tách thêm các cờ cảnh báo độc lập:

- IsOverdue.
- HasShortage.
- HasOverDelivery.
- HasPendingQC.
- HasQCFail.
- HasPriceVariance.
- NeedBuyerAction.

## 5. Recommended columns

### PO header report

| Cột | Nguồn dữ liệu | Công thức / ghi chú |
| --- | --- | --- |
| PurchaseOrderId | PurchaseOrder | Khóa nội bộ |
| POExternalId | PurchaseOrder.ExternalId | Mã PO hiển thị |
| OrderType | PurchaseOrder.OrderType | NVL/vật tư/loại mua |
| SupplierId | PurchaseOrder.SupplierId | Join Supplier nếu có tên NCC |
| SupplierName | Supplier | Nếu có entity Supplier |
| CompanyId | PurchaseOrder.CompanyId | Lọc theo công ty |
| CreatedDate | PurchaseOrder.CreateDate | Ngày tạo PO |
| CreatedBy | PurchaseOrder.CreatedBy | Người tạo |
| RequestDeliveryDate | PurchaseOrder.RequestDeliveryDate | Ngày yêu cầu giao cấp header |
| RealDeliveryDate | PurchaseOrder.RealDeliveryDate | Có thể dùng làm ngày hoàn tất thực tế nếu đang được cập nhật |
| POStatus | PurchaseOrder.Status | Trạng thái gốc |
| ReportStatus | Calculated | Theo status matrix |
| TotalLines | PurchaseOrderDetail | Count active lines |
| OrderedQuantity | PurchaseOrderDetail | Sum RequestQuantity |
| WarehouseReceivedQuantity | WarehouseVoucherDetail | Sum QtyKg đã về kho |
| AcceptedQuantity | QCInputByQC + WarehouseVoucherDetail | Sum QCPass/Special |
| PendingQcQuantity | QCInputByQC + WarehouseVoucherDetail | Sum Waiter/chưa QC |
| RejectedQuantity | QCInputByQC + WarehouseVoucherDetail | Sum QCFail |
| RemainingToReceive | Calculated | Max(Ordered - Received, 0) |
| RemainingToAccept | Calculated | Max(Ordered - Accepted, 0) |
| OverReceivedQuantity | Calculated | Max(Received - Ordered, 0) |
| CompletionPercent | Calculated | Accepted / Ordered |
| WarehouseReceiptPercent | Calculated | Received / Ordered |
| TotalPurchaseValue | PurchaseOrderDetail | Sum TotalPriceAgreed |
| IsOverdue | Calculated | Today > RequestDeliveryDate and Accepted < Ordered |
| DelayDays | Calculated | Actual complete/last receipt date - request date |
| HasQCFail | Calculated | Rejected > 0 |
| HasPendingQC | Calculated | Pending QC > 0 |
| BuyerAction | Calculated | Follow up NCC / Chase QC / Handle fail / Review over delivery |
| PLPUComment | PurchaseOrder.PLPUComment | Ghi chú phòng PLPU |
| Comment | PurchaseOrder.Comment | Ghi chú chung |

### PO detail report

| Cột | Nguồn dữ liệu | Công thức / ghi chú |
| --- | --- | --- |
| PurchaseOrderDetailId | PurchaseOrderDetail | Khóa line |
| PurchaseOrderId | PurchaseOrderDetail | Link header |
| POExternalId | PurchaseOrder | Join header |
| LineNo | PurchaseOrderDetail.LineNo | Số dòng |
| MaterialId | PurchaseOrderDetail.MaterialId | Khóa vật tư |
| MaterialCode | PurchaseOrderDetail.MaterialExternalIDSnapshot | Snapshot mã vật tư |
| MaterialName | PurchaseOrderDetail.MaterialNameSnapshot | Snapshot tên vật tư |
| Package | PurchaseOrderDetail.Package | Quy cách |
| OrderedQuantity | PurchaseOrderDetail.RequestQuantity | Số lượng đặt |
| WarehouseReceivedQuantity | WarehouseVoucherDetail | Sum QtyKg theo line/material |
| AcceptedQuantity | QCInputByQC | Sum QtyKg QCPass/Special |
| PendingQcQuantity | QCInputByQC | Sum QtyKg Waiter/chưa QC |
| RejectedQuantity | QCInputByQC | Sum QtyKg QCFail |
| RemainingToReceive | Calculated | Max(Ordered - Received, 0) |
| RemainingToAccept | Calculated | Max(Ordered - Accepted, 0) |
| OverReceivedQuantity | Calculated | Max(Received - Ordered, 0) |
| DeliveryDate | PurchaseOrderDetail.DeliveryDate | Ngày yêu cầu giao line nếu có |
| RequestDeliveryDate | PurchaseOrder | Fallback khi line DeliveryDate null |
| FirstReceiptDate | WarehouseVoucherDetail.CreatedDate | Min receipt date |
| LastReceiptDate | WarehouseVoucherDetail.CreatedDate | Max receipt date |
| DelayDays | Calculated | Last/complete receipt date - due date |
| LineReportStatus | Calculated | Theo detail matrix |
| BaseCostSnapshot | PurchaseOrderDetail.BaseCostSnapshot | Giá cơ sở |
| BaseDateSnapshot | PurchaseOrderDetail.BaseDateSnapshot | Ngày giá cơ sở |
| UnitPriceAgreed | PurchaseOrderDetail.UnitPriceAgreed | Giá mua đã chốt |
| TotalPriceAgreed | PurchaseOrderDetail.TotalPriceAgreed | Giá trị line |
| PriceVariance | Calculated | UnitPriceAgreed - BaseCostSnapshot |
| PriceVariancePercent | Calculated | PriceVariance / BaseCostSnapshot |
| Note | PurchaseOrderDetail.Note | Ghi chú line |

### Warehouse receipt report

| Cột | Nguồn dữ liệu | Công thức / ghi chú |
| --- | --- | --- |
| WarehouseRequestId | WarehouseRequest | Phiếu/yêu cầu kho |
| CodeFromRequest | WarehouseRequest.codeFromRequest | Link với PurchaseOrder.ExternalId |
| WarehouseVoucherId | WarehouseVoucher | Phiếu nhập |
| WarehouseVoucherDetailId | WarehouseVoucherDetail | Dòng nhập |
| VoucherType | WarehouseVoucher/WarehouseVoucherDetail | Loại phiếu |
| ReceiptDate | WarehouseVoucherDetail.CreatedDate | Ngày nhập thực tế |
| POExternalId | PurchaseOrder.ExternalId | Join codeFromRequest |
| LineNo | WarehouseVoucherDetail.LineNo | Dùng match line nếu tin cậy |
| ProductCode | WarehouseVoucherDetail.ProductCode | Match MaterialExternalIDSnapshot |
| ProductName | WarehouseVoucherDetail.ProductName | Đối chiếu snapshot |
| LotNumber | WarehouseVoucherDetail.LotNumber | Truy vết lô |
| QtyKg | WarehouseVoucherDetail.QtyKg | Số lượng kho nhận |
| QCResult | QCInputByQC | Kết quả QC mới nhất/hiệu lực |
| AcceptedQty | Calculated | QtyKg nếu QCPass/Special |
| PendingQcQty | Calculated | QtyKg nếu Waiter/chưa QC |
| RejectedQty | Calculated | QtyKg nếu QCFail |

### QC report

| Cột | Nguồn dữ liệu | Công thức / ghi chú |
| --- | --- | --- |
| QCInputId | QCInputByQC | Khóa QC |
| VoucherDetailId | QCInputByQC.VoucherDetailId | Link voucher detail |
| POExternalId | PurchaseOrder.ExternalId | Qua WarehouseRequest.codeFromRequest |
| SupplierId | PurchaseOrder.SupplierId | Đánh giá NCC |
| MaterialCode | WarehouseVoucherDetail.ProductCode | Vật tư/NVL |
| MaterialName | WarehouseVoucherDetail.ProductName | Tên vật tư/NVL |
| LotNumber | WarehouseVoucherDetail.LotNumber | Lô |
| QtyKg | WarehouseVoucherDetail.QtyKg | Số lượng được QC |
| QCResult | QCInputByQC | QCPass/Special/QCFail/Waiter |
| QCGroup | Calculated | Accepted/Pending/Rejected |
| ReceiptDate | WarehouseVoucherDetail.CreatedDate | Ngày nhập |
| QCAgeDays | Calculated | Today - ReceiptDate nếu pending |
| BuyerAction | Calculated | Follow QC / Work with supplier / Accept special |

### Supplier analysis report

| Cột | Nguồn dữ liệu | Công thức / ghi chú |
| --- | --- | --- |
| SupplierId | PurchaseOrder.SupplierId | Khóa NCC |
| SupplierName | Supplier | Nếu có entity Supplier |
| TotalPO | PurchaseOrder | Count distinct PO |
| TotalLines | PurchaseOrderDetail | Count lines |
| TotalPurchaseValue | PurchaseOrderDetail | Sum TotalPriceAgreed |
| OrderedQuantity | PurchaseOrderDetail | Sum RequestQuantity |
| WarehouseReceivedQuantity | WarehouseVoucherDetail | Sum QtyKg |
| AcceptedQuantity | QCInputByQC | Sum QCPass/Special |
| PendingQcQuantity | QCInputByQC | Sum Waiter/chưa QC |
| RejectedQuantity | QCInputByQC | Sum QCFail |
| SupplierOTD | Calculated | On-time lines / total lines |
| SupplierFillRate | Calculated | Sum Min(Received, Ordered) / Sum Ordered |
| SupplierAcceptanceRate | Calculated | Accepted / Received |
| SupplierQcFailRate | Calculated | Rejected / Received |
| AverageDelayDays | Calculated | Avg DelayDays |
| AverageUnitPrice | Calculated | Sum TotalPriceAgreed / Sum Ordered |
| PriceVarianceAmount | Calculated | Sum Ordered * (UnitPriceAgreed - BaseCostSnapshot) |
| RiskScore | Calculated | Weighted score: overdue, shortage, QC fail, price variance |

## 6. Aggregation formulas

Nên chuẩn hóa các khái niệm sau:

```text
OrderedQuantity = Sum(PurchaseOrderDetail.RequestQuantity)

WarehouseReceivedQuantity =
    Sum(WarehouseVoucherDetail.QtyKg)

AcceptedQuantity =
    Sum(WarehouseVoucherDetail.QtyKg where QCResult in (QCPass, Special))

PendingQcQuantity =
    Sum(WarehouseVoucherDetail.QtyKg where QCResult is null or QCResult = Waiter)

RejectedQuantity =
    Sum(WarehouseVoucherDetail.QtyKg where QCResult = QCFail)

RemainingToReceive =
    Max(OrderedQuantity - WarehouseReceivedQuantity, 0)

RemainingToAccept =
    Max(OrderedQuantity - AcceptedQuantity, 0)

OverReceivedQuantity =
    Max(WarehouseReceivedQuantity - OrderedQuantity, 0)

CompletionPercent =
    If OrderedQuantity = 0 then 0
    Else Min(AcceptedQuantity / OrderedQuantity, 1) * 100

WarehouseReceiptPercent =
    If OrderedQuantity = 0 then 0
    Else Min(WarehouseReceivedQuantity / OrderedQuantity, 1) * 100

OnTimeDeliveryFlag =
    AcceptedQuantity >= OrderedQuantity
    and DateWhenAcceptedEnough <= DueDate

DelayDays =
    If AcceptedQuantity >= OrderedQuantity
        Max(DateWhenAcceptedEnough.Date - DueDate.Date, 0)
    Else If Today > DueDate
        Today.Date - DueDate.Date
    Else 0

PriceVariance =
    UnitPriceAgreed - BaseCostSnapshot

PriceVariancePercent =
    If BaseCostSnapshot = 0 then null
    Else (UnitPriceAgreed - BaseCostSnapshot) / BaseCostSnapshot * 100

SupplierOTD =
    Count(lines where OnTimeDeliveryFlag = true)
    / Count(lines with OrderedQuantity > 0)

SupplierAcceptanceRate =
    Sum(AcceptedQuantity)
    / Sum(WarehouseReceivedQuantity)
```

Phân biệt các lớp số lượng:

- Hàng đã về kho: WarehouseReceivedQuantity, lấy từ WarehouseVoucherDetail.QtyKg.
- Hàng QC đạt: AcceptedQuantity, lấy từ voucher detail có QCResult QCPass/Special.
- Hàng chờ QC: PendingQcQuantity, đã về nhưng chưa đủ điều kiện xem là hợp lệ.
- Hàng bị lỗi: RejectedQuantity, QCFail.
- Hàng hoàn thành PO: nên dùng AcceptedQuantity >= OrderedQuantity, trừ khi nghiệp vụ cho phép hoàn thành theo "đã về kho chưa QC".

## 7. Technical implementation suggestion

### DTO/query model nên tạo

- PLPUPurchaseOverviewQuery
  - From, To.
  - CompanyId.
  - SupplierId.
  - MaterialId.
  - OrderType.
  - Status.
  - IncludeClosed.
  - OnlyNeedAction.
  - Pagination/sort.

- PLPUPurchaseOverviewSummaryDto
  - Nhóm KPI summary.

- PLPUPurchaseOrderHeaderReportDto
  - Một dòng mỗi PO.

- PLPUPurchaseOrderDetailReportDto
  - Một dòng mỗi PO detail.

- PLPUPurchaseReceiptReportDto
  - Một dòng mỗi voucher detail.

- PLPUPurchaseQcReportDto
  - Một dòng mỗi QC record hoặc QC trạng thái hiệu lực.

- PLPUSupplierPurchasePerformanceDto
  - Một dòng mỗi NCC.

### Repository query nên join

Luồng join chính:

```text
PurchaseOrder
    -> PurchaseOrderDetail
    -> WarehouseRequest by WarehouseRequest.codeFromRequest = PurchaseOrder.ExternalId
    -> WarehouseVoucher by WarehouseRequest/WarehouseVoucher relationship
    -> WarehouseVoucherDetail by WarehouseVoucherId
    -> QCInputByQC by QCInputByQC.VoucherDetailId = WarehouseVoucherDetail.Id
```

Khi match PO detail với voucher detail, nên ưu tiên:

1. POExternalId qua WarehouseRequest.codeFromRequest.
2. LineNo nếu WarehouseVoucherDetail.LineNo tin cậy và được copy từ PurchaseOrderDetail.LineNo.
3. ProductCode = PurchaseOrderDetail.MaterialExternalIDSnapshot.
4. Nếu có MaterialId mapping chuẩn thì dùng MaterialId thay vì snapshot text.

Không nên chỉ match bằng ProductCode nếu một PO có cùng material ở nhiều line khác nhau, vì dễ double count.

### Tách summary và detail

- Endpoint summary: trả KPI + chart data, aggregate theo PO/detail/supplier/material.
- Endpoint header detail: trả danh sách PO header có paging.
- Endpoint line detail: drill-down theo PurchaseOrderId hoặc filter.
- Endpoint receipt detail: drill-down theo PurchaseOrderDetailId hoặc POExternalId.
- Endpoint QC detail: drill-down theo VoucherDetailId/POExternalId.
- Endpoint export Excel: có thể xuất nhiều sheet: KPI, PO Header, PO Detail, Receipt, QC, Supplier.

### Tính trong SQL hay service

Nên tính trong SQL/repository:

- Sum QtyKg theo VoucherDetailId/PO line.
- Sum OrderedQuantity, TotalPriceAgreed.
- Group by supplier/material/PO.
- Min/Max receipt date.
- Count distinct PO/line.

Nên tính trong service:

- ReportStatus theo priority matrix.
- BuyerAction.
- RiskScore.
- Diễn giải trạng thái tiếng Việt.
- Các rule có nhiều fallback như DueDate, DateWhenAcceptedEnough.

### Tránh sai số và double count

- Aggregate QC về một record hiệu lực cho mỗi VoucherDetailId trước khi join lên PO line.
- Nếu QCInputByQC có nhiều lần cập nhật cho cùng VoucherDetailId, phải chọn bản ghi mới nhất hoặc bản ghi IsActive/approved theo rule nghiệp vụ.
- Aggregate WarehouseVoucherDetail theo PO line trước, rồi mới join vào PurchaseOrderDetail.
- Count distinct PurchaseOrderId khi tính tổng PO.
- Không sum TotalPriceAgreed sau khi join trực tiếp với voucher/QC nhiều dòng, vì sẽ nhân bản giá trị line.
- Với PO có nhiều voucher/lần nhập, tính receipt aggregate riêng:
  - ReceivedQuantityByLine.
  - AcceptedQuantityByLine.
  - PendingQcQuantityByLine.
  - RejectedQuantityByLine.
- Với Special, nên có cột riêng SpecialAcceptedQuantity nếu phòng mua muốn phân biệt đạt chuẩn và được chấp nhận đặc biệt.

## 8. Risks / edge cases

- WarehouseRequest.codeFromRequest không khớp PurchaseOrder.ExternalId do typo hoặc đổi mã.
- Một PO có cùng material ở nhiều line, match bằng ProductCode sẽ sai.
- WarehouseVoucherDetail.LineNo không cùng chuẩn với PurchaseOrderDetail.LineNo.
- QCInputByQC có nhiều record cho một VoucherDetailId, gây double count.
- Hàng QCFail có thể được nhập lại, đổi lot, hoặc trả NCC; cần rule xử lý vòng đời lỗi.
- Special được tính là accepted nhưng vẫn nên tách riêng để đánh giá chất lượng NCC.
- PO bị đóng khi chưa nhận đủ do thỏa thuận kinh doanh; cần ClosedReason nếu có.
- Giao vượt có thể được chấp nhận hoặc không; nên có tolerance theo material/supplier.
- UnitPriceAgreed, BaseCostSnapshot có thể khác đơn vị tính hoặc tiền tệ.
- RequestDeliveryDate header khác DeliveryDate line; nên ưu tiên line due date nếu có.
- RealDeliveryDate ở header có thể không phản ánh từng line; không dùng duy nhất field này để đánh giá trễ.
- VoucherType cần lọc đúng phiếu nhập mua hàng, tránh lẫn nhập điều chỉnh/trả hàng/chuyển kho.
- IsActive cần áp dụng nhất quán ở PO, detail, warehouse, QC.

## 9. Final recommendation

Báo cáo mua hàng tốt nhất cho PLPU nên dùng 3 lớp sự thật:

1. Đã đặt mua: PurchaseOrderDetail.RequestQuantity và giá thỏa thuận.
2. Đã về kho: WarehouseVoucherDetail.QtyKg.
3. Đã hợp lệ sau QC: QtyKg có QCResult QCPass hoặc Special.

Không nên dùng "QC đạt" làm duy nhất "đã nhận hàng", vì phòng mua sẽ mất khả năng nhìn thấy hàng đã về nhưng đang chờ QC hoặc bị QC fail. Cách đúng hơn là:

- ReceivedQuantity = hàng đã về kho.
- AcceptedQuantity = hàng QC đạt hoặc Special.
- PendingQcQuantity = hàng đã về nhưng chưa có kết luận.
- RejectedQuantity = hàng QC fail.
- PO Completed = tất cả active line có AcceptedQuantity >= OrderedQuantity, hoặc PO được đóng thủ công với lý do rõ ràng.

Dashboard cho trưởng phòng mua nên ưu tiên ra quyết định:

- PO nào cần xử lý ngay.
- NCC nào giao trễ/giao thiếu/QC fail nhiều.
- Vật tư/NVL nào có rủi ro thiếu hàng.
- Vật tư/NVL nào tăng giá bất thường.
- Bao nhiêu tiền mua hàng đang bị treo do chưa QC hoặc QC fail.

Nếu triển khai đúng, báo cáo này không chỉ là file Excel tổng hợp mà trở thành bảng điều hành mua hàng: nhìn được tiến độ, chất lượng NCC, rủi ro sản xuất và biến động chi phí trong cùng một màn hình.
