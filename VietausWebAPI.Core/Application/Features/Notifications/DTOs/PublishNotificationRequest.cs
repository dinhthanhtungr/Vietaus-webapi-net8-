using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Notifications;

namespace VietausWebAPI.Core.Application.Features.Notifications.DTOs
{
    public sealed class PublishNotificationRequest
    {
        public Guid CompanyId { get; set; }
        public Guid CreatedBy { get; set; }          
        public string? CreatedByNameSnapshot { get; set; } 

        public TopicNotifications Topic { get; set; } = default!;
        public NotificationSeverity Severity { get; set; } = NotificationSeverity.Info;
        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string? Link { get; set; }
        public string? PayloadJson { get; set; }

        // Gửi ai?
        public List<Guid>? TargetUserIds { get; set; }
        public List<string>? TargetRoles { get; set; }
        public List<Guid>? TargetTeamIds { get; set; }
    }
}
