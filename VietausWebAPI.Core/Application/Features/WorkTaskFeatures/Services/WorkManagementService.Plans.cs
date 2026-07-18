using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.DTOs;
using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.Queries;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.WorkTaskSchema;

namespace VietausWebAPI.Core.Application.Features.WorkTaskFeatures.Services;

public sealed partial class WorkManagementService
{
    public async Task<OperationResult<Guid>> CreatePlanAsync(CreateWorkPlanRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.PlanName))
            return OperationResult<Guid>.Fail("Tên kế hoạch là bắt buộc.");

        var viewer = await BuildViewerAsync(ct);
        var referenceValidation = await ValidateReferencesAsync(request.References, viewer, ct);
        if (!referenceValidation.Success)
            return OperationResult<Guid>.Fail(referenceValidation.Message!);

        var assignedEmployeeId = ResolveScopedEmployeeId(viewer, request.AssignedToEmployeeId);
        if (assignedEmployeeId == Guid.Empty)
            return OperationResult<Guid>.Fail("Bạn không có quyền giao kế hoạch cho nhân viên ngoài phạm vi.");

        var plan = new WorkPlan
        {
            Id = Guid.CreateVersion7(),
            CompanyId = viewer.CompanyId,
            PlanName = request.PlanName.Trim(),
            Objective = request.Objective,
            Strategy = request.Strategy,
            DiscussionSummary = request.DiscussionSummary,
            NextAction = request.NextAction,
            Status = request.Status,
            Priority = request.Priority,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            NextFollowUpDate = request.NextFollowUpDate,
            AssignedToEmployeeId = assignedEmployeeId ?? viewer.EmployeeId,
            CreatedDate = DateTime.Now,
            CreatedBy = viewer.EmployeeId,
            IsActive = true
        };

        await _planRepository.AddAsync(plan, ct);
        await ReplacePlanReferencesAsync(plan, request.References, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return OperationResult<Guid>.Ok(plan.Id);
    }

    public async Task<OperationResult> UpdatePlanAsync(Guid id, UpdateWorkPlanRequest request, CancellationToken ct = default)
    {
        var viewer = await BuildViewerAsync(ct);
        var plan = await ApplyPlanVisibility(_planRepository.Query(track: true), viewer)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (plan == null)
            return OperationResult.Fail("Không tìm thấy work plan.");

        if (request.References != null)
        {
            var referenceValidation = await ValidateReferencesAsync(request.References, viewer, ct);
            if (!referenceValidation.Success)
                return referenceValidation;
        }

        var assignedEmployeeId = ResolveScopedEmployeeId(viewer, request.AssignedToEmployeeId);
        if (assignedEmployeeId == Guid.Empty)
            return OperationResult.Fail("Bạn không có quyền giao kế hoạch cho nhân viên ngoài phạm vi.");

        if (request.PlanName != null) plan.PlanName = request.PlanName.Trim();
        if (request.Objective != null) plan.Objective = request.Objective;
        if (request.Strategy != null) plan.Strategy = request.Strategy;
        if (request.DiscussionSummary != null) plan.DiscussionSummary = request.DiscussionSummary;
        if (request.NextAction != null) plan.NextAction = request.NextAction;
        if (request.Status.HasValue) plan.Status = request.Status.Value;
        if (request.Priority.HasValue) plan.Priority = request.Priority.Value;
        if (request.StartDate.HasValue) plan.StartDate = request.StartDate;
        if (request.EndDate.HasValue) plan.EndDate = request.EndDate;
        if (request.NextFollowUpDate.HasValue) plan.NextFollowUpDate = request.NextFollowUpDate;
        if (request.ClearAssignedToEmployee) plan.AssignedToEmployeeId = null;
        else if (assignedEmployeeId.HasValue) plan.AssignedToEmployeeId = assignedEmployeeId;
        if (request.IsActive.HasValue) plan.IsActive = request.IsActive.Value;

        plan.UpdatedDate = DateTime.Now;
        plan.UpdatedBy = viewer.EmployeeId;

        if (request.References != null)
            await ReplacePlanReferencesAsync(plan, request.References, ct);

        await _unitOfWork.SaveChangesAsync(ct);
        return OperationResult.Ok("Cập nhật work plan thành công.");
    }

    public async Task<OperationResult<WorkPlanDto>> GetPlanAsync(Guid id, CancellationToken ct = default)
    {
        var viewer = await BuildViewerAsync(ct);
        var item = await BuildPlanDtoQuery(ApplyPlanVisibility(_planRepository.Query(), viewer))
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        return item == null
            ? OperationResult<WorkPlanDto>.Fail("Không tìm thấy work plan.")
            : OperationResult<WorkPlanDto>.Ok(item);
    }

    public async Task<OperationResult<PagedResult<WorkPlanDto>>> GetPlansAsync(WorkPlanQuery query, CancellationToken ct = default)
    {
        query ??= new WorkPlanQuery();
        var viewer = await BuildViewerAsync(ct);
        var source = ApplyPlanVisibility(_planRepository.Query(), viewer);

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
        if (query.From.HasValue) source = source.Where(x => x.NextFollowUpDate >= query.From.Value || x.StartDate >= query.From.Value);
        if (query.To.HasValue) source = source.Where(x => x.NextFollowUpDate <= query.To.Value || x.EndDate <= query.To.Value);
        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            var keyword = query.Keyword.Trim();
            source = source.Where(x =>
                x.PlanName.Contains(keyword) ||
                (x.Objective ?? "").Contains(keyword) ||
                (x.Strategy ?? "").Contains(keyword) ||
                (x.NextAction ?? "").Contains(keyword) ||
                x.References.Any(r =>
                    (r.ReferenceCodeSnapshot ?? "").Contains(keyword) ||
                    (r.ReferenceNameSnapshot ?? "").Contains(keyword)));
        }

        var ordered = BuildPlanDtoQuery(source)
            .OrderBy(x => x.NextFollowUpDate ?? DateTime.MaxValue)
            .ThenByDescending(x => x.CreatedDate);

        var result = await ToPagedResultAsync(ordered, query.PageNumber, query.PageSize, ct);
        return OperationResult<PagedResult<WorkPlanDto>>.Ok(result);
    }

    private static IQueryable<WorkPlanDto> BuildPlanDtoQuery(IQueryable<WorkPlan> query)
    {
        return query.Select(x => new WorkPlanDto
        {
            Id = x.Id,
            PlanName = x.PlanName,
            Objective = x.Objective,
            Strategy = x.Strategy,
            DiscussionSummary = x.DiscussionSummary,
            NextAction = x.NextAction,
            Status = x.Status,
            Priority = x.Priority,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            NextFollowUpDate = x.NextFollowUpDate,
            AssignedToEmployeeId = x.AssignedToEmployeeId,
            AssignedToEmployeeName = x.AssignedToEmployee != null ? x.AssignedToEmployee.FullName : null,
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
