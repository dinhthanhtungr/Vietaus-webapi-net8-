using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Warehouse.Services;
using VietausWebAPI.Infrastructure.Repositories.Warehouses;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class WarehouseDI
    {
        public static IServiceCollection AddWarehouseModule(this IServiceCollection services)
        {
            // Repos
            services.AddScoped<IWarehouseShelfStockRepository, WarehouseShelfStockRepository>();
            services.AddScoped<IWarehouseTempStockRepository, WarehouseTempStockRepository>();
            services.AddScoped<IWarehouseRequestDetailRepository, WarehouseRequestDetailRepository>();
            services.AddScoped<IWarehouseRequestRepository, WarehouseRequestRepository>();
            // Services
            services.AddScoped<IWarehouseReadService, WarehouseReadService>();
            services.AddScoped<IWarehouseReservationService, WarehouseReservationService>();
            services.AddScoped<IWarehouseSnapshotService, WarehouseSnapshotService>();
            return services;
        }
    }
}
