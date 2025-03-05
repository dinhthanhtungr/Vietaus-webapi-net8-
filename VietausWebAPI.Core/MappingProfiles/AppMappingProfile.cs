using AutoMapper;
using VietausWebAPI.Core.DTO;
using VietausWebAPI.Core.Models;
using VietausWebAPI.Infrastructure.Models;


namespace VietausWebAPI.Core.MappingProfiles
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<InventoryReceiptsDTO, InventoryReceiptsMaterialDatum>().ReverseMap();
        }
    }
}
