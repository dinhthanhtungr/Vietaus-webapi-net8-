using VietausWebAPI.Core.Application.Features.CompanyFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.Services;
using VietausWebAPI.Infrastructure.Repositories.Companies;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class CompanyDI
    {
        public static IServiceCollection AddCompanyModule(this IServiceCollection services)
        {
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICompanyService, CompanyService>();
            return services;
        }
    }
}
