using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders
{
    public class GetSampleMfgFormula
    {
        public Guid ManufacturingFormulaId { get; set; }
        public string? MfgProductionOrderExternalId { get; set; }
        public string? ExternalId { get; set; }
        public string? Name { get; set; }

        public Guid? VUFormulaId { get; set; }
        public string? FormulaExternalIdSnapshot { get; set; }
        public Guid? MfgFormulaId { get; set; }
        public string? MfgFormulaExternalIdSnapshot { get; set; }

        // Trạng thái hiện tại của công thức
        public string? Status { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool? isStandard { get; set; } = false;
        public bool? IsSelect { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
