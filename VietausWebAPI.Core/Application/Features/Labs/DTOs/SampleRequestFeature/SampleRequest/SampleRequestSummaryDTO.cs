using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest
{
    public class SampleRequestSummaryDTO
    {
        public Guid SampleRequestId { get; set; }
        public string? ExternalId { get; set; }
        public Guid? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ColourCode { get; set; }
        public string? Status { get; set; } = string.Empty;
        public string? CustomerName { get; set; } = string.Empty;
        public string? CustomerExternalId { get; set; } = string.Empty; 
        public string? LabName { get; set; } = string.Empty;
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? RequestDeliveryDate { get; set; }
        public DateTime? RealDeliveryDate { get; set; }
        public DateTime? RealPriceQuoteDate { get; set; }
        public DateTime? ExpectedPriceQuoteDate { get; set; }
    }
}
