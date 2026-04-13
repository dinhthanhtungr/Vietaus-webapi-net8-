using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Attachments.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Attachments.RepositoriesContracts.GalleryItemFeatures;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public sealed partial class UnitOfWork
    {
        public IAttachmentModelRepository AttachmentModelRepository { get; }
        public IAttachmentCollectionRepository AttachmentCollectionRepository { get; }

        //==================== Gallery Item Features ====================//
        public IImageGalleryReadRepository ImageGalleryReadRepository { get; }
    }
}
