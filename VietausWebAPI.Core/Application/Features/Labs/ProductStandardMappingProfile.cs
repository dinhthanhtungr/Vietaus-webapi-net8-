
//using AutoMapper;
//using Microsoft.AspNetCore.Routing;
//using System;
//using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
//using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductFeatures;
//using VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.ProductInspectionFeature;
//using VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.ProductStandardFeature;
//using VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.ProductTestFeature;
//using VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.QCOutputFeature;
//using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest;
//using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders;
//using VietausWebAPI.Core.Domain.Entities;
//using VietausWebAPI.Core.Domain.Entities.DevandqaSchema;
//using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;

//namespace VietausWebAPI.Core.Application.Features.Labs
//{
//    public class ProductStandardMappingProfile : Profile 
//    {
//        public ProductStandardMappingProfile()
//        {
//            // Product Standard Mapping
//            CreateMap<ProductStandardSummaryDTO, ProductStandard>().ReverseMap()
//                //.ForMember(x => x.ColourCode, opt => opt.MapFrom(src => src.ColourCode))

//                //.ForMember(x => x.Packaging, opt => opt.MapFrom(src => src.Package));

//            CreateMap<ProductStandardInformation, ProductStandard>().ReverseMap()
//                .ForMember(x => x.ProductStandardId, opt => opt.MapFrom(src => src.Id));

//            CreateMap<PDFSpecificationsValue, ProductStandard>().ReverseMap();

//            // Product Inspection Mapping
//            // Map từ entity → DTO
//            CreateMap<ProductInspection, ProductInspectionInformation>()
//                .ForMember(x => x.machineId, opt => opt.MapFrom(src => src.Qcdetails.MachineExternalId));

//            // Map từ DTO → entity (dùng khi POST)
//            CreateMap<ProductInspectionInformation, ProductInspection>()
//                .ForMember(dest => dest.Qcdetails, opt => opt.Ignore())     // ⛔ Chặn map QCDetail gây lỗi EF
//                .ForMember(dest => dest.Id, opt => opt.Ignore())           // ✅ Để EF tự sinh
//                .ForMember(dest => dest.CreateDate, opt => opt.Ignore());  


//            CreateMap<ProductInspection, ProductInspectionSummary>()
//                .ForMember(x => x.ColourCode, opt => opt.MapFrom(src => src.ProductCode))
//                .ForMember(x => x.ProductName, opt => opt.MapFrom(src => src.ProductName))
//                .ForMember(x => x.BatchNumber, opt => opt.MapFrom(src => src.BatchId))
//                .ForMember(x => x.Result, opt => opt.MapFrom(src => src.DeliveryAccepted))
//                .ForMember(x => x.Types, opt => opt.MapFrom(src => src.Types != null && src.Types.StartsWith("QCOUT_")
//                    ? (src.Types == "QCOUT_Final" ? "Kết thúc" : src.Types.Replace("QCOUT_", "QC lần "))
//                    : src.Types))
//                .ForMember(x => x.MachineId, opt => opt.MapFrom(src => src.Qcdetails.MachineExternalId))
//                .ForMember(x => x.QCId, opt => opt.MapFrom(src => src.Qcdetails.Id))
//                .AfterMap((src, dest) =>
//                {
//                    var notes = new List<string>();

//                    if (src.DefectImpurity.GetValueOrDefault()) notes.Add("Trộn hàng");
//                    if (src.DefectBlackDot.GetValueOrDefault()) notes.Add("Có chấm đen");
//                    if (src.DefectShortFiber.GetValueOrDefault()) notes.Add("Có xơ ngắn");
//                    if (src.DefectMoist.GetValueOrDefault()) notes.Add("Bị ẩm");
//                    if (src.DefectDusty.GetValueOrDefault()) notes.Add("Có bụi bẩn");
//                    if (src.DefectWrongColor.GetValueOrDefault()) notes.Add("Sai màu");

//                    dest.Status = string.Join(", ", notes); // giả sử Note là string? trong ProductInspectionSummary
//                });

//            CreateMap<PDFResultValue, ProductInspection>().ReverseMap();

//            // Product Test Mapping
//            CreateMap<ProductTestDTO, ProductTest>().ReverseMap();

//            //MfgProductOrder
//            CreateMap<MfgProductDTOs, MfgProductionOrdersPlan>()
//                .ReverseMap();

//            //QC Detail
//            CreateMap<QCDetailDTO, Qcdetail>().ReverseMap();

//            //SampleRequest
//            CreateMap<CreateSampleRequest, SampleRequest>()
//                // Audit/Server fields: KHÔNG nhận từ client
//                .ForMember(d => d.SampleRequestId, opt => opt.Ignore())
//                .ForMember(d => d.CreatedDate, opt => opt.MapFrom(_ => DateTime.Now))
//                .ForMember(d => d.ProductId, opt => opt.Ignore())
//                .ForMember(d => d.ExpectedDeliveryDate, opt => opt.Ignore())

//                .ForMember(d => d.ExpectedPriceQuoteDate, opt => opt.Ignore())
//                //.ForMember(d => d.ExternalId, opt => opt.Ignore())
//                .ForMember(d => d.FormulaId, opt => opt.Ignore())
//                .ForMember(d => d.InfoType, opt => opt.Ignore())
//                .ForMember(d => d.RealPriceQuoteDate, opt => opt.Ignore())
//                .ForMember(d => d.ResponseDeliveryDate, opt => opt.Ignore())

//                .ForMember(d => d.UpdatedDate, opt => opt.Ignore())
//                .ForMember(d => d.UpdatedBy, opt => opt.Ignore());

//            CreateMap<SampleRequest, SampleRequestSummaryDTO>()
//                .ForMember(d => d.ProductName, opt => opt.MapFrom(src => src.Product.Name))
//                .ForMember(d => d.ColourCode, opt => opt.MapFrom(src => src.Product.ColourCode))
//                .ForMember(d => d.CreatedBy, opt => opt.MapFrom(src => src.CreatedByNavigation.FullName))
//                .ForMember(d => d.Status, opt => opt.MapFrom(src => src.Status))
//                .ForMember(d => d.CustomerName, opt => opt.MapFrom(src => src.Customer.CustomerName))
//                .ForMember(d => d.LabName, opt => opt.MapFrom(src => src.Product.CreatedByNavigation.FullName))
//                .ReverseMap();

//            CreateMap<SampleRequest, GetSampleRequest>()
//                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.CustomerName))
//                .ForMember(d => d.CustomerCode, o => o.MapFrom(s => s.Customer.ExternalId))
//                .ForMember(d => d.ManagerName, o => o.MapFrom(s => s.CreatedByNavigation.FullName))
//                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ProductId)) // tránh map nhầm ColourCode
//                .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.CreatedDate))
//                .ForMember(d => d.RealDeliveryDate, o => o.MapFrom(s => s.RealDeliveryDate))
//                .ForMember(d => d.ExpectedDeliveryDate, o => o.MapFrom(s => s.ExpectedDeliveryDate))
//                .ForMember(d => d.RequestDeliveryDate, o => o.MapFrom(s => s.RequestDeliveryDate))
//                .ForMember(d => d.RequestTestSampleDate, o => o.MapFrom(s => s.RequestTestSampleDate))
//                .ForMember(d => d.RealPriceQuoteDate, o => o.MapFrom(s => s.RealPriceQuoteDate))
//                .ForMember(d => d.ResponseDeliveryDate, o => o.MapFrom(s => s.ResponseDeliveryDate))
//                .ForMember(d => d.ExpectedPriceQuoteDate, o => o.MapFrom(s => s.ExpectedPriceQuoteDate));


//            CreateMap<UpdateSampleRequest, SampleRequest>()
//                .ForMember(d => d.SampleRequestId, opt => opt.Ignore());
//            //Product
//            CreateMap<CreateProductRequest, Product>()
//                .ForMember(d => d.ProductId, opt => opt.Ignore())
//                .ForMember(d => d.CreatedDate, opt => opt.MapFrom(_ => DateTime.Now))
//                .ForMember(d => d.CreatedBy, opt => opt.Ignore())
//                .ForMember(d => d.UpdatedDate, opt => opt.Ignore())
//                .ForMember(d => d.Additive, opt => opt.Ignore())
//                .ForMember(d => d.UpdatedDate, opt => opt.Ignore())
//                .ForMember(d => d.ColourCode, opt => opt.Ignore())
//                .ForMember(d => d.ColourName, opt => opt.Ignore())
//                .ForMember(d => d.ExpiryType, opt => opt.Ignore())
//                .ForMember(d => d.LabComment, opt => opt.Ignore())
//                .ForMember(d => d.Name, opt => opt.Ignore())
//                .ForMember(d => d.Requirement, opt => opt.Ignore())
//                .ForMember(d => d.StorageCondition, opt => opt.Ignore())

//                .ForMember(d => d.UpdatedDate, opt => opt.Ignore())
//                .ForMember(d => d.UpdatedBy, opt => opt.Ignore());// Chặn map

//            CreateMap<Product, GetProduct>().ReverseMap();
                

//            //CreateMap<Product, ProductDTO>().ReverseMap();

//            // Map "bao" ra DTO lồng
//            CreateMap<SampleRequest, GetSampleWithProductRequest>()
//                .ForMember(d => d.Product, o => o.MapFrom(s => s.Product)) // dùng map Product->GetProductRequest
//                .ForMember(d => d.Sample, o => o.MapFrom(s => s));        // dùng map SampleRequest->GetSampleRequest


//            // Formula Mapping
//            CreateMap<PostMaterialFormula, FormulaMaterial>();
//            CreateMap<FormulaMaterial, GetMaterialFormula>();


//            CreateMap<PostFormula, Formula>()
//                .ForMember(d => d.FormulaId, opt => opt.Ignore())
//                .ForMember(d => d.ExternalId, opt => opt.Ignore())
//                .ForMember(d => d.Name, opt => opt.Ignore())
//                .ForMember(d => d.FormulaMaterials, opt => opt.MapFrom(s => s.materialFormulas))
//                .ForMember(d => d.CreatedDate, opt => opt.MapFrom(_ => DateTime.Now))
//                .ForMember(d => d.UpdatedDate, opt => opt.MapFrom(_ => DateTime.Now));

//            CreateMap<Formula, GetFormula>()
//                .ForMember(d => d.CreatedByName, opt => opt.MapFrom(s => s.CreatedByNavigation.FullName))
//                .ForMember(d => d.materialFormulas, opt => opt.MapFrom(s => s.FormulaMaterials))
//                .ForMember(d => d.ProductCode, opt => opt.MapFrom(s => s.Product.ColourCode))
//                    .ForMember(d => d.materialFormulas,
//                            opt => opt.MapFrom(s => s.FormulaMaterials.Where(x => x.IsActive == true    )));

//            CreateMap<Formula, GetSampleFormula>()
//                .ForMember(d => d.FormulaId, opt => opt.MapFrom(s => s.FormulaId))
//                .ForMember(d => d.ExternalId, opt => opt.MapFrom(s => s.ExternalId))
//                .ForMember(d => d.Note, opt => opt.MapFrom(s => s.Note))
//                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
//                .ForMember(d => d.ProductCode, opt => opt.MapFrom(s => s.Product.ColourCode))
//                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status))
//                .ForMember(d => d.TotalPrice, opt => opt.MapFrom(s => s.TotalPrice));



//        }
//    }
//}
