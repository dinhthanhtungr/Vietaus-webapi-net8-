using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.WorkTaskSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.WorkTaskFeatures;

public sealed class WorkTaskRepository : Repository<WorkTask>, IWorkTaskRepository
{
    public WorkTaskRepository(ApplicationDbContext context) : base(context) { }
}

public sealed class WorkTaskAssigneeRepository : Repository<WorkTaskAssignee>, IWorkTaskAssigneeRepository
{
    public WorkTaskAssigneeRepository(ApplicationDbContext context) : base(context) { }
}

public sealed class WorkTaskReferenceRepository : Repository<WorkTaskReference>, IWorkTaskReferenceRepository
{
    public WorkTaskReferenceRepository(ApplicationDbContext context) : base(context) { }
}

public sealed class WorkPlanRepository : Repository<WorkPlan>, IWorkPlanRepository
{
    public WorkPlanRepository(ApplicationDbContext context) : base(context) { }
}

public sealed class WorkPlanAssigneeRepository : Repository<WorkPlanAssignee>, IWorkPlanAssigneeRepository
{
    public WorkPlanAssigneeRepository(ApplicationDbContext context) : base(context) { }
}

public sealed class WorkPlanReferenceRepository : Repository<WorkPlanReference>, IWorkPlanReferenceRepository
{
    public WorkPlanReferenceRepository(ApplicationDbContext context) : base(context) { }
}
