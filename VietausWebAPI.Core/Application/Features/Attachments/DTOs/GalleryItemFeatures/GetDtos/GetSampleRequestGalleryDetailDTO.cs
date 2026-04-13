using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.OrderAttachment;

namespace VietausWebAPI.Core.Application.Features.Attachments.DTOs.GalleryItemFeatures.GetDtos
{
    public class GetSampleRequestGalleryDetailDTO
    {
        public Guid SampleRequestId { get; set; }
        public Guid ProductId { get; set; }
        public string? EndUserName { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? RealDeliveryDate { get; set; }
        public DateTime? RealPriceQuoteDate { get; set; }
        public DateTime? ExpectedPriceQuoteDate { get; set; }

        public List<GetGalleryImageItemDTO> Images { get; set; } = new();
        public GetGalleryImageItemDTO? Thumbnail { get; set; }
    }
}
