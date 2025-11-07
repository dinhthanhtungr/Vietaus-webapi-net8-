using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.MerchadiseTimeline
{
    public class GetMerchadiseTimelineDetail
    {
        public string Status { get; set; } = string.Empty;
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string CompanyName { get; set; } = string.Empty;

        public string? Note { get; set; }
    }
}
