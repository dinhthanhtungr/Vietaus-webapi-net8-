using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.Application.DTOs.Approval;
using VietausWebAPI.Core.Application.DTOs.InventoryReceipts;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Usecases.InventoryReceipts
{
    public class InventoryReceiptMappingProfile : Profile
    {
        public InventoryReceiptMappingProfile()
        {
            CreateMap<InventoryReceiptsMaterialDatum, InventoryReceiptsMaterialDTO>().ReverseMap();
            CreateMap<InventoryReceiptsMaterialDatum, InventoryDetailMaterialDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Material.Name))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Material.Unit))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Material.MaterialGroup.MaterialGroupName))
                .ReverseMap();
        }
    }
}
