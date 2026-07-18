using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using CustomerTaskPriority = VietausWebAPI.Core.Domain.Enums.CustomerEnum.CustomerFollowUpPriority;
using LegacyTaskPriority = VietausWebAPI.Core.Domain.Enums.WorkTaskEnums.WorkTaskPriority;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.CustomerCrmMigrationFeatures
{
    /// <summary>
    /// Migration service for moving old generic Work task/plan data into the customer-specific CRM tables.
    /// The exported workbook is intentionally human-readable so legacy data can be reviewed before import.
    /// </summary>
    public sealed class CustomerCrmMigrationService : ICustomerCrmMigrationService
    {
        private const string InteractionSheet = "CustomerInteractions";
        private const string TaskSheet = "CustomerFollowUpTasks";
        private const string TaskAssigneeSheet = "TaskAssignees";
        private const string WorkPlanSheet = "CustomerWorkPlans";
        private const string ReadmeSheet = "README";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkTaskRepository _legacyTaskRepository;
        private readonly IWorkPlanRepository _legacyPlanRepository;
        private readonly ICustomerInteractionRepository _interactionRepository;
        private readonly ICustomerFollowUpTaskRepository _taskRepository;
        private readonly ICustomerFollowUpTaskAssigneeRepository _taskAssigneeRepository;
        private readonly ICustomerWorkPlanRepository _workPlanRepository;

        public CustomerCrmMigrationService(
            IUnitOfWork unitOfWork,
            IWorkTaskRepository legacyTaskRepository,
            IWorkPlanRepository legacyPlanRepository,
            ICustomerInteractionRepository interactionRepository,
            ICustomerFollowUpTaskRepository taskRepository,
            ICustomerFollowUpTaskAssigneeRepository taskAssigneeRepository,
            ICustomerWorkPlanRepository workPlanRepository)
        {
            _unitOfWork = unitOfWork;
            _legacyTaskRepository = legacyTaskRepository;
            _legacyPlanRepository = legacyPlanRepository;
            _interactionRepository = interactionRepository;
            _taskRepository = taskRepository;
            _taskAssigneeRepository = taskAssigneeRepository;
            _workPlanRepository = workPlanRepository;
        }

        public async Task<byte[]> ExportLegacyWorkToCustomerCrmWorkbookAsync(
            CustomerCrmLegacyExportQuery query,
            CancellationToken ct = default)
        {
            query ??= new CustomerCrmLegacyExportQuery();

            var legacyTasks = await ApplyLegacyTaskFilter(_legacyTaskRepository.Query(track: false), query)
                .Include(x => x.References)
                .Include(x => x.Assignees)
                .OrderBy(x => x.CreatedDate)
                .ToListAsync(ct);

            var legacyPlans = await ApplyLegacyPlanFilter(_legacyPlanRepository.Query(track: false), query)
                .Include(x => x.References)
                .OrderBy(x => x.CreatedDate)
                .ToListAsync(ct);

            var customerIds = legacyTasks
                .Select(ResolveCustomerId)
                .Concat(legacyPlans.Select(ResolveCustomerId))
                .Where(x => x.HasValue)
                .Select(x => x!.Value)
                .Distinct()
                .ToList();

            var customerLookup = customerIds.Count == 0
                ? new Dictionary<Guid, CustomerSnapshot>()
                : await _unitOfWork.CustomerRepository.Query()
                    .Where(x => customerIds.Contains(x.CustomerId))
                    .Select(x => new CustomerSnapshot(
                        x.CustomerId,
                        x.ExternalId,
                        x.CustomerName))
                    .ToDictionaryAsync(x => x.CustomerId, ct);

            using var wb = new XLWorkbook();
            BuildReadmeSheet(wb);
            BuildInteractionTemplateSheet(wb);
            BuildTaskSheet(wb, legacyTasks, customerLookup);
            BuildTaskAssigneeSheet(wb, legacyTasks);
            BuildWorkPlanSheet(wb, legacyPlans, customerLookup);

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        public async Task<byte[]> ExportCurrentCustomerCrmWorkbookAsync(
            CustomerCrmLegacyExportQuery query,
            CancellationToken ct = default)
        {
            query ??= new CustomerCrmLegacyExportQuery();

            var interactions = await ApplyCustomerCrmFilter(_interactionRepository.Query(track: false), query)
                .OrderBy(x => x.CreatedDate)
                .ToListAsync(ct);

            var tasks = await ApplyCustomerCrmFilter(_taskRepository.Query(track: false), query)
                .Include(x => x.Assignees)
                .OrderBy(x => x.CreatedDate)
                .ToListAsync(ct);

            var plans = await ApplyCustomerCrmFilter(_workPlanRepository.Query(track: false), query)
                .OrderBy(x => x.CreatedDate)
                .ToListAsync(ct);

            var customerIds = interactions.Select(x => x.CustomerId)
                .Concat(tasks.Select(x => x.CustomerId))
                .Concat(plans.Select(x => x.CustomerId))
                .Distinct()
                .ToList();

            var customerLookup = customerIds.Count == 0
                ? new Dictionary<Guid, CustomerSnapshot>()
                : await _unitOfWork.CustomerRepository.Query()
                    .Where(x => customerIds.Contains(x.CustomerId))
                    .Select(x => new CustomerSnapshot(
                        x.CustomerId,
                        x.ExternalId,
                        x.CustomerName))
                    .ToDictionaryAsync(x => x.CustomerId, ct);

            using var wb = new XLWorkbook();
            BuildReadmeSheet(wb);
            BuildCurrentInteractionSheet(wb, interactions, customerLookup);
            BuildCurrentTaskSheet(wb, tasks, customerLookup);
            BuildCurrentTaskAssigneeSheet(wb, tasks);
            BuildCurrentWorkPlanSheet(wb, plans, customerLookup);

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        public async Task<OperationResult<CustomerCrmMigrationImportResultDto>> ImportCustomerCrmWorkbookAsync(
            Stream workbookStream,
            CustomerCrmMigrationImportOptions options,
            CancellationToken ct = default)
        {
            options ??= new CustomerCrmMigrationImportOptions();

            if (workbookStream == null || !workbookStream.CanRead)
                return OperationResult<CustomerCrmMigrationImportResultDto>.Fail("Workbook stream không hợp lệ.");

            using var wb = new XLWorkbook(workbookStream);
            var result = new CustomerCrmMigrationImportResultDto { DryRun = options.DryRun };

            var interactionRows = ReadSheet(wb, InteractionSheet, result);
            var taskRows = ReadSheet(wb, TaskSheet, result);
            var assigneeRows = ReadSheet(wb, TaskAssigneeSheet, result);
            var planRows = ReadSheet(wb, WorkPlanSheet, result);

            result.InteractionRows = interactionRows.Count;
            result.TaskRows = taskRows.Count;
            result.TaskAssigneeRows = assigneeRows.Count;
            result.WorkPlanRows = planRows.Count;

            var interactionIdByLegacyId = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
            var taskIdByLegacyId = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);

            await using var tx = await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await ImportInteractionsAsync(interactionRows, interactionIdByLegacyId, result, options, ct);
                await ImportTasksAsync(taskRows, interactionIdByLegacyId, taskIdByLegacyId, result, options, ct);
                await ImportTaskAssigneesAsync(assigneeRows, taskIdByLegacyId, result, options, ct);
                await ImportWorkPlansAsync(planRows, result, options, ct);

                if (result.Issues.Count > 0)
                {
                    await tx.RollbackAsync(ct);
                    return OperationResult<CustomerCrmMigrationImportResultDto>.Fail(result, "Workbook còn lỗi, chưa import.");
                }

                if (options.DryRun)
                {
                    await tx.RollbackAsync(ct);
                    return OperationResult<CustomerCrmMigrationImportResultDto>.Ok(result, "Dry-run hợp lệ, chưa ghi dữ liệu.");
                }

                if (options.SyncCustomerCrmSnapshot)
                    await SyncCustomerSnapshotsAsync(ct);

                await _unitOfWork.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);

                return OperationResult<CustomerCrmMigrationImportResultDto>.Ok(result, "Import CRM migration thành công.");
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync(ct);
                return OperationResult<CustomerCrmMigrationImportResultDto>.Fail(
                    result,
                    ex.InnerException?.Message ?? ex.Message);
            }
        }

        private static IQueryable<WorkTask> ApplyLegacyTaskFilter(IQueryable<WorkTask> query, CustomerCrmLegacyExportQuery filter)
        {
            if (filter.CompanyId.HasValue)
                query = query.Where(x => x.CompanyId == filter.CompanyId.Value);

            if (!filter.IncludeInactive)
                query = query.Where(x => x.IsActive);

            if (filter.From.HasValue)
                query = query.Where(x => x.CreatedDate >= filter.From.Value.Date);

            if (filter.To.HasValue)
            {
                var toExclusive = filter.To.Value.Date.AddDays(1);
                query = query.Where(x => x.CreatedDate < toExclusive);
            }

            if (filter.CustomerOnly)
                query = query.Where(x => x.References.Any(r => r.ReferenceType == WorkReferenceType.Customer));

            return query;
        }

        private static IQueryable<WorkPlan> ApplyLegacyPlanFilter(IQueryable<WorkPlan> query, CustomerCrmLegacyExportQuery filter)
        {
            if (filter.CompanyId.HasValue)
                query = query.Where(x => x.CompanyId == filter.CompanyId.Value);

            if (!filter.IncludeInactive)
                query = query.Where(x => x.IsActive);

            if (filter.From.HasValue)
                query = query.Where(x => x.CreatedDate >= filter.From.Value.Date);

            if (filter.To.HasValue)
            {
                var toExclusive = filter.To.Value.Date.AddDays(1);
                query = query.Where(x => x.CreatedDate < toExclusive);
            }

            if (filter.CustomerOnly)
                query = query.Where(x => x.References.Any(r => r.ReferenceType == WorkReferenceType.Customer));

            return query;
        }

        private static IQueryable<T> ApplyCustomerCrmFilter<T>(IQueryable<T> query, CustomerCrmLegacyExportQuery filter)
            where T : class
        {
            if (typeof(T) == typeof(CustomerInteraction))
            {
                var q = query.Cast<CustomerInteraction>();
                if (filter.CompanyId.HasValue)
                    q = q.Where(x => x.CompanyId == filter.CompanyId.Value);
                if (!filter.IncludeInactive)
                    q = q.Where(x => x.IsActive);
                if (filter.From.HasValue)
                    q = q.Where(x => x.CreatedDate >= filter.From.Value.Date);
                if (filter.To.HasValue)
                {
                    var toExclusive = filter.To.Value.Date.AddDays(1);
                    q = q.Where(x => x.CreatedDate < toExclusive);
                }
                return q.Cast<T>();
            }

            if (typeof(T) == typeof(CustomerFollowUpTask))
            {
                var q = query.Cast<CustomerFollowUpTask>();
                if (filter.CompanyId.HasValue)
                    q = q.Where(x => x.CompanyId == filter.CompanyId.Value);
                if (!filter.IncludeInactive)
                    q = q.Where(x => x.IsActive);
                if (filter.From.HasValue)
                    q = q.Where(x => x.CreatedDate >= filter.From.Value.Date);
                if (filter.To.HasValue)
                {
                    var toExclusive = filter.To.Value.Date.AddDays(1);
                    q = q.Where(x => x.CreatedDate < toExclusive);
                }
                return q.Cast<T>();
            }

            if (typeof(T) == typeof(CustomerWorkPlan))
            {
                var q = query.Cast<CustomerWorkPlan>();
                if (filter.CompanyId.HasValue)
                    q = q.Where(x => x.CompanyId == filter.CompanyId.Value);
                if (!filter.IncludeInactive)
                    q = q.Where(x => x.IsActive);
                if (filter.From.HasValue)
                    q = q.Where(x => x.CreatedDate >= filter.From.Value.Date);
                if (filter.To.HasValue)
                {
                    var toExclusive = filter.To.Value.Date.AddDays(1);
                    q = q.Where(x => x.CreatedDate < toExclusive);
                }
                return q.Cast<T>();
            }

            return query;
        }

        private static void BuildReadmeSheet(XLWorkbook wb)
        {
            var ws = wb.Worksheets.Add(ReadmeSheet);
            ws.Cell(1, 1).Value = "Customer CRM Migration Workbook";
            ws.Cell(2, 1).Value = "1. Export dữ liệu WorkTask/WorkPlan cũ ra file này.";
            ws.Cell(3, 1).Value = "2. Kiểm tra các dòng MissingCustomerReference trước khi import.";
            ws.Cell(4, 1).Value = "3. Enum có thể nhập bằng tên hoặc số.";
            ws.Cell(5, 1).Value = "4. Chạy import DryRun=true trước, chỉ ghi DB khi không còn lỗi.";
            ws.Columns().AdjustToContents();
        }

        private static void BuildInteractionTemplateSheet(XLWorkbook wb)
        {
            var ws = wb.Worksheets.Add(InteractionSheet);
            WriteHeader(ws, InteractionHeaders);
            ws.Cell(2, 1).Value = "TEMPLATE-ONLY";
            ws.Cell(2, 17).Value = "Delete this row before import. Fill this sheet only if legacy data has customer contact history.";
            ws.Columns().AdjustToContents();
        }

        private static void BuildCurrentInteractionSheet(
            XLWorkbook wb,
            List<CustomerInteraction> interactions,
            Dictionary<Guid, CustomerSnapshot> customerLookup)
        {
            var ws = wb.Worksheets.Add(InteractionSheet);
            WriteHeader(ws, InteractionHeaders);

            var row = 2;
            foreach (var interaction in interactions)
            {
                customerLookup.TryGetValue(interaction.CustomerId, out var customer);

                ws.Cell(row, 1).Value = interaction.Id.ToString();
                ws.Cell(row, 2).Value = interaction.CustomerId.ToString();
                ws.Cell(row, 3).Value = customer?.CustomerCode ?? string.Empty;
                ws.Cell(row, 4).Value = customer?.CustomerName ?? string.Empty;
                ws.Cell(row, 5).Value = interaction.ContactId?.ToString() ?? string.Empty;
                ws.Cell(row, 6).Value = interaction.InteractionType.ToString();
                ws.Cell(row, 7).Value = interaction.Subject;
                ws.Cell(row, 8).Value = interaction.Content;
                ws.Cell(row, 9).Value = interaction.Outcome;
                ws.Cell(row, 10).Value = interaction.NextAction;
                ws.Cell(row, 11).Value = interaction.InteractionAt;
                ws.Cell(row, 12).Value = interaction.NextFollowUpDate;
                ws.Cell(row, 13).Value = interaction.AssignedSaleEmployeeId?.ToString() ?? string.Empty;
                ws.Cell(row, 14).Value = interaction.CompanyId.ToString();
                ws.Cell(row, 15).Value = interaction.CreatedBy.ToString();
                ws.Cell(row, 16).Value = interaction.CreatedDate;
                ws.Cell(row, 17).Value = interaction.UpdatedBy?.ToString() ?? string.Empty;
                ws.Cell(row, 18).Value = interaction.UpdatedDate;
                ws.Cell(row, 19).Value = interaction.IsActive;
                ws.Cell(row, 20).Value = "Ready";
                row++;
            }

            ws.Columns().AdjustToContents();
        }

        private static void BuildCurrentTaskSheet(
            XLWorkbook wb,
            List<CustomerFollowUpTask> tasks,
            Dictionary<Guid, CustomerSnapshot> customerLookup)
        {
            var ws = wb.Worksheets.Add(TaskSheet);
            WriteHeader(ws, TaskHeaders);

            var row = 2;
            foreach (var task in tasks)
            {
                customerLookup.TryGetValue(task.CustomerId, out var customer);

                ws.Cell(row, 1).Value = task.Id.ToString();
                ws.Cell(row, 2).Value = task.CustomerId.ToString();
                ws.Cell(row, 3).Value = customer?.CustomerCode ?? string.Empty;
                ws.Cell(row, 4).Value = customer?.CustomerName ?? string.Empty;
                ws.Cell(row, 5).Value = task.CustomerInteractionId?.ToString() ?? string.Empty;
                ws.Cell(row, 6).Value = task.Title;
                ws.Cell(row, 7).Value = task.Description;
                ws.Cell(row, 8).Value = task.NextAction;
                ws.Cell(row, 9).Value = task.Status.ToString();
                ws.Cell(row, 10).Value = task.Priority.ToString();
                ws.Cell(row, 11).Value = task.DueDate;
                ws.Cell(row, 12).Value = task.CompletedDate;
                ws.Cell(row, 13).Value = task.CompletedBy?.ToString() ?? string.Empty;
                ws.Cell(row, 14).Value = task.CompletionNote;
                ws.Cell(row, 15).Value = task.AssignedSaleEmployeeId?.ToString() ?? string.Empty;
                ws.Cell(row, 16).Value = task.CompanyId.ToString();
                ws.Cell(row, 17).Value = task.CreatedBy.ToString();
                ws.Cell(row, 18).Value = task.CreatedDate;
                ws.Cell(row, 19).Value = task.UpdatedBy?.ToString() ?? string.Empty;
                ws.Cell(row, 20).Value = task.UpdatedDate;
                ws.Cell(row, 21).Value = task.IsActive;
                ws.Cell(row, 22).Value = "Ready";
                row++;
            }

            ws.Columns().AdjustToContents();
        }

        private static void BuildCurrentTaskAssigneeSheet(XLWorkbook wb, List<CustomerFollowUpTask> tasks)
        {
            var ws = wb.Worksheets.Add(TaskAssigneeSheet);
            WriteHeader(ws, TaskAssigneeHeaders);

            var row = 2;
            foreach (var task in tasks)
            {
                foreach (var assignee in task.Assignees)
                {
                    ws.Cell(row, 1).Value = assignee.CustomerFollowUpTaskId.ToString();
                    ws.Cell(row, 2).Value = assignee.EmployeeId.ToString();
                    ws.Cell(row, 3).Value = assignee.IsPrimary;
                    ws.Cell(row, 4).Value = assignee.CreatedBy.ToString();
                    ws.Cell(row, 5).Value = assignee.CreatedDate;
                    ws.Cell(row, 6).Value = assignee.IsActive;
                    row++;
                }
            }

            ws.Columns().AdjustToContents();
        }

        private static void BuildCurrentWorkPlanSheet(
            XLWorkbook wb,
            List<CustomerWorkPlan> plans,
            Dictionary<Guid, CustomerSnapshot> customerLookup)
        {
            var ws = wb.Worksheets.Add(WorkPlanSheet);
            WriteHeader(ws, WorkPlanHeaders);

            var row = 2;
            foreach (var plan in plans)
            {
                customerLookup.TryGetValue(plan.CustomerId, out var customer);

                ws.Cell(row, 1).Value = plan.Id.ToString();
                ws.Cell(row, 2).Value = plan.CustomerId.ToString();
                ws.Cell(row, 3).Value = customer?.CustomerCode ?? string.Empty;
                ws.Cell(row, 4).Value = customer?.CustomerName ?? string.Empty;
                ws.Cell(row, 5).Value = plan.PlanName;
                ws.Cell(row, 6).Value = plan.Objective;
                ws.Cell(row, 7).Value = plan.Strategy;
                ws.Cell(row, 8).Value = plan.DiscussionSummary;
                ws.Cell(row, 9).Value = plan.NextAction;
                ws.Cell(row, 10).Value = plan.Status.ToString();
                ws.Cell(row, 11).Value = plan.Priority.ToString();
                ws.Cell(row, 12).Value = plan.StartDate;
                ws.Cell(row, 13).Value = plan.EndDate;
                ws.Cell(row, 14).Value = plan.NextFollowUpDate;
                ws.Cell(row, 15).Value = plan.AssignedSaleEmployeeId?.ToString() ?? string.Empty;
                ws.Cell(row, 16).Value = plan.CompanyId.ToString();
                ws.Cell(row, 17).Value = plan.CreatedBy.ToString();
                ws.Cell(row, 18).Value = plan.CreatedDate;
                ws.Cell(row, 19).Value = plan.UpdatedBy?.ToString() ?? string.Empty;
                ws.Cell(row, 20).Value = plan.UpdatedDate;
                ws.Cell(row, 21).Value = plan.IsActive;
                ws.Cell(row, 22).Value = "Ready";
                row++;
            }

            ws.Columns().AdjustToContents();
        }

        private static void BuildTaskSheet(
            XLWorkbook wb,
            List<WorkTask> tasks,
            Dictionary<Guid, CustomerSnapshot> customerLookup)
        {
            var ws = wb.Worksheets.Add(TaskSheet);
            WriteHeader(ws, TaskHeaders);

            var row = 2;
            foreach (var task in tasks)
            {
                var customerId = ResolveCustomerId(task);
                customerLookup.TryGetValue(customerId ?? Guid.Empty, out var customer);

                ws.Cell(row, 1).Value = task.Id.ToString();
                ws.Cell(row, 2).Value = customerId?.ToString() ?? string.Empty;
                ws.Cell(row, 3).Value = customer?.CustomerCode ?? string.Empty;
                ws.Cell(row, 4).Value = customer?.CustomerName ?? string.Empty;
                ws.Cell(row, 5).Value = string.Empty;
                ws.Cell(row, 6).Value = task.Title;
                ws.Cell(row, 7).Value = task.Description;
                ws.Cell(row, 8).Value = task.NextAction;
                ws.Cell(row, 9).Value = MapTaskStatus(task.Status).ToString();
                ws.Cell(row, 10).Value = MapPriority(task.Priority).ToString();
                ws.Cell(row, 11).Value = task.DueDate;
                ws.Cell(row, 12).Value = task.CompletedDate;
                ws.Cell(row, 13).Value = task.CompletedBy?.ToString() ?? string.Empty;
                ws.Cell(row, 14).Value = task.CompletionNote;
                ws.Cell(row, 15).Value = task.AssignedToEmployeeId?.ToString() ?? string.Empty;
                ws.Cell(row, 16).Value = task.CompanyId.ToString();
                ws.Cell(row, 17).Value = task.CreatedBy.ToString();
                ws.Cell(row, 18).Value = task.CreatedDate;
                ws.Cell(row, 19).Value = task.UpdatedBy?.ToString() ?? string.Empty;
                ws.Cell(row, 20).Value = task.UpdatedDate;
                ws.Cell(row, 21).Value = task.IsActive;
                ws.Cell(row, 22).Value = customerId.HasValue ? "Ready" : "MissingCustomerReference";
                row++;
            }

            ws.Columns().AdjustToContents();
        }

        private static void BuildTaskAssigneeSheet(XLWorkbook wb, List<WorkTask> tasks)
        {
            var ws = wb.Worksheets.Add(TaskAssigneeSheet);
            WriteHeader(ws, TaskAssigneeHeaders);

            var row = 2;
            foreach (var task in tasks)
            {
                foreach (var assignee in task.Assignees.Where(x => x.IsActive))
                {
                    ws.Cell(row, 1).Value = task.Id.ToString();
                    ws.Cell(row, 2).Value = assignee.EmployeeId.ToString();
                    ws.Cell(row, 3).Value = assignee.IsPrimary;
                    ws.Cell(row, 4).Value = assignee.CreatedBy.ToString();
                    ws.Cell(row, 5).Value = assignee.CreatedDate;
                    ws.Cell(row, 6).Value = assignee.IsActive;
                    row++;
                }
            }

            ws.Columns().AdjustToContents();
        }

        private static void BuildWorkPlanSheet(
            XLWorkbook wb,
            List<WorkPlan> plans,
            Dictionary<Guid, CustomerSnapshot> customerLookup)
        {
            var ws = wb.Worksheets.Add(WorkPlanSheet);
            WriteHeader(ws, WorkPlanHeaders);

            var row = 2;
            foreach (var plan in plans)
            {
                var customerId = ResolveCustomerId(plan);
                customerLookup.TryGetValue(customerId ?? Guid.Empty, out var customer);

                ws.Cell(row, 1).Value = plan.Id.ToString();
                ws.Cell(row, 2).Value = customerId?.ToString() ?? string.Empty;
                ws.Cell(row, 3).Value = customer?.CustomerCode ?? string.Empty;
                ws.Cell(row, 4).Value = customer?.CustomerName ?? string.Empty;
                ws.Cell(row, 5).Value = plan.PlanName;
                ws.Cell(row, 6).Value = plan.Objective;
                ws.Cell(row, 7).Value = plan.Strategy;
                ws.Cell(row, 8).Value = plan.DiscussionSummary;
                ws.Cell(row, 9).Value = plan.NextAction;
                ws.Cell(row, 10).Value = MapWorkPlanStatus(plan.Status).ToString();
                ws.Cell(row, 11).Value = MapPriority(plan.Priority).ToString();
                ws.Cell(row, 12).Value = plan.StartDate;
                ws.Cell(row, 13).Value = plan.EndDate;
                ws.Cell(row, 14).Value = plan.NextFollowUpDate;
                ws.Cell(row, 15).Value = plan.AssignedToEmployeeId?.ToString() ?? string.Empty;
                ws.Cell(row, 16).Value = plan.CompanyId.ToString();
                ws.Cell(row, 17).Value = plan.CreatedBy.ToString();
                ws.Cell(row, 18).Value = plan.CreatedDate;
                ws.Cell(row, 19).Value = plan.UpdatedBy?.ToString() ?? string.Empty;
                ws.Cell(row, 20).Value = plan.UpdatedDate;
                ws.Cell(row, 21).Value = plan.IsActive;
                ws.Cell(row, 22).Value = customerId.HasValue ? "Ready" : "MissingCustomerReference";
                row++;
            }

            ws.Columns().AdjustToContents();
        }

        private async Task ImportInteractionsAsync(
            List<RowReader> rows,
            Dictionary<string, Guid> interactionIdByLegacyId,
            CustomerCrmMigrationImportResultDto result,
            CustomerCrmMigrationImportOptions options,
            CancellationToken ct)
        {
            foreach (var row in rows.Where(x => !x.IsTemplateRow))
            {
                var legacyId = row.Get("LegacyId");
                if (!TryGuid(row, "CustomerId", out var customerId, result)) continue;
                if (!TryGuid(row, "CompanyId", out var companyId, result)) continue;
                if (!TryGuid(row, "CreatedBy", out var createdBy, result)) continue;

                var interactionAt = row.GetDate("InteractionAt") ?? row.GetDate("CreatedDate") ?? DateTime.Now;
                var content = row.Get("Content");
                if (string.IsNullOrWhiteSpace(content))
                {
                    AddIssue(result, row, "Content không được rỗng.");
                    continue;
                }

                var exists = options.SkipExisting && await _interactionRepository.Query()
                    .AnyAsync(x => x.CustomerId == customerId
                                   && x.CompanyId == companyId
                                   && x.InteractionAt == interactionAt
                                   && x.Content == content, ct);
                if (exists)
                {
                    result.SkippedRows++;
                    continue;
                }

                var id = Guid.CreateVersion7();
                if (!string.IsNullOrWhiteSpace(legacyId))
                    interactionIdByLegacyId[legacyId] = id;

                if (!options.DryRun)
                {
                    await _interactionRepository.AddAsync(new CustomerInteraction
                    {
                        Id = id,
                        CustomerId = customerId,
                        ContactId = row.GetGuid("ContactId"),
                        InteractionType = row.GetEnum("InteractionType", CustomerInteractionType.Other),
                        Subject = row.GetNullable("Subject"),
                        Content = content,
                        Outcome = row.GetNullable("Outcome"),
                        NextAction = row.GetNullable("NextAction"),
                        InteractionAt = interactionAt,
                        NextFollowUpDate = row.GetDate("NextFollowUpDate"),
                        AssignedSaleEmployeeId = row.GetGuid("AssignedSaleEmployeeId"),
                        CompanyId = companyId,
                        CreatedBy = createdBy,
                        CreatedDate = row.GetDate("CreatedDate") ?? interactionAt,
                        UpdatedBy = row.GetGuid("UpdatedBy"),
                        UpdatedDate = row.GetDate("UpdatedDate"),
                        IsActive = row.GetBool("IsActive", true)
                    }, ct);
                }

                result.CreatedInteractions++;
            }
        }

        private async Task ImportTasksAsync(
            List<RowReader> rows,
            Dictionary<string, Guid> interactionIdByLegacyId,
            Dictionary<string, Guid> taskIdByLegacyId,
            CustomerCrmMigrationImportResultDto result,
            CustomerCrmMigrationImportOptions options,
            CancellationToken ct)
        {
            foreach (var row in rows.Where(x => !x.IsTemplateRow))
            {
                var legacyId = row.Get("LegacyId");
                if (!TryGuid(row, "CustomerId", out var customerId, result)) continue;
                if (!TryGuid(row, "CompanyId", out var companyId, result)) continue;
                if (!TryGuid(row, "CreatedBy", out var createdBy, result)) continue;

                var title = row.Get("Title");
                if (string.IsNullOrWhiteSpace(title))
                {
                    AddIssue(result, row, "Title không được rỗng.");
                    continue;
                }

                var createdDate = row.GetDate("CreatedDate") ?? DateTime.Now;
                var exists = options.SkipExisting && await _taskRepository.Query()
                    .AnyAsync(x => x.CustomerId == customerId
                                   && x.CompanyId == companyId
                                   && x.Title == title
                                   && x.CreatedDate == createdDate, ct);
                if (exists)
                {
                    result.SkippedRows++;
                    continue;
                }

                var id = Guid.CreateVersion7();
                if (!string.IsNullOrWhiteSpace(legacyId))
                    taskIdByLegacyId[legacyId] = id;

                var legacyInteractionId = row.Get("LegacyInteractionId");
                interactionIdByLegacyId.TryGetValue(legacyInteractionId, out var interactionId);

                if (!options.DryRun)
                {
                    await _taskRepository.AddAsync(new CustomerFollowUpTask
                    {
                        Id = id,
                        CustomerId = customerId,
                        CustomerInteractionId = interactionId == Guid.Empty ? null : interactionId,
                        Title = title,
                        Description = row.GetNullable("Description"),
                        NextAction = row.GetNullable("NextAction"),
                        Status = row.GetEnum("Status", CustomerFollowUpTaskStatus.Pending),
                        Priority = row.GetEnum("Priority", CustomerTaskPriority.Normal),
                        DueDate = row.GetDate("DueDate"),
                        DueReminderSentAt = null,
                        CompletedDate = row.GetDate("CompletedDate"),
                        CompletedBy = row.GetGuid("CompletedBy"),
                        CompletionNote = row.GetNullable("CompletionNote"),
                        AssignedSaleEmployeeId = row.GetGuid("AssignedSaleEmployeeId"),
                        CompanyId = companyId,
                        CreatedBy = createdBy,
                        CreatedDate = createdDate,
                        UpdatedBy = row.GetGuid("UpdatedBy"),
                        UpdatedDate = row.GetDate("UpdatedDate"),
                        IsActive = row.GetBool("IsActive", true)
                    }, ct);
                }

                result.CreatedTasks++;
            }
        }

        private async Task ImportTaskAssigneesAsync(
            List<RowReader> rows,
            Dictionary<string, Guid> taskIdByLegacyId,
            CustomerCrmMigrationImportResultDto result,
            CustomerCrmMigrationImportOptions options,
            CancellationToken ct)
        {
            foreach (var row in rows.Where(x => !x.IsTemplateRow))
            {
                var legacyTaskId = row.Get("LegacyTaskId");
                if (!taskIdByLegacyId.TryGetValue(legacyTaskId, out var taskId))
                {
                    AddIssue(result, row, $"Không tìm thấy task mới cho LegacyTaskId '{legacyTaskId}'.");
                    continue;
                }

                if (!TryGuid(row, "EmployeeId", out var employeeId, result)) continue;
                if (!TryGuid(row, "CreatedBy", out var createdBy, result)) continue;

                if (!options.DryRun)
                {
                    await _taskAssigneeRepository.AddAsync(new CustomerFollowUpTaskAssignee
                    {
                        Id = Guid.CreateVersion7(),
                        CustomerFollowUpTaskId = taskId,
                        EmployeeId = employeeId,
                        IsPrimary = row.GetBool("IsPrimary", false),
                        IsActive = row.GetBool("IsActive", true),
                        CreatedBy = createdBy,
                        CreatedDate = row.GetDate("CreatedDate") ?? DateTime.Now
                    }, ct);
                }

                result.CreatedTaskAssignees++;
            }
        }

        private async Task ImportWorkPlansAsync(
            List<RowReader> rows,
            CustomerCrmMigrationImportResultDto result,
            CustomerCrmMigrationImportOptions options,
            CancellationToken ct)
        {
            foreach (var row in rows.Where(x => !x.IsTemplateRow))
            {
                if (!TryGuid(row, "CustomerId", out var customerId, result)) continue;
                if (!TryGuid(row, "CompanyId", out var companyId, result)) continue;
                if (!TryGuid(row, "CreatedBy", out var createdBy, result)) continue;

                var planName = row.Get("PlanName");
                if (string.IsNullOrWhiteSpace(planName))
                {
                    AddIssue(result, row, "PlanName không được rỗng.");
                    continue;
                }

                var createdDate = row.GetDate("CreatedDate") ?? DateTime.Now;
                var exists = options.SkipExisting && await _workPlanRepository.Query()
                    .AnyAsync(x => x.CustomerId == customerId
                                   && x.CompanyId == companyId
                                   && x.PlanName == planName
                                   && x.CreatedDate == createdDate, ct);
                if (exists)
                {
                    result.SkippedRows++;
                    continue;
                }

                if (!options.DryRun)
                {
                    await _workPlanRepository.AddAsync(new CustomerWorkPlan
                    {
                        Id = Guid.CreateVersion7(),
                        CustomerId = customerId,
                        PlanName = planName,
                        Objective = row.GetNullable("Objective"),
                        Strategy = row.GetNullable("Strategy"),
                        DiscussionSummary = row.GetNullable("DiscussionSummary"),
                        NextAction = row.GetNullable("NextAction"),
                        Status = row.GetEnum("Status", CustomerWorkPlanStatus.Active),
                        Priority = row.GetEnum("Priority", CustomerTaskPriority.Normal),
                        StartDate = row.GetDate("StartDate"),
                        EndDate = row.GetDate("EndDate"),
                        NextFollowUpDate = row.GetDate("NextFollowUpDate"),
                        AssignedSaleEmployeeId = row.GetGuid("AssignedSaleEmployeeId"),
                        CompanyId = companyId,
                        CreatedBy = createdBy,
                        CreatedDate = createdDate,
                        UpdatedBy = row.GetGuid("UpdatedBy"),
                        UpdatedDate = row.GetDate("UpdatedDate"),
                        IsActive = row.GetBool("IsActive", true)
                    }, ct);
                }

                result.CreatedWorkPlans++;
            }
        }

        private async Task SyncCustomerSnapshotsAsync(CancellationToken ct)
        {
            var affectedCustomerIds = await _taskRepository.Query()
                .Where(x => x.IsActive)
                .Select(x => x.CustomerId)
                .Concat(_interactionRepository.Query().Where(x => x.IsActive).Select(x => x.CustomerId))
                .Distinct()
                .ToListAsync(ct);

            if (affectedCustomerIds.Count == 0)
                return;

            var customers = await _unitOfWork.CustomerRepository.Query(track: true)
                .Where(x => affectedCustomerIds.Contains(x.CustomerId))
                .ToListAsync(ct);

            foreach (var customer in customers)
            {
                customer.LastContactDate = await _interactionRepository.Query()
                    .Where(x => x.CustomerId == customer.CustomerId && x.IsActive)
                    .MaxAsync(x => (DateTime?)x.InteractionAt, ct);

                customer.NextFollowUpDate = await _taskRepository.Query()
                    .Where(x => x.CustomerId == customer.CustomerId
                                && x.IsActive
                                && x.Status != CustomerFollowUpTaskStatus.Done
                                && x.Status != CustomerFollowUpTaskStatus.Canceled
                                && x.DueDate.HasValue)
                    .MinAsync(x => (DateTime?)x.DueDate, ct);

                customer.CurrentSaleId = await _taskRepository.Query()
                    .Where(x => x.CustomerId == customer.CustomerId && x.IsActive && x.AssignedSaleEmployeeId.HasValue)
                    .OrderByDescending(x => x.UpdatedDate ?? x.CreatedDate)
                    .Select(x => x.AssignedSaleEmployeeId)
                    .FirstOrDefaultAsync(ct);
            }
        }

        private static List<RowReader> ReadSheet(
            XLWorkbook wb,
            string sheetName,
            CustomerCrmMigrationImportResultDto result)
        {
            if (!wb.TryGetWorksheet(sheetName, out var ws))
                return new List<RowReader>();

            var headerRow = ws.Row(1);
            var headers = headerRow.CellsUsed()
                .ToDictionary(
                    c => c.GetString().Trim(),
                    c => c.Address.ColumnNumber,
                    StringComparer.OrdinalIgnoreCase);

            var lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;
            var rows = new List<RowReader>();

            for (var rowNumber = 2; rowNumber <= lastRow; rowNumber++)
            {
                var row = ws.Row(rowNumber);
                if (row.CellsUsed().All(c => string.IsNullOrWhiteSpace(c.GetString())))
                    continue;

                rows.Add(new RowReader(sheetName, rowNumber, row, headers));
            }

            return rows;
        }

        private static void WriteHeader(IXLWorksheet ws, IReadOnlyList<string> headers)
        {
            for (var i = 0; i < headers.Count; i++)
                ws.Cell(1, i + 1).Value = headers[i];

            var range = ws.Range(1, 1, 1, headers.Count);
            range.Style.Font.Bold = true;
            range.Style.Fill.BackgroundColor = XLColor.LightGray;
        }

        private static bool TryGuid(
            RowReader row,
            string columnName,
            out Guid value,
            CustomerCrmMigrationImportResultDto result)
        {
            value = row.GetGuid(columnName) ?? Guid.Empty;
            if (value != Guid.Empty)
                return true;

            AddIssue(result, row, $"{columnName} không hợp lệ hoặc đang rỗng.");
            return false;
        }

        private static void AddIssue(CustomerCrmMigrationImportResultDto result, RowReader row, string message)
        {
            result.Issues.Add(new CustomerCrmMigrationRowIssueDto
            {
                Sheet = row.SheetName,
                RowNumber = row.RowNumber,
                LegacyId = row.Get("LegacyId"),
                Message = message
            });
        }

        private static Guid? ResolveCustomerId(WorkTask task)
        {
            return task.References
                .Where(x => x.ReferenceType == WorkReferenceType.Customer)
                .OrderByDescending(x => x.IsPrimary)
                .Select(x => (Guid?)x.ReferenceId)
                .FirstOrDefault();
        }

        private static Guid? ResolveCustomerId(WorkPlan plan)
        {
            return plan.References
                .Where(x => x.ReferenceType == WorkReferenceType.Customer)
                .OrderByDescending(x => x.IsPrimary)
                .Select(x => (Guid?)x.ReferenceId)
                .FirstOrDefault();
        }

        private static CustomerFollowUpTaskStatus MapTaskStatus(WorkTaskStatus status)
            => Enum.IsDefined(typeof(CustomerFollowUpTaskStatus), (int)status)
                ? (CustomerFollowUpTaskStatus)(int)status
                : CustomerFollowUpTaskStatus.Pending;

        private static CustomerTaskPriority MapPriority(LegacyTaskPriority priority)
            => Enum.IsDefined(typeof(CustomerTaskPriority), (int)priority)
                ? (CustomerTaskPriority)(int)priority
                : CustomerTaskPriority.Normal;

        private static CustomerWorkPlanStatus MapWorkPlanStatus(WorkPlanStatus status)
            => Enum.IsDefined(typeof(CustomerWorkPlanStatus), (int)status)
                ? (CustomerWorkPlanStatus)(int)status
                : CustomerWorkPlanStatus.Active;

        private static readonly string[] InteractionHeaders =
        {
            "LegacyId", "CustomerId", "CustomerExternalId", "CustomerName", "ContactId",
            "InteractionType", "Subject", "Content", "Outcome", "NextAction",
            "InteractionAt", "NextFollowUpDate", "AssignedSaleEmployeeId",
            "CompanyId", "CreatedBy", "CreatedDate", "UpdatedBy", "UpdatedDate", "IsActive", "MigrationNote"
        };

        private static readonly string[] TaskHeaders =
        {
            "LegacyId", "CustomerId", "CustomerExternalId", "CustomerName", "LegacyInteractionId",
            "Title", "Description", "NextAction", "Status", "Priority", "DueDate",
            "CompletedDate", "CompletedBy", "CompletionNote", "AssignedSaleEmployeeId",
            "CompanyId", "CreatedBy", "CreatedDate", "UpdatedBy", "UpdatedDate", "IsActive", "MigrationNote"
        };

        private static readonly string[] TaskAssigneeHeaders =
        {
            "LegacyTaskId", "EmployeeId", "IsPrimary", "CreatedBy", "CreatedDate", "IsActive"
        };

        private static readonly string[] WorkPlanHeaders =
        {
            "LegacyId", "CustomerId", "CustomerExternalId", "CustomerName", "PlanName",
            "Objective", "Strategy", "DiscussionSummary", "NextAction", "Status", "Priority",
            "StartDate", "EndDate", "NextFollowUpDate", "AssignedSaleEmployeeId",
            "CompanyId", "CreatedBy", "CreatedDate", "UpdatedBy", "UpdatedDate", "IsActive", "MigrationNote"
        };

        private sealed record CustomerSnapshot(Guid CustomerId, string CustomerCode, string CustomerName);

        private sealed class RowReader
        {
            private readonly IXLRow _row;
            private readonly Dictionary<string, int> _headers;

            public RowReader(string sheetName, int rowNumber, IXLRow row, Dictionary<string, int> headers)
            {
                SheetName = sheetName;
                RowNumber = rowNumber;
                _row = row;
                _headers = headers;
            }

            public string SheetName { get; }
            public int RowNumber { get; }
            public bool IsTemplateRow => string.Equals(Get("LegacyId"), "TEMPLATE-ONLY", StringComparison.OrdinalIgnoreCase);

            public string Get(string columnName)
            {
                return _headers.TryGetValue(columnName, out var column)
                    ? _row.Cell(column).GetString().Trim()
                    : string.Empty;
            }

            public string? GetNullable(string columnName)
            {
                var value = Get(columnName);
                return string.IsNullOrWhiteSpace(value) ? null : value;
            }

            public Guid? GetGuid(string columnName)
            {
                var value = Get(columnName);
                return Guid.TryParse(value, out var parsed) && parsed != Guid.Empty
                    ? parsed
                    : null;
            }

            public DateTime? GetDate(string columnName)
            {
                if (!_headers.TryGetValue(columnName, out var column))
                    return null;

                var cell = _row.Cell(column);
                if (cell.TryGetValue<DateTime>(out var date))
                    return date;

                var value = cell.GetString().Trim();
                if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
                    return parsed;

                return DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.None, out parsed)
                    ? parsed
                    : null;
            }

            public bool GetBool(string columnName, bool fallback)
            {
                var value = Get(columnName);
                if (string.IsNullOrWhiteSpace(value))
                    return fallback;

                if (bool.TryParse(value, out var parsed))
                    return parsed;

                if (int.TryParse(value, out var number))
                    return number != 0;

                return fallback;
            }

            public TEnum GetEnum<TEnum>(string columnName, TEnum fallback)
                where TEnum : struct, Enum
            {
                var value = Get(columnName);
                if (string.IsNullOrWhiteSpace(value))
                    return fallback;

                if (Enum.TryParse<TEnum>(value, true, out var parsed))
                    return parsed;

                return int.TryParse(value, out var number) && Enum.IsDefined(typeof(TEnum), number)
                    ? (TEnum)Enum.ToObject(typeof(TEnum), number)
                    : fallback;
            }
        }
    }
}
