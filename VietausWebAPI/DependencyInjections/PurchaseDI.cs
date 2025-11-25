using VietausWebAPI.Core.Application.Features.PurchaseFeatures.Helpers;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.Services;
using VietausWebAPI.Infrastructure.Repositories.Purchases;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class PurchaseDI
    {
        public static IServiceCollection AddPurchaseModule(this IServiceCollection services)
        {
            // Repos
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
            services.AddScoped<IPurchaseOrderDetailRepository, PurchaseOrderDetailRepository>();
            services.AddScoped<IPurchaseOrderSnapshotRepository, PurchaseOrderSnapshotRepository>();
            services.AddScoped<IPurchaseOrderLinkRepository, PurchaseOrderLinkRepository>();

            // Services & helpers
            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            services.AddScoped<IPurchaseOrderPdfService, PurchaseOrderPdfService>();
            services.AddScoped<IPurchaseOrderPdfRenderHelper, PurchaseOrderPdfRenderHelper>();
            return services;
        }
    }
}
