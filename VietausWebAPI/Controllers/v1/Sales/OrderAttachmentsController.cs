//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Net.Mail;
//using System.Security.Claims;
//using VietausWebAPI.Core.Application.Features.Sales.DTOs.OrderAttachment;
//using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.MerchandiseOrderFeatures;
//using VietausWebAPI.Core.Application.Shared.Helper.ImageStorage;
//using VietausWebAPI.Core.Domain.Entities;
//using VietausWebAPI.Core.Domain.Enums.Attachment;

//namespace VietausWebAPI.WebAPI.Controllers.v1.Sales
//{
//    [ApiController]
//    [Route("api/orders/{orderId:guid}/attachments")]
//    [AllowAnonymous]
//    public class OrderAttachmentsController : Controller
//    {
//        private readonly IAttachmentService _attachmentService;

//        public OrderAttachmentsController(IAttachmentService attachmentService)
//        {
//            _attachmentService = attachmentService;
//        }

//        [HttpPost]
//        [RequestSizeLimit(50_000_000)]
//        [Consumes("multipart/form-data")]
//        [RequestFormLimits(MultipartBodyLengthLimit = 50_000_000)]

//        public async Task<IActionResult> Upload(Guid orderId, [FromQuery] AttachmentSlot slot, [FromForm] List<IFormFile> files, [FromQuery] Guid id , CancellationToken ct)
//        {
//            //Identity mặc định nhét UserId vào claim NameIdentifier (aka ClaimTypes.NameIdentifier).
//            //var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier); // string
//            //Guid.TryParse(userIdStr, out var userId);
//            var atts = await _attachmentService.UploadAsync(orderId, files, slot, id, ct);
//            return Ok(atts.Select(a => new { a.AttachmentId, a.Slot, a.FileName, a.SizeBytes, a.StoragePath, a.CreateDate }));
//        }

//        [HttpGet]
//        public Task<List<OrderAttachmentDTO>> List(Guid orderId, [FromQuery] AttachmentSlot? slot, CancellationToken ct)
//            => _attachmentService.ListAsync(orderId, slot, ct);


//        // inline (cho <img>/<iframe>)
//        [HttpGet("api/attachments/{id:guid}/content")]
//        public async Task<IActionResult> ViewInline(Guid id, CancellationToken ct)
//        {
//            var res = await _attachmentService.GetContentAsync(id, ct);
//            Response.Headers["Content-Disposition"] = $"inline; filename=\"{res.FileName}\"";
//            Response.Headers["Cache-Control"] = "public, max-age=86400";
//            return File(res.Stream, res.ContentType, enableRangeProcessing: true);
//        }


//        // download (gợi ý lưu file)
//        [HttpGet("api/attachments/{id:guid}/download")]
//        public async Task<IActionResult> Download(Guid id, CancellationToken ct)
//        {
//            var res = await _attachmentService.GetDownloadAsync(id, ct);
//            return File(res.Stream, res.ContentType, fileDownloadName: res.FileName, enableRangeProcessing: true);
//        }


//        // GET /api/orders/{orderId}/attachments/images?slot=Contract
//        [HttpGet("api/attachments/images")]
//        public Task<List<ImageItemDTO>> ListImages(
//            Guid orderId, [FromQuery] AttachmentSlot? slot, CancellationToken ct)
//            => _attachmentService.ListImagesAsync(orderId, slot, ct);
//    }
//}
