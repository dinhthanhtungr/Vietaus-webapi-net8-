using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Manufacturing.Services;
using VietausWebAPI.Infrastructure.Repositories.Manufacturing;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class ManufacturingDI
    {
        public static IServiceCollection AddManufacturingModule(this IServiceCollection services)
        {
            services.AddScoped<IMfgProductionOrderRepository, MfgProductionOrderRepository>();
            services.AddScoped<IMfgProductionOrderService, MfgProductionOrderService>();

            services.AddScoped<IManufacturingFormulaRepository, ManufacturingFormulaRepository>();
            services.AddScoped<IManufacturingFormulaMaterialRepository, ManufacturingFormulaMaterialRepository>();
            services.AddScoped<IMfgFormulaService, MfgFormulaService>();

            services.AddScoped<IProductStandardFormulaRepository, ProductStandardFormulaRepository>();
            services.AddScoped<IProductionSelectVersionRepository, ProductionSelectVersionRepository>();
            services.AddScoped<IMfgOrderPORepository, MfgOrderPORepository>();
            services.AddScoped<ISchedualMfgRepository, SchedualMfgRepository>();
            // services.AddScoped<IManufacturingFormulaLogRepository, ManufacturingFormulaLogRepository>();

            return services;
        }
    }
}
