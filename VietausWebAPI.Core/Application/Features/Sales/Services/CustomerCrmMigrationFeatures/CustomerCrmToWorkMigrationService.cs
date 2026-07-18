using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Migrations;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerCrmFeatures;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerCrmFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Entities.WorkTaskSchema;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;
using VietausWebAPI.Core.Domain.Enums.WorkTaskEnums;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.CustomerCrmMigrationFeatures
{
    public sealed class CustomerCrmToWorkMigrationService : ICustomerCrmToWorkMigrationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerFollowUpTaskRepository _customerTaskRepository;
        private readonly ICustomerWorkPlanRepository _customerWorkPlanRepository;
        private readonly IWorkTaskRepository _workTaskRepository;
        private readonly IWorkTaskAssigneeRepository _workTaskAssigneeRepository;
        private readonly IWorkTaskReferenceRepository _workTaskReferenceRepository;
        private readonly IWorkPlanRepository _workPlanRepository;
        private readonly IWorkPlanAssigneeRepository _workPlanAssigneeRepository;
        private readonly IWorkPlanReferenceRepository _workPlanReferenceRepository;

        public CustomerCrmToWorkMigrationService(
            IUnitOfWork unitOfWork,
            ICustomerFollowUpTaskRepository customerTaskRepository,
            ICustomerWorkPlanRepository customerWorkPlanRepository,
            IWorkTaskRepository workTaskRepository,
            IWorkTaskAssigneeRepository workTaskAssigneeRepository,
            IWorkTaskReferenceRepository workTaskReferenceRepository,
            IWorkPlanRepository workPlanRepository,
            IWorkPlanAssigneeRepository workPlanAssigneeRepository,
            IWorkPlanReferenceRepository workPlanReferenceRepository)
        {
            _unitOfWork = unitOfWork;
            _customerTaskRepository = customerTaskRepository;
            _customerWorkPlanRepository = customerWorkPlanRepository;
            _workTaskRepository = workTaskRepository;
            _workTaskAssigneeRepository = workTaskAssigneeRepository;
            _workTaskReferenceRepository = workTaskReferenceRepository;
            _workPlanRepository = workPlanRepository;
            _workPlanAssigneeRepository = workPlanAssigneeRepository;
            _workPlanReferenceRepository = workPlanReferenceRepository;
        }

        public async Task<OperationResult<CustomerCrmToWorkMigrationResultDto>> MigrateAsync(
            CustomerCrmToWorkMigrationOptions options,
            CancellationToken ct = default)
        {
            options ??= new CustomerCrmToWorkMigrationOptions();
            var result = new CustomerCrmToWorkMigrationResultDto { DryRun = options.DryRun };

            var customerTasks = await ApplyTaskFilter(_customerTaskRepository.Query(track: false), options)
                .Include(x => x.Assignees)
                .Include(x => x.CustomerInteraction)
                .OrderBy(x => x.CreatedDate)
                .ToListAsync(ct);

            var customerPlans = await ApplyPlanFilter(_customerWorkPlanRepository.Query(track: false), options)
                .OrderBy(x => x.CreatedDate)
                .ToListAsync(ct);

            result.SourceTaskCount = customerTasks.Count;
            result.SourceWorkPlanCount = customerPlans.Count;

            var customerIds = customerTasks.Select(x => x.CustomerId)
                .Concat(customerPlans.Select(x => x.CustomerId))
                .Distinct()
                .ToList();

            var customerLookup = customerIds.Count == 0
                ? new Dictionary<Guid, CustomerSnapshot>()
                : await _unitOfWork.CustomerRepository.Query()
                    .Where(x => customerIds.Contains(x.CustomerId))
                    .Select(x => new CustomerSnapshot(x.CustomerId, x.ExternalId, x.CustomerName))
                    .ToDictionaryAsync(x => x.CustomerId, ct);

            await using var tx = await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                foreach (var sourceTask in customerTasks)
                    await MigrateTaskAsync(sourceTask, customerLookup, options, result, ct);

                foreach (var sourcePlan in customerPlans)
                    await MigratePlanAsync(sourcePlan, customerLookup, options, result, ct);

                if (result.Issues.Count > 0)
                {
                    await tx.RollbackAsync(ct);
                    return OperationResult<CustomerCrmToWorkMigrationResultDto>.Fail(result, "Migration còn lỗi, chưa ghi dữ liệu.");
                }

                if (options.DryRun)
                {
                    await tx.RollbackAsync(ct);
                    return OperationResult<CustomerCrmToWorkMigrationResultDto>.Ok(result, "Dry-run hợp lệ, chưa ghi dữ liệu.");
                }

                await _unitOfWork.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);
                return OperationResult<CustomerCrmToWorkMigrationResultDto>.Ok(result, "Migration Customer CRM sang WorkTask/WorkPlan thành công.");
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync(ct);
                return OperationResult<CustomerCrmToWorkMigrationResultDto>.Fail(
                    result,
                    ex.InnerException?.Message ?? ex.Message);
            }
        }

        private async Task MigrateTaskAsync(
            CustomerFollowUpTask source,
            Dictionary<Guid, CustomerSnapshot> customerLookup,
            CustomerCrmToWorkMigrationOptions options,
            CustomerCrmToWorkMigrationResultDto result,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(source.Title))
            {
                AddIssue(result, "CustomerFollowUpTask", source.Id, "Title rỗng, không thể tạo WorkTask.");
                return;
            }

            customerLookup.TryGetValue(source.CustomerId, out var customer);
            var referenceType = source.CustomerInteractionId.HasValue
                ? WorkReferenceType.CustomerInteraction
                : WorkReferenceType.Customer;
            var referenceId = source.CustomerInteractionId ?? source.CustomerId;
            var referenceCodeSnapshot = referenceType == WorkReferenceType.CustomerInteraction
                ? customer?.CustomerCode
                : customer?.CustomerCode;
            var referenceNameSnapshot = referenceType == WorkReferenceType.CustomerInteraction
                ? BuildInteractionReferenceName(source, customer)
                : customer?.CustomerName;

            var exists = options.SkipExisting && await _workTaskRepository.Query()
                .AnyAsync(x =>
                    x.CompanyId == source.CompanyId &&
                    x.Title == source.Title &&
                    x.CreatedDate == source.CreatedDate &&
                    x.References.Any(r => r.ReferenceType == referenceType && r.ReferenceId == referenceId),
                    ct);

            if (exists)
            {
                result.SkippedTaskCount++;
                return;
            }

            var workTask = new WorkTask
            {
                Id = Guid.CreateVersion7(),
                Title = source.Title.Trim(),
                Description = source.Description,
                NextAction = source.NextAction,
                Status = MapTaskStatus(source.Status),
                Priority = MapPriority(source.Priority),
                DueDate = source.DueDate,
                DueReminderSentAt = source.DueReminderSentAt,
                CompletedDate = source.CompletedDate,
                CompletedBy = source.CompletedBy,
                CompletionNote = source.CompletionNote,
                AssignedToEmployeeId = source.AssignedSaleEmployeeId,
                CompanyId = source.CompanyId,
                CreatedDate = source.CreatedDate,
                CreatedBy = source.CreatedBy,
                UpdatedDate = source.UpdatedDate,
                UpdatedBy = source.UpdatedBy,
                IsActive = source.IsActive
            };

            await _workTaskRepository.AddAsync(workTask, ct);
            result.CreatedWorkTaskCount++;

            await _workTaskReferenceRepository.AddAsync(new WorkTaskReference
            {
                Id = Guid.CreateVersion7(),
                WorkTaskId = workTask.Id,
                ReferenceType = referenceType,
                ReferenceId = referenceId,
                ReferenceCodeSnapshot = referenceCodeSnapshot,
                ReferenceNameSnapshot = referenceNameSnapshot,
                IsPrimary = true
            }, ct);
            result.CreatedWorkTaskReferenceCount++;

            var migratedAssignees = new HashSet<Guid>();
            foreach (var sourceAssignee in source.Assignees)
            {
                await AddWorkTaskAssigneeAsync(
                    workTask.Id,
                    sourceAssignee.EmployeeId,
                    sourceAssignee.IsPrimary,
                    sourceAssignee.IsActive,
                    sourceAssignee.CreatedBy,
                    sourceAssignee.CreatedDate,
                    migratedAssignees,
                    result,
                    ct);
            }

            if (options.CreatePrimaryAssigneeFromAssignedSale &&
                source.AssignedSaleEmployeeId.HasValue &&
                !migratedAssignees.Contains(source.AssignedSaleEmployeeId.Value))
            {
                await AddWorkTaskAssigneeAsync(
                    workTask.Id,
                    source.AssignedSaleEmployeeId.Value,
                    migratedAssignees.Count == 0,
                    true,
                    source.CreatedBy,
                    source.CreatedDate,
                    migratedAssignees,
                    result,
                    ct);
            }
        }

        private async Task MigratePlanAsync(
            CustomerWorkPlan source,
            Dictionary<Guid, CustomerSnapshot> customerLookup,
            CustomerCrmToWorkMigrationOptions options,
            CustomerCrmToWorkMigrationResultDto result,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(source.PlanName))
            {
                AddIssue(result, "CustomerWorkPlan", source.Id, "PlanName rỗng, không thể tạo WorkPlan.");
                return;
            }

            customerLookup.TryGetValue(source.CustomerId, out var customer);

            var exists = options.SkipExisting && await _workPlanRepository.Query()
                .AnyAsync(x =>
                    x.CompanyId == source.CompanyId &&
                    x.PlanName == source.PlanName &&
                    x.CreatedDate == source.CreatedDate &&
                    x.References.Any(r => r.ReferenceType == WorkReferenceType.Customer && r.ReferenceId == source.CustomerId),
                    ct);

            if (exists)
            {
                result.SkippedWorkPlanCount++;
                return;
            }

            var workPlan = new WorkPlan
            {
                Id = Guid.CreateVersion7(),
                CompanyId = source.CompanyId,
                PlanName = source.PlanName.Trim(),
                Objective = source.Objective,
                Strategy = source.Strategy,
                DiscussionSummary = source.DiscussionSummary,
                NextAction = source.NextAction,
                Status = MapPlanStatus(source.Status),
                Priority = MapPriority(source.Priority),
                StartDate = source.StartDate,
                EndDate = source.EndDate,
                NextFollowUpDate = source.NextFollowUpDate,
                AssignedToEmployeeId = source.AssignedSaleEmployeeId,
                CreatedDate = source.CreatedDate,
                CreatedBy = source.CreatedBy,
                UpdatedDate = source.UpdatedDate,
                UpdatedBy = source.UpdatedBy,
                IsActive = source.IsActive
            };

            await _workPlanRepository.AddAsync(workPlan, ct);
            result.CreatedWorkPlanCount++;

            await _workPlanReferenceRepository.AddAsync(new WorkPlanReference
            {
                Id = Guid.CreateVersion7(),
                WorkPlanId = workPlan.Id,
                ReferenceType = WorkReferenceType.Customer,
                ReferenceId = source.CustomerId,
                ReferenceCodeSnapshot = customer?.CustomerCode,
                ReferenceNameSnapshot = customer?.CustomerName,
                IsPrimary = true
            }, ct);
            result.CreatedWorkPlanReferenceCount++;

            if (options.CreatePrimaryAssigneeFromAssignedSale && source.AssignedSaleEmployeeId.HasValue)
            {
                await _workPlanAssigneeRepository.AddAsync(new WorkPlanAssignee
                {
                    Id = Guid.CreateVersion7(),
                    WorkPlanId = workPlan.Id,
                    EmployeeId = source.AssignedSaleEmployeeId.Value,
                    IsPrimary = true,
                    IsActive = true,
                    CreatedBy = source.CreatedBy,
                    CreatedDate = source.CreatedDate
                }, ct);
                result.CreatedWorkPlanAssigneeCount++;
            }
        }

        private async Task AddWorkTaskAssigneeAsync(
            Guid workTaskId,
            Guid employeeId,
            bool isPrimary,
            bool isActive,
            Guid createdBy,
            DateTime createdDate,
            HashSet<Guid> migratedAssignees,
            CustomerCrmToWorkMigrationResultDto result,
            CancellationToken ct)
        {
            if (!migratedAssignees.Add(employeeId))
                return;

            await _workTaskAssigneeRepository.AddAsync(new WorkTaskAssignee
            {
                Id = Guid.CreateVersion7(),
                WorkTaskId = workTaskId,
                EmployeeId = employeeId,
                IsPrimary = isPrimary,
                IsActive = isActive,
                CreatedBy = createdBy,
                CreatedDate = createdDate
            }, ct);
            result.CreatedWorkTaskAssigneeCount++;
        }

        private static IQueryable<CustomerFollowUpTask> ApplyTaskFilter(
            IQueryable<CustomerFollowUpTask> query,
            CustomerCrmToWorkMigrationOptions options)
        {
            if (options.CompanyId.HasValue)
                query = query.Where(x => x.CompanyId == options.CompanyId.Value);

            if (!options.IncludeInactive)
                query = query.Where(x => x.IsActive);

            if (options.From.HasValue)
                query = query.Where(x => x.CreatedDate >= options.From.Value.Date);

            if (options.To.HasValue)
            {
                var toExclusive = options.To.Value.Date.AddDays(1);
                query = query.Where(x => x.CreatedDate < toExclusive);
            }

            return query;
        }

        private static IQueryable<CustomerWorkPlan> ApplyPlanFilter(
            IQueryable<CustomerWorkPlan> query,
            CustomerCrmToWorkMigrationOptions options)
        {
            if (options.CompanyId.HasValue)
                query = query.Where(x => x.CompanyId == options.CompanyId.Value);

            if (!options.IncludeInactive)
                query = query.Where(x => x.IsActive);

            if (options.From.HasValue)
                query = query.Where(x => x.CreatedDate >= options.From.Value.Date);

            if (options.To.HasValue)
            {
                var toExclusive = options.To.Value.Date.AddDays(1);
                query = query.Where(x => x.CreatedDate < toExclusive);
            }

            return query;
        }

        private static WorkTaskStatus MapTaskStatus(CustomerFollowUpTaskStatus status)
            => Enum.IsDefined(typeof(WorkTaskStatus), (int)status)
                ? (WorkTaskStatus)(int)status
                : WorkTaskStatus.Pending;

        private static string? BuildInteractionReferenceName(CustomerFollowUpTask source, CustomerSnapshot? customer)
        {
            var subject = source.CustomerInteraction?.Subject;
            if (string.IsNullOrWhiteSpace(subject))
                subject = source.CustomerInteraction?.Content;

            if (string.IsNullOrWhiteSpace(subject))
                return customer?.CustomerName;

            return string.IsNullOrWhiteSpace(customer?.CustomerName)
                ? subject
                : $"{customer.CustomerName} - {subject}";
        }

        private static WorkTaskPriority MapPriority(CustomerFollowUpPriority priority)
            => Enum.IsDefined(typeof(WorkTaskPriority), (int)priority)
                ? (WorkTaskPriority)(int)priority
                : WorkTaskPriority.Normal;

        private static WorkPlanStatus MapPlanStatus(CustomerWorkPlanStatus status)
            => Enum.IsDefined(typeof(WorkPlanStatus), (int)status)
                ? (WorkPlanStatus)(int)status
                : WorkPlanStatus.Active;

        private static void AddIssue(
            CustomerCrmToWorkMigrationResultDto result,
            string source,
            Guid sourceId,
            string message)
        {
            result.Issues.Add(new CustomerCrmToWorkMigrationIssueDto
            {
                Source = source,
                SourceId = sourceId,
                Message = message
            });
        }

        private sealed record CustomerSnapshot(Guid CustomerId, string CustomerCode, string CustomerName);
    }
}
