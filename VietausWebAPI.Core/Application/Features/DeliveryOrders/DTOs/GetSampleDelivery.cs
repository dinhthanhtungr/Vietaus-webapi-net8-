using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs
{
    public class GetSampleDelivery
    {
        public Guid Id { get; set; }
        public string? ExternalId { get; set; }
        public string? MerchandiseOrderExternalIdList { get; set; }

        public string? CustomerExternalIdSnapShot { get; set; }
        public string? CustomerName { get; set; }
        public string? PaymentDeadline { get; set; }
        public string? DelivererList { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string? Note { get; set; }

        // Bổ sung
        public List<GetSampleDeliveryDetail> Details { get; set; } = new();
    }
}
