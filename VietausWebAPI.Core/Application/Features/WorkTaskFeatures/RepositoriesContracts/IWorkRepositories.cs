using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Domain.Entities.WorkTaskSchema;

namespace VietausWebAPI.Core.Application.Features.WorkTaskFeatures.RepositoriesContracts;

public interface IWorkTaskRepository : IRepository<WorkTask> { }
public interface IWorkTaskAssigneeRepository : IRepository<WorkTaskAssignee> { }
public interface IWorkTaskReferenceRepository : IRepository<WorkTaskReference> { }
public interface IWorkPlanRepository : IRepository<WorkPlan> { }
public interface IWorkPlanAssigneeRepository : IRepository<WorkPlanAssignee> { }
public interface IWorkPlanReferenceRepository : IRepository<WorkPlanReference> { }
