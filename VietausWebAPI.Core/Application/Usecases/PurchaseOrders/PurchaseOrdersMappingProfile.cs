using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.Application.DTOs.PurchaseOrders;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Usecases.PurchaseOrders
{
    public class PurchaseOrdersMappingProfile : Profile
    {
        public PurchaseOrdersMappingProfile()
        {
            CreateMap<CreatePurchaseOrderDTO, PurchaseOrdersMaterialDatum>().ReverseMap();
            CreateMap<PurchaseOrderDetailsDTO, PurchaseOrderDetailsMaterialDatum>()
                .ForMember(dest => dest.Poid, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ShowPurchaseOrderDetailsDTO, PurchaseOrderDetailsMaterialDatum>().ReverseMap()
                .ForMember(d => d.name, opt => opt.MapFrom(src => src.Material.Name))
                .ForMember(d => d.MaterialGroupName, opt => opt.MapFrom(src => src.Material.MaterialGroup.MaterialGroupName))
                .ForMember(d => d.externalId, opt => opt.MapFrom(src => src.Material.ExternalId))
                .ForMember(d => d.unit, opt => opt.MapFrom(src => src.Material.Unit))
                .ForMember(d => d.RequestedQuantity, opt => opt.MapFrom(src => src.Quantity));

            CreateMap<PurchaseOrderDTO, PurchaseOrdersMaterialDatum>().ReverseMap()
                .ForMember(d => d.FullName, opt => opt.MapFrom(src => src.Employee.FullName))
                .ForMember(d => d.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name));

        }
    }
}
