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
            CreateMap<InventoryReceiptsMaterialDTO, InventoryReceiptsMaterialDatum>().ReverseMap();
            CreateMap<InventoryReceiptsForInputGetDTO, InventoryReceiptsMaterialDatum>().ReverseMap()
                .ForMember(d => d.RequestDate, opt => opt.MapFrom(src => src.Request.RequestDate))
                .ForMember(d => d.RequestStatus, opt => opt.MapFrom(src => src.Request.RequestStatus));
            //CreateMap<InventoryReceiptsGetDTO, InventoryReceiptsMaterialDatum>().ReverseMap();
            CreateMap<EmployeesCommonDatumDTO, EmployeesCommonDatum>().ReverseMap()
                .ForMember(d => d.PartName, opt => opt.MapFrom(src => src.Part.PartName));
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
                //.ForMember(dest => dest.MaterialGroupId, opt => opt.MapFrom(src => src.MaterialGroupId))
                .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.MaterialName))
                .ForMember(dest => dest.RequestedQuantity, opt => opt.MapFrom(src => src.RequestedQuantity))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit));


            CreateMap<RequestDetailMaterialDatum, RequestDetailResponseGetDto>()
                .ForMember(dest => dest.MaterialGroupName, opt => opt.MapFrom(src => src.MaterialGroup.MaterialGroupName));

            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));

            CreateMap<MaterialSuppliersMaterialDatum, MaterialSuppliersDTO>().ReverseMap();
            CreateMap<ApprovalHistoryMaterialPostDTO, ApprovalHistoryMaterialDatum>().ReverseMap()
                .ForMember(d => d.requestStatus, opt => opt.MapFrom(src => src.Request.RequestStatus));
            CreateMap<ApprovalHistoryMaterialGetDTO, ApprovalHistoryMaterialDatum>().ReverseMap()
                .ForMember(d => d.FullName, opt => opt.MapFrom(src => src.Employee.FullName))
                .ForMember(d => d.requestStatus, opt => opt.MapFrom(src => src.Request.RequestStatus));


        }
    }
}
