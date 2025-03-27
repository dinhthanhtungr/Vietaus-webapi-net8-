using AutoMapper;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;


namespace VietausWebAPI.Core.Service
{
    public class SupplyRequestsMaterialDatumService : ISupplyRequestsMaterialDatumService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public SupplyRequestsMaterialDatumService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /// <summary>
        /// Thêm đề xuất mới
        /// </summary>
        /// <param name="supplyRequestsMaterialDatumDTO"></param>
        /// <returns></returns>
        public async Task AddSupplyRequestsMaterialDatumAsync(SupplyRequestsMaterialDatumDTO supplyRequestsMaterialDatumDTO)
        {
            var supplyRequestMaterialDatum = _mapper.Map<List<SupplyRequestsMaterialDatum>>(supplyRequestsMaterialDatumDTO);
            await _unitOfWork.SupplyRequestsMaterialDatumRepository.AddSupplyRequestsMaterialDatumRepository(supplyRequestMaterialDatum);
        }
        /// <summary>
        /// Lấy tất cả đề xuất
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SupplyRequestsMaterialDatumDTO>> GetAllSupplyRequestsMaterialDatumAsync()
        {
            var supplyRequestMaterialDatum = await _unitOfWork.SupplyRequestsMaterialDatumRepository.GetAllSupplyRequestsMaterialDatumRepository();
            var result = _mapper.Map<IEnumerable<SupplyRequestsMaterialDatumDTO>>(supplyRequestMaterialDatum);

            return result;
        }
        /// <summary>
        /// Cập nhật trạng thái đề xuất
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="requestStatus"></param>
        /// <returns></returns>
        public async Task UpdateRequestStatusAsyncService(string requestId, string requestStatus)
        {
            await _unitOfWork.SupplyRequestsMaterialDatumRepository.UpdateRequestStatusAsyncRepository(requestId, requestStatus);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
