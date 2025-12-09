using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.CompanySchema;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Entities.HrSchema;

namespace VietausWebAPI.Core.Domain.Entities.DeliverySchema
{
    public class DeliveryOrder
    {
        public Guid Id { get; set; }
        public string? ExternalId { get; set; }
        public string Status { get; set; } = string.Empty; // "Pending", "In Transit", "Delivered", "Cancelled"

        public string? CustomerExternalIdSnapShot { get; set; }
        public Guid CustomerId { get; set; }

        public string? Receiver { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? PaymentType { get; set; }
        public string? PaymentDeadline { get; set; }
        public string? TaxNumber { get; set; }
        public string? PhoneSnapshot { get; set; }

        public string? Note { get; set; }
        public decimal? DeliveryPrice { get; set; }
        public bool IsActive { get; set; } = true;
        public bool HasPrinted { get; set; } = false;

        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid CompanyId { get; set; }


        public Company Company { get; set; } = default!;
        public Customer Customer { get; set; } = default!;
        public Employee CreatedByNavigation { get; set; } = default!;
        public Employee UpdatedByNavigation { get; set; } = default!;

        public ICollection<DeliveryOrderPO> DeliveryOrderPOs { get; set; } = new List<DeliveryOrderPO>();
        public ICollection<DeliveryOrderDetail> Details { get; set; } = new List<DeliveryOrderDetail>();
        public ICollection<Deliverer> Deliverers { get; set; } = new List<Deliverer>();
    }
}
