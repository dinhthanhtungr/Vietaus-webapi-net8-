using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;

namespace VietausWebAPI.Core.Domain.Entities.CustomerSchema
{
    public class QuotationLine
    {
        public Guid QuotationLineId { get; set; }
        public Guid QuotationId { get; set; }

        public Guid ProductId { get; set; }
        public string ProductExternalIdSnapshot { get; set; } = string.Empty;
        public string ProductNameSnapshot { get; set; } = string.Empty;

        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal LineTotal { get; set; }

        public string? Note { get; set; }
        public int SortOrder { get; set; }

        public virtual Product ProductNavigation { get; set; } = null!;
        public virtual Quotation Quotation { get; set; } = null!;
    }
}
