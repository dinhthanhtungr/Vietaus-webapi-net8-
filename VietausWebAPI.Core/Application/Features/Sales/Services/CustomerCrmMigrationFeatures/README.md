# Customer CRM Migration

Service này dùng để chuyển dữ liệu từ model công việc cũ/generic trong `WorkTaskSchema` sang model CRM mới trong `CustomerSchema`.

## Service

```text
ICustomerCrmMigrationService
CustomerCrmMigrationService
```

Vị trí:

```text
Application/Features/Sales/ServiceContracts/CustomerCrmFeatures/ICustomerCrmMigrationService.cs
Application/Features/Sales/Services/CustomerCrmMigrationFeatures/CustomerCrmMigrationService.cs
```

Service này tách riêng khỏi `CustomerCrmService` để migration không làm rối nghiệp vụ CRM đang chạy thật.

## Model Cũ

Nguồn export hiện tại là model generic:

```text
Work.WorkTasks
Work.WorkTaskAssignees
Work.WorkTaskReferences
Work.WorkPlans
Work.WorkPlanReferences
```

Entity tương ứng:

```text
WorkTask
WorkTaskAssignee
WorkTaskReference
WorkPlan
WorkPlanReference
```

Mapping khách hàng được lấy từ `WorkTaskReference` hoặc `WorkPlanReference` có:

```text
ReferenceType = Customer
ReferenceId   = CustomerId
```

Nếu task/plan cũ không có reference kiểu `Customer`, dòng đó vẫn được export nhưng cột `MigrationNote` sẽ là:

```text
MissingCustomerReference
```

Dòng này cần sửa thủ công trong Excel trước khi import.

## Model Mới

Dữ liệu sau import sẽ vào:

```text
Customer.CustomerInteractions
Customer.CustomerFollowUpTasks
Customer.CustomerFollowUpTaskAssignees
Customer.CustomerWorkPlans
```

Entity mới:

```text
CustomerInteraction
CustomerFollowUpTask
CustomerFollowUpTaskAssignee
CustomerWorkPlan
```

## Mapping Enum

Các enum cũ và mới đang cùng numeric value nên mapping có thể cast theo số.

Task status:

```text
WorkTaskStatus.Pending    -> CustomerFollowUpTaskStatus.Pending
WorkTaskStatus.InProgress -> CustomerFollowUpTaskStatus.InProgress
WorkTaskStatus.Done       -> CustomerFollowUpTaskStatus.Done
WorkTaskStatus.Canceled   -> CustomerFollowUpTaskStatus.Canceled
WorkTaskStatus.Overdue    -> CustomerFollowUpTaskStatus.Overdue
```

Priority:

```text
WorkTaskPriority.Low    -> CustomerFollowUpPriority.Low
WorkTaskPriority.Normal -> CustomerFollowUpPriority.Normal
WorkTaskPriority.High   -> CustomerFollowUpPriority.High
WorkTaskPriority.Urgent -> CustomerFollowUpPriority.Urgent
```

Plan status:

```text
WorkPlanStatus.Draft     -> CustomerWorkPlanStatus.Draft
WorkPlanStatus.Active    -> CustomerWorkPlanStatus.Active
WorkPlanStatus.Paused    -> CustomerWorkPlanStatus.Paused
WorkPlanStatus.Completed -> CustomerWorkPlanStatus.Completed
WorkPlanStatus.Canceled  -> CustomerWorkPlanStatus.Canceled
```

Trong Excel có thể nhập enum bằng tên hoặc bằng số.

## Export

Gọi:

```csharp
Task<byte[]> ExportLegacyWorkToCustomerCrmWorkbookAsync(
    CustomerCrmLegacyExportQuery query,
    CancellationToken ct = default);
```

Query:

```csharp
public sealed class CustomerCrmLegacyExportQuery
{
    public Guid? CompanyId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public bool IncludeInactive { get; set; }
    public bool CustomerOnly { get; set; } = true;
}
```

Workbook export gồm 5 sheet:

```text
README
CustomerInteractions
CustomerFollowUpTasks
TaskAssignees
CustomerWorkPlans
```

### Sheet CustomerInteractions

Sheet này là template. `WorkTask` cũ không đủ dữ liệu để tự tạo lịch sử liên hệ, nên nếu có dữ liệu liên hệ cũ riêng thì điền vào sheet này trước khi import.

Cột chính:

```text
LegacyId
CustomerId
ContactId
InteractionType
Subject
Content
Outcome
NextAction
InteractionAt
NextFollowUpDate
AssignedSaleEmployeeId
CompanyId
CreatedBy
CreatedDate
IsActive
```

### Sheet CustomerFollowUpTasks

Nguồn từ `WorkTask`.

Mapping chính:

```text
WorkTask.Id                   -> LegacyId
WorkTaskReference.Customer    -> CustomerId
WorkTask.Title                -> Title
WorkTask.Description          -> Description
WorkTask.NextAction           -> NextAction
WorkTask.Status               -> Status
WorkTask.Priority             -> Priority
WorkTask.DueDate              -> DueDate
WorkTask.CompletedDate        -> CompletedDate
WorkTask.CompletedBy          -> CompletedBy
WorkTask.CompletionNote       -> CompletionNote
WorkTask.AssignedToEmployeeId -> AssignedSaleEmployeeId
WorkTask.CompanyId            -> CompanyId
WorkTask.CreatedBy            -> CreatedBy
WorkTask.CreatedDate          -> CreatedDate
WorkTask.IsActive             -> IsActive
```

Nếu muốn task mới link với interaction mới, điền `LegacyInteractionId` trùng với `LegacyId` của dòng trong sheet `CustomerInteractions`.

### Sheet TaskAssignees

Nguồn từ `WorkTaskAssignee`.

Mapping:

```text
WorkTaskAssignee.WorkTaskId -> LegacyTaskId
WorkTaskAssignee.EmployeeId -> EmployeeId
WorkTaskAssignee.IsPrimary  -> IsPrimary
WorkTaskAssignee.CreatedBy  -> CreatedBy
WorkTaskAssignee.CreatedDate-> CreatedDate
WorkTaskAssignee.IsActive   -> IsActive
```

Khi import, `LegacyTaskId` sẽ được map sang `CustomerFollowUpTask.Id` mới tạo.

### Sheet CustomerWorkPlans

Nguồn từ `WorkPlan`.

Mapping:

```text
WorkPlan.Id                   -> LegacyId
WorkPlanReference.Customer    -> CustomerId
WorkPlan.PlanName             -> PlanName
WorkPlan.Objective            -> Objective
WorkPlan.Strategy             -> Strategy
WorkPlan.DiscussionSummary    -> DiscussionSummary
WorkPlan.NextAction           -> NextAction
WorkPlan.Status               -> Status
WorkPlan.Priority             -> Priority
WorkPlan.StartDate            -> StartDate
WorkPlan.EndDate              -> EndDate
WorkPlan.NextFollowUpDate     -> NextFollowUpDate
WorkPlan.AssignedToEmployeeId -> AssignedSaleEmployeeId
WorkPlan.CompanyId            -> CompanyId
WorkPlan.CreatedBy            -> CreatedBy
WorkPlan.CreatedDate          -> CreatedDate
WorkPlan.IsActive             -> IsActive
```

## Import

Gọi:

```csharp
Task<OperationResult<CustomerCrmMigrationImportResultDto>> ImportCustomerCrmWorkbookAsync(
    Stream workbookStream,
    CustomerCrmMigrationImportOptions options,
    CancellationToken ct = default);
```

Options:

```csharp
public sealed class CustomerCrmMigrationImportOptions
{
    public bool DryRun { get; set; } = true;
    public bool SkipExisting { get; set; } = true;
    public bool SyncCustomerCrmSnapshot { get; set; } = true;
}
```

Luồng import:

```text
1. Đọc workbook.
2. Import CustomerInteractions trước để tạo map LegacyInteractionId -> Id mới.
3. Import CustomerFollowUpTasks, dùng LegacyInteractionId nếu có.
4. Import TaskAssignees, dùng LegacyTaskId -> CustomerFollowUpTask.Id mới.
5. Import CustomerWorkPlans.
6. Nếu DryRun=true thì rollback transaction sau khi validate.
7. Nếu DryRun=false thì SaveChanges và commit.
8. Nếu SyncCustomerCrmSnapshot=true thì đồng bộ Customer.LastContactDate, Customer.NextFollowUpDate, Customer.CurrentSaleId.
```

## Chống Trùng

Khi `SkipExisting=true`, service bỏ qua:

```text
Interaction trùng CustomerId + CompanyId + InteractionAt + Content
Task trùng CustomerId + CompanyId + Title + CreatedDate
WorkPlan trùng CustomerId + CompanyId + PlanName + CreatedDate
```

Đây là rule chống trùng mềm. Nếu cần import nhiều lần tuyệt đối an toàn, nên bổ sung bảng map riêng:

```text
MigrationImportMap
- SourceTable
- LegacyId
- NewId
- ImportedAt
```

Hiện service chưa tạo bảng này để tránh phát sinh migration DB ngoài phạm vi chuyển dữ liệu.

## Cách Chạy Khuyến Nghị

1. Export workbook từ dữ liệu cũ.
2. Mở Excel và lọc `MigrationNote = MissingCustomerReference`.
3. Điền `CustomerId` cho các dòng thiếu.
4. Nếu có lịch sử liên hệ cũ, điền thêm vào sheet `CustomerInteractions`.
5. Import với `DryRun = true`.
6. Nếu result không có `Issues`, import lại với `DryRun = false`.
7. Kiểm tra màn CRM mới: task, work plan, lịch, dashboard, report.

## Lưu Ý

- Service này không xóa dữ liệu cũ.
- Service này không tự đoán khách hàng nếu thiếu `CustomerId`.
- Service này không tự tạo `CustomerInteraction` từ `WorkTask`, vì task cũ thường không đủ ý nghĩa để biết đó là Call, Meeting, Email hay Visit.
- Nếu cần migrate từ bảng cũ thật sự tên `CustomerTask` ngoài repo hiện tại, có thể thêm một export method khác query đúng bảng đó rồi ghi ra cùng workbook format này.
