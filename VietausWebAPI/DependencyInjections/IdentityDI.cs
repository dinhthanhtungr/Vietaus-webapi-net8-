using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.HR.ServiceContracts;
using VietausWebAPI.Core.Application.Features.HR.Services;
using VietausWebAPI.Core.Application.Features.Identity.RepositoriesContracts;
using VietausWebAPI.Infrastructure.Repositories.Identity;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class IdentityDI
    {
        public static IServiceCollection AddIdentityModel(this IServiceCollection services)
        {
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IApplicationUserRoleRepository, ApplicationUserRoleRepository>();
            services.AddScoped<IApplicationRoleRepository, ApplicationRoleRepository>();
            return services;
        }
    }
}
