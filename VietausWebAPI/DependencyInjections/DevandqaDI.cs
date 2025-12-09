using VietausWebAPI.Core.Application.Features.DeliveryOrders.Helpers;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.ServiceContracts;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.Services;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.Services;
using VietausWebAPI.Infrastructure.Repositories.DeliveryOrders;
using VietausWebAPI.Infrastructure.Repositories.Devandqas;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class DevandqaDI
    {
        public static IServiceCollection AddDevandqaModule(this IServiceCollection services)
        {
            // Repos
            services.AddScoped<IProductStandardRepository, ProductStandardRepository>();
            services.AddScoped<IProductTestRepository, ProductTestRepository>();

            // Services
            services.AddScoped<IProductStandardService, ProductStandardService>();
            services.AddScoped<IProductTestService, ProductTestService>();
            return services;
        }
    }
}
