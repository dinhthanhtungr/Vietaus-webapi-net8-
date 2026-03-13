using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.MerchadiseTimeline
{
    public class GetMerchadiseTimeline
    {
        public Guid MerchandiseOrderId { get; set; }
        public string ExternalId { get; set; } = string.Empty;

        public string CreatedName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string CustomerExternalId { get; set; } = string.Empty;

        public decimal TotalPrice { get; set; }
        public decimal Vat { get; set; }

        public List<GetMerchadiseTimelineDetail> Details { get; set; } = new();
    }
}
