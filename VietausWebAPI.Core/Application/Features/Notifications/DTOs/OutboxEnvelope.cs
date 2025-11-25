using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Notifications.DTOs
{
    public sealed class OutboxEnvelope
    {
        public Guid CompanyId { get; set; }
        public Guid? NotificationId { get; set; }

        // Đích nhận (tùy chọn)
        public List<string>? TargetRoles { get; set; }
        public List<Guid>? TargetUserIds { get; set; }
        public List<Guid>? TargetTeamIds { get; set; }
        public string? TargetComposite { get; set; } // ví dụ: "LEADER+LABUSER"

        // Metadata bổ sung nếu cần
        public Dictionary<string, string>? Meta { get; set; }
    }
}
