using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Employees;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Groups;
using VietausWebAPI.Core.Domain.Entities.CompanySchema;
using VietausWebAPI.Core.Domain.Entities.HrSchema;
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
                .ForMember(x => x.EmployeeId, opt => opt.MapFrom(src => src.ProfileNavigation.EmployeeId))
                .ReverseMap();
            CreateMap<Group, GetGroupDTOs>()
                .ForMember(dest => dest.LeaderName, opt => opt.MapFrom(src =>
                    src.MemberInGroups
                        .Where(m => m.IsAdmin == true)
                        .Select(m => m.ProfileNavigation.FullName)
                        .FirstOrDefault()
                ))
                .ForMember(dest => dest.MemberCount, opt => opt.MapFrom(src =>
                    src.MemberInGroups.Count(m => m.IsActive == true)
                ));


            CreateMap<Group, PostGroupDTOs>().ReverseMap();


            // Map MemberInGroup -> GroupLiteDTO
            CreateMap<MemberInGroup, GroupLiteDTO>()
                .ForMember(d => d.GroupId, opt => opt.MapFrom(s => s.Group.GroupId))
                .ForMember(d => d.GroupCode, opt => opt.MapFrom(s => s.Group.ExternalId))
                .ForMember(d => d.GroupName, opt => opt.MapFrom(s => s.Group.Name))
                .ForMember(d => d.GroupType, opt => opt.MapFrom(s => s.Group.GroupType))
                .ForMember(d => d.IsAdmin, opt => opt.MapFrom(s => s.IsAdmin ?? false));

            // Map Employee -> EmployeeGroupDTO
            // Chú ý: Groups sẽ tự động map nhờ ICollection<MemberInGroup>
            CreateMap<Employee, EmployeeGroupDTO>()
                .ForMember(d => d.EmployeeCode, opt => opt.MapFrom(s => s.ExternalId))
                .ForMember(d => d.Groups, opt => opt.MapFrom(s => s.MemberInGroups));

        }
    }
}
