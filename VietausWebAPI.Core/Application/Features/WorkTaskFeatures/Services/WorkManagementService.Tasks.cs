using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.DTOs;
using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.Queries;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.WorkTaskSchema;
using VietausWebAPI.Core.Domain.Enums.WorkTaskEnums;

namespace VietausWebAPI.Core.Application.Features.WorkTaskFeatures.Services;

public sealed partial class WorkManagementService
{
    public async Task<OperationResult<Guid>> CreateTaskAsync(CreateWorkTaskRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return OperationResult<Guid>.Fail("Tiêu đề task là bắt buộc.");

        var viewer = await BuildViewerAsync(ct);
        var referenceValidation = await ValidateReferencesAsync(request.References, viewer, ct);
        if (!referenceValidation.Success)
            return OperationResult<Guid>.Fail(referenceValidation.Message!);

        var assignedEmployeeId = ResolveScopedEmployeeId(viewer, request.AssignedToEmployeeId);
        if (assignedEmployeeId == Guid.Empty)
            return OperationResult<Guid>.Fail("Bạn không có quyền giao task cho nhân viên ngoài phạm vi.");

        var now = DateTime.Now;
        var task = new WorkTask
        {
            Id = Guid.CreateVersion7(),
            Title = request.Title.Trim(),
            Description = request.Description,
            NextAction = request.NextAction,
            Status = request.Status,
            Priority = request.Priority,
            DueDate = request.DueDate,
            AssignedToEmployeeId = assignedEmployeeId ?? viewer.EmployeeId,
            CompanyId = viewer.CompanyId,
            CreatedDate = now,
            CreatedBy = viewer.EmployeeId,
            IsActive = true
        };

        await _taskRepository.AddAsync(task, ct);
        await ReplaceTaskReferencesAsync(task, request.References, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return OperationResult<Guid>.Ok(task.Id);
    }

    public async Task<OperationResult> UpdateTaskAsync(Guid id, UpdateWorkTaskRequest request, CancellationToken ct = default)
    {
        var viewer = await BuildViewerAsync(ct);
        var task = await ApplyTaskVisibility(_taskRepository.Query(track: true), viewer)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (task == null)
            return OperationResult.Fail("Không tìm thấy work task.");

        if (request.References != null)
        {
            var referenceValidation = await ValidateReferencesAsync(request.References, viewer, ct);
            if (!referenceValidation.Success)
                return referenceValidation;
        }

        var assignedEmployeeId = ResolveScopedEmployeeId(viewer, request.AssignedToEmployeeId);
        if (assignedEmployeeId == Guid.Empty)
            return OperationResult.Fail("Bạn không có quyền giao task cho nhân viên ngoài phạm vi.");

        if (request.Title != null) task.Title = request.Title.Trim();
        if (request.Description != null) task.Description = request.Description;
        if (request.NextAction != null) task.NextAction = request.NextAction;
        if (request.Status.HasValue) task.Status = request.Status.Value;
        if (request.Priority.HasValue) task.Priority = request.Priority.Value;
        if (request.ClearDueDate)
        {
            task.DueDate = null;
            task.DueReminderSentAt = null;
        }
        else if (request.DueDate.HasValue && task.DueDate != request.DueDate)
        {
            task.DueDate = request.DueDate;
            task.DueReminderSentAt = null;
        }
        if (request.ClearAssignedToEmployee) task.AssignedToEmployeeId = null;
        else if (assignedEmployeeId.HasValue) task.AssignedToEmployeeId = assignedEmployeeId;
        if (request.IsActive.HasValue) task.IsActive = request.IsActive.Value;

        task.UpdatedDate = DateTime.Now;
        task.UpdatedBy = viewer.EmployeeId;

        if (request.References != null)
            await ReplaceTaskReferencesAsync(task, request.References, ct);

        await _unitOfWork.SaveChangesAsync(ct);
        return OperationResult.Ok("Cập nhật work task thành công.");
    }

    public async Task<OperationResult> CompleteTaskAsync(Guid id, CompleteWorkTaskRequest request, CancellationToken ct = default)
    {
        var viewer = await BuildViewerAsync(ct);
        var task = await ApplyTaskVisibility(_taskRepository.Query(track: true), viewer)
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, ct);

        if (task == null)
            return OperationResult.Fail("Không tìm thấy work task.");

        var now = DateTime.Now;
        task.Status = request.Status;
        task.CompletedDate = now;
        task.CompletedBy = viewer.EmployeeId;
        task.CompletionNote = request.CompletionNote;
        task.UpdatedDate = now;
        task.UpdatedBy = viewer.EmployeeId;

        await _unitOfWork.SaveChangesAsync(ct);
        return OperationResult.Ok("Hoàn tất work task thành công.");
    }

    public async Task<OperationResult<WorkTaskDto>> GetTaskAsync(Guid id, CancellationToken ct = default)
    {
        var viewer = await BuildViewerAsync(ct);
        var item = await BuildTaskDtoQuery(ApplyTaskVisibility(_taskRepository.Query(), viewer))
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        return item == null
            ? OperationResult<WorkTaskDto>.Fail("Không tìm thấy work task.")
            : OperationResult<WorkTaskDto>.Ok(item);
    }

    public async Task<OperationResult<PagedResult<WorkTaskDto>>> GetTasksAsync(WorkTaskQuery query, CancellationToken ct = default)
    {
        query ??= new WorkTaskQuery();
        var viewer = await BuildViewerAsync(ct);
        var source = ApplyTaskVisibility(_taskRepository.Query(), viewer);

        if (!query.IncludeInactive) source = source.Where(x => x.IsActive);
        if (query.OnlyMine) source = source.Where(x =>
            x.CreatedBy == viewer.EmployeeId ||
            x.AssignedToEmployeeId == viewer.EmployeeId ||
            x.Assignees.Any(a => a.IsActive && a.EmployeeId == viewer.EmployeeId));
        if (query.AssignedToEmployeeId.HasValue) source = source.Where(x => x.AssignedToEmployeeId == query.AssignedToEmployeeId.Value);
        if (query.Status.HasValue) source = source.Where(x => x.Status == query.Status.Value);
        if (query.Priority.HasValue) source = source.Where(x => x.Priority == query.Priority.Value);
        if (query.ReferenceType.HasValue) source = source.Where(x => x.References.Any(r => r.ReferenceType == query.ReferenceType.Value));
        if (query.ReferenceId.HasValue) source = source.Where(x => x.References.Any(r => r.ReferenceId == query.ReferenceId.Value));
        if (query.OnlyOverdue == true) source = source.Where(x =>
            x.DueDate.HasValue &&
            x.DueDate.Value < DateTime.Now &&
            x.Status != WorkTaskStatus.Done &&
            x.Status != WorkTaskStatus.Canceled);
        if (query.DueFrom.HasValue) source = source.Where(x => x.DueDate >= query.DueFrom.Value);
        if (query.DueTo.HasValue) source = source.Where(x => x.DueDate <= query.DueTo.Value);
        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            var keyword = query.Keyword.Trim();
            source = source.Where(x =>
                x.Title.Contains(keyword) ||
                (x.Description ?? "").Contains(keyword) ||
                (x.NextAction ?? "").Contains(keyword) ||
                x.References.Any(r =>
                    (r.ReferenceCodeSnapshot ?? "").Contains(keyword) ||
                    (r.ReferenceNameSnapshot ?? "").Contains(keyword)));
        }

        var ordered = BuildTaskDtoQuery(source)
            .OrderByDescending(x => x.IsOverdue)
            .ThenBy(x => x.DueDate ?? DateTime.MaxValue)
            .ThenByDescending(x => x.CreatedDate);

        var result = await ToPagedResultAsync(ordered, query.PageNumber, query.PageSize, ct);
        return OperationResult<PagedResult<WorkTaskDto>>.Ok(result);
    }

    private static IQueryable<WorkTaskDto> BuildTaskDtoQuery(IQueryable<WorkTask> query)
    {
        var today = DateTime.Today;
        return query.Select(x => new WorkTaskDto
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            NextAction = x.NextAction,
            Status = x.Status,
            Priority = x.Priority,
            DueDate = x.DueDate,
            CompletedDate = x.CompletedDate,
            CompletedBy = x.CompletedBy,
            CompletionNote = x.CompletionNote,
            AssignedToEmployeeId = x.AssignedToEmployeeId,
            AssignedToEmployeeName = x.AssignedToEmployee != null ? x.AssignedToEmployee.FullName : null,
            IsOverdue = x.DueDate.HasValue && x.DueDate.Value.Date < today &&
                        x.Status != WorkTaskStatus.Done && x.Status != WorkTaskStatus.Canceled,
            CompanyId = x.CompanyId,
            CreatedDate = x.CreatedDate,
            CreatedBy = x.CreatedBy,
            IsActive = x.IsActive,
            References = x.References
                .OrderByDescending(r => r.IsPrimary)
                .Select(r => new WorkReferenceDto
                {
                    Id = r.Id,
                    ReferenceType = r.ReferenceType,
                    ReferenceId = r.ReferenceId,
                    ReferenceCodeSnapshot = r.ReferenceCodeSnapshot,
                    ReferenceNameSnapshot = r.ReferenceNameSnapshot,
                    IsPrimary = r.IsPrimary
                }).ToList(),
            Assignees = x.Assignees
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.IsPrimary)
                .Select(a => new WorkAssigneeDto
                {
                    Id = a.Id,
                    EmployeeId = a.EmployeeId,
                    EmployeeCode = a.Employee.ExternalId,
                    EmployeeName = a.Employee.FullName,
                    IsPrimary = a.IsPrimary,
                    IsActive = a.IsActive,
                    CreatedDate = a.CreatedDate
                }).ToList()
        });
    }
}
