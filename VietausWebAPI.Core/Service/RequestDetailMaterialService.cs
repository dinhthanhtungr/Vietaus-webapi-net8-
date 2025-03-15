using AutoMapper;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.Core.Service
{
    public class RequestDetailMaterialService : IRequestDetailMaterialService
    {
        private readonly IRequestDetailMaterialRepository _requestDetailMaterialRepository;
        private readonly IMapper _mapper;
        public RequestDetailMaterialService(IRequestDetailMaterialRepository requestDetailMaterialRepository, IMapper mapper)
        {
            _requestDetailMaterialRepository = requestDetailMaterialRepository;
            _mapper = mapper;
        }

        public async Task AddRequestDetailServiceAsync(RequestDetailMaterialDatumPostDTO requestDetailMaterialDatumPostDTO)
        {
            var result = _mapper.Map<IEnumerable<RequestDetailMaterialDatum>>(requestDetailMaterialDatumPostDTO);
            await _requestDetailMaterialRepository.AddRequetMaterialRepository(result);
        }

        public async Task<IEnumerable<RequestDetailMaterialDatumPostDTO>> GetAllRequestDetailServiceAsync()
        {
            var RequestDetailMaterial = await _requestDetailMaterialRepository.GetAllRequestMaterialRepository();
            var result = _mapper.Map<IEnumerable<RequestDetailMaterialDatumPostDTO>>(RequestDetailMaterial);
            return result;
        }
    }
}
