using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Attachments.DTOs.GalleryItemFeatures.GetDtos
{
    public class GetSampleRequestSummaryPlusDTO
    {
        public Guid SampleRequestId { get; set; }
        public string? ExternalId { get; set; }

        public Guid? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ColourCode { get; set; }

        public string? Status { get; set; }
        public string? CategoryName { get; set; }
        public string? ColourName { get; set; }
        public string? Additive { get; set; }

        public string? CustomerName { get; set; }
        public string? CustomerExternalId { get; set; }

        public string? LabName { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? RequestDeliveryDate { get; set; }

        public Guid? AttachmentId { get; set; }
        public Guid? AttachmentCollectionId { get; set; }
        public int? Slot { get; set; }
        public string? FileName { get; set; }
        public string? StoragePath { get; set; }
        public long? SizeBytes { get; set; }

        public decimal? L { get; set; }
        public decimal? A { get; set; }
        public decimal? B { get; set; }

        public double? DeltaE { get; set; }
    }
}
