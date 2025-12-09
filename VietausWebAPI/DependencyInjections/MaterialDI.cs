using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Services;
using VietausWebAPI.Infrastructure.Repositories.Materials;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class MaterialDI
    {
        public static IServiceCollection AddMaterialModule(this IServiceCollection services)
        {
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IMaterialRepository, MaterialsRepository>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IMaterialsSupplierRepository, MaterialsSupplierRepository>();
            services.AddScoped<IPriceHistorieRepository, PriceHistorieRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            return services;
        }
    }
}
