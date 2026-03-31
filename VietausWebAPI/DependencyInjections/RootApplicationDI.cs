using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Infrastructure.DataUnitOfWork;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class RootApplicationDI
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Module registrations
            services
                .AddAttachmentsModule()
                .AddCompanyModule()
                .AddDevandqaModule()
                .AddExtruderOperationHistoryModule()
                .AddFinishReportModule()
                .AddHRModule()
                .AddIdentityModel()
                .AddSalesModule()
                .AddLabsModule()
                .AddPlanningModule()
                .AddMaterialModule()
                .AddManufacturingModule()
                .AddWarehouseModule()
                .AddDeliveryModule()
                .AddPurchaseModule()
                .AddTimelineModule()
                .AddNotificationsModule()   // NEW
                .AddHelpersModule();        // Helpers chung

            return services;
        }
    }
}
