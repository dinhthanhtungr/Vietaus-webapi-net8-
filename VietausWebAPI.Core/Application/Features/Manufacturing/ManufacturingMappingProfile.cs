using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;

namespace VietausWebAPI.Core.Application.Features.Manufacturing
{
    public class ManufacturingMappingProfile : Profile
    {
        public ManufacturingMappingProfile()
        {

            CreateMap<GetSummaryMfgProductionOrder, MfgProductionOrder>().ReverseMap();

            //CreateMap<MfgProductionOrder, GetMfgProductionOrder>()
            //    .ForMember(d => d.UsageRate, opt => opt.MapFrom(s => s.Product.UsageRate))
            //    // Lấy công thức có IsSelect == true (nếu có)
            //    .ForMember(dest => dest.selectManufacturing,
            //               opt => opt.MapFrom(src =>
            //                   src.ManufacturingFormulas
            //                      .Where(f => f.IsSelect == true)
            //                      .Select(f => f)      // có thể map sang DTO khác
            //                      .FirstOrDefault()));

            //CreateMap<ManufacturingFormula, GetManufacturingFormula>()
            //    .ForMember(d => d.IsRecycle, opt => opt.MapFrom(s => s.MfgProductionOrder.Product.IsRecycle))
            //    .ForMember(d => d.TotalQuantityRequest, opt => opt.MapFrom(s => s.MfgProductionOrder.TotalQuantityRequest))
            //    .ForMember(d => d.ExternalId, opt => opt.MapFrom(s => s.ExternalId))
            //    .ForMember(d => d.mfgProductionOrderId,
            //        o => o.MapFrom(s => s.MfgProductionOrderId))
            //    .ForMember(d => d.MfgExternalId,
            //        o => o.MapFrom(s => s.MfgProductionOrder.ExternalId))
            //    .ForMember(d => d.MerchandiseOrderExternalId,
            //        o => o.MapFrom(s => s.MfgProductionOrder.MerchandiseOrderExternalId))
            //    .ForMember(d => d.ProductId,
            //        o => o.MapFrom(s => s.MfgProductionOrder.ProductId))
            //    .ForMember(d => d.ProductNameSnapshot,
            //        o => o.MapFrom(s => s.MfgProductionOrder.ProductNameSnapshot))
            //    .ForMember(d => d.ProductExternalIdSnapshot,
            //        o => o.MapFrom(s => s.MfgProductionOrder.ProductExternalIdSnapshot))
            //    .ForMember(d => d.CustomerNameSnapshot,
            //        o => o.MapFrom(s => s.MfgProductionOrder.CustomerNameSnapshot))
            //    .ForMember(d => d.MfgTotalPrice,
            //        o => o.MapFrom(s => s.MfgProductionOrder.UnitPriceAgreed)); // Gía từ sale
            //CreateMap<ManufacturingFormulaMaterial, GetManufacturingFormulaMaterial>();


            //CreateMap<ManufacturingFormula, GetSummaryMfgFormula>();   
            //CreateMap<ManufacturingFormulaMaterial, GetSampleMfgFormulaMaterial>();



            ////
            //CreateMap<PostMfgFormula, ManufacturingFormula>()
            //    .ForMember(d => d.ManufacturingFormulaId, o => o.Ignore())
            //    .ForMember(d => d.ExternalId, o => o.Ignore());  

            //CreateMap<PostManufacturingFormulaMaterial, ManufacturingFormulaMaterial>()
            //    .ForMember(d => d.ManufacturingFormulaMaterialId, o => o.Ignore());

            //CreateMap<PatchMfgFormulaMaterial, ManufacturingFormulaMaterial>()
            //    .ForMember(d => d.ManufacturingFormulaMaterialId, o => o.Ignore());

            //CreateMap<PatchMfgProductionOrderInformation, MfgProductionOrder>();




            //// Root
            //CreateMap<ManufacturingFormula, GetSummaryMfgFormula>()
            //    .ForMember(d => d.ExternalId, o => o.MapFrom(s => s.ExternalId))
            //    .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
            //    .ForMember(d => d.isStandard, o => o.MapFrom(s => s.IsStandard)) // DTO dùng "isStandard" (chữ i thường)
            //    .ForMember(d => d.IsSelect, o => o.MapFrom(s => s.IsSelect))
            //    .ForMember(d => d.ManufacturingFormulaMaterials, o => o.MapFrom(s => s.ManufacturingFormulaMaterials));
            //// "Materials" là navigation của bạn tới bảng con (ví dụ ManufacturingFormulaMaterials)

            //// Child
            //CreateMap<ManufacturingFormulaMaterial, GetSampleMfgFormulaMaterial>()
            //    .ForMember(d => d.MaterialId, o => o.MapFrom(s => s.MaterialId))
            //    .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.CategoryId))
            //    .ForMember(d => d.MaterialNameSnapshot, o => o.MapFrom(s => s.MaterialNameSnapshot))
            //    .ForMember(d => d.MaterialExternalIdSnapshot, o => o.MapFrom(s => s.MaterialExternalIdSnapshot))
            //    .ForMember(d => d.Unit, o => o.MapFrom(s => s.Unit));



            //// Root
            //CreateMap<Formula, GetSummaryMfgFormula>()
            //    .ForMember(d => d.ManufacturingFormulaId, o => o.MapFrom(s => s.FormulaId))
            //    .ForMember(d => d.ExternalId, o => o.MapFrom(s => s.ExternalId))
            //    .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
            //    .ForMember(d => d.IsSelect, o => o.MapFrom(s => false))   // nếu không có, để null hoặc false
            //    .ForMember(d => d.isStandard, o => o.MapFrom(s => false))  // nếu không có, để null hoặc false
            //    .ForMember(d => d.ManufacturingFormulaMaterials, o => o.MapFrom(s => s.FormulaMaterials));
            //// "Materials" là navigation bên research, ví dụ ResearchFormulaMaterials

            //// Child
            //CreateMap<FormulaMaterial, GetSampleMfgFormulaMaterial>()
            //    .ForMember(d => d.MaterialId, o => o.MapFrom(s => s.MaterialId))
            //    .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.CategoryId))
            //    .ForMember(d => d.MaterialNameSnapshot, o => o.MapFrom(s => s.MaterialNameSnapshot))
            //    .ForMember(d => d.MaterialExternalIdSnapshot, o => o.MapFrom(s => s.MaterialExternalIdSnapshot))
            //    .ForMember(d => d.Unit, o => o.MapFrom(s => s.Unit));





            //// ManufacturingFormula -> GetSampleMfgFormula
            //CreateMap<ManufacturingFormula, GetSampleMfgFormula>()
            //    .ForMember(d => d.MfgProductionOrderExternalId,
            //        opt => opt.MapFrom(s => s.MfgProductionOrder.ExternalId))
            //    // Name ưu tiên tên của ManufacturingFormula (nếu có), fallback về Formula.Name
            //    .ForMember(d => d.Name,
            //        opt => opt.MapFrom(s => s.Name ?? s.VUFormula.Name))
            //    .ForMember(d => d.Status,
            //        opt => opt.MapFrom(s => s.Status))
            //    .ForMember(d => d.TotalPrice,
            //        opt => opt.MapFrom(s => s.TotalPrice))
            //    .ForMember(d => d.isStandard, opt => opt.MapFrom(s => s.IsStandard));

            //// Formula -> GetFormulaAndMfgFormula
            //CreateMap<Formula, GetFormulaAndMfgFormula>()
            //    .ForMember(d => d.ProductCode,
            //        opt => opt.MapFrom(s => s.Product.ColourCode ?? s.Product.Code)) // tuỳ bạn muốn map trường nào
            //    .ForMember(d => d.getSampleMfgFormulas,
            //        opt => opt.MapFrom(s => s.ManufacturingFormulas
            //            .Where(mf => mf.IsActive == true))) // lọc collection ngay trong projection
            //    .ForMember(d => d.Status,
            //        opt => opt.MapFrom(s => s.Status))
            //    .ForMember(d => d.TotalPrice,
            //        opt => opt.MapFrom(s => s.TotalPrice))
            //    .ForMember(d => d.CreatedDate,
            //        opt => opt.MapFrom(s => s.CreatedDate))
            //    .ForMember(d => d.isCustomerSelect,
            //        opt => opt.MapFrom(s => s.IsSelect == true)) // Giả sử IsSelect của Formula tương ứng với isCustomerSelect
            //    .ForMember(d => d.isHasStandard,
            //        opt => opt.MapFrom(s => s.ManufacturingFormulas.Any(mf => mf.IsStandard == true))); // Kiểm tra nếu có bất kỳ ManufacturingFormula nào là standard

            //CreateMap<ManufacturingFormula, GetSampleMfgFormula>()
            //    .ForMember(d => d.CreatedDate,
            //        opt => opt.MapFrom(s => s.CreatedDate));
        }
    }
}
