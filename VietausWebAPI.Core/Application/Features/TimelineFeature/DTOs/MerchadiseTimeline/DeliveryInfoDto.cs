using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.MerchadiseTimeline
{
    public class DeliveryInfoDto
    {
        public string DOExternalId { get; set; } = string.Empty;
        public string? LotNoList { get; set; }
    }
}
