using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs
{
    public class PatchPauseDeliveryOrder
    {
        public Guid MerchandiseOrderId { get; set; }
        public bool IsDeliveryPaused { get; set; } = false;
        public DateTime? DeliveryPausedFrom { get; set; }
        public DateTime? DeliveryPausedTo { get; set; }
        public string? DeliveryPauseReason { get; set; }
        public string? DeliveryPauseType { get; set; }
    }
}
