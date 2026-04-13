using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Attachments.DTOs.GalleryItemFeatures.GetDtos;
using VietausWebAPI.Core.Application.Features.Attachments.Queries.GalleryItemFeatures;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;

namespace VietausWebAPI.Core.Application.Features.Attachments.RepositoriesContracts.GalleryItemFeatures
{
    public interface IImageGalleryReadRepository
    {

        Task<GetSampleRequestGalleryDetailDTO?> GetDetailBySampleRequestIdAsync(
            Guid sampleRequestId,
            CancellationToken ct);

        Task<PagedResult<GetSampleRequestSummaryPlusDTO>> GetPagedSampleRequestSummariesAsync(
            ImageGalleryQuery query,
            CancellationToken ct);

        Task<PagedResult<AttachmentModel>> GetPagedImagesAsync(
            ImageGalleryQuery query,
            CancellationToken ct);

        Task<AttachmentModel?> GetImageByIdAsync(
            Guid attachmentId,
            CancellationToken ct);
    }
}
