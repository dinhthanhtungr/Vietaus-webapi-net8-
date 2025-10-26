using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.OrderAttachment;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Enums;

namespace VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.MerchandiseOrderFeatures
{
    public interface IAttachmentService
    {
        Task<List<OrderAttachment>> UploadAsync(Guid orderId, List<IFormFile> files, AttachmentSlot slot, Guid? CreatedBy, CancellationToken ct = default);
        Task<List<OrderAttachmentDTO>> ListAsync(Guid orderId, AttachmentSlot? slot, CancellationToken ct = default);

        Task<StreamResult> GetContentAsync(Guid attachmentId, CancellationToken ct = default); // xem trực tiếp
        Task<StreamResult> GetDownloadAsync(Guid attachmentId, CancellationToken ct = default); // tải về (khác tên file)

        Task<List<ImageItemDTO>> ListImagesAsync(
        Guid orderId, AttachmentSlot? slot, CancellationToken ct = default);

    }
}
