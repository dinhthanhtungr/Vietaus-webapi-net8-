using AutoMapper;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Core.Entities;


namespace VietausWebAPI.Core.MappingProfiles
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<MaterialsMaterialGroupsDatum, MaterialGroupsDTO>().ReverseMap();
            CreateMap<SendData, InventoryReceiptsMaterialDatum>().ReverseMap();
            CreateMap<InventoryReceiptsGetDTO, InventoryReceiptsMaterialDatum>().ReverseMap();
            //CreateMap<InventoryReceiptsGetDTO, InventoryReceiptsMaterialDatum>().ReverseMap();
            CreateMap<EmployeesCommonDatumDTO, EmployeesCommonDatum>().ReverseMap();
            CreateMap<SupplyRequestsMaterialDatumDTO, SupplyRequestsMaterialDatum>().ReverseMap();
            CreateMap<SupplyRequestsMaterialDatum, RequestIdDTO>().ReverseMap().ForMember(dest => dest.RequestId, opt => opt.MapFrom(src => src.RequestId));
            CreateMap<SupplyRequestsMaterialDatum, RequestMaterialDTO>().ReverseMap().ForMember(dest => dest.RequestDetailMaterialData, opt => opt.MapFrom(src => src.RequestDetails)).ForMember(dest => dest.RequestDetailMaterialData, opt => opt.Ignore());

            CreateMap<SupplyRequestsMaterialDatum, RequestMaterialDTO>()
            .ForMember(dest => dest.RequestDetails, opt => opt.MapFrom(src => src.RequestDetailMaterialData ?? new List<RequestDetailMaterialDatum>()))
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.EmployeeId : "Không có nhân viên"))
            .ForMember(dest => dest.PartName, opt => opt.MapFrom(src => src.Employee != null && src.Employee.Part != null ? src.Employee.Part.PartName : "Không có bộ phận"));

            CreateMap<RequestDetailMaterialDatum, RequestDetailMaterialDatumPostDTO>()
                .ReverseMap()
                //.ForMember(dest => dest.MaterialGroupId, opt => opt.MapFrom(src => src.RequestId))
                .ForMember(dest => dest.MaterialGroupId, opt => opt.MapFrom(src => src.MaterialGroupId))
                .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.MaterialName))
                .ForMember(dest => dest.RequestedQuantity, opt => opt.MapFrom(src => src.RequestedQuantity))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit));

            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));

            CreateMap<MaterialSuppliersMaterialDatum, MaterialSuppliersDTO>().ReverseMap();
            CreateMap<ApprovalHistoryMaterialPostDTO, ApprovalHistoryMaterialDatum>().ReverseMap();

            
        }
    }
}
