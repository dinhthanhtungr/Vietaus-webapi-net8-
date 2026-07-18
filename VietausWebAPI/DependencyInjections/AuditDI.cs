using VietausWebAPI.Core.Application.Features.Audits.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.Services;
using VietausWebAPI.Infrastructure.Repositories.Audits;
using VietausWebAPI.Infrastructure.Repositories.Companies;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class AuditDI
    {
        public static IServiceCollection AddAuditModule(this IServiceCollection services)
        {
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            return services;
        }
    }
}
