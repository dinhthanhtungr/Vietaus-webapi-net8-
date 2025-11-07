using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.ManufacturingTimeline
{
    public class GetManufacturingTimeline
    {
        public string? ExternalId { get; set; }
        public string? ColourCode { get; set; }

        public decimal? RequestQuantity { get; set; }
        public decimal? RealQuantity { get; set; }
        public List<GetManufacturingTimelineDetail> Details { get; set; } = new List<GetManufacturingTimelineDetail>();
    }
}
