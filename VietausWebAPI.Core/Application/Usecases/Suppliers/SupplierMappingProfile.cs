using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.Application.DTOs.Suppliers;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Usecases.Suppliers
{
    public class SupplierMappingProfile : Profile
    {
        public SupplierMappingProfile() 
        {
            CreateMap<SupplierInformationsDTO, SuppliersMaterialDatum>().ReverseMap()
                .ForMember(x => x.VendorPhone, opt => opt.MapFrom(src => src.Phone));
            //CreateMap<SupplierAddressDTO, SupplierAddressesMaterialDatum>().ReverseMap();
        }
    }
}
