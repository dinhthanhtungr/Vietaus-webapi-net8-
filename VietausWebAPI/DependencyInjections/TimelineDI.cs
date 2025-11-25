using VietausWebAPI.Core.Application.Features.TimelineFeature.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.TimelineFeature.ServiceContracts;
using VietausWebAPI.Core.Application.Features.TimelineFeature.Services;
using VietausWebAPI.Infrastructure.Repositories.Audits;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class TimelineDI
    {
        public static IServiceCollection AddTimelineModule(this IServiceCollection services)
        {
            services.AddScoped<IEventLogRepository, EventLogRepository>();
            services.AddScoped<ITimelineService, TimelineService>();
            return services;
        }
    }
}
