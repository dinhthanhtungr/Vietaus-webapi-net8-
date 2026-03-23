using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.ServiceContracts.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Services.PLPUReports;
using VietausWebAPI.Infrastructure.Repositories.ReportFeatures.PLPUReports;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class FinishReportDI
    {
        public static IServiceCollection AddFinishReportModule(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IFinishPLPUReportService, FinishPLPUReportService>();

            // Repos
            services.AddScoped<IFinishPLPUReportRepository, FinishPLPUReportRepository>();
            return services;
        }
    }
}
