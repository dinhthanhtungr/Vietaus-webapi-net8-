using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest
{
    public class GetSampleRequest
    {
        public Guid SampleRequestId { get; set; }

        public string? ExternalId { get; set; }

        public string? CustomerName { get; set; }
        public string? CustomerCode { get; set; }
        public Guid? CustomerId { get; set; }

        public string? ManagerName { get; set; }

        public string? ProductId { get; set; }

        public DateTime? RealDeliveryDate { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? RequestDeliveryDate { get; set; }
        public DateTime? RequestTestSampleDate { get; set; }
        public DateTime? ResponseDeliveryDate { get; set; }
        public DateTime? RealPriceQuoteDate { get; set; }

        public DateTime? ExpectedPriceQuoteDate { get; set; }

        public string? AdditionalComment { get; set; }
        public string? CustomerProductCode { get; set; }


        public string? RequestType { get; set; }

        public double? ExpectedQuantity { get; set; }

        public decimal? ExpectedPrice { get; set; }

        public double? SampleQuantity { get; set; }

        public string? OtherComment { get; set; }

        public string? InfoType { get; set; }

        public GetSampleFormula? Formula { get; set; }

        public string? Comment { get; set; }

        public string? Image { get; set; }

        public int? Branch { get; set; }

        public string? Status { get; set; }

        public string? Package { get; set; }

        public DateTime? CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public Guid? CompanyId { get; set; }
    }
}
