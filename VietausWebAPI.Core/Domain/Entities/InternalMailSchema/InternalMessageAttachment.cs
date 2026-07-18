using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;

namespace VietausWebAPI.Core.Domain.Entities.InternalMailSchema
{
    public class InternalMessageAttachment
    {
        public Guid InternalMessageAttachmentId { get; set; }

        public Guid InternalMessageId { get; set; }
        public InternalMessage Message { get; set; } = default!;

        public Guid AttachmentId { get; set; }
        public AttachmentModel Attachment { get; set; } = default!;

        public DateTime AttachedAt { get; set; }
    }
}
