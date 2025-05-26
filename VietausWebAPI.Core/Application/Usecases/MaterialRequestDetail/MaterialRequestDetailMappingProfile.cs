using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.Application.DTOs.MaterialRequestDetails;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Usecases.MaterialRequestDetail
{
    public class MaterialRequestDetailMappingProfile : Profile
    {
        public MaterialRequestDetailMappingProfile() 
        {
            CreateMap<MaterialRequestDetailGetDTO, RequestDetailMaterialDatum>().ReverseMap();
            CreateMap<MaterialRequestDetailPostDTO, RequestDetailMaterialDatum>().ReverseMap()
                .ForMember(x => x.MaterialName, opt => opt.MapFrom(src => src.Material.Name))
                .ForMember(x => x.Unit, opt => opt.MapFrom(src => src.Material.Unit));
        }
    }
}
