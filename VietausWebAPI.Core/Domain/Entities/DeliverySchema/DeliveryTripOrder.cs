using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Entities.DeliverySchema
{
    public class DeliveryTripOrder
    {
        public Guid DeliveryTripId { get; set; }
        public Guid DeliveryOrderId { get; set; }

        public int StopSequence { get; set; }
        public bool IsActive { get; set; } = true;

        public DeliveryTrip DeliveryTrip { get; set; } = default!;
        public DeliveryOrder DeliveryOrder { get; set; } = default!;
    }
}
