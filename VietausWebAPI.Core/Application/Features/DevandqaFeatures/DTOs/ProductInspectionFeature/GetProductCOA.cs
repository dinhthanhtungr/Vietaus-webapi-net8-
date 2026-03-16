using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.ProductInspectionFeature
{
    public class GetProductCOA
    {
        public Guid id { get; set; }
        public string? externalId { get; set; }

        public DateTime? manufacturingDate { get; set; }
        public DateTime? expiryDate { get; set; }

        public string? productPackage { get; set; }
        public float? ProductWeight { get; set; }

        public Guid? ProductId { get; set; }
        public string? productExternalId { get; set; }
        public string? productName { get; set; }

        public decimal? TotalQuantityRequest { get; set; }

        public string? ColourCode { get; set; }
    }
}
