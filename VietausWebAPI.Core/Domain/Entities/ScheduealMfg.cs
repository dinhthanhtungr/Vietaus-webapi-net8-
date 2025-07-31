using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Entities
{
    public class SchedualMfg
    {
        public Guid Id { get; set; }

        public string? ExternalId { get; set; }
        public string? MachineId { get; set; }
        public int? Number { get; set; }

        public string? ColorName { get; set; }
        public string? ColorCode { get; set; }

        public int? Quantity { get; set; }

        public string? CustomerExternalId { get; set; }

        public DateTime? CustomerRequiredDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }

        public string? VerifyBatches { get; set; }
        public string? Note { get; set; }
        public string? BagType { get; set; }

        public DateTime? ExpectedCompletionDate { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string? Status { get; set; }
        public DateTime? PlanDate { get; set; }

        public string? QCStatus { get; set; } = string.Empty;
    }

}
