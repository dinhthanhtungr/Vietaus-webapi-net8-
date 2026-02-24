using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures
{
    public class GetProductSampleRequestDTO
    {
        public Guid ProductId { get; set; }
        public string? ColourCode { get; set; }
        public string? Name { get; set; } = string.Empty;        // bắt buộc
        public string? ColourName { get; set; }
        public double? UsageRate { get; set; }


        public string? ExternalId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerCode { get; set; }
        public string? SaleComment { get; set; }
        public string? AdditionalComment { get; set; } = string.Empty;

        public List<GetFormula>? SampleFormulas { get; set; }
    }
}
