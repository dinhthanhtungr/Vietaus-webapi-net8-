using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Formats.Jpeg;
using VietausWebAPI.Core.Application.Features.Attachments.DTOs;
using VietausWebAPI.Core.Application.Features.Attachments.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.OrderAttachment;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Domain.Enums.Attachment;
using VietausWebAPI.WebAPI.Helpers;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace VietausWebAPI.WebAPI.Controllers.v1.Attachments
{
    [ApiController]
    [Route("api/collections/{collectionId:guid}/attachmentSchemas")]
    [AllowAnonymous]
    public class CollectionAttachmentsController : Controller
    {
        private readonly IAttachmentSchemaService _attachmentSchemaService;
        private readonly ICurrentUser _CurrentUser;

        public CollectionAttachmentsController(IAttachmentSchemaService attachmentSchemaService, ICurrentUser currentUser)
        {
            _attachmentSchemaService = attachmentSchemaService;
            _CurrentUser = currentUser;
        }

        [HttpPost]
        [RequestSizeLimit(50_000_000)]
        [Consumes("multipart/form-data")]
        [RequestFormLimits(MultipartBodyLengthLimit = 50_000_000)]
        public async Task<IActionResult> UploadList(Guid collectionId, [FromQuery] AttachmentSlot slot, [FromForm] List<IFormFile> files, [FromQuery] Guid id, CancellationToken ct)
        {
            if (files is null || files.Count == 0)
                return BadRequest("No files provided.");

            var userId = _CurrentUser.EmployeeId;   // bỏ hẳn [FromQuery] id, lấy từ JWT
            var atts = await _attachmentSchemaService.UploadListAsync(collectionId, slot, files, userId, ct);

            // trả body JSON để FE đọc được
            return StatusCode(StatusCodes.Status201Created, atts);
        }

        // LIST
        [HttpGet]
        public async Task<ActionResult<List<AttachmentDTO>>> List(
            Guid collectionId, [FromQuery] AttachmentSlot? slot, CancellationToken ct)
            => Ok(await _attachmentSchemaService.ListAsync(collectionId, slot, ct));

        // DOWNLOAD / VIEW
        // mode=inline (xem) | mode=download (tải)
        [HttpGet("{attachmentId:guid}/content")]
        public async Task<IActionResult> GetContent(
            Guid attachmentId,
            [FromQuery] string mode = "inline",
            [FromQuery] string? size = null,
            CancellationToken ct = default)
        {
            var sr = await _attachmentSchemaService.GetContentAsync(attachmentId, ct);
            var isDownload = string.Equals(mode, "download", StringComparison.OrdinalIgnoreCase);
            var isThumb = string.Equals(size, "thumb", StringComparison.OrdinalIgnoreCase);

            if (isDownload)
                return File(sr.Stream, sr.ContentType, fileDownloadName: sr.FileName);

            if (isThumb && sr.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                using var image = await Image.LoadAsync(sr.Stream, ct);

                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(320, 320)
                }));

                var output = new MemoryStream();
                await image.SaveAsJpegAsync(output, new JpegEncoder
                {
                    Quality = 70
                }, ct);

                output.Position = 0;
                Response.Headers["Content-Disposition"] = $"inline; filename*=UTF-8''{Uri.EscapeDataString(sr.FileName)}";
                return File(output, "image/jpeg");
            }

            Response.Headers["Content-Disposition"] = $"inline; filename*=UTF-8''{Uri.EscapeDataString(sr.FileName)}";
            return File(sr.Stream, sr.ContentType);
        }

        // SOFT DELETE
        [HttpPatch("{attachmentId:guid}")]
        public async Task<IActionResult> Delete(Guid attachmentId, CancellationToken ct)
        {
            await _attachmentSchemaService.DeleteAsync(attachmentId, ct);
            return NoContent();
        }

        // HARD DELETE
        [HttpDelete("{attachmentId:guid}/hard")]
        public async Task<IActionResult> HardDelete(Guid attachmentId, CancellationToken ct)
        {
            await _attachmentSchemaService.HardDeleteAsync(attachmentId, ct);
            return NoContent();
        }
    }
}
