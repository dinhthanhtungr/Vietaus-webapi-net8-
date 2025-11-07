using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductFeatures;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest
{
    public class CreateSampleRequest
    {
        public Guid CustomerId { get; set; }

        public Guid ManagerBy { get; set; }

        public Guid ProductId { get; set; }
        public Guid? AttachmentCollectionId { get; set; }

        public DateTime? RealDeliveryDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }

        public DateTime? RequestDeliveryDate { get; set; }
        public DateTime? RequestTestSampleDate { get; set; }
        public DateTime? ResponseDeliveryDate { get; set; }

        public DateTime? RealPriceQuoteDate { get; set; }
        public DateTime? ExpectedPriceQuoteDate { get; set; }

        public string RequestType { get; set; } = string.Empty;

        public double? ExpectedQuantity { get; set; }
        public decimal? ExpectedPrice { get; set; }
        public double? SampleQuantity { get; set; }

        public string? OtherComment { get; set; }
        public string? InfoType { get; set; }

        public Guid? FormulaId { get; set; }

        public string? SaleComment { get; set; }

        public string? AdditionalComment { get; set; } = string.Empty;
        public string? CustomerProductCode { get; set; } = string.Empty;

        public Guid BranchId { get; set; }

        public string Status { get; set; } = "New";

        public string Package { get; set; } = string.Empty;

    }
}
