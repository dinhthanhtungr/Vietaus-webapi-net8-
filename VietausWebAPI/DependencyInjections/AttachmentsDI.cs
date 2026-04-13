using VietausWebAPI.Core.Application.Features.Attachments.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Attachments.RepositoriesContracts.GalleryItemFeatures;
using VietausWebAPI.Core.Application.Features.Attachments.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Attachments.ServiceContracts.GalleryItemFeatures;
using VietausWebAPI.Core.Application.Features.Attachments.Services;
using VietausWebAPI.Core.Application.Features.Attachments.Services.GalleryItemFeatures;
using VietausWebAPI.Infrastructure.Repositories.Attachments;
using VietausWebAPI.Infrastructure.Repositories.Attachments.GalleryItemFeatures;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class AttachmentsDI
    {
        public static IServiceCollection AddAttachmentsModule(this IServiceCollection services)
        {
            services.AddScoped<IAttachmentCollectionRepository, AttachmentCollectionRepository>();
            services.AddScoped<IAttachmentModelRepository, AttachmentModelRepository>();
            services.AddScoped<IAttachmentSchemaService, AttachmentSchemaService>();

            // => GalleryItemFeatures
            services.AddScoped<IImageGalleryReadRepository, ImageGalleryReadRepository>();

            services.AddScoped<IImageGalleryReadService, ImageGalleryReadService>();

            return services;
        }
    }
}
