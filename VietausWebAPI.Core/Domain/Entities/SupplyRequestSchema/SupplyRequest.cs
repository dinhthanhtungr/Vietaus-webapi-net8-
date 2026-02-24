using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.HrSchema;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;

namespace VietausWebAPI.Core.Domain.Entities.SupplyRequestSchema
{
    public partial class SupplyRequest
    {
        public Guid SupplyRequestId { get; set; }          // PK
        public string ExternalId { get; set; } = default!; // VARCHAR(16)
        public string RequestStatus { get; set; } = default!; // VARCHAR(16)
        public string? Note { get; set; }                  // NVARCHAR(MAX)
        public DateTime CreatedDate { get; set; }          // UTC
        public Guid CreatedBy { get; set; }                // FK -> Employee
        public bool IsActive { get; set; } = true;

        public virtual ICollection<SupplyRequestDetail> Details { get; set; } = new List<SupplyRequestDetail>();
        public virtual ICollection<PurchaseOrderLink> PurchaseOrderLinks { get; set; } = new List<PurchaseOrderLink>();
        public virtual Employee? CreatedByNavigation { get; set; }
    }
}
