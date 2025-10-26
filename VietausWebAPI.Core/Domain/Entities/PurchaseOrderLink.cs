using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Entities
{
    public class PurchaseOrderLink
    {
        public Guid PurchaseOrderLinkId { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public Guid? MerchandiseOrderId { get; set; }
        public bool IsActive { get; set; } = true;

        public PurchaseOrder PurchaseOrder { get; set; } = default!;
        public MerchandiseOrder? MerchandiseOrder { get; set; } = default!;

    }
}
