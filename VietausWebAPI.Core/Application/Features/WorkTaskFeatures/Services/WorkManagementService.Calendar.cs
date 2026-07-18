using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.DTOs;
using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.Queries;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.WorkTaskEnums;

namespace VietausWebAPI.Core.Application.Features.WorkTaskFeatures.Services;

public sealed partial class WorkManagementService
{
    public async Task<OperationResult<IReadOnlyList<WorkCalendarEventDto>>> GetCalendarAsync(
        WorkCalendarQuery query,
        CancellationToken ct = default)
    {
        query ??= new WorkCalendarQuery();
        var viewer = await BuildViewerAsync(ct);
        var from = query.From?.Date ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        var toExclusive = query.To?.Date.AddDays(1) ?? from.AddMonths(1);
        var employeeId = query.OnlyMine ? viewer.EmployeeId : query.AssignedToEmployeeId;
        var events = new List<WorkCalendarEventDto>();

        var tasks = ApplyTaskVisibility(_taskRepository.Query(), viewer)
            .Where(x => x.IsActive && x.DueDate.HasValue && x.DueDate.Value >= from && x.DueDate.Value < toExclusive);

        if (employeeId.HasValue)
            tasks = tasks.Where(x => x.AssignedToEmployeeId == employeeId.Value || x.CreatedBy == employeeId.Value ||
                                     x.Assignees.Any(a => a.IsActive && a.EmployeeId == employeeId.Value));
        if (query.ReferenceType.HasValue)
            tasks = tasks.Where(x => x.References.Any(r => r.ReferenceType == query.ReferenceType.Value));
        if (query.ReferenceId.HasValue)
            tasks = tasks.Where(x => x.References.Any(r => r.ReferenceId == query.ReferenceId.Value));
        if (!query.ShowCompleted) tasks = tasks.Where(x => x.Status != WorkTaskStatus.Done);
        if (!query.ShowCanceled) tasks = tasks.Where(x => x.Status != WorkTaskStatus.Canceled);

        events.AddRange(await tasks.Select(x => new WorkCalendarEventDto
        {
            SourceId = x.Id,
            SourceType = "WorkTask",
            Title = x.Title,
            Description = x.NextAction ?? x.Description,
            Start = x.DueDate!.Value,
            Status = x.Status.ToString(),
            Priority = x.Priority.ToString(),
            IsOverdue = x.DueDate.Value < DateTime.Now && x.Status != WorkTaskStatus.Done && x.Status != WorkTaskStatus.Canceled,
            ColorKey = x.Status == WorkTaskStatus.Done ? "done"
                : x.Status == WorkTaskStatus.Canceled ? "canceled"
                : x.DueDate.Value < DateTime.Now ? "overdue"
                : x.Priority == WorkTaskPriority.Urgent ? "urgent"
                : x.Priority == WorkTaskPriority.High ? "high"
                : "task",
            AssignedToEmployeeId = x.AssignedToEmployeeId,
            AssignedToEmployeeName = x.AssignedToEmployee != null ? x.AssignedToEmployee.FullName : null,
            PrimaryReference = x.References.Where(r => r.IsPrimary).Select(r => new WorkReferenceDto
            {
                Id = r.Id,
                ReferenceType = r.ReferenceType,
                ReferenceId = r.ReferenceId,
                ReferenceCodeSnapshot = r.ReferenceCodeSnapshot,
                ReferenceNameSnapshot = r.ReferenceNameSnapshot,
                IsPrimary = true
            }).FirstOrDefault()
        }).ToListAsync(ct));

        var plans = ApplyPlanVisibility(_planRepository.Query(), viewer)
            .Where(x => x.IsActive && x.NextFollowUpDate.HasValue &&
                        x.NextFollowUpDate.Value >= from && x.NextFollowUpDate.Value < toExclusive);

        if (employeeId.HasValue)
            plans = plans.Where(x => x.AssignedToEmployeeId == employeeId.Value || x.CreatedBy == employeeId.Value ||
                                     x.Assignees.Any(a => a.IsActive && a.EmployeeId == employeeId.Value));
        if (query.ReferenceType.HasValue)
            plans = plans.Where(x => x.References.Any(r => r.ReferenceType == query.ReferenceType.Value));
        if (query.ReferenceId.HasValue)
            plans = plans.Where(x => x.References.Any(r => r.ReferenceId == query.ReferenceId.Value));
        if (!query.ShowCompleted) plans = plans.Where(x => x.Status != WorkPlanStatus.Completed);
        if (!query.ShowCanceled) plans = plans.Where(x => x.Status != WorkPlanStatus.Canceled);

        events.AddRange(await plans.Select(x => new WorkCalendarEventDto
        {
            SourceId = x.Id,
            SourceType = "WorkPlan",
            Title = x.PlanName,
            Description = x.NextAction ?? x.Objective,
            Start = x.NextFollowUpDate!.Value,
            End = x.EndDate,
            Status = x.Status.ToString(),
            Priority = x.Priority.ToString(),
            IsOverdue = x.NextFollowUpDate.Value < DateTime.Now &&
                        x.Status != WorkPlanStatus.Completed && x.Status != WorkPlanStatus.Canceled,
            ColorKey = x.Status == WorkPlanStatus.Completed ? "done"
                : x.Status == WorkPlanStatus.Canceled ? "canceled"
                : x.NextFollowUpDate.Value < DateTime.Now ? "overdue"
                : "work-plan",
            AssignedToEmployeeId = x.AssignedToEmployeeId,
            AssignedToEmployeeName = x.AssignedToEmployee != null ? x.AssignedToEmployee.FullName : null,
            PrimaryReference = x.References.Where(r => r.IsPrimary).Select(r => new WorkReferenceDto
            {
                Id = r.Id,
                ReferenceType = r.ReferenceType,
                ReferenceId = r.ReferenceId,
                ReferenceCodeSnapshot = r.ReferenceCodeSnapshot,
                ReferenceNameSnapshot = r.ReferenceNameSnapshot,
                IsPrimary = true
            }).FirstOrDefault()
        }).ToListAsync(ct));

        foreach (var item in events)
            item.Id = $"{item.SourceType}:{item.SourceId}";

        return OperationResult<IReadOnlyList<WorkCalendarEventDto>>.Ok(events.OrderBy(x => x.Start).ThenBy(x => x.Title).ToList());
    }
}
