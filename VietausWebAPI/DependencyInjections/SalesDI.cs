using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Application.Features.Sales.Services.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.Services.MerchandiseOrderFeatures;
using VietausWebAPI.Infrastructure.Repositories.Sales;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class SalesDI
    {
        public static IServiceCollection AddSalesModule(this IServiceCollection services)
        {
            // Repos
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ITransferCustomerRepository, TransferCustomerRepository>();
            services.AddScoped<ICustomerAssignmentRepository, CustomerAssignmentRepository>();
            services.AddScoped<ICustomerTransferLogRepository, CustomerTransferLogRepository>();
            services.AddScoped<IMerchandiseOrderRepository, MerchandiseOrderRepository>();
            // Services
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ITransferCustomerService, TransferCustomerService>();
            services.AddScoped<IMerchandiseOrderService, MerchandiseOrderService>();
            return services;
        }
    }
}
