using VietausWebAPI.Core.Domain.Entities.CompanySchema;
using VietausWebAPI.Core.Domain.Entities.HrSchema;

namespace VietausWebAPI.Core.Domain.Entities.Notifications
{
    public class WebPushSubscription
    {
        public Guid WebPushSubscriptionId { get; set; }

        public Guid CompanyId { get; set; }
        public Guid EmployeeId { get; set; }

        public string Endpoint { get; set; } = string.Empty;
        public string P256dh { get; set; } = string.Empty;
        public string Auth { get; set; } = string.Empty;

        public string? DeviceName { get; set; }
        public string? UserAgent { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public DateTime? LastSuccessAt { get; set; }
        public DateTime? LastFailureAt { get; set; }
        public int FailureCount { get; set; }

        public virtual Company Company { get; set; } = default!;
        public virtual Employee Employee { get; set; } = default!;
    }
}
