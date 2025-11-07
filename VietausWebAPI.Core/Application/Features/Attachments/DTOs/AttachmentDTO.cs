using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Attachment;

namespace VietausWebAPI.Core.Application.Features.Attachments.DTOs
{
    public class AttachmentDTO
    {
        public Guid AttachmentId { get; init; }
        public Guid AttachmentCollectionId { get; init; }
        public AttachmentSlot Slot { get; init; }
        public string FileName { get; init; } = default!;
        public long SizeBytes { get; init; }
        public string StoragePath { get; init; } = default!;
        public bool IsActive { get; init; }
    }
}
