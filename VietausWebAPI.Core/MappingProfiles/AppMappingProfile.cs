using AutoMapper;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.Entities;


namespace VietausWebAPI.Core.MappingProfiles
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<SendData, InventoryReceiptsMaterialDatum>().ReverseMap()
                //.ForMember(dest => dest.MaterialGroupId, opt => opt.Ignore()) // Không map navigation
                //.ForMember(dest => dest.RequestId, opt => opt.Ignore())       // Không map navigation
            ;
            CreateMap<InventoryReceiptsGetDTO, InventoryReceiptsMaterialDatum>().ReverseMap();
            CreateMap<SupplyRequestsMaterialDatum, SupplyRequestsMaterialDatumDTO>().ReverseMap();
            CreateMap<SupplyRequestsMaterialDatum, RequestIdDTO>().ReverseMap().ForMember(dest => dest.RequestId, opt => opt.MapFrom(src => src.RequestId));
            CreateMap<SupplyRequestsMaterialDatum, RequestMaterialDTO>().ReverseMap().ForMember(dest => dest.RequestDetailMaterialData, opt => opt.MapFrom(src => src.RequestDetails)).ForMember(dest => dest.RequestDetailMaterialData, opt => opt.Ignore());

            CreateMap<SupplyRequestsMaterialDatum, RequestMaterialDTO>()
            .ForMember(dest => dest.RequestDetails, opt => opt.MapFrom(src => src.RequestDetailMaterialData ?? new List<RequestDetailMaterialDatum>()));

            CreateMap<RequestDetailMaterialDatum, RequestDetailMaterialDatumPostDTO>()
                .ReverseMap()
                //.ForMember(dest => dest.MaterialGroupId, opt => opt.MapFrom(src => src.RequestId))
                .ForMember(dest => dest.MaterialGroupId, opt => opt.MapFrom(src => src.MaterialGroupId))
                .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.MaterialName))
                .ForMember(dest => dest.RequestedQuantity, opt => opt.MapFrom(src => src.RequestedQuantity))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit));

            CreateMap<MaterialSuppliersMaterialDatum, MaterialSuppliersDTO>().ReverseMap();
            CreateMap<ApprovalHistoryMaterialPostDTO, ApprovalHistoryMaterialDatum>().ReverseMap();
        }
    }
}
