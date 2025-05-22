using AutoMapper;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.Core.Service
{
    public class RequestDetailMaterialService : IRequestDetailMaterialService
    {
        private readonly IRequestDetailMaterialRepository _requestDetailMaterialRepository;
        private readonly IMapper _mapper;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requestDetailMaterialRepository"></param>
        /// <param name="mapper"></param>
        public RequestDetailMaterialService(IRequestDetailMaterialRepository requestDetailMaterialRepository, IMapper mapper)
        {
            _requestDetailMaterialRepository = requestDetailMaterialRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Thêm mới danh sách các vật tư đề xuất đi chung với lại request
        /// </summary>
        /// <param name="requestDetailMaterialDatumPostDTO"></param>
        /// <returns></returns>
        public async Task AddRequestDetailServiceAsync(RequestDetailMaterialDatumPostDTO requestDetailMaterialDatumPostDTO)
        {
            var result = _mapper.Map<IEnumerable<RequestDetailMaterialDatum>>(requestDetailMaterialDatumPostDTO);
            await _requestDetailMaterialRepository.AddRequetMaterialRepository(result);
        }
        /// <summary>
        /// Lấy tất cả danh sách vật tư đề xuất
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<RequestDetailMaterialDatumPostDTO>> GetAllRequestDetailServiceAsync()
        {
            var RequestDetailMaterial = await _requestDetailMaterialRepository.GetAllRequestMaterialRepository();
            var result = _mapper.Map<IEnumerable<RequestDetailMaterialDatumPostDTO>>(RequestDetailMaterial);
            return result;
        }
    }
}
