using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Entities
{
    public class DeliveryOrderPO
    {
        public Guid DeliveryOrderId { get; set; }
        public Guid MerchandiseOrderId { get; set; }

        // Sau khi tạo DO xong, bạn sẽ sinh 1 WarehouseRequest cho từng PO
        public int? WarehouseRequestId { get; set; }
        public bool IsActive { get; set; } = true;
        public DeliveryOrder DeliveryOrder { get; set; } = default!;
        public MerchandiseOrder MerchandiseOrder { get; set; } = default!;
        public WarehouseRequest? WarehouseRequest { get; set; }
    }
}
