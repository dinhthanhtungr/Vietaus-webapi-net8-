using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts.ReadRepositories;
using VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Warehouse.Services;
using VietausWebAPI.Infrastructure.Repositories.Warehouses;
using VietausWebAPI.Infrastructure.Repositories.Warehouses.ReadRepositories;

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


            services.AddScoped<IWarehouseVoucherReadRepository, WarehouseVoucherReadRepository>();
            services.AddScoped<IWarehouseVoucherDetailReadRepository, WarehouseVoucherDetailReadRepository>();

            // Services
            services.AddScoped<IWarehouseReadService, WarehouseReadService>();
            services.AddScoped<IWarehouseReservationService, WarehouseReservationService>();
            services.AddScoped<IWarehouseSnapshotService, WarehouseSnapshotService>();
            return services;
        }
    }
}
