using VietausWebAPI.Core.Application.Features.Planning.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Planning.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Planning.Services;
using VietausWebAPI.Infrastructure.Repositories.Planning.Schedueal;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class PlanningDI
    {
        public static IServiceCollection AddPlanningModule(this IServiceCollection services)
        {
            services.AddScoped<IScheduealRepository, ScheduealRepository>();
            services.AddScoped<IScheduealService, ScheduealService>();
          
            return services;
        }
    }
}
