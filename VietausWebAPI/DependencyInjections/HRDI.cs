using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.HR.ServiceContracts;
using VietausWebAPI.Core.Application.Features.HR.Services;
using VietausWebAPI.Infrastructure.Repositories.HR;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class HRDI
    {
        public static IServiceCollection AddHRModule(this IServiceCollection services)
        {
            services.AddScoped<IEmployeesRepository, EmployeesRepository>();
            services.AddScoped<IEmployeesService, EmployeesService>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IMemberInGroupRepository, MemberInGroupRepository>();
            return services;
        }
    }
}
