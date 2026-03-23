using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts.MFGProductionOrderFeatures;
using VietausWebAPI.Core.Application.Features.Manufacturing.Services;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.MfgGetInformationDtos;
using VietausWebAPI.Core.Application.Features.ManufacturingFeature.Services;
using VietausWebAPI.Infrastructure.Repositories.Manufacturing;
using VietausWebAPI.Core.Application.Features.Manufacturing.Services.MFGProductionOrderFeatures;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class ManufacturingDI
    {
        public static IServiceCollection AddManufacturingModule(this IServiceCollection services)
        {
            services.AddScoped<IMfgProductionOrderRepository, MfgProductionOrderRepository>();
            services.AddScoped<IMfgProductionOrderService, MfgProductionOrderService>();

            services.AddScoped<IMfgProductionOrderRWService, MfgProductionOrderRWService>();
            services.AddScoped<IMfgGetInformationService, MfgGetInformationService>();
            services.AddScoped<IMfgPostInformationService, MfgPostInformationService>();
            services.AddScoped<IMfgUpsertInformationService, MfgUpsertInformationService>();


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
