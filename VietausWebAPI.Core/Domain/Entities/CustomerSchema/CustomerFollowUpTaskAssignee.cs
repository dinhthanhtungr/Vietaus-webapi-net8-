using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.HrSchema;

namespace VietausWebAPI.Core.Domain.Entities.CustomerSchema
{
    public class CustomerFollowUpTaskAssignee
    {
        public Guid Id { get; set; }
        public Guid CustomerFollowUpTaskId { get; set; }
        public Guid EmployeeId { get; set; }

        public bool IsPrimary { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }

        public virtual CustomerFollowUpTask CustomerFollowUpTask { get; set; } = null!;
        public virtual Employee Employee { get; set; } = null!;
        public virtual Employee CreatedByNavigation { get; set; } = null!;
    }
}
