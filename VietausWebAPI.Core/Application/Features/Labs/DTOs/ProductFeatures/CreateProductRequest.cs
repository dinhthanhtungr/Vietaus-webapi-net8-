using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductFeatures
{
    public class CreateProductRequest
    {
        public string? ColourCode { get; set; }
        public string? Name { get; set; } = string.Empty;        // bắt buộc
        public string? ColourName { get; set; }
        public string? Additive { get; set; }
        public float? UsageRate { get; set; }
        public float? DeltaE { get; set; }
        public string? Requirement { get; set; }
        public string? ExpiryType { get; set; }
        public string? StorageCondition { get; set; }
        public string? LabComment { get; set; }
        public string? ProductType { get; set; }
        public string? Procedure { get; set; }
        public float? RecycleRate { get; set; }
        public float? TaicalRate { get; set; }
        public string? Application { get; set; }
        public string? ProductUsage { get; set; }
        public string? PolymerMatchedIn { get; set; }
        public string? Code { get; set; }
        public string? EndUser { get; set; }
        public bool? FoodSafety { get; set; }
        public bool? RohsStandard { get; set; }
        public float? MaxTemp { get; set; }
        public string? WeatherResistance { get; set; }
        public string? LightCondition { get; set; }
        public string? VisualTest { get; set; }
        public bool? ReturnSample { get; set; }
        public string? OtherComment { get; set; }
        public Guid? CategoryId { get; set; }
        public float? Weight { get; set; }
        public bool? IsRecycle { get; set; }
        public string? Unit { get; set; }
        public Guid CompanyId { get; set; }                 // bắt buộc
        public Guid? CreatedBy { get; set; }                 
    }
}
