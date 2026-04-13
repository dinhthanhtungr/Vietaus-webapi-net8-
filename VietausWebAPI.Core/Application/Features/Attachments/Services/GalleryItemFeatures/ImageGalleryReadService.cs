using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Attachments.DTOs.GalleryItemFeatures.GetDtos;
using VietausWebAPI.Core.Application.Features.Attachments.Queries.GalleryItemFeatures;
using VietausWebAPI.Core.Application.Features.Attachments.ServiceContracts.GalleryItemFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Attachments.Services.GalleryItemFeatures
{

    public class ImageGalleryReadService : IImageGalleryReadService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImageGalleryReadService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetSampleRequestGalleryDetailDTO?> GetDetailBySampleRequestIdAsync(
            Guid sampleRequestId,
            CancellationToken ct)
        {
            if (sampleRequestId == Guid.Empty)
                return null;

            return await _unitOfWork.ImageGalleryReadRepository
                .GetDetailBySampleRequestIdAsync(sampleRequestId, ct);
        }

        public async Task<PagedResult<GetSampleRequestSummaryPlusDTO>> GetPagedSampleRequestSummariesAsync(
            ImageGalleryQuery query,
            CancellationToken ct)
        {
            if (query.PageNumber <= 0) query.PageNumber = 1;
            if (query.PageSize <= 0) query.PageSize = 20;
            if (query.PageSize > 100) query.PageSize = 100;

            return await _unitOfWork.ImageGalleryReadRepository
                .GetPagedSampleRequestSummariesAsync(query, ct);
        }

        public async Task<PagedResult<GetImageGalleryItemDTO>> GetPagedAsync(
            ImageGalleryQuery query,
            CancellationToken ct)
        {
            if (query.PageNumber <= 0) query.PageNumber = 1;
            if (query.PageSize <= 0) query.PageSize = 20;
            if (query.PageSize > 100) query.PageSize = 100;

            var data = await _unitOfWork.ImageGalleryReadRepository.GetPagedImagesAsync(query, ct);

            return new PagedResult<GetImageGalleryItemDTO>
            (
                data.Items.Select(x => new GetImageGalleryItemDTO
                {
                    AttachmentId = x.AttachmentId,
                    AttachmentCollectionId = x.AttachmentCollectionId,
                    Slot = x.Slot,
                    FileName = x.FileName,
                    StoragePath = x.StoragePath,
                    SizeBytes = x.SizeBytes,
                    CreateDate = x.CreateDate,
                    CreateBy = x.CreateBy
                }).ToList(),
                data.TotalCount,
                query.PageNumber,
                query.PageSize
            );
        }

        public async Task<GetImageGalleryDetailDTO> GetByIdAsync(
            Guid attachmentId,
            CancellationToken ct)
        {
            var image = await _unitOfWork.ImageGalleryReadRepository.GetImageByIdAsync(attachmentId, ct)
                        ?? throw new KeyNotFoundException("Image not found.");

            return new GetImageGalleryDetailDTO
            {
                AttachmentId = image.AttachmentId,
                AttachmentCollectionId = image.AttachmentCollectionId,
                Slot = image.Slot,
                FileName = image.FileName,
                StoragePath = image.StoragePath,
                SizeBytes = image.SizeBytes,
                IsActive = image.IsActive,
                CreateDate = image.CreateDate,
                CreateBy = image.CreateBy
            };
        }
    }
    
}
