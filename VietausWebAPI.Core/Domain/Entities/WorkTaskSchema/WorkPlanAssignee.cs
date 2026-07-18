using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.HrSchema;

namespace VietausWebAPI.Core.Domain.Entities.WorkTaskSchema
{
    public class WorkPlanAssignee
    {
        public Guid Id { get; set; }

        public Guid WorkPlanId { get; set; }
        public Guid EmployeeId { get; set; }

        public bool IsPrimary { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid CreatedBy { get; set; }

        public virtual WorkPlan WorkPlan { get; set; } = null!;
        public virtual Employee Employee { get; set; } = null!;
        public virtual Employee CreatedByNavigation { get; set; } = null!;
    }
}
