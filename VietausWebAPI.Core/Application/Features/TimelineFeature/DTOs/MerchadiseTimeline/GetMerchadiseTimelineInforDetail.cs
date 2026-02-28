using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.MerchadiseTimeline
{
    public class GetMerchadiseTimelineInforDetail
    {
        public string? ExternalId { get; set; }
        public string? ColourCode { get; set; }
        public string? ProductName { get; set; }

        //public string? MfgList { get; set; }
        public string? DeliveryList { get; set; }

        public DateTime RequestDate { get; set; }
        public DateTime? ExpectedDate { get; set; }

        public decimal? RequestQuantity { get; set; }
        public decimal? RealQuantity { get; set; }

        public List<GetMerchadiseTimelineDetail> Details { get; set; } = new List<GetMerchadiseTimelineDetail>();
    }
}
