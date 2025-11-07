using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Attachment;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.OrderAttachment
{
    public record ImageItemDTO(
        Guid Id,
        string FileName,
        AttachmentSlot Slot,
        long SizeBytes,
        DateTime CreateDate,
        string ViewUrl,     // dùng cho <img src> / <iframe src>
        string DownloadUrl  // dùng để tải về
    );

}
