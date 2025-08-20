
using AutoMapper;
using Microsoft.AspNetCore.Routing;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.ProductInspectionFeature;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.ProductStandardFeature;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.ProductTestFeature;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.QCOutputFeature;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ProductFeature;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Labs
{
    public class ProductStandardMappingProfile : Profile 
    {
        public ProductStandardMappingProfile()
        {
            // Product Standard Mapping
            CreateMap<ProductStandardSummaryDTO, ProductStandard>().ReverseMap()
                .ForMember(x => x.ColourCode, opt => opt.MapFrom(src => src.ColourCode))

                .ForMember(x => x.Packaging, opt => opt.MapFrom(src => src.Package));

            CreateMap<ProductStandardInformation, ProductStandard>().ReverseMap()
                .ForMember(x => x.ProductStandardId, opt => opt.MapFrom(src => src.Id));

            CreateMap<PDFSpecificationsValue, ProductStandard>().ReverseMap();

            // Product Inspection Mapping
            // Map từ entity → DTO
            CreateMap<ProductInspection, ProductInspectionInformation>()
                .ForMember(x => x.machineId, opt => opt.MapFrom(src => src.Qcdetails.MachineExternalId));

            // Map từ DTO → entity (dùng khi POST)
            CreateMap<ProductInspectionInformation, ProductInspection>()
                .ForMember(dest => dest.Qcdetails, opt => opt.Ignore())     // ⛔ Chặn map QCDetail gây lỗi EF
                .ForMember(dest => dest.Id, opt => opt.Ignore())           // ✅ Để EF tự sinh
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore());  


            CreateMap<ProductInspection, ProductInspectionSummary>()
                .ForMember(x => x.ColourCode, opt => opt.MapFrom(src => src.ProductCode))
                .ForMember(x => x.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(x => x.BatchNumber, opt => opt.MapFrom(src => src.BatchId))
                .ForMember(x => x.Result, opt => opt.MapFrom(src => src.DeliveryAccepted))
                .ForMember(x => x.Types, opt => opt.MapFrom(src => src.Types != null && src.Types.StartsWith("QCOUT_")
                    ? (src.Types == "QCOUT_Final" ? "Kết thúc" : src.Types.Replace("QCOUT_", "QC lần "))
                    : src.Types))
                .ForMember(x => x.MachineId, opt => opt.MapFrom(src => src.Qcdetails.MachineExternalId))
                .ForMember(x => x.QCId, opt => opt.MapFrom(src => src.Qcdetails.Id))
                .AfterMap((src, dest) =>
                {
                    var notes = new List<string>();

                    if (src.DefectImpurity.GetValueOrDefault()) notes.Add("Trộn hàng");
                    if (src.DefectBlackDot.GetValueOrDefault()) notes.Add("Có chấm đen");
                    if (src.DefectShortFiber.GetValueOrDefault()) notes.Add("Có xơ ngắn");
                    if (src.DefectMoist.GetValueOrDefault()) notes.Add("Bị ẩm");
                    if (src.DefectDusty.GetValueOrDefault()) notes.Add("Có bụi bẩn");
                    if (src.DefectWrongColor.GetValueOrDefault()) notes.Add("Sai màu");

                    dest.Status = string.Join(", ", notes); // giả sử Note là string? trong ProductInspectionSummary
                });

            CreateMap<PDFResultValue, ProductInspection>().ReverseMap();

            // Product Test Mapping
            CreateMap<ProductTestDTO, ProductTest>().ReverseMap();

            //MfgProductOrder
            CreateMap<MfgProductDTOs, MfgProductionOrdersPlan>()
                .ReverseMap();

            //QC Detail
            CreateMap<QCDetailDTO, Qcdetail>().ReverseMap();

            //SampleRequest
            CreateMap<CreateSampleRequest, SampleRequest>()
                // Audit/Server fields: KHÔNG nhận từ client
                .ForMember(d => d.SampleRequestId, opt => opt.Ignore())
                .ForMember(d => d.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(d => d.UpdatedDate, opt => opt.Ignore())
                .ForMember(d => d.UpdatedBy, opt => opt.Ignore());

            CreateMap<SampleRequest, SampleRequestDTO>().ReverseMap();

            //Product
            CreateMap<CreateProductRequest, Product>()
                .ForMember(d => d.ProductId, opt => opt.Ignore())
                .ForMember(d => d.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(d => d.UpdatedDate, opt => opt.Ignore())
                .ForMember(d => d.UpdatedBy, opt => opt.Ignore());// Chặn map

            CreateMap<Product, ProductDTO>().ReverseMap();
        }
    }
}
