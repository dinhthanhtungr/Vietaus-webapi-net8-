using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using VietausWebAPI.Core.Application.Features.Notifications.DTOs;
using VietausWebAPI.Core.Domain.Entities.Notifications;
using VietausWebAPI.Core.Domain.Enums.Notifications;
using VietausWebAPI.Core.Domain.Enums.WorkTaskEnums;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.WebAPI.Background;

/// <summary>
/// Scans generic work tasks that reached their due date and enqueues one reminder per task.
/// </summary>
public sealed class WorkTaskDueReminderWorker : BackgroundService
{
    private static readonly TimeSpan ScanInterval = TimeSpan.FromMinutes(1);
    private const int BatchSize = 100;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WorkTaskDueReminderWorker> _logger;

    public WorkTaskDueReminderWorker(IServiceProvider serviceProvider, ILogger<WorkTaskDueReminderWorker> logger)
        => (_serviceProvider, _logger) = (serviceProvider, logger);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Work task due reminder worker failed.");
            }

            await Task.Delay(ScanInterval, stoppingToken);
        }
    }

    private async Task ProcessAsync(CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var now = DateTime.Now;

        var tasks = await db.WorkTasks
            .Include(x => x.Assignees)
            .Include(x => x.References)
            .Where(x => x.IsActive && x.DueDate.HasValue && x.DueDate.Value <= now &&
                        x.DueReminderSentAt == null &&
                        (x.Status == WorkTaskStatus.Pending ||
                         x.Status == WorkTaskStatus.InProgress ||
                         x.Status == WorkTaskStatus.Overdue))
            .OrderBy(x => x.DueDate)
            .Take(BatchSize)
            .ToListAsync(ct);

        foreach (var task in tasks)
        {
            var targets = new HashSet<Guid> { task.CreatedBy };
            if (task.AssignedToEmployeeId.HasValue) targets.Add(task.AssignedToEmployeeId.Value);
            foreach (var assignee in task.Assignees.Where(x => x.IsActive)) targets.Add(assignee.EmployeeId);
            targets.Remove(Guid.Empty);

            if (targets.Count == 0)
                continue;

            var primaryReference = task.References.FirstOrDefault(x => x.IsPrimary);
            var referenceText = primaryReference == null
                ? string.Empty
                : $" - {primaryReference.ReferenceCodeSnapshot ?? primaryReference.ReferenceNameSnapshot}";

            var request = new PublishNotificationRequest
            {
                CompanyId = task.CompanyId,
                CreatedBy = task.CreatedBy,
                CreatedByNameSnapshot = "Work Management",
                Topic = TopicNotifications.WorkTaskDue,
                Severity = NotificationSeverity.Warning,
                Title = "Việc cần làm đến hạn",
                Message = $"{task.Title}{referenceText} đến hạn vào {task.DueDate:dd/MM/yyyy HH:mm}",
                Link = $"/work-management/tasks/{task.Id}",
                PayloadJson = JsonSerializer.Serialize(new { taskId = task.Id, task.DueDate }),
                TargetUserIds = targets.ToList()
            };

            await db.OutboxMessages.AddAsync(new OutboxMessage
            {
                Type = "Notification.Build",
                PayloadJson = JsonSerializer.Serialize(request),
                CreatedAt = now
            }, ct);

            task.DueReminderSentAt = now;
            task.UpdatedDate = now;
        }

        if (tasks.Count > 0)
            await db.SaveChangesAsync(ct);
    }
}
