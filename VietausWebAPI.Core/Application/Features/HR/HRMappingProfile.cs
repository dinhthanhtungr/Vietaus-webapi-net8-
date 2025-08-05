using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.DTOs;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Identity;

namespace VietausWebAPI.Core.Application.Features.HR
{
    public class HRMappingProfile : Profile
    {
        public HRMappingProfile()
        {
            CreateMap<Employee, EmployeesPostDTOs>().ReverseMap();
            CreateMap<Employee, EmployeeSummary>()
                .ForMember(x => x.PartName, opt => opt.MapFrom(src => src.Part.PartName))
                .ReverseMap();

            CreateMap<AccountDTOs, ApplicationUser>().ReverseMap();
                //.ForMember(x => x.)
        }
    }
}
