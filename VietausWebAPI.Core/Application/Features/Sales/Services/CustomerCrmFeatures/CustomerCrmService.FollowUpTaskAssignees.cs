using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using VietausWebAPI.Core.Application.Features.Notifications.DTOs;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Posts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Entities.HrSchema;
using VietausWebAPI.Core.Domain.Entities.Notifications;
using VietausWebAPI.Core.Domain.Enums.Notifications;
using VietausWebAPI.Core.Domain.Enums.Visibilitys;
using VietausWebAPI.WebAPI.Helpers.Securities.Roles;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.CustomerCrmFeatures
{
    /// <summary>
    /// Feature for assigning support employees to follow CRM follow-up tasks.
    /// </summary>
    public partial class CustomerCrmService
    {
        /// <summary>
        /// Adds an employee as a support assignee for a follow-up task and notifies that employee.
        /// </summary>
        public async Task<OperationResult<CustomerFollowUpTaskAssigneeDto>> AddFollowUpTaskAssigneeAsync(
            Guid taskId,
            AddCustomerFollowUpTaskAssigneeRequest request,
            CancellationToken ct = default)
        {
            if (request.EmployeeId == Guid.Empty)
                return OperationResult<CustomerFollowUpTaskAssigneeDto>.Fail("EmployeeId is required.");

            var now = DateTime.Now;
            var currentEmployeeId = _currentUser.EmployeeId;
            var companyId = _currentUser.CompanyId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var task = await _followUpTaskRepository.Query(track: false)
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x =>
                    x.Id == taskId &&
                    x.CompanyId == companyId &&
                    x.IsActive &&
                    visibleCustomerIds.Contains(x.CustomerId), ct);

            if (task == null)
                return OperationResult<CustomerFollowUpTaskAssigneeDto>.Fail("Follow-up task was not found.");

            var scopedEmployeeId = ResolveScopedEmployeeId(viewer, request.EmployeeId, onlyMine: false);
            if (scopedEmployeeId == Guid.Empty)
                return OperationResult<CustomerFollowUpTaskAssigneeDto>.Fail("You cannot assign an employee outside your CRM scope.");

            var employee = await _unitOfWork.EmployeesRepository.Query(track: false)
                .FirstOrDefaultAsync(x =>
                    x.EmployeeId == request.EmployeeId &&
                    x.CompanyId == companyId &&
                    x.IsActive, ct);

            if (employee == null)
                return OperationResult<CustomerFollowUpTaskAssigneeDto>.Fail("Employee was not found.");

            if (request.IsPrimary)
                await ClearPrimaryAssigneesAsync(task.Id, ct);

            var (assignee, shouldNotify) = await AddOrReactivateAssigneeAsync(
                task.Id,
                request.EmployeeId,
                request.IsPrimary,
                now,
                currentEmployeeId,
                ct);

            if (shouldNotify)
            {
                await EnqueueAssigneeAddedNotificationAsync(
                    task,
                    new List<Guid> { employee.EmployeeId },
                    request.NotifySaleAdmin,
                    request.NotifyLeader,
                    now,
                    ct);
            }

            try
            {
                await _unitOfWork.SaveChangesAsync(ct);
            }
            catch (DbUpdateConcurrencyException)
            {
                return OperationResult<CustomerFollowUpTaskAssigneeDto>.Fail("Task assignee was changed by another request. Please reload and try again.");
            }

            return OperationResult<CustomerFollowUpTaskAssigneeDto>.Ok(ToAssigneeDto(assignee, employee), "Assignee was added.");
        }

        /// <summary>
        /// Adds support assignees for a follow-up task by resolving active employees from a company group.
        /// </summary>
        public async Task<OperationResult<IReadOnlyList<CustomerFollowUpTaskAssigneeDto>>> AddFollowUpTaskGroupAssigneesAsync(
            Guid taskId,
            AddCustomerFollowUpTaskGroupAssigneesRequest request,
            CancellationToken ct = default)
        {
            if (request.GroupId == Guid.Empty)
                return OperationResult<IReadOnlyList<CustomerFollowUpTaskAssigneeDto>>.Fail("GroupId is required.");

            if (!request.AssignMembers && !request.AssignLeader)
                return OperationResult<IReadOnlyList<CustomerFollowUpTaskAssigneeDto>>.Fail("Select AssignMembers or AssignLeader.");

            var now = DateTime.Now;
            var currentEmployeeId = _currentUser.EmployeeId;
            var companyId = _currentUser.CompanyId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var task = await _followUpTaskRepository.Query(track: false)
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x =>
                    x.Id == taskId &&
                    x.CompanyId == companyId &&
                    x.IsActive &&
                    visibleCustomerIds.Contains(x.CustomerId), ct);

            if (task == null)
                return OperationResult<IReadOnlyList<CustomerFollowUpTaskAssigneeDto>>.Fail("Follow-up task was not found.");

            var isFullScope = viewer.ScopeType is ViewerScopeType.AdminFull or ViewerScopeType.LabFull;
            if (!isFullScope && viewer.GroupId != request.GroupId)
                return OperationResult<IReadOnlyList<CustomerFollowUpTaskAssigneeDto>>.Fail("You cannot assign employees from another group.");

            var groupExists = await _unitOfWork.GroupRepository.Query()
                .AnyAsync(x =>
                    x.GroupId == request.GroupId &&
                    x.CompanyId == companyId, ct);

            if (!groupExists)
                return OperationResult<IReadOnlyList<CustomerFollowUpTaskAssigneeDto>>.Fail("Group was not found.");

            var employeeIds = await _unitOfWork.MemberInGroupRepository.Query()
                .Where(x =>
                    x.GroupId == request.GroupId &&
                    x.IsActive &&
                    x.Profile.HasValue &&
                    (request.AssignMembers || (request.AssignLeader && x.IsAdmin == true)))
                .Select(x => x.Profile!.Value)
                .Distinct()
                .ToListAsync(ct);

            if (!isFullScope)
            {
                employeeIds = employeeIds
                    .Where(x => viewer.EmployeeIdsInScope.Contains(x))
                    .ToList();
            }

            if (employeeIds.Count == 0)
                return OperationResult<IReadOnlyList<CustomerFollowUpTaskAssigneeDto>>.Fail("No active employees were found for the selected group option.");

            var employees = await _unitOfWork.EmployeesRepository.Query(track: false)
                .Where(x =>
                    employeeIds.Contains(x.EmployeeId) &&
                    x.CompanyId == companyId &&
                    x.IsActive)
                .OrderBy(x => x.FullName)
                .ToListAsync(ct);

            if (employees.Count == 0)
                return OperationResult<IReadOnlyList<CustomerFollowUpTaskAssigneeDto>>.Fail("No active employees were found.");

            var result = new List<CustomerFollowUpTaskAssigneeDto>();
            var notifyEmployeeIds = new List<Guid>();

            foreach (var employee in employees)
            {
                var (assignee, shouldNotify) = await AddOrReactivateAssigneeAsync(
                    task.Id,
                    employee.EmployeeId,
                    isPrimary: false,
                    now,
                    currentEmployeeId,
                    ct);

                if (shouldNotify)
                    notifyEmployeeIds.Add(employee.EmployeeId);

                result.Add(ToAssigneeDto(assignee, employee));
            }

            if (notifyEmployeeIds.Count > 0)
            {
                await EnqueueAssigneeAddedNotificationAsync(
                    task,
                    notifyEmployeeIds,
                    request.NotifySaleAdmin,
                    request.NotifyLeader,
                    now,
                    ct);
            }

            try
            {
                await _unitOfWork.SaveChangesAsync(ct);
            }
            catch (DbUpdateConcurrencyException)
            {
                return OperationResult<IReadOnlyList<CustomerFollowUpTaskAssigneeDto>>.Fail("Task assignee was changed by another request. Please reload and try again.");
            }

            return OperationResult<IReadOnlyList<CustomerFollowUpTaskAssigneeDto>>.Ok(result, "Group assignees were added.");
        }

        /// <summary>
        /// Soft-removes an employee from the support assignee list of a follow-up task.
        /// </summary>
        public async Task<OperationResult> RemoveFollowUpTaskAssigneeAsync(
            Guid taskId,
            Guid employeeId,
            CancellationToken ct = default)
        {
            var companyId = _currentUser.CompanyId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var task = await _followUpTaskRepository.Query(track: false)
                .FirstOrDefaultAsync(x =>
                    x.Id == taskId &&
                    x.CompanyId == companyId &&
                    x.IsActive &&
                    visibleCustomerIds.Contains(x.CustomerId), ct);

            if (task == null)
                return OperationResult.Fail("Follow-up task was not found.");

            var assignee = await _followUpTaskAssigneeRepository.Query(track: true)
                .FirstOrDefaultAsync(x =>
                    x.CustomerFollowUpTaskId == task.Id &&
                    x.EmployeeId == employeeId &&
                    x.IsActive, ct);

            if (assignee == null)
                return OperationResult.Fail("Assignee was not found.");

            assignee.IsActive = false;
            assignee.IsPrimary = false;

            try
            {
                await _unitOfWork.SaveChangesAsync(ct);
            }
            catch (DbUpdateConcurrencyException)
            {
                return OperationResult.Fail("Task assignee was changed by another request. Please reload and try again.");
            }

            return OperationResult.Ok("Assignee was removed.");
        }

        /// <summary>
        /// Gets the support assignees of a follow-up task.
        /// </summary>
        public async Task<OperationResult<IReadOnlyList<CustomerFollowUpTaskAssigneeDto>>> GetFollowUpTaskAssigneesAsync(
            Guid taskId,
            CancellationToken ct = default)
        {
            var companyId = _currentUser.CompanyId;
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var taskExists = await _followUpTaskRepository.Query(track: false)
                .AnyAsync(x =>
                    x.Id == taskId &&
                    x.CompanyId == companyId &&
                    x.IsActive &&
                    visibleCustomerIds.Contains(x.CustomerId), ct);

            if (!taskExists)
                return OperationResult<IReadOnlyList<CustomerFollowUpTaskAssigneeDto>>.Fail("Follow-up task was not found.");

            var items = await _followUpTaskAssigneeRepository.Query(track: false)
                .Where(x => x.CustomerFollowUpTaskId == taskId && x.IsActive)
                .OrderByDescending(x => x.IsPrimary)
                .ThenBy(x => x.Employee.FullName)
                .Select(x => new CustomerFollowUpTaskAssigneeDto
                {
                    Id = x.Id,
                    CustomerFollowUpTaskId = x.CustomerFollowUpTaskId,
                    EmployeeId = x.EmployeeId,
                    EmployeeCode = x.Employee.ExternalId,
                    EmployeeName = x.Employee.FullName,
                    IsPrimary = x.IsPrimary,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync(ct);

            return OperationResult<IReadOnlyList<CustomerFollowUpTaskAssigneeDto>>.Ok(items);
        }

        private async Task<(CustomerFollowUpTaskAssignee Assignee, bool ShouldNotify)> AddOrReactivateAssigneeAsync(
            Guid taskId,
            Guid employeeId,
            bool isPrimary,
            DateTime now,
            Guid currentEmployeeId,
            CancellationToken ct)
        {
            var assignee = await _followUpTaskAssigneeRepository.Query(track: true)
                .FirstOrDefaultAsync(x =>
                    x.CustomerFollowUpTaskId == taskId &&
                    x.EmployeeId == employeeId, ct);

            if (assignee == null)
            {
                assignee = new CustomerFollowUpTaskAssignee
                {
                    Id = Guid.CreateVersion7(),
                    CustomerFollowUpTaskId = taskId,
                    EmployeeId = employeeId,
                    IsPrimary = isPrimary,
                    IsActive = true,
                    CreatedDate = now,
                    CreatedBy = currentEmployeeId
                };

                await _followUpTaskAssigneeRepository.AddAsync(assignee, ct);
                return (assignee, true);
            }

            if (!assignee.IsActive)
            {
                assignee.IsActive = true;
                assignee.IsPrimary = isPrimary;
                assignee.CreatedDate = now;
                assignee.CreatedBy = currentEmployeeId;
                return (assignee, true);
            }

            assignee.IsPrimary = isPrimary;
            return (assignee, false);
        }

        private async Task ClearPrimaryAssigneesAsync(Guid taskId, CancellationToken ct)
        {
            var primaryAssignees = await _followUpTaskAssigneeRepository.Query(track: true)
                .Where(x =>
                    x.CustomerFollowUpTaskId == taskId &&
                    x.IsActive &&
                    x.IsPrimary)
                .ToListAsync(ct);

            foreach (var existing in primaryAssignees)
                existing.IsPrimary = false;
        }

        private async Task EnqueueAssigneeAddedNotificationAsync(
            CustomerFollowUpTask task,
            IReadOnlyCollection<Guid> targetEmployeeIds,
            bool notifySaleAdmin,
            bool notifyLeader,
            DateTime now,
            CancellationToken ct)
        {
            var targetRoles = BuildAssigneeAddedTargetRoles(notifySaleAdmin, notifyLeader);

            var payloadJson = JsonSerializer.Serialize(new
            {
                taskId = task.Id,
                customerId = task.CustomerId,
                customerCode = task.Customer.ExternalId,
                assignedByEmployeeId = _currentUser.EmployeeId,
                supportEmployeeIds = targetEmployeeIds,
                dueDate = task.DueDate,
                notifySaleAdmin,
                notifyLeader
            });

            var notificationRequest = new PublishNotificationRequest
            {
                CompanyId = task.CompanyId,
                CreatedBy = _currentUser.EmployeeId,
                CreatedByNameSnapshot = _currentUser.personName,
                Topic = TopicNotifications.CustomerFollowUpTaskAssigneeAdded,
                Severity = NotificationSeverity.Info,
                Title = $"{_currentUser.personName} yêu cầu hỗ trợ",
                Message = $"{_currentUser.personName} yêu cầu hỗ trợ cho tác vụ: {task.Title} từ khách hàng [{task.Customer.ExternalId}] {task.Customer.CustomerName}",
                Link = $"/sales/customer-crm?customerId={task.CustomerId}&taskId={task.Id}",
                PayloadJson = payloadJson,
                TargetUserIds = targetEmployeeIds.ToList(),
                TargetRoles = targetRoles
            };

            await _unitOfWork.OutboxMessages.AddAsync(new OutboxMessage
            {
                Type = "Notification.Build",
                PayloadJson = JsonSerializer.Serialize(notificationRequest),
                CreatedAt = now
            }, ct);
        }

        private static List<string>? BuildAssigneeAddedTargetRoles(bool notifySaleAdmin, bool notifyLeader)
        {
            var roles = new List<string>();

            if (notifySaleAdmin)
                roles.Add(AppRoles.SaleAdmin);

            if (notifyLeader)
                roles.Add(AppRoles.Leader);

            return roles.Count == 0 ? null : roles;
        }

        private static CustomerFollowUpTaskAssigneeDto ToAssigneeDto(
            CustomerFollowUpTaskAssignee assignee,
            Employee employee)
        {
            return new CustomerFollowUpTaskAssigneeDto
            {
                Id = assignee.Id,
                CustomerFollowUpTaskId = assignee.CustomerFollowUpTaskId,
                EmployeeId = employee.EmployeeId,
                EmployeeCode = employee.ExternalId,
                EmployeeName = employee.FullName,
                IsPrimary = assignee.IsPrimary,
                IsActive = assignee.IsActive,
                CreatedDate = assignee.CreatedDate
            };
        }
    }
}
