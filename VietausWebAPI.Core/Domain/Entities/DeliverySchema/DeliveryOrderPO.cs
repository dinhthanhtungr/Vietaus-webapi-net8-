using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;

namespace VietausWebAPI.Core.Domain.Entities.DeliverySchema
{
    public class DeliveryOrderPO
    {
        public Guid DeliveryOrderId { get; set; }
        public Guid MerchandiseOrderId { get; set; }
        public bool IsActive { get; set; } = true;
        public DeliveryOrder DeliveryOrder { get; set; } = default!;
        public MerchandiseOrder MerchandiseOrder { get; set; } = default!;
    }
}
