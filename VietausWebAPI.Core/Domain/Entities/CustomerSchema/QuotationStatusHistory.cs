using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.HrSchema;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Domain.Entities.CustomerSchema
{
    public class QuotationStatusHistory
    {
        public Guid Id { get; set; }
        public Guid QuotationId { get; set; }

        public QuotationStatus FromStatus { get; set; }
        public QuotationStatus ToStatus { get; set; }

        public string? Note { get; set; }

        public Guid ChangedBy { get; set; }
        public DateTime ChangedDate { get; set; }

        public virtual Employee ChangedByNavigation { get; set; } = null!;

        public virtual Quotation Quotation { get; set; } = null!;
    }
}
