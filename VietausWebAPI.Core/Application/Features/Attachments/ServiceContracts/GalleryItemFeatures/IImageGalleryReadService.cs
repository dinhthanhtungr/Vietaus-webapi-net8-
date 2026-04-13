using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Attachments.DTOs.GalleryItemFeatures.GetDtos;
using VietausWebAPI.Core.Application.Features.Attachments.Queries.GalleryItemFeatures;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Attachments.ServiceContracts.GalleryItemFeatures
{
    public interface IImageGalleryReadService
    {
        Task<GetSampleRequestGalleryDetailDTO?> GetDetailBySampleRequestIdAsync(
            Guid sampleRequestId,
            CancellationToken ct);

        Task<PagedResult<GetSampleRequestSummaryPlusDTO>> GetPagedSampleRequestSummariesAsync(
            ImageGalleryQuery query,
            CancellationToken ct);

        Task<PagedResult<GetImageGalleryItemDTO>> GetPagedAsync(
            ImageGalleryQuery query,
            CancellationToken ct);

        Task<GetImageGalleryDetailDTO> GetByIdAsync(
            Guid attachmentId,
            CancellationToken ct);
    }
}
