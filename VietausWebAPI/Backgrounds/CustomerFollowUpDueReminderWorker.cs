using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using VietausWebAPI.Core.Application.Features.Notifications.DTOs;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Entities.Notifications;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;
using VietausWebAPI.Core.Domain.Enums.Notifications;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.WebAPI.Background
{
    /// <summary>
    /// Scans active CRM follow-up tasks that have reached their due date and enqueues notification build messages.
    /// Each task is marked with DueReminderSentAt after an outbox message is created to avoid repeated reminders.
    /// </summary>
    public sealed class CustomerFollowUpDueReminderWorker : BackgroundService
    {
        private static readonly TimeSpan ScanInterval = TimeSpan.FromMinutes(1);
        private const int BatchSize = 100;

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CustomerFollowUpDueReminderWorker> _logger;

        public CustomerFollowUpDueReminderWorker(
            IServiceProvider serviceProvider,
            ILogger<CustomerFollowUpDueReminderWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessDueTasksAsync(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Customer follow-up due reminder worker failed.");
                }

                await Task.Delay(ScanInterval, stoppingToken);
            }
        }

        private async Task ProcessDueTasksAsync(CancellationToken ct)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var now = DateTime.Now;

            var dueTasks = await db.CustomerFollowUpTasks
                .Include(x => x.Customer)
                .Include(x => x.Assignees)
                .Where(x =>
                    x.IsActive &&
                    x.DueDate.HasValue &&
                    x.DueDate.Value <= now &&
                    x.DueReminderSentAt == null &&
                    (x.Status == CustomerFollowUpTaskStatus.Pending ||
                     x.Status == CustomerFollowUpTaskStatus.InProgress ||
                     x.Status == CustomerFollowUpTaskStatus.Overdue))
                .OrderBy(x => x.DueDate)
                .Take(BatchSize)
                .ToListAsync(ct);

            if (dueTasks.Count == 0)
                return;

            foreach (var task in dueTasks)
            {
                var targetUserIds = BuildTargetUserIds(task.AssignedSaleEmployeeId, task.CreatedBy, task.Assignees);
                if (targetUserIds.Count == 0)
                    continue;

                var request = BuildNotificationRequest(task, targetUserIds);

                await db.OutboxMessages.AddAsync(new OutboxMessage
                {
                    Type = "Notification.Build",
                    PayloadJson = JsonSerializer.Serialize(request),
                    CreatedAt = now
                }, ct);

                task.DueReminderSentAt = now;
                task.UpdatedDate = now;
            }

            try
            {
                await db.SaveChangesAsync(ct);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Skipped CRM follow-up due reminder batch because one or more tasks were changed by another request.");
            }
        }

        private static List<Guid> BuildTargetUserIds(
            Guid? assignedSaleEmployeeId,
            Guid createdBy,
            IEnumerable<CustomerFollowUpTaskAssignee> assignees)
        {
            var targetUserIds = new HashSet<Guid>();

            if (assignedSaleEmployeeId.HasValue && assignedSaleEmployeeId.Value != Guid.Empty)
                targetUserIds.Add(assignedSaleEmployeeId.Value);

            if (createdBy != Guid.Empty)
                targetUserIds.Add(createdBy);

            foreach (var assignee in assignees.Where(x => x.IsActive && x.EmployeeId != Guid.Empty))
                targetUserIds.Add(assignee.EmployeeId);

            return targetUserIds.ToList();
        }

        private static PublishNotificationRequest BuildNotificationRequest(
            CustomerFollowUpTask task,
            List<Guid> targetUserIds)
        {
            var customerName = string.IsNullOrWhiteSpace(task.Customer.CustomerName)
                ? task.Customer.ExternalId
                : task.Customer.CustomerName;

            var dueText = task.DueDate.HasValue
                ? task.DueDate.Value.ToString("dd/MM/yyyy HH:mm")
                : string.Empty;

            var payloadJson = JsonSerializer.Serialize(new
            {
                taskId = task.Id,
                customerId = task.CustomerId,
                customerCode = task.Customer.ExternalId,
                dueDate = task.DueDate,
                assignedSaleEmployeeId = task.AssignedSaleEmployeeId
            });

            return new PublishNotificationRequest
            {
                CompanyId = task.CompanyId,
                CreatedBy = task.CreatedBy,
                CreatedByNameSnapshot = "CRM",
                Topic = TopicNotifications.CustomerFollowUpTaskDue,
                Severity = NotificationSeverity.Warning,
                Title = "Việc cần làm",
                Message = $"{task.Title} - {customerName} hết hạng vào {dueText}",
                Link = $"/sales/customer-crm?customerId={task.CustomerId}&taskId={task.Id}&mode=activity",
                PayloadJson = payloadJson,
                TargetUserIds = targetUserIds
            };
        }
    }
}
