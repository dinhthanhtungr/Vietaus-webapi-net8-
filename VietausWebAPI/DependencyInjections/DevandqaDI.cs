using VietausWebAPI.Core.Application.Features.DeliveryOrders.Helpers;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.ServiceContracts;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.Services;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.Helpers.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.RepositoriesContracts.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.ServiceContracts.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.Services;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.Services.QCInputByQCFeatures;
using VietausWebAPI.Infrastructure.Repositories.DeliveryOrders;
using VietausWebAPI.Infrastructure.Repositories.Devandqas;
using VietausWebAPI.Infrastructure.Repositories.Devandqas.QCInputByQCFeatures;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class DevandqaDI
    {
        public static IServiceCollection AddDevandqaModule(this IServiceCollection services)
        {
            // Repos
            services.AddScoped<IProductStandardRepository, ProductStandardRepository>();
            services.AddScoped<IProductTestRepository, ProductTestRepository>();
            services.AddScoped<IProductInspectionRepository, ProductInspectionRepository>();
            services.AddScoped<IQCInputByQCRepository, QCInputByQCRepository>();
            //services.AddScoped<IQCInputByWarehouseRepository, QCInputByWarehouseRepository>();

            // Services
            services.AddScoped<IProductInspectionService, ProductInspectionService>();  
            services.AddScoped<IProductStandardService, ProductStandardService>();
            services.AddScoped<IProductTestService, ProductTestService>();
            //services.AddScoped<IQCInputByWarehouseService, QCInputByWarehouseService>();


            // QCInputByQCFeatures
            services.AddScoped<IQCInputByQCService, QCInputByQCService>();
            services.AddScoped<IExportQCInputByQCExcel, ExportQCInputByQCExcel>();



            services.AddScoped<IQCInputByQCReadRepository, QCInputByQCReadRepository>();
            services.AddScoped<IQCInputByQCWriteRepository, QCInputByQCWriteRepository>();  
            return services;
        }
    }
}
