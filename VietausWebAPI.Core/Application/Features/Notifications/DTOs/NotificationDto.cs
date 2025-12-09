using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Notifications;

namespace VietausWebAPI.Core.Application.Features.Notifications.DTOs
{
    public class NotificationDto
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();

        public TopicNotifications Topic { get; set; } = default!;            
        public NotificationSeverity Severity { get; set; } = NotificationSeverity.Info;

        public string Title { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string? Link { get; set; }                            
        public string? PayloadJson { get; set; }                  

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid CompanyId { get; set; }

        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }

        public Guid? CreatedBy { get; set; }
        public string? CreatedByNameSnapshot { get; set; }
    }
}
