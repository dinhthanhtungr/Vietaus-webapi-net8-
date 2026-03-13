using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Manufacturings;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs
{
    public class CopyMfgProductionOrderResult
    {
        public Guid MfgProductionOrderId { get; set; }
        public string? ExternalId { get; set; }
    }
}
