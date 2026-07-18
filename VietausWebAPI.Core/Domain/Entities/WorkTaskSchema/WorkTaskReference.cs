using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.WorkTaskEnums;

namespace VietausWebAPI.Core.Domain.Entities.WorkTaskSchema
{
    public class WorkTaskReference
    {
        public Guid Id { get; set; }

        public Guid WorkTaskId { get; set; }

        public WorkReferenceType ReferenceType { get; set; }

        public Guid ReferenceId { get; set; }

        public string? ReferenceCodeSnapshot { get; set; }
        public string? ReferenceNameSnapshot { get; set; }

        public bool IsPrimary { get; set; }

        public virtual WorkTask WorkTask { get; set; } = null!;
    }
}
