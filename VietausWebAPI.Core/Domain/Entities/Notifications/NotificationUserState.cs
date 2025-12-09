using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.HrSchema;

namespace VietausWebAPI.Core.Domain.Entities.Notifications
{
    public class NotificationUserState
    {
        public Guid NotificationId { get; set; }
        public Notification Notification { get; set; } = default!;

        public Guid UserId { get; set; }

        public bool IsRead { get; set; } = false;
        public DateTime? ReadDate { get; set; }
        public bool IsArchived { get; set; } = false;
    
        public Employee User { get; set; } = default!;
    }
}
