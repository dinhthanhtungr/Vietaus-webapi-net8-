using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Entities.DeliverySchema
{
    public class DeliveryStop
    {
        public Guid Id { get; set; }
        public Guid DeliveryTripId { get; set; }
        public Guid? DeliveryOrderId { get; set; }

        public int SequenceNo { get; set; }
        public string StopType { get; set; } = string.Empty; // Warehouse, Customer, Return

        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? ContactName { get; set; }
        public string? ContactPhone { get; set; }

        public DateTime? PlannedArrivalTime { get; set; }
        public DateTime? PlannedDepartureTime { get; set; }
        public DateTime? ActualArrivalTime { get; set; }
        public DateTime? ActualDepartureTime { get; set; }

        public string Status { get; set; } = string.Empty; // Planned, Arrived, Delivered, Failed, Skipped
        public string? FailureReason { get; set; }

        public bool IsActive { get; set; } = true;

        public DeliveryTrip DeliveryTrip { get; set; } = default!;
        public DeliveryOrder? DeliveryOrder { get; set; }
    }
}
