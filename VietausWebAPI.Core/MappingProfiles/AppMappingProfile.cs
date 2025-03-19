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
                .ForMember(dest => dest.MaterialGroupId, opt => opt.Ignore()) // Không map navigation
                .ForMember(dest => dest.RequestId, opt => opt.Ignore())       // Không map navigation
            ;
            CreateMap<InventoryReceiptsGetDTO, InventoryReceiptsMaterialDatum>().ReverseMap();
            CreateMap<SupplyRequestsMaterialDatum, SupplyRequestsMaterialDatumDTO>().ReverseMap();
            CreateMap<RequestDetailMaterialDatum, RequestDetailMaterialDatumPostDTO>().ReverseMap();
            CreateMap<SupplyRequestsMaterialDatum, RequestDTO>().ReverseMap().ForMember(dest => dest.RequestDetailMaterialData, opt => opt.MapFrom(src => src.RequestDetails)).ForMember(dest => dest.RequestDetailMaterialData, opt => opt.Ignore());
        }
    }
}
