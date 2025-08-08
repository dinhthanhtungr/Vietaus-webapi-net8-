using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Employees;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Groups;
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

            CreateMap<ApplicationUser, AccountDTOs>()
                .ForMember(x => x.Roles, opt => opt.MapFrom(src => src.UserRoles
                    .Where(ur => ur.IsActive)
                    .Select(ur => ur.Role.Name)
                    .ToList()))

                .ForMember(x => x.CancelRoles, opt => opt.MapFrom(src => src.UserRoles
                    .Where(ur => !ur.IsActive)
                    .Select(ur => ur.Role.Name)
                    .ToList()))
                .ReverseMap();
            //.ForMember(x => x.)

            //Groups
            CreateMap<MemberInGroup, PostMemberDTO>().ReverseMap();
            CreateMap<MemberInGroup, GetMemberDTO>()
                .ForMember(x => x.MemberName, opt => opt.MapFrom(src => src.ProfileNavigation.FullName))
                .ForMember(x => x.ExternalId, opt => opt.MapFrom(src => src.ProfileNavigation.ExternalId))
                .ReverseMap();

            CreateMap<Group, GetGroupDTOs>()
                .ForMember(dest => dest.LeaderName, opt => opt.MapFrom(src =>
                    src.MemberInGroups
                        .Where(m => m.IsAdmin == true)
                        .Select(m => m.ProfileNavigation.FullName) // hoặc tên nếu có
                        .FirstOrDefault()
                ))
                .ForMember(x => x.MemberCount, opt => opt.MapFrom(src => src.MemberInGroups.Count));
                


            CreateMap<Group, PostGroupDTOs>().ReverseMap();

        }
    }
}
