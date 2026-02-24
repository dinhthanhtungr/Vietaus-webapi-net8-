using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts.SupplierFeatures;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts.SupplierFeatures;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Services;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Services.SupplierFeatures;
using VietausWebAPI.Infrastructure.Repositories.Materials;
using VietausWebAPI.Infrastructure.Repositories.Materials.SupplierFeatures;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class MaterialDI
    {
        public static IServiceCollection AddMaterialModule(this IServiceCollection services)
        {
            services.AddScoped<IMaterialRepository, MaterialsRepository>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IMaterialsSupplierRepository, MaterialsSupplierRepository>();
            services.AddScoped<IPriceHistorieRepository, PriceHistorieRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            // ======================================================================== SupplierFeature ======================================================================== 
            services.AddScoped<ISupplierService, SupplierService>();

            services.AddScoped<ISupplierReadRepository, SupplierReadRepository>();
            services.AddScoped<ISupplierWriteRepository, SupplierWriteRepository>();
            return services;
        }
    }
}
