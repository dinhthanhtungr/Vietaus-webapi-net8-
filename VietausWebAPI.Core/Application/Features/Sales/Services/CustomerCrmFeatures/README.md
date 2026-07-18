# Customer CRM Feature

Feature này dùng cho nghiệp vụ CRM thủ công của sale: ghi nhận lịch sử liên hệ, tạo việc cần làm, lập kế hoạch chăm sóc khách hàng, hiển thị calendar và dashboard.

Không có tích hợp Zalo OA, webhook hoặc hệ thống bên thứ ba trong phase này.

## Entity Chính

### CustomerInteraction

Lưu lịch sử sale đã tương tác với khách hàng.

Ví dụ nghiệp vụ:

- Gọi điện cho khách hàng.
- Gặp trực tiếp.
- Gửi email.
- Đi thăm khách.
- Ghi nhận nội dung trao đổi, kết quả, hành động tiếp theo.

Field quan trọng:

- `CustomerId`: khách hàng được liên hệ.
- `ContactId`: người liên hệ cụ thể của khách hàng, nullable.
- `InteractionType`: loại liên hệ, ví dụ `Call`, `Meeting`, `Email`, `Visit`, `Other`.
- `Subject`: tiêu đề liên hệ.
- `Content`: nội dung trao đổi.
- `Outcome`: kết quả sau khi liên hệ.
- `NextAction`: hành động tiếp theo.
- `InteractionAt`: thời điểm liên hệ thực tế.
- `NextFollowUpDate`: ngày cần follow-up tiếp theo.
- `AssignedSaleEmployeeId`: sale phụ trách.
- `CompanyId`, `CreatedBy`, `CreatedDate`, `UpdatedBy`, `UpdatedDate`, `IsActive`: audit/scope theo convention hiện tại.

Khi tạo interaction có `NextFollowUpDate`, service tự tạo thêm `CustomerFollowUpTask`.

### CustomerFollowUpTask

Lưu việc cần làm của sale với khách hàng.

Ví dụ nghiệp vụ:

- Gọi lại khách vào ngày hẹn.
- Gửi báo giá.
- Nhắc khách phản hồi mẫu.
- Hẹn gặp lại.

Field quan trọng:

- `CustomerId`: khách hàng cần follow-up.
- `CustomerInteractionId`: interaction gốc tạo ra task, nullable.
- `Title`: tên việc cần làm.
- `Description`: mô tả.
- `NextAction`: hành động cần làm.
- `Status`: `Pending`, `InProgress`, `Done`, `Canceled`, `Overdue`.
- `Priority`: `Low`, `Normal`, `High`, `Urgent`.
- `DueDate`: ngày đến hạn.
- `CompletedDate`: ngày hoàn tất.
- `CompletedBy`: người hoàn tất.
- `CompletionNote`: ghi chú khi hoàn tất.
- `AssignedSaleEmployeeId`: sale phụ trách.

Task quá hạn được hiểu là:

```text
DueDate < hôm nay
và Status không phải Done/Canceled
```

### CustomerWorkPlan

Lưu kế hoạch chăm sóc dài hạn theo khách hàng.

Ví dụ nghiệp vụ:

- Kế hoạch phát triển khách hàng A trong quý này.
- Chiến lược chăm sóc khách hàng tiềm năng.
- Mục tiêu tăng sản lượng hoặc mở nhóm sản phẩm mới.

Field quan trọng:

- `CustomerId`: khách hàng được lập kế hoạch.
- `PlanName`: tên kế hoạch.
- `Objective`: mục tiêu.
- `Strategy`: chiến lược.
- `DiscussionSummary`: tóm tắt trao đổi.
- `NextAction`: hành động tiếp theo.
- `Status`: `Draft`, `Active`, `Paused`, `Completed`, `Canceled`.
- `Priority`: `Low`, `Normal`, `High`, `Urgent`.
- `StartDate`, `EndDate`: thời gian kế hoạch.
- `NextFollowUpDate`: ngày cần follow-up tiếp theo.
- `AssignedSaleEmployeeId`: sale phụ trách.

## Service

Service chính:

```text
ICustomerCrmService
CustomerCrmService
```

`CustomerCrmService` được tách bằng `partial class` để dễ bảo trì:

- `CustomerCrmService.cs`: constructor và dependency.
- `CustomerCrmService.Interactions.cs`: lịch sử tương tác.
- `CustomerCrmService.FollowUpTasks.cs`: follow-up task.
- `CustomerCrmService.WorkPlans.cs`: kế hoạch chăm sóc khách hàng.
- `CustomerCrmService.Calendar.cs`: gom dữ liệu calendar.
- `CustomerCrmService.Dashboard.cs`: dashboard sale/leader/director.
- `CustomerCrmService.QueryHelpers.cs`: projection, filter, paging helper.

## Luồng Nghiệp Vụ

### 1. Sale ghi nhận liên hệ

Endpoint:

```http
POST /api/customer-crm/interactions
```

Service:

```csharp
CreateInteractionAsync(CreateCustomerInteractionRequest request, CancellationToken ct)
```

Luồng xử lý:

1. Kiểm tra khách hàng tồn tại trong `CompanyId` hiện tại.
2. Tạo `CustomerInteraction`.
3. Cập nhật `Customer.LastContactDate`.
4. Cập nhật `Customer.CurrentSaleId`.
5. Nếu có `NextFollowUpDate`, tự tạo `CustomerFollowUpTask`.
6. Nếu có task mới, cập nhật `Customer.NextFollowUpDate`.

### 2. Sale xem lịch sử liên hệ của khách

Endpoint:

```http
GET /api/customer-crm/customers/{customerId}/interactions
```

Service:

```csharp
GetCustomerInteractionsAsync(Guid customerId, CustomerInteractionQuery query, CancellationToken ct)
```

Hỗ trợ lọc theo:

- loại tương tác
- sale phụ trách
- từ ngày / đến ngày
- keyword
- include inactive
- phân trang

### 3. Sale tạo task thủ công

Endpoint:

```http
POST /api/customer-crm/follow-up-tasks
```

Service:

```csharp
CreateFollowUpTaskAsync(CreateCustomerFollowUpTaskRequest request, CancellationToken ct)
```

Luồng xử lý:

1. Kiểm tra khách hàng tồn tại.
2. Tạo `CustomerFollowUpTask`.
3. Nếu không truyền sale phụ trách, mặc định là sale hiện tại.
4. Đồng bộ `Customer.NextFollowUpDate` theo task còn mở gần nhất.

### 4. Sale hoàn tất task

Endpoint:

```http
POST /api/customer-crm/follow-up-tasks/{id}/complete
```

Service:

```csharp
CompleteFollowUpTaskAsync(Guid id, CompleteCustomerFollowUpTaskRequest request, CancellationToken ct)
```

Luồng xử lý:

1. Tìm task còn active trong công ty hiện tại.
2. Set `Status = Done`.
3. Set `CompletedDate = now`.
4. Set `CompletedBy = current employee`.
5. Lưu `CompletionNote`.
6. Đồng bộ lại `Customer.NextFollowUpDate`.

### 5. Sale lập kế hoạch chăm sóc khách hàng

Endpoint:

```http
POST /api/customer-crm/work-plans
```

Service:

```csharp
CreateWorkPlanAsync(CreateCustomerWorkPlanRequest request, CancellationToken ct)
```

Luồng xử lý:

1. Kiểm tra khách hàng tồn tại.
2. Tạo `CustomerWorkPlan`.
3. Nếu không truyền sale phụ trách, mặc định là sale hiện tại.

## Calendar

Endpoint:

```http
GET /api/customer-crm/calendar/events
```

Service:

```csharp
GetCalendarEventsAsync(CustomerCrmCalendarQuery query, CancellationToken ct)
```

Calendar không dùng bảng riêng. Dữ liệu được gom từ 3 nguồn:

- `CustomerFollowUpTask.DueDate`
- `CustomerInteraction.InteractionAt`
- `CustomerWorkPlan.NextFollowUpDate`

Query ví dụ:

```http
GET /api/customer-crm/calendar/events?from=2026-05-01&to=2026-05-31
GET /api/customer-crm/calendar/events?from=2026-05-01&to=2026-05-31&onlyMine=true
GET /api/customer-crm/calendar/events?customerId={customerId}&from=2026-05-01&to=2026-05-31
GET /api/customer-crm/calendar/events?assignedSaleEmployeeId={employeeId}&from=2026-05-01&to=2026-05-31
```

Query params:

- `view`: chế độ lịch, nhận `Day`, `Week`, `Month`, `Year`, `Schedule`, `4Days`. Dùng để tính khoảng ngày mặc định nếu không truyền `from/to`.
- `from`: ngày bắt đầu.
- `to`: ngày kết thúc.
- `customerId`: lọc theo khách hàng.
- `assignedSaleEmployeeId`: lọc theo sale phụ trách.
- `onlyMine`: chỉ lấy dữ liệu của sale hiện tại.
- `activityTypes`: danh sách loại dữ liệu cần lấy. Giá trị hợp lệ: `FollowUpTask`, `Interaction`, `WorkPlan`.
- `showWeekends`: có hiện thứ bảy/chủ nhật hay không.
- `showDeclinedEvents`: có hiện event đã hủy hay không.
- `showCompletedTasks`: có hiện task/work plan đã hoàn tất hay không.

Response event quan trọng:

```json
{
  "id": "FollowUpTask:019...",
  "sourceId": "019...",
  "sourceType": "FollowUpTask",
  "customerId": "019...",
  "customerExternalId": "KH001",
  "customerName": "Cong ty ABC",
  "assignedSaleEmployeeId": "019...",
  "assignedSaleEmployeeName": "Nguyen Van A",
  "title": "Goi lai bao gia",
  "description": "Trao doi don hang moi",
  "start": "2026-05-23T09:00:00",
  "end": null,
  "status": "Pending",
  "priority": "High",
  "isOverdue": false,
  "colorKey": "high"
}
```

`sourceType` dùng để FE biết event đến từ nghiệp vụ nào:

```text
FollowUpTask
Interaction
WorkPlan
```

`colorKey` gợi ý màu hiển thị:

```text
overdue
urgent
high
done
canceled
meeting
visit
call
work-plan
task
interaction
```

## Dashboard

### Sale dashboard

Endpoint:

```http
GET /api/customer-crm/dashboard/sale
```

Theo sale hiện tại, gồm:

- task hôm nay
- task quá hạn
- task đang mở
- task hoàn tất trong tháng
- interaction trong tháng
- ngày follow-up kế tiếp

### Leader dashboard

Endpoint:

```http
GET /api/customer-crm/dashboard/leader
```

Tổng hợp cấp leader, gồm:

- khách hàng active
- task đang mở
- task quá hạn
- interaction trong tháng
- work plan active

### Director dashboard

Endpoint:

```http
GET /api/customer-crm/dashboard/director
```

Tổng hợp cấp director/toàn công ty, gồm:

- khách hàng active
- task đang mở
- task quá hạn
- task hoàn tất trong tháng
- interaction trong tháng
- work plan active

## Controller Endpoints

Base route:

```http
/api/customer-crm
```

Interaction:

```http
POST /interactions
PUT /interactions/{id}
GET /interactions/{id}
GET /customers/{customerId}/interactions
```

Follow-up task:

```http
POST /follow-up-tasks
PUT /follow-up-tasks/{id}
POST /follow-up-tasks/{id}/complete
GET /follow-up-tasks/my
GET /customers/{customerId}/follow-up-tasks
```

Work plan:

```http
POST /work-plans
PUT /work-plans/{id}
GET /customers/{customerId}/work-plans
```

Calendar:

```http
GET /calendar/events
```

Dashboard:

```http
GET /dashboard/sale
GET /dashboard/leader
GET /dashboard/director
```

## FE Mapping Gợi Ý

Khi dùng calendar library, map response như sau:

```ts
const events = response.data.data.map((x: any) => ({
  id: x.id,
  title: `${x.title} - ${x.customerName ?? ''}`,
  start: x.start,
  end: x.end,
  color: getCrmEventColor(x.colorKey),
  extendedProps: x
}))
```

Khi click event:

```ts
function onEventClick(info: any) {
  const item = info.event.extendedProps

  if (item.sourceType === 'FollowUpTask') {
    // mở modal follow-up task
  }

  if (item.sourceType === 'Interaction') {
    // mở modal lịch sử tương tác
  }

  if (item.sourceType === 'WorkPlan') {
    // mở modal kế hoạch chăm sóc
  }
}
```

Gợi ý màu:

```ts
function getCrmEventColor(colorKey: string) {
  switch (colorKey) {
    case 'overdue': return '#ef4444'
    case 'urgent': return '#dc2626'
    case 'high': return '#f59e0b'
    case 'done': return '#22c55e'
    case 'canceled': return '#94a3b8'
    case 'meeting': return '#8b5cf6'
    case 'visit': return '#06b6d4'
    case 'call': return '#3b82f6'
    case 'work-plan': return '#14b8a6'
    default: return '#64748b'
  }
}
```

## Ghi Chú Bảo Trì

- Không hard delete CRM history. Nếu cần ẩn dữ liệu thì dùng `IsActive = false`.
- Calendar chỉ là view tổng hợp, không có bảng riêng.
- Khi thêm loại event mới, ưu tiên thêm vào `CustomerCrmService.Calendar.cs`.
- Khi thêm filter chung, ưu tiên thêm vào `CustomerCrmService.QueryHelpers.cs`.
- Khi thêm endpoint mới, cập nhật thêm README này để FE dễ bám theo.
