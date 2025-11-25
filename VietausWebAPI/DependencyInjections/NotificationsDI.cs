using VietausWebAPI.Core.Application.Features.Notifications.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Notifications.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Notifications.Services;
using VietausWebAPI.Infrastructure.Repositories.Notifications;
using VietausWebAPI.WebAPI.Background;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class NotificationsDI
    {
        public static IServiceCollection AddNotificationsModule(this IServiceCollection services)
        {
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationRecipientRepository, NotificationRecipientRepository>();
            services.AddScoped<INotificationUserStateRepository, NotificationUserStateRepository>();
            services.AddScoped<IUserNotificationSettingRepository, UserNotificationSettingRepository>();
            services.AddScoped<INotificationTemplateRepository, NotificationTemplateRepository>();
            services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();

            // Service + background jobs:
            services.AddScoped<INotificationService, NotificationService>();     // service app
            services.AddHostedService<OutboxProcessor>();                        // worker đẩy

            return services;
        }
    }
}
