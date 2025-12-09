using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.HrSchema;
using VietausWebAPI.Core.Domain.Enums.Notifications;

namespace VietausWebAPI.Core.Domain.Entities.Notifications
{
    public class UserNotificationSetting
    {
        public Guid UserId { get; set; }             // PK = UserId
        public string Channels { get; set; } = "inapp";     // "inapp,email,telegram"
        public NotificationSeverity MinSeverity { get; set; } = NotificationSeverity.Info;

        public TimeSpan? QuietFrom { get; set; }     // giờ im lặng (local)
        public TimeSpan? QuietTo { get; set; }
        public string? Locale { get; set; }          // "vi-VN"
    
        public Employee User { get; set; } = default!;
    }
}
