
using AutoMapper;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductInspectionFeature;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductStandardFeature;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductTestFeature;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Labs
{
    public class ProductStandardMappingProfile : Profile 
    {
        public ProductStandardMappingProfile()
        {
            // Product Standard Mapping
            CreateMap<ProductStandardSummaryDTO, ProductStandard>().ReverseMap()
                .ForMember(x => x.ColourCode, opt => opt.MapFrom(src => src.colourCode))

                .ForMember(x => x.Packaging, opt => opt.MapFrom(src => src.Package));

            CreateMap<ProductStandardInformation, ProductStandard>().ReverseMap()
                .ForMember(x => x.ProductStandardId, opt => opt.MapFrom(src => src.Id));

            CreateMap<PDFSpecificationsValue, ProductStandard>().ReverseMap();

            // Product Inspection Mapping
            CreateMap<ProductInspectionInformation, ProductInspection>().ReverseMap();
            CreateMap<ProductInspection, ProductInspectionSummary>()
                .ForMember(x => x.ColourCode, opt => opt.MapFrom(src => src.ProductCode))
                .ForMember(x => x.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(x => x.BatchNumber, opt => opt.MapFrom(src => src.BatchId))
                .ForMember(x => x.Result, opt => opt.MapFrom(src => src.DeliveryAccepted))
                .AfterMap((src, dest) =>
                {
                    var notes = new List<string>();

                    if (src.Defect_Impurity.GetValueOrDefault()) notes.Add("Có tạp chất");
                    if (src.Defect_BlackDot.GetValueOrDefault()) notes.Add("Có chấm đen");
                    if (src.Defect_ShortFiber.GetValueOrDefault()) notes.Add("Có xơ ngắn");
                    if (src.Defect_Moist.GetValueOrDefault()) notes.Add("Bị ẩm");
                    if (src.Defect_Dusty.GetValueOrDefault()) notes.Add("Có bụi bẩn");

                    dest.Status = string.Join(", ", notes); // giả sử Note là string? trong ProductInspectionSummary
                });

            CreateMap<PDFResultValue, ProductInspection>().ReverseMap();

            // Product Test Mapping
            CreateMap<ProductTestDTO, ProductTest>().ReverseMap();

        }
    }
}
