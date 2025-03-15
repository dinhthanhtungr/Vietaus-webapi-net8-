using AutoMapper;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;


namespace VietausWebAPI.Core.Service
{
    public class SupplyRequestsMaterialDatumService : ISupplyRequestsMaterialDatumService
    {
        private readonly ISupplyRequestsMaterialDatumRepository _supplyRequestsMaterialDatumRepository;
        private readonly IMapper _mapper;
        public SupplyRequestsMaterialDatumService(ISupplyRequestsMaterialDatumRepository supplyRequestsMaterialDatumRepository, IMapper mapper)
        {
            _supplyRequestsMaterialDatumRepository = supplyRequestsMaterialDatumRepository;
            _mapper = mapper;
        }

        public async Task AddSupplyRequestsMaterialDatumAsync(SupplyRequestsMaterialDatumDTO supplyRequestsMaterialDatumDTO)
        {
            var supplyRequestMaterialDatum = _mapper.Map<List<SupplyRequestsMaterialDatum>>(supplyRequestsMaterialDatumDTO);
            await _supplyRequestsMaterialDatumRepository.AddSupplyRequestsMaterialDatumRepository(supplyRequestMaterialDatum);
        }

        public async Task<IEnumerable<SupplyRequestsMaterialDatumDTO>> GetAllSupplyRequestsMaterialDatumAsync()
        {
            var supplyRequestMaterialDatum = await _supplyRequestsMaterialDatumRepository.GetAllSupplyRequestsMaterialDatumRepository();
            var result = _mapper.Map<IEnumerable<SupplyRequestsMaterialDatumDTO>>(supplyRequestMaterialDatum);

            return result;
        }
    }
}
