using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;
using VietausWebAPI.Core.DTO.QueryObject;



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

        public async Task ApproveAndUpdateAsync(ApproveReceiptDTO inventoryReceiptsMaterialDatum)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Cập nhật trạng thái đề xuất
                await _unitOfWork.SupplyRequestsMaterialDatumRepository.UpdateRequestStatusAsyncRepository(inventoryReceiptsMaterialDatum.RequestId, inventoryReceiptsMaterialDatum.status);
                // Thêm mới phiếu nhập kho
                var inventoryReceipts = _mapper.Map<List<InventoryReceiptsMaterialDatum>>(inventoryReceiptsMaterialDatum.Items);
                //await _unitOfWork.InventoryReceiptsRepository.AddInventoryReceiptsRepositoryAsync(inventoryReceipts);
                // Lưu thay đổi
                await _unitOfWork.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Đã xảy ra lỗi: " + ex.Message);
            }
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
        /// Cập nhật trạng thái đề xuất thành công
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="note"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task SuccessRequestStatusAsyncService(string requestId, string note, string status)
        {
            await _unitOfWork.SupplyRequestsMaterialDatumRepository.SuccessRequestStatusAsyncRepository(requestId, note, status);
            await _unitOfWork.SaveChangesAsync();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResult<ProgressTimeLineDTO>> GetSearchSupplyRequestsMaterialDatumService(SupplyRequestQuery query)
        {
            var supplyInformation = await _unitOfWork.SupplyRequestsMaterialDatumRepository.GetSearchSupplyRequestsMaterialDatumRepository(query);
            var result = _mapper.Map<PagedResult<ProgressTimeLineDTO>>(supplyInformation);
            return result;
        }
    }
}
