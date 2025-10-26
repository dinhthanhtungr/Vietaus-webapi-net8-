using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders
{
    public class GetFormulaAndMfgFormula
    {
        public Guid FormulaId { get; set; }

        public string? ExternalId { get; set; }
        public string? Name { get; set; }
        public string? ProductCode { get; set; }
        public bool? IsSelect { get; set; }

        // Trạng thái hiện tại của công thức
        public string Status { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool? isCustomerSelect { get; set; } = false;
        public bool? isHasStandard { get; set; } = false;
        public DateTime? CreatedDate { get; set; }
        public List<GetSampleMfgFormula> getSampleMfgFormulas { get; set; } = new List<GetSampleMfgFormula>();
    }

}
