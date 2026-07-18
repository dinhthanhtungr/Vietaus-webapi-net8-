using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.WorkTaskFeatures.Services;
using VietausWebAPI.Infrastructure.Repositories.WorkTaskFeatures;

namespace VietausWebAPI.WebAPI.DependencyInjections;

public static class WorkManagementDI
{
    public static IServiceCollection AddWorkManagementModule(this IServiceCollection services)
    {
        services.AddScoped<IWorkTaskRepository, WorkTaskRepository>();
        services.AddScoped<IWorkTaskAssigneeRepository, WorkTaskAssigneeRepository>();
        services.AddScoped<IWorkTaskReferenceRepository, WorkTaskReferenceRepository>();
        services.AddScoped<IWorkPlanRepository, WorkPlanRepository>();
        services.AddScoped<IWorkPlanAssigneeRepository, WorkPlanAssigneeRepository>();
        services.AddScoped<IWorkPlanReferenceRepository, WorkPlanReferenceRepository>();
        services.AddScoped<IWorkManagementService, WorkManagementService>();
        return services;
    }
}
