using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ProductFeature;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest
{
    public class UpdateSampleRequest
    {
        public Guid SampleRequestId { get; set; }

        public string? ExternalId { get; set; }

        public Guid CustomerId { get; set; }

        public Guid ManagerBy { get; set; }

        public Guid ProductId { get; set; }

        public DateTime? RealDeliveryDate { get; set; }
        public DateTime? RequestTestSampleDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? RequestDeliveryDate { get; set; }
        public DateTime? ResponseDeliveryDate { get; set; }

        public DateTime? RealPriceQuoteDate { get; set; }

        public DateTime? ExpectedPriceQuoteDate { get; set; }

        public string? AdditionalComment { get; set; }

        public string? RequestType { get; set; }

        public double? ExpectedQuantity { get; set; }

        public decimal? ExpectedPrice { get; set; }

        public double? SampleQuantity { get; set; }

        public string? OtherComment { get; set; }

        public string? InfoType { get; set; }

        public Guid? FormulaId { get; set; }

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
        public bool? IsActive { get; set; }

        public GetProductRequest Product { get; set; } = null!;
    }
}
