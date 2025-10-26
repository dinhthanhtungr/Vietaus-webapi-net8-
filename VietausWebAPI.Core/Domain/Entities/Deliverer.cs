using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Entities
{
    public class Deliverer
    {
        public Guid Id { get; set; }

        public Guid DeliveryOrderId { get; set; }

        public Guid DelivererInforId { get; set; }

        public DeliveryOrder DeliveryOrder { get; set; } = default!;
        public DelivererInfor DelivererInfor { get; set; } = default!;
    }
}
