using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Patchs;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Posts;
using VietausWebAPI.Core.Application.Features.Sales.Querys.CustomerCrmQuerys;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerCrmFeatures;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerCrmFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.Services.CustomerCrmFeatures
{
    /// <summary>
    /// Feature gom dữ liệu CRM thành sự kiện lịch để hiển thị kiểu calendar.
    /// </summary>
    public partial class CustomerCrmService
    {
        /// <summary>
        /// Lấy sự kiện calendar từ follow-up task, lịch sử tương tác và work plan trong một khoảng thời gian.
        /// </summary>
        public async Task<OperationResult<IReadOnlyList<CustomerCrmCalendarEventDto>>> GetCalendarEventsAsync(CustomerCrmCalendarQuery query, CancellationToken ct = default)
        {
            query ??= new CustomerCrmCalendarQuery();

            var now = DateTime.Now;
            var today = now.Date;
            var (from, toExclusive) = ResolveCalendarRange(query, now);
            var activityTypes = ResolveActivityTypes(query.ActivityTypes);
            var viewer = await BuildCrmViewerScopeAsync(ct);
            var visibleCustomerIds = BuildVisibleCustomerIdsQuery(viewer);

            var companyId = _currentUser.CompanyId;
            Guid? assignedSaleEmployeeId = ResolveScopedEmployeeId(viewer, query.AssignedSaleEmployeeId, query.OnlyMine);

            var events = new List<CustomerCrmCalendarEventDto>();

            if (activityTypes.Contains(EventTypeColorKey.FollowUpTask))
            {
                var taskQ = _followUpTaskRepository.Query()
                    .Where(x => x.CompanyId == companyId
                                && x.IsActive
                                && x.DueDate.HasValue
                                && x.DueDate.Value >= from
                                && x.DueDate.Value < toExclusive
                                && visibleCustomerIds.Contains(x.CustomerId));

                if (query.CustomerId.HasValue)
                    taskQ = taskQ.Where(x => x.CustomerId == query.CustomerId.Value);

                if (assignedSaleEmployeeId.HasValue)
                    taskQ = taskQ.Where(x => x.AssignedSaleEmployeeId == assignedSaleEmployeeId.Value || x.CreatedBy == assignedSaleEmployeeId.Value);

                if (!query.ShowCompletedTasks)
                    taskQ = taskQ.Where(x => x.Status != CustomerFollowUpTaskStatus.Done);

                if (!query.ShowDeclinedEvents)
                    taskQ = taskQ.Where(x => x.Status != CustomerFollowUpTaskStatus.Canceled);

                var taskEvents = await taskQ
                    .Select(x => new CustomerCrmCalendarEventDto
                    {
                        SourceId = x.Id,
                        SourceType = EventTypeColorKey.FollowUpTask.ToString(),
                        CustomerId = x.CustomerId,
                        CustomerExternalId = x.Customer.ExternalId,
                        CustomerName = x.Customer.CustomerName,
                        AssignedSaleEmployeeId = x.AssignedSaleEmployeeId,
                        AssignedSaleEmployeeName = x.AssignedSaleEmployee != null ? x.AssignedSaleEmployee.FullName : null,
                        Title = x.Title,
                        Description = x.NextAction ?? x.Description,
                        Start = x.DueDate!.Value,
                        Status = x.Status == CustomerFollowUpTaskStatus.Pending ? "Pending"
                            : x.Status == CustomerFollowUpTaskStatus.InProgress ? "InProgress"
                            : x.Status == CustomerFollowUpTaskStatus.Done ? "Done"
                            : x.Status == CustomerFollowUpTaskStatus.Canceled ? "Canceled"
                            : "Overdue",
                        Priority = x.Priority == CustomerFollowUpPriority.Low ? "Low"
                            : x.Priority == CustomerFollowUpPriority.Normal ? "Normal"
                            : x.Priority == CustomerFollowUpPriority.High ? "High"
                            : "Urgent",
                        IsOverdue = x.DueDate.Value.Date < today
                                    && x.Status != CustomerFollowUpTaskStatus.Done
                                    && x.Status != CustomerFollowUpTaskStatus.Canceled,
                        ColorKey = x.Status == CustomerFollowUpTaskStatus.Done ? "done"
                            : x.Status == CustomerFollowUpTaskStatus.Canceled ? "canceled"
                            : x.DueDate.Value.Date < today ? "overdue"
                            : x.Priority == CustomerFollowUpPriority.Urgent ? "urgent"
                            : x.Priority == CustomerFollowUpPriority.High ? "high"
                            : "task"
                    })
                    .ToListAsync(ct);

                events.AddRange(taskEvents);
            }

            if (activityTypes.Contains(EventTypeColorKey.Interaction))
            {
                var interactionQ = _interactionRepository.Query()
                    .Where(x => x.CompanyId == companyId
                                && x.IsActive
                                && x.InteractionAt >= from
                                && x.InteractionAt < toExclusive
                                && visibleCustomerIds.Contains(x.CustomerId));

                if (query.CustomerId.HasValue)
                    interactionQ = interactionQ.Where(x => x.CustomerId == query.CustomerId.Value);

                if (assignedSaleEmployeeId.HasValue)
                    interactionQ = interactionQ.Where(x => x.AssignedSaleEmployeeId == assignedSaleEmployeeId.Value || x.CreatedBy == assignedSaleEmployeeId.Value);

                var interactionEvents = await interactionQ
                    .Select(x => new CustomerCrmCalendarEventDto
                    {
                        SourceId = x.Id,
                        SourceType = EventTypeColorKey.Interaction.ToString(),
                        CustomerId = x.CustomerId,
                        CustomerExternalId = x.Customer.ExternalId,
                        CustomerName = x.Customer.CustomerName,
                        AssignedSaleEmployeeId = x.AssignedSaleEmployeeId,
                        AssignedSaleEmployeeName = x.AssignedSaleEmployee != null ? x.AssignedSaleEmployee.FullName : null,
                        Title = x.Subject ?? "Ghi nhận liên hệ",
                        Description = x.Content,
                        Start = x.InteractionAt,
                        Status = x.InteractionType == CustomerInteractionType.Call ? "Call"
                            : x.InteractionType == CustomerInteractionType.Meeting ? "Meeting"
                            : x.InteractionType == CustomerInteractionType.Email ? "Email"
                            : x.InteractionType == CustomerInteractionType.Zalo ? "Zalo"
                            : x.InteractionType == CustomerInteractionType.Visit ? "Visit"
                            : "Other",
                        IsOverdue = false,
                        ColorKey = x.InteractionType == CustomerInteractionType.Meeting ? "meeting"
                            : x.InteractionType == CustomerInteractionType.Visit ? "visit"
                            : x.InteractionType == CustomerInteractionType.Call ? "call"
                            : "interaction"
                    })
                    .ToListAsync(ct);

                events.AddRange(interactionEvents);
            }

            if (activityTypes.Contains(EventTypeColorKey.WorkPlan))
            {
                var workPlanQ = _workPlanRepository.Query()
                    .Where(x => x.CompanyId == companyId
                                && x.IsActive
                                && x.NextFollowUpDate.HasValue
                                && x.NextFollowUpDate.Value >= from
                                && x.NextFollowUpDate.Value < toExclusive
                                && visibleCustomerIds.Contains(x.CustomerId));

                if (query.CustomerId.HasValue)
                    workPlanQ = workPlanQ.Where(x => x.CustomerId == query.CustomerId.Value);

                if (assignedSaleEmployeeId.HasValue)
                    workPlanQ = workPlanQ.Where(x => x.AssignedSaleEmployeeId == assignedSaleEmployeeId.Value || x.CreatedBy == assignedSaleEmployeeId.Value);

                if (!query.ShowCompletedTasks)
                    workPlanQ = workPlanQ.Where(x => x.Status != CustomerWorkPlanStatus.Completed);

                if (!query.ShowDeclinedEvents)
                    workPlanQ = workPlanQ.Where(x => x.Status != CustomerWorkPlanStatus.Canceled);

                var workPlanEvents = await workPlanQ
                    .Select(x => new CustomerCrmCalendarEventDto
                    {
                        SourceId = x.Id,
                        SourceType = EventTypeColorKey.WorkPlan.ToString(),
                        CustomerId = x.CustomerId,
                        CustomerExternalId = x.Customer.ExternalId,
                        CustomerName = x.Customer.CustomerName,
                        AssignedSaleEmployeeId = x.AssignedSaleEmployeeId,
                        AssignedSaleEmployeeName = x.AssignedSaleEmployee != null ? x.AssignedSaleEmployee.FullName : null,
                        Title = x.PlanName,
                        Description = x.NextAction ?? x.Objective,
                        Start = x.NextFollowUpDate!.Value,
                        End = x.EndDate,
                        Status = x.Status == CustomerWorkPlanStatus.Draft ? "Draft"
                            : x.Status == CustomerWorkPlanStatus.Active ? "Active"
                            : x.Status == CustomerWorkPlanStatus.Paused ? "Paused"
                            : x.Status == CustomerWorkPlanStatus.Completed ? "Completed"
                            : "Canceled",
                        Priority = x.Priority == CustomerFollowUpPriority.Low ? "Low"
                            : x.Priority == CustomerFollowUpPriority.Normal ? "Normal"
                            : x.Priority == CustomerFollowUpPriority.High ? "High"
                            : "Urgent",
                        IsOverdue = x.NextFollowUpDate.Value.Date < today
                                    && x.Status != CustomerWorkPlanStatus.Completed
                                    && x.Status != CustomerWorkPlanStatus.Canceled,
                        ColorKey = x.Status == CustomerWorkPlanStatus.Completed ? "done"
                            : x.Status == CustomerWorkPlanStatus.Canceled ? "canceled"
                            : x.NextFollowUpDate.Value.Date < today ? "overdue"
                            : "work-plan"
                    })
                    .ToListAsync(ct);

                events.AddRange(workPlanEvents);
            }

            foreach (var item in events)
                item.Id = $"{item.SourceType}:{item.SourceId}";

            if (!query.ShowWeekends)
            {
                events = events
                    .Where(x => x.Start.DayOfWeek != DayOfWeek.Saturday
                                && x.Start.DayOfWeek != DayOfWeek.Sunday)
                    .ToList();
            }

            var orderedEvents = events
                .OrderBy(x => x.Start)
                .ThenBy(x => x.CustomerName)
                .ToList();

            return OperationResult<IReadOnlyList<CustomerCrmCalendarEventDto>>.Ok(orderedEvents);
        }

        private static (DateTime From, DateTime ToExclusive) ResolveCalendarRange(CustomerCrmCalendarQuery query, DateTime now)
        {
            var view = (query.View ?? "Month").Trim().ToLowerInvariant();

            var from = query.From?.Date;
            var toExclusive = query.To?.Date.AddDays(1);

            if (from.HasValue && toExclusive.HasValue && toExclusive.Value > from.Value)
                return (from.Value, toExclusive.Value);

            var today = now.Date;

            return view switch
            {
                "day" => (from ?? today, (from ?? today).AddDays(1)),
                "week" => GetWeekRange(from ?? today),
                "year" => GetYearRange(from ?? today),
                "schedule" => (from ?? today, (from ?? today).AddDays(30)),
                "4days" or "4 days" or "fourdays" => (from ?? today, (from ?? today).AddDays(4)),
                _ => GetMonthRange(from ?? today)
            };
        }

        private static (DateTime From, DateTime ToExclusive) GetWeekRange(DateTime date)
        {
            var diff = ((int)date.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            var start = date.AddDays(-diff).Date;
            return (start, start.AddDays(7));
        }

        private static (DateTime From, DateTime ToExclusive) GetMonthRange(DateTime date)
        {
            var start = new DateTime(date.Year, date.Month, 1);
            return (start, start.AddMonths(1));
        }

        private static (DateTime From, DateTime ToExclusive) GetYearRange(DateTime date)
        {
            var start = new DateTime(date.Year, 1, 1);
            return (start, start.AddYears(1));
        }
    }
}
