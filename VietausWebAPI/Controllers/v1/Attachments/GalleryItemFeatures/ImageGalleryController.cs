using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.Attachments.Queries.GalleryItemFeatures;
using VietausWebAPI.Core.Application.Features.Attachments.ServiceContracts.GalleryItemFeatures;

namespace VietausWebAPI.WebAPI.Controllers.v1.Attachments.GalleryItemFeatures
{
    [ApiController]
    [Route("api/image-gallery")]
    [AllowAnonymous]
    public class ImageGalleryController : ControllerBase
    {
        private readonly IImageGalleryReadService _imageGalleryReadService;

        public ImageGalleryController(IImageGalleryReadService imageGalleryReadService)
        {
            _imageGalleryReadService = imageGalleryReadService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] ImageGalleryQuery query,
            CancellationToken ct)
        {
            var result = await _imageGalleryReadService.GetPagedAsync(query, ct);
            return Ok(result);
        }

        [HttpGet("{sampleRequestId:guid}/detail")]
        public async Task<IActionResult> GetDetail(Guid sampleRequestId, CancellationToken ct)
        {
            var result = await _imageGalleryReadService
                .GetDetailBySampleRequestIdAsync(sampleRequestId, ct);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("sample-requests")]
        public async Task<IActionResult> GetPagedSampleRequests(
            [FromQuery] ImageGalleryQuery query,
            CancellationToken ct)
        {
            var result = await _imageGalleryReadService
                .GetPagedSampleRequestSummariesAsync(query, ct);

            return Ok(result);
        }

        [HttpGet("{attachmentId:guid}")]
        public async Task<IActionResult> GetById(
            Guid attachmentId,
            CancellationToken ct)
        {
            var result = await _imageGalleryReadService.GetByIdAsync(attachmentId, ct);
            return Ok(result);
        }
    }
}
