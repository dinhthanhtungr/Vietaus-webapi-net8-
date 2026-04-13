using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Attachments.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Attachments.RepositoriesContracts.GalleryItemFeatures;

namespace VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        IAttachmentModelRepository AttachmentModelRepository { get; }
        IAttachmentCollectionRepository AttachmentCollectionRepository { get; }

        //==================== Gallery Item Features ====================//
        IImageGalleryReadRepository ImageGalleryReadRepository   { get; }

    }
}
