using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders.GetRepositories
{
    public class MfgFormulaHistoryRow
    {
        public Guid MfgProductionOrderId { get; set; }
        public string? MfgProductionOrderExternalId { get; set; }
        public string MfgFormulaExternalId { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
