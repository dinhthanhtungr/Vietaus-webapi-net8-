using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders
{
    public class GetSummaryMfgProductionOrder
    {

        public Guid MfgProductionOrderId { get; set; }
        public string? ExternalId { get; set; }
        public string? MfgFormualaExternalIdSnapshot { get; set; }
        public Guid MerchandiseOrderId { get; set; }
        public string? MerchandiseOrderExternalId { get; set; }
        public string? ProductExternalIdSnapshot { get; set; }
        public string? ProductNameSnapshot { get; set; }
        public string? CustomerNameSnapshot { get; set; }
        public string? CustomerExternalIdSnapshot { get; set; }
        public decimal? TotalQuantity { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? BagType { get; set; }
    }
}
