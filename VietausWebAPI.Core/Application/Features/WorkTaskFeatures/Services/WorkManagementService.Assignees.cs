using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using VietausWebAPI.Core.Application.Features.Notifications.DTOs;
using VietausWebAPI.Core.Application.Features.Shared.DTO.Visibility;
using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.DTOs;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.WorkTaskSchema;
using VietausWebAPI.Core.Domain.Enums.Notifications;
using VietausWebAPI.Core.Domain.Enums.Visibilitys;
using VietausWebAPI.WebAPI.Helpers.Securities.Roles;

namespace VietausWebAPI.Core.Application.Features.WorkTaskFeatures.Services;

public sealed partial class WorkManagementService
{
    public async Task<OperationResult<WorkAssigneeDto>> AddTaskAssigneeAsync(
        Guid taskId,
        AddWorkAssigneeRequest request,
        CancellationToken ct = default)
    {
        var viewer = await BuildViewerAsync(ct);
        var task = await ApplyTaskVisibility(_taskRepository.Query(), viewer)
            .FirstOrDefaultAsync(x => x.Id == taskId && x.IsActive, ct);
        if (task == null) return OperationResult<WorkAssigneeDto>.Fail("Không tìm thấy work task.");

        var employee = await ResolveEmployeeAsync(request.EmployeeId, viewer, ct);
        if (employee == null) return OperationResult<WorkAssigneeDto>.Fail("Không tìm thấy nhân viên hoặc nhân viên nằm ngoài phạm vi.");

        if (request.IsPrimary)
            await ClearTaskPrimaryAssigneesAsync(taskId, ct);

        var (assignee, shouldNotify) = await AddOrReactivateTaskAssigneeAsync(taskId, employee.EmployeeId, request.IsPrimary, viewer.EmployeeId, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        if (shouldNotify)
            await PublishAssigneeNotificationAsync(task.Id, task.Title, task.CompanyId, new[] { employee.EmployeeId }, request.NotifySaleAdmin, request.NotifyLeader, false, ct);

        assignee.Employee = employee;
        return OperationResult<WorkAssigneeDto>.Ok(ToAssigneeDto(assignee), "Đã thêm người hỗ trợ.");
    }

    public async Task<OperationResult<IReadOnlyList<WorkAssigneeDto>>> AddTaskGroupAssigneesAsync(
        Guid taskId,
        AddWorkGroupAssigneesRequest request,
        CancellationToken ct = default)
    {
        var viewer = await BuildViewerAsync(ct);
        var task = await ApplyTaskVisibility(_taskRepository.Query(), viewer)
            .FirstOrDefaultAsync(x => x.Id == taskId && x.IsActive, ct);
        if (task == null) return OperationResult<IReadOnlyList<WorkAssigneeDto>>.Fail("Không tìm thấy work task.");

        var employees = await ResolveGroupEmployeesAsync(request, viewer, ct);
        if (employees.Count == 0) return OperationResult<IReadOnlyList<WorkAssigneeDto>>.Fail("Không tìm thấy nhân viên phù hợp trong nhóm.");

        var result = new List<WorkAssigneeDto>();
        var notifyIds = new List<Guid>();
        foreach (var employee in employees)
        {
            var (assignee, shouldNotify) = await AddOrReactivateTaskAssigneeAsync(taskId, employee.EmployeeId, false, viewer.EmployeeId, ct);
            assignee.Employee = employee;
            result.Add(ToAssigneeDto(assignee));
            if (shouldNotify) notifyIds.Add(employee.EmployeeId);
        }

        await _unitOfWork.SaveChangesAsync(ct);
        if (notifyIds.Count > 0)
            await PublishAssigneeNotificationAsync(task.Id, task.Title, task.CompanyId, notifyIds, request.NotifySaleAdmin, request.NotifyLeader, false, ct);

        return OperationResult<IReadOnlyList<WorkAssigneeDto>>.Ok(result, "Đã thêm người hỗ trợ theo nhóm.");
    }

    public async Task<OperationResult> RemoveTaskAssigneeAsync(Guid taskId, Guid employeeId, CancellationToken ct = default)
    {
        var viewer = await BuildViewerAsync(ct);
        var exists = await ApplyTaskVisibility(_taskRepository.Query(), viewer).AnyAsync(x => x.Id == taskId, ct);
        if (!exists) return OperationResult.Fail("Không tìm thấy work task.");

        var assignee = await _taskAssigneeRepository.Query(track: true)
            .FirstOrDefaultAsync(x => x.WorkTaskId == taskId && x.EmployeeId == employeeId && x.IsActive, ct);
        if (assignee == null) return OperationResult.Fail("Không tìm thấy người hỗ trợ.");

        assignee.IsActive = false;
        assignee.IsPrimary = false;
        await _unitOfWork.SaveChangesAsync(ct);
        return OperationResult.Ok("Đã xóa người hỗ trợ.");
    }

    public async Task<OperationResult<IReadOnlyList<WorkAssigneeDto>>> GetTaskAssigneesAsync(Guid taskId, CancellationToken ct = default)
    {
        var viewer = await BuildViewerAsync(ct);
        var exists = await ApplyTaskVisibility(_taskRepository.Query(), viewer).AnyAsync(x => x.Id == taskId, ct);
        if (!exists) return OperationResult<IReadOnlyList<WorkAssigneeDto>>.Fail("Không tìm thấy work task.");

        var items = await _taskAssigneeRepository.Query()
            .Where(x => x.WorkTaskId == taskId && x.IsActive)
            .OrderByDescending(x => x.IsPrimary)
            .ThenBy(x => x.Employee.FullName)
            .Select(x => new WorkAssigneeDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                EmployeeCode = x.Employee.ExternalId,
                EmployeeName = x.Employee.FullName,
                IsPrimary = x.IsPrimary,
                IsActive = x.IsActive,
                CreatedDate = x.CreatedDate
            }).ToListAsync(ct);

        return OperationResult<IReadOnlyList<WorkAssigneeDto>>.Ok(items);
    }

    public async Task<OperationResult<WorkAssigneeDto>> AddPlanAssigneeAsync(
        Guid planId,
        AddWorkAssigneeRequest request,
        CancellationToken ct = default)
    {
        var viewer = await BuildViewerAsync(ct);
        var plan = await ApplyPlanVisibility(_planRepository.Query(), viewer)
            .FirstOrDefaultAsync(x => x.Id == planId && x.IsActive, ct);
        if (plan == null) return OperationResult<WorkAssigneeDto>.Fail("Không tìm thấy work plan.");

        var employee = await ResolveEmployeeAsync(request.EmployeeId, viewer, ct);
        if (employee == null) return OperationResult<WorkAssigneeDto>.Fail("Không tìm thấy nhân viên hoặc nhân viên nằm ngoài phạm vi.");

        if (request.IsPrimary)
            await ClearPlanPrimaryAssigneesAsync(planId, ct);

        var (assignee, shouldNotify) = await AddOrReactivatePlanAssigneeAsync(planId, employee.EmployeeId, request.IsPrimary, viewer.EmployeeId, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        if (shouldNotify)
            await PublishAssigneeNotificationAsync(plan.Id, plan.PlanName, plan.CompanyId, new[] { employee.EmployeeId }, request.NotifySaleAdmin, request.NotifyLeader, true, ct);

        assignee.Employee = employee;
        return OperationResult<WorkAssigneeDto>.Ok(ToAssigneeDto(assignee), "Đã thêm người hỗ trợ.");
    }

    public async Task<OperationResult<IReadOnlyList<WorkAssigneeDto>>> AddPlanGroupAssigneesAsync(
        Guid planId,
        AddWorkGroupAssigneesRequest request,
        CancellationToken ct = default)
    {
        var viewer = await BuildViewerAsync(ct);
        var plan = await ApplyPlanVisibility(_planRepository.Query(), viewer)
            .FirstOrDefaultAsync(x => x.Id == planId && x.IsActive, ct);
        if (plan == null) return OperationResult<IReadOnlyList<WorkAssigneeDto>>.Fail("Không tìm thấy work plan.");

        var employees = await ResolveGroupEmployeesAsync(request, viewer, ct);
        if (employees.Count == 0) return OperationResult<IReadOnlyList<WorkAssigneeDto>>.Fail("Không tìm thấy nhân viên phù hợp trong nhóm.");

        var result = new List<WorkAssigneeDto>();
        var notifyIds = new List<Guid>();
        foreach (var employee in employees)
        {
            var (assignee, shouldNotify) = await AddOrReactivatePlanAssigneeAsync(planId, employee.EmployeeId, false, viewer.EmployeeId, ct);
            assignee.Employee = employee;
            result.Add(ToAssigneeDto(assignee));
            if (shouldNotify) notifyIds.Add(employee.EmployeeId);
        }

        await _unitOfWork.SaveChangesAsync(ct);
        if (notifyIds.Count > 0)
            await PublishAssigneeNotificationAsync(plan.Id, plan.PlanName, plan.CompanyId, notifyIds, request.NotifySaleAdmin, request.NotifyLeader, true, ct);

        return OperationResult<IReadOnlyList<WorkAssigneeDto>>.Ok(result, "Đã thêm người hỗ trợ theo nhóm.");
    }

    public async Task<OperationResult> RemovePlanAssigneeAsync(Guid planId, Guid employeeId, CancellationToken ct = default)
    {
        var viewer = await BuildViewerAsync(ct);
        var exists = await ApplyPlanVisibility(_planRepository.Query(), viewer).AnyAsync(x => x.Id == planId, ct);
        if (!exists) return OperationResult.Fail("Không tìm thấy work plan.");

        var assignee = await _planAssigneeRepository.Query(track: true)
            .FirstOrDefaultAsync(x => x.WorkPlanId == planId && x.EmployeeId == employeeId && x.IsActive, ct);
        if (assignee == null) return OperationResult.Fail("Không tìm thấy người hỗ trợ.");

        assignee.IsActive = false;
        assignee.IsPrimary = false;
        await _unitOfWork.SaveChangesAsync(ct);
        return OperationResult.Ok("Đã xóa người hỗ trợ.");
    }

    public async Task<OperationResult<IReadOnlyList<WorkAssigneeDto>>> GetPlanAssigneesAsync(Guid planId, CancellationToken ct = default)
    {
        var viewer = await BuildViewerAsync(ct);
        var exists = await ApplyPlanVisibility(_planRepository.Query(), viewer).AnyAsync(x => x.Id == planId, ct);
        if (!exists) return OperationResult<IReadOnlyList<WorkAssigneeDto>>.Fail("Không tìm thấy work plan.");

        var items = await _planAssigneeRepository.Query()
            .Where(x => x.WorkPlanId == planId && x.IsActive)
            .OrderByDescending(x => x.IsPrimary)
            .ThenBy(x => x.Employee.FullName)
            .Select(x => new WorkAssigneeDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                EmployeeCode = x.Employee.ExternalId,
                EmployeeName = x.Employee.FullName,
                IsPrimary = x.IsPrimary,
                IsActive = x.IsActive,
                CreatedDate = x.CreatedDate
            }).ToListAsync(ct);

        return OperationResult<IReadOnlyList<WorkAssigneeDto>>.Ok(items);
    }

    private async Task<VietausWebAPI.Core.Domain.Entities.HrSchema.Employee?> ResolveEmployeeAsync(Guid employeeId, ViewerScope viewer, CancellationToken ct)
    {
        if (employeeId == Guid.Empty || ResolveScopedEmployeeId(viewer, employeeId) == Guid.Empty)
            return null;

        return await _unitOfWork.EmployeesRepository.Query(track: false)
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.CompanyId == viewer.CompanyId && x.IsActive, ct);
    }

    private async Task<List<VietausWebAPI.Core.Domain.Entities.HrSchema.Employee>> ResolveGroupEmployeesAsync(
        AddWorkGroupAssigneesRequest request,
        ViewerScope viewer,
        CancellationToken ct)
    {
        if (request.GroupId == Guid.Empty || (!request.AssignMembers && !request.AssignLeader))
            return new();

        var isFullScope = viewer.ScopeType is ViewerScopeType.AdminFull or ViewerScopeType.LabFull;
        if (!isFullScope && viewer.GroupId != request.GroupId)
            return new();

        var employeeIds = await _unitOfWork.MemberInGroupRepository.Query()
            .Where(x => x.GroupId == request.GroupId && x.Group.CompanyId == viewer.CompanyId &&
                        x.IsActive && x.Profile.HasValue &&
                        (request.AssignMembers || (request.AssignLeader && x.IsAdmin == true)))
            .Select(x => x.Profile!.Value)
            .Distinct()
            .ToListAsync(ct);

        return await _unitOfWork.EmployeesRepository.Query(track: false)
            .Where(x => employeeIds.Contains(x.EmployeeId) && x.CompanyId == viewer.CompanyId && x.IsActive)
            .OrderBy(x => x.FullName)
            .ToListAsync(ct);
    }

    private async Task<(WorkTaskAssignee Assignee, bool Notify)> AddOrReactivateTaskAssigneeAsync(
        Guid taskId, Guid employeeId, bool isPrimary, Guid createdBy, CancellationToken ct)
    {
        var item = await _taskAssigneeRepository.Query(track: true)
            .Where(x => x.WorkTaskId == taskId && x.EmployeeId == employeeId)
            .OrderByDescending(x => x.IsActive)
            .FirstOrDefaultAsync(ct);
        if (item == null)
        {
            item = new WorkTaskAssignee
            {
                Id = Guid.CreateVersion7(), WorkTaskId = taskId, EmployeeId = employeeId,
                IsPrimary = isPrimary, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = createdBy
            };
            await _taskAssigneeRepository.AddAsync(item, ct);
            return (item, true);
        }
        var notify = !item.IsActive;
        item.IsActive = true;
        item.IsPrimary = isPrimary;
        return (item, notify);
    }

    private async Task<(WorkPlanAssignee Assignee, bool Notify)> AddOrReactivatePlanAssigneeAsync(
        Guid planId, Guid employeeId, bool isPrimary, Guid createdBy, CancellationToken ct)
    {
        var item = await _planAssigneeRepository.Query(track: true)
            .Where(x => x.WorkPlanId == planId && x.EmployeeId == employeeId)
            .OrderByDescending(x => x.IsActive)
            .FirstOrDefaultAsync(ct);
        if (item == null)
        {
            item = new WorkPlanAssignee
            {
                Id = Guid.CreateVersion7(), WorkPlanId = planId, EmployeeId = employeeId,
                IsPrimary = isPrimary, IsActive = true, CreatedDate = DateTime.Now, CreatedBy = createdBy
            };
            await _planAssigneeRepository.AddAsync(item, ct);
            return (item, true);
        }
        var notify = !item.IsActive;
        item.IsActive = true;
        item.IsPrimary = isPrimary;
        return (item, notify);
    }

    private async Task ClearTaskPrimaryAssigneesAsync(Guid taskId, CancellationToken ct)
    {
        var items = await _taskAssigneeRepository.Query(track: true)
            .Where(x => x.WorkTaskId == taskId && x.IsActive && x.IsPrimary).ToListAsync(ct);
        foreach (var item in items) item.IsPrimary = false;
    }

    private async Task ClearPlanPrimaryAssigneesAsync(Guid planId, CancellationToken ct)
    {
        var items = await _planAssigneeRepository.Query(track: true)
            .Where(x => x.WorkPlanId == planId && x.IsActive && x.IsPrimary).ToListAsync(ct);
        foreach (var item in items) item.IsPrimary = false;
    }

    private async Task PublishAssigneeNotificationAsync(
        Guid sourceId, string title, Guid companyId, IEnumerable<Guid> employeeIds,
        bool notifySaleAdmin, bool notifyLeader, bool isPlan, CancellationToken ct)
    {
        var roles = new List<string>();
        if (notifySaleAdmin) roles.Add(AppRoles.SaleAdmin);
        if (notifyLeader) roles.Add(AppRoles.Leader);

        await _notificationService.PublishAsync(new PublishNotificationRequest
        {
            CompanyId = companyId,
            CreatedBy = _currentUser.EmployeeId,
            CreatedByNameSnapshot = _currentUser.personName,
            Topic = isPlan ? TopicNotifications.WorkPlanAssigneeAdded : TopicNotifications.WorkTaskAssigneeAdded,
            Severity = NotificationSeverity.Info,
            Title = $"{_currentUser.personName} yêu cầu hỗ trợ",
            Message = $"{_currentUser.personName} yêu cầu hỗ trợ cho {(isPlan ? "kế hoạch" : "công việc")}: {title}",
            Link = $"/work-management/{(isPlan ? "plans" : "tasks")}/{sourceId}",
            PayloadJson = JsonSerializer.Serialize(new { sourceId, isPlan }),
            TargetUserIds = employeeIds.Distinct().ToList(),
            TargetRoles = roles.Count == 0 ? null : roles
        }, ct);
    }
}
