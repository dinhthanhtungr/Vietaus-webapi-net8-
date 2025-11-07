using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.OrderAttachment;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Enums.Attachment;

namespace VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.MerchandiseOrderFeatures
{
    //public interface IAttachmentService
    //{
    //    /// <summary>
    //    /// Tải lên tệp đính kèm cho đơn hàng
    //    /// </summary>
    //    /// <param name="orderId"></param>
    //    /// <param name="files"></param>
    //    /// <param name="slot"></param>
    //    /// <param name="CreatedBy"></param>
    //    /// <param name="ct"></param>
    //    /// <returns></returns>
    //    Task<List<OrderAttachment>> UploadAsync(Guid orderId, List<IFormFile> files, AttachmentSlot slot, Guid? CreatedBy, CancellationToken ct = default);

    //    /// <summary>
    //    /// Danh sách tệp đính kèm của đơn hàng
    //    /// </summary>
    //    /// <param name="orderId"></param>
    //    /// <param name="slot"></param>
    //    /// <param name="ct"></param>
    //    /// <returns></returns>
    //    Task<List<OrderAttachmentDTO>> ListAsync(Guid orderId, AttachmentSlot? slot, CancellationToken ct = default);

    //    /// <summary>
    //    /// Lấy nội dung tệp đính kèm
    //    /// </summary>
    //    /// <param name="attachmentId"></param>
    //    /// <param name="ct"></param>
    //    /// <returns></returns>
    //    Task<StreamResult> GetContentAsync(Guid attachmentId, CancellationToken ct = default); // xem trực tiếp

    //    /// <summary>
    //    /// Tải về tệp đính kèm
    //    /// </summary>
    //    /// <param name="attachmentId"></param>
    //    /// <param name="ct"></param>
    //    /// <returns></returns>
    //    Task<StreamResult> GetDownloadAsync(Guid attachmentId, CancellationToken ct = default); // tải về (khác tên file)

    //    /// <summary>
    //    /// Danh sách hình ảnh của đơn hàng
    //    /// </summary>
    //    /// <param name="orderId"></param>
    //    /// <param name="slot"></param>
    //    /// <param name="ct"></param>
    //    /// <returns></returns>
    //    Task<List<ImageItemDTO>> ListImagesAsync(
    //    Guid orderId, AttachmentSlot? slot, CancellationToken ct = default);

    //}
}
