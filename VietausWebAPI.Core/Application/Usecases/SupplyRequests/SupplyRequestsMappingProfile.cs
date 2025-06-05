using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.Application.DTOs.Approval;
using VietausWebAPI.Core.Application.DTOs.Materials;
using VietausWebAPI.Core.Application.DTOs.SupplyRequests;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.DTO.GetDTO;

namespace VietausWebAPI.Core.Application.Usecases.SupplyRequests
{ 
    public class SupplyRequestsMappingProfile : Profile
    {
        public SupplyRequestsMappingProfile()
        {
            CreateMap<MaterialSearchResultDto, MaterialsMaterialDatum>().ReverseMap()
                .ForMember(d => d.MaterialName, opt => opt.MapFrom(src => src.Name));
            CreateMap<SupplyRequestData, SupplyRequestsMaterialDatum>().ReverseMap();
            CreateMap<MaterialsDTO, MaterialsMaterialDatum>().ReverseMap()
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(d => d.Unit, opt => opt.MapFrom(src => src.Unit))
                .ForMember(d => d.CreateDate, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(d => d.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId));
        }
    }
}
