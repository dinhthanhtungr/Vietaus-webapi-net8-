using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.CompanySchema;
using VietausWebAPI.Core.Domain.Entities.HrSchema;
using VietausWebAPI.Core.Domain.Enums.WorkTaskEnums;

namespace VietausWebAPI.Core.Domain.Entities.WorkTaskSchema
{
    public class WorkPlan
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }

        public string PlanName { get; set; } = string.Empty;
        public string? Objective { get; set; }
        public string? Strategy { get; set; }
        public string? DiscussionSummary { get; set; }
        public string? NextAction { get; set; }

        public WorkPlanStatus Status { get; set; }
        public WorkTaskPriority Priority { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? NextFollowUpDate { get; set; }

        public Guid? AssignedToEmployeeId { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual Company Company { get; set; } = null!;
        public virtual Employee? AssignedToEmployee { get; set; }
        public virtual Employee CreatedByNavigation { get; set; } = null!;
        public virtual Employee? UpdatedByNavigation { get; set; }
        public virtual ICollection<WorkPlanAssignee> Assignees { get; set; } = new List<WorkPlanAssignee>();
        public virtual ICollection<WorkPlanReference> References { get; set; } = new List<WorkPlanReference>();
    }
}
