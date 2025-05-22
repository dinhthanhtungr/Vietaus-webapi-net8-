using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.Application.DTOs.Approval;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Usecases.Approvals
{
    public class ApprovalsMappingProfile : Profile
    {
        public ApprovalsMappingProfile()
        { 
            CreateMap<ApprovalRequestDTO, ApprovalHistoryMaterialDatum>().ReverseMap();
            CreateMap<ApprovalResponceListDTO, SupplyRequestsMaterialDatum>().ReverseMap()
                .ForMember(d => d.fullName, opt => opt.MapFrom(src => src.Employee.FullName))
                .ForMember(d => d.partName, opt => opt.MapFrom(src => src.Employee.Part.PartName));
        }
    }
}
