using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.CompanySchema;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Entities.HrSchema;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;
using VietausWebAPI.Core.Domain.Enums.WorkTaskEnums;

namespace VietausWebAPI.Core.Domain.Entities.WorkTaskSchema
{
    public class WorkTask
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? NextAction { get; set; }

        public WorkTaskStatus Status { get; set; } = WorkTaskStatus.Pending;
        public WorkTaskPriority Priority { get; set; } = WorkTaskPriority.Normal;

        public DateTime? DueDate { get; set; }
        public DateTime? DueReminderSentAt { get; set; }
        public DateTime? CompletedDate { get; set; }
        public Guid? CompletedBy { get; set; }
        public string? CompletionNote { get; set; }

        public Guid? AssignedToEmployeeId { get; set; }

        public Guid CompanyId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual Company Company { get; set; } = null!;
        public virtual Employee? CompletedByNavigation { get; set; }
        public virtual Employee? AssignedToEmployee { get; set; }
        public virtual Employee CreatedByNavigation { get; set; } = null!;
        public virtual Employee? UpdatedByNavigation { get; set; }
        public virtual ICollection<WorkTaskAssignee> Assignees { get; set; } = new List<WorkTaskAssignee>();
        public virtual ICollection<WorkTaskReference> References { get; set; } = new List<WorkTaskReference>();
    }
}
