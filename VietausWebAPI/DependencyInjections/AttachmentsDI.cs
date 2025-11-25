using VietausWebAPI.Core.Application.Features.Attachments.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Attachments.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Attachments.Services;
using VietausWebAPI.Infrastructure.Repositories.Attachments;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class AttachmentsDI
    {
        public static IServiceCollection AddAttachmentsModule(this IServiceCollection services)
        {
            services.AddScoped<IAttachmentCollectionRepository, AttachmentCollectionRepository>();
            services.AddScoped<IAttachmentModelRepository, AttachmentModelRepository>();
            services.AddScoped<IAttachmentSchemaService, AttachmentSchemaService>();
            return services;
        }
    }
}
