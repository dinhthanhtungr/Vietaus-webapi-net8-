using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Planning.DTOs.SchedualFeatures;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Planning
{
    public class PlanningMappingProfile : Profile
    {
        public PlanningMappingProfile() 
        {
            CreateMap<SchedualMfgDTO, SchedualMfg>().ReverseMap();
        }
    }
}
