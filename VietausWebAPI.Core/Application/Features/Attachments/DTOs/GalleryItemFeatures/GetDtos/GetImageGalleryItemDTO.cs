using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Attachment;

namespace VietausWebAPI.Core.Application.Features.Attachments.DTOs.GalleryItemFeatures.GetDtos
{
    public class GetImageGalleryItemDTO
    {
        public Guid AttachmentId { get; set; }
        public Guid AttachmentCollectionId { get; set; }
        public AttachmentSlot Slot { get; set; }
        public string FileName { get; set; } = default!;
        public string StoragePath { get; set; } = default!;
        public long SizeBytes { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateBy { get; set; }
    }
}
