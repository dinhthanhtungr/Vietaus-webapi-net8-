using AutoMapper;
using Microsoft.AspNetCore.SignalR;
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
        private readonly INotificationService _notificationService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        /// <param name="hubContext"></param>
        public SupplyRequestsMaterialDatumService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
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
            //var request = await _unitOfWork.SupplyRequestsMaterialDatumRepository.GetWithId(requestId);
            //if (request == null)
            //{
            //    throw new KeyNotFoundException($"Supply request with ID {requestId} not found");
            //}

            //if (request.RequestStatus == requestStatus)
            //{
            //    return;
            //}

            //var oldStatus = request.RequestStatus;
            //request.RequestStatus = requestStatus;

            await _unitOfWork.SupplyRequestsMaterialDatumRepository.UpdateRequestStatusAsyncRepository(requestId, requestStatus);
            await _unitOfWork.SaveChangesAsync();

            //var requestor = await _unitOfWork.UserRepository.GetUserById(request.RequestorId);
        }
    }
}
