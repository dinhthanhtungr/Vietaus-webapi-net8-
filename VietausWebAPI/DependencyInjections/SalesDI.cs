using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerCrmFeatures;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.QuotationFeatures;
using VietausWebAPI.Core.Application.Features.Sales.Helpers.CustomerCrmFeatures;
using VietausWebAPI.Core.Application.Features.Sales.Helpers.QuotationFeatures;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerCrmFeatures;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.QuotationFeatures;
using VietausWebAPI.Core.Application.Features.Sales.Services.CustomerCrmFeatures;
using VietausWebAPI.Core.Application.Features.Sales.Services.CustomerCrmMigrationFeatures;
using VietausWebAPI.Core.Application.Features.Sales.Services.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.Services.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Application.Features.Sales.Services.QuotationFeatures;
using VietausWebAPI.Infrastructure.Repositories.Sales;
using VietausWebAPI.Infrastructure.Repositories.Sales.CustomerCrmFeatures;
using VietausWebAPI.Infrastructure.Repositories.Sales.QuotationFeatures;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class SalesDI
    {
        public static IServiceCollection AddSalesModule(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ITransferCustomerRepository, TransferCustomerRepository>();
            services.AddScoped<ICustomerAssignmentRepository, CustomerAssignmentRepository>();
            services.AddScoped<ICustomerTransferLogRepository, CustomerTransferLogRepository>();
            services.AddScoped<ICustomerClaimRepository, CustomerClaimRepository>();
            services.AddScoped<ICustomerNoteRepository, CustomerNoteRepository>();

            services.AddScoped<ICustomerInteractionRepository, CustomerInteractionRepository>();
            services.AddScoped<ICustomerFollowUpTaskRepository, CustomerFollowUpTaskRepository>();
            services.AddScoped<ICustomerFollowUpTaskAssigneeRepository, CustomerFollowUpTaskAssigneeRepository>();
            services.AddScoped<ICustomerWorkPlanRepository, CustomerWorkPlanRepository>();
            services.AddScoped<ICustomerInteractionAiSummaryRepository, CustomerInteractionAiSummaryRepository>();

            services.AddScoped<IMerchandiseOrderRepository, MerchandiseOrderRepository>();

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ITransferCustomerService, TransferCustomerService>();
            services.AddScoped<ICustomerCrmService, CustomerCrmService>();
            services.AddScoped<ICustomerCrmMigrationService, CustomerCrmMigrationService>();
            services.AddScoped<ICustomerCrmToWorkMigrationService, CustomerCrmToWorkMigrationService>();
            services.AddScoped<IMerchandiseOrderService, MerchandiseOrderService>();
            services.AddSingleton<IGeminiRateLimitService, GeminiRateLimitService>();
            services.AddHttpClient<IGeminiCustomerSummaryClient, GeminiCustomerSummaryClient>((sp, client) =>
            {
                var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<GeminiOptions>>().Value;
                client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds <= 0 ? 120 : options.TimeoutSeconds);
            });

            // Quoute: "Add more services and repositories as needed for the Sales module"
            services.AddScoped<IQuotationLineRepository, QuotationLineRepository>();
            services.AddScoped<IQuotationRepository, QuotationRepository>();
            services.AddScoped<IQuotationStatusHistoryRepository, QuotationStatusHistoryRepository>();
            services.AddScoped<IQuotationPdf, QuotationPdf>();
            services.AddScoped<IQuotationService, QuotationService>();


            return services;
        }
    }
}


