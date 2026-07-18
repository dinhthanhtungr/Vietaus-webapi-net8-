using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Domain.Entities.WorkTaskSchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs
{
    public partial class ApplicationDbContext
    {
        public virtual DbSet<WorkTask> WorkTasks { get; set; } = default!;
        public virtual DbSet<WorkTaskAssignee> WorkTaskAssignees { get; set; } = default!;
        public virtual DbSet<WorkTaskReference> WorkTaskReferences { get; set; } = default!;
        public virtual DbSet<WorkPlan> WorkPlans { get; set; } = default!;
        public virtual DbSet<WorkPlanAssignee> WorkPlanAssignees { get; set; } = default!;
        public virtual DbSet<WorkPlanReference> WorkPlanReferences { get; set; } = default!;
    }
}
