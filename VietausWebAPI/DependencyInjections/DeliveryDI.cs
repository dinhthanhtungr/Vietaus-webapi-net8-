using VietausWebAPI.Core.Application.Features.DeliveryOrders.Helpers;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.Helpers.Excels;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.ServiceContracts;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.Services;
using VietausWebAPI.Infrastructure.Repositories.DeliveryOrders;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class DeliveryDI
    {
        public static IServiceCollection AddDeliveryModule(this IServiceCollection services)
        {
            // Repos
            services.AddScoped<IDeliveryOrderRepository, DeliveryOrderRepository>();
            services.AddScoped<IDeliveryOrderDetailRepository, DeliveryOrderDetailRepository>();
            services.AddScoped<IDelivererRepository, DelivererRepository>();
            services.AddScoped<IDelivererInforRepository, DelivererInforRepository>();
            services.AddScoped<IDeliveryOrderPORepository, DeliveryOrderPORepository>();
            // Services
            services.AddScoped<IDeliveryOrderService, DeliveryOrderService>();
            services.AddScoped<IDeliveryOrderPdfService, DeliveryOrderPdfService>();
            services.AddScoped<IDeliveryOrderPdfRenderHelper, DeliveryOrderPdfRenderHelper>();

            // Helpers
            services.AddScoped<IExportDeliveryPlan, ExportDeliveryPlan>();
            return services;
        }
    }
}
