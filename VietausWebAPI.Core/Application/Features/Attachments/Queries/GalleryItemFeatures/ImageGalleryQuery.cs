using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.Attachment;

namespace VietausWebAPI.Core.Application.Features.Attachments.Queries.GalleryItemFeatures
{
    public class ImageGalleryQuery : PaginationQuery
    {
        public string? Keyword { get; set; }
        public decimal? L { get; set; }
        public decimal? A { get; set; }
        public decimal? B { get; set; }

        public bool? HasLABColor { get; set; }
        public string? Status { get; set; } = string.Empty;


        public string? ColorSpace { get; set; }
        public string? Additive { get; set; }
        public Guid? CategoryId { get; set; }
        public AttachmentSlot? Slot { get; set; }
        public Guid? CollectionId { get; set; }
    }
}
