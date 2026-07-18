using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.Shared.DTO.Visibility;
using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.DTOs;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.WorkTaskSchema;
using VietausWebAPI.Core.Domain.Enums.Visibilitys;
using VietausWebAPI.Core.Domain.Enums.WorkTaskEnums;

namespace VietausWebAPI.Core.Application.Features.WorkTaskFeatures.Services;

public sealed partial class WorkManagementService
{
    private async Task<ViewerScope> BuildViewerAsync(CancellationToken ct)
        => await _visibilityHelper.BuildViewerScopeAsync(ct);

    private static Guid? ResolveScopedEmployeeId(ViewerScope viewer, Guid? requestedEmployeeId)
    {
        if (!requestedEmployeeId.HasValue)
            return null;

        if (viewer.ScopeType is ViewerScopeType.AdminFull or ViewerScopeType.LabFull)
            return requestedEmployeeId.Value;

        return viewer.EmployeeIdsInScope.Contains(requestedEmployeeId.Value)
            ? requestedEmployeeId.Value
            : Guid.Empty;
    }

    private IQueryable<WorkTask> ApplyTaskVisibility(IQueryable<WorkTask> query, ViewerScope viewer)
    {
        query = query.Where(x => x.CompanyId == viewer.CompanyId);
        if (viewer.ScopeType is ViewerScopeType.AdminFull or ViewerScopeType.LabFull)
            return query;

        var employeeIds = viewer.EmployeeIdsInScope.ToList();
        return query.Where(x =>
            employeeIds.Contains(x.CreatedBy) ||
            (x.AssignedToEmployeeId.HasValue && employeeIds.Contains(x.AssignedToEmployeeId.Value)) ||
            x.Assignees.Any(a => a.IsActive && employeeIds.Contains(a.EmployeeId)));
    }

    private IQueryable<WorkPlan> ApplyPlanVisibility(IQueryable<WorkPlan> query, ViewerScope viewer)
    {
        query = query.Where(x => x.CompanyId == viewer.CompanyId);
        if (viewer.ScopeType is ViewerScopeType.AdminFull or ViewerScopeType.LabFull)
            return query;

        var employeeIds = viewer.EmployeeIdsInScope.ToList();
        return query.Where(x =>
            employeeIds.Contains(x.CreatedBy) ||
            (x.AssignedToEmployeeId.HasValue && employeeIds.Contains(x.AssignedToEmployeeId.Value)) ||
            x.Assignees.Any(a => a.IsActive && employeeIds.Contains(a.EmployeeId)));
    }

    private async Task<OperationResult> ValidateReferencesAsync(
        IEnumerable<WorkReferenceRequest> references,
        ViewerScope viewer,
        CancellationToken ct)
    {
        var items = references.ToList();
        if (items.Count(x => x.IsPrimary) > 1)
            return OperationResult.Fail("Chỉ được chọn một reference chính.");

        if (items.Any(x => x.ReferenceId == Guid.Empty))
            return OperationResult.Fail("ReferenceId không hợp lệ.");

        if (items.GroupBy(x => new { x.ReferenceType, x.ReferenceId }).Any(x => x.Count() > 1))
            return OperationResult.Fail("Danh sách reference bị trùng.");

        var customerIds = items
            .Where(x => x.ReferenceType == WorkReferenceType.Customer)
            .Select(x => x.ReferenceId)
            .Distinct()
            .ToList();

        if (customerIds.Count == 0)
            return OperationResult.Ok();

        var visibleCustomerCount = await _visibilityHelper
            .ApplyCustomer(_unitOfWork.CustomerRepository.Query(), viewer)
            .CountAsync(x => x.CompanyId == viewer.CompanyId && customerIds.Contains(x.CustomerId), ct);

        return visibleCustomerCount == customerIds.Count
            ? OperationResult.Ok()
            : OperationResult.Fail("Có khách hàng không tồn tại hoặc nằm ngoài phạm vi xem.");
    }

    private async Task ReplaceTaskReferencesAsync(
        WorkTask task,
        IEnumerable<WorkReferenceRequest> references,
        CancellationToken ct)
    {
        var existing = await _taskReferenceRepository.Query(track: true)
            .Where(x => x.WorkTaskId == task.Id)
            .ToListAsync(ct);

        foreach (var item in existing)
            _taskReferenceRepository.Remove(item);

        var entities = references.Select(x => new WorkTaskReference
        {
            Id = Guid.CreateVersion7(),
            WorkTaskId = task.Id,
            ReferenceType = x.ReferenceType,
            ReferenceId = x.ReferenceId,
            ReferenceCodeSnapshot = x.ReferenceCodeSnapshot,
            ReferenceNameSnapshot = x.ReferenceNameSnapshot,
            IsPrimary = x.IsPrimary
        });

        await _taskReferenceRepository.AddRangeAsync(entities, ct);
    }

    private async Task ReplacePlanReferencesAsync(
        WorkPlan plan,
        IEnumerable<WorkReferenceRequest> references,
        CancellationToken ct)
    {
        var existing = await _planReferenceRepository.Query(track: true)
            .Where(x => x.WorkPlanId == plan.Id)
            .ToListAsync(ct);

        foreach (var item in existing)
            _planReferenceRepository.Remove(item);

        var entities = references.Select(x => new WorkPlanReference
        {
            Id = Guid.CreateVersion7(),
            WorkPlanId = plan.Id,
            ReferenceType = x.ReferenceType,
            ReferenceId = x.ReferenceId,
            ReferenceCodeSnapshot = x.ReferenceCodeSnapshot,
            ReferenceNameSnapshot = x.ReferenceNameSnapshot,
            IsPrimary = x.IsPrimary
        });

        await _planReferenceRepository.AddRangeAsync(entities, ct);
    }

    private static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken ct)
    {
        if (pageNumber <= 0) pageNumber = 1;
        if (pageSize <= 0) pageSize = 15;

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<T>(items, total, pageNumber, pageSize);
    }

    private static WorkReferenceDto ToReferenceDto(WorkTaskReference x) => new()
    {
        Id = x.Id,
        ReferenceType = x.ReferenceType,
        ReferenceId = x.ReferenceId,
        ReferenceCodeSnapshot = x.ReferenceCodeSnapshot,
        ReferenceNameSnapshot = x.ReferenceNameSnapshot,
        IsPrimary = x.IsPrimary
    };

    private static WorkReferenceDto ToReferenceDto(WorkPlanReference x) => new()
    {
        Id = x.Id,
        ReferenceType = x.ReferenceType,
        ReferenceId = x.ReferenceId,
        ReferenceCodeSnapshot = x.ReferenceCodeSnapshot,
        ReferenceNameSnapshot = x.ReferenceNameSnapshot,
        IsPrimary = x.IsPrimary
    };

    private static WorkAssigneeDto ToAssigneeDto(WorkTaskAssignee x) => new()
    {
        Id = x.Id,
        EmployeeId = x.EmployeeId,
        EmployeeCode = x.Employee.ExternalId,
        EmployeeName = x.Employee.FullName,
        IsPrimary = x.IsPrimary,
        IsActive = x.IsActive,
        CreatedDate = x.CreatedDate
    };

    private static WorkAssigneeDto ToAssigneeDto(WorkPlanAssignee x) => new()
    {
        Id = x.Id,
        EmployeeId = x.EmployeeId,
        EmployeeCode = x.Employee.ExternalId,
        EmployeeName = x.Employee.FullName,
        IsPrimary = x.IsPrimary,
        IsActive = x.IsActive,
        CreatedDate = x.CreatedDate
    };
}
