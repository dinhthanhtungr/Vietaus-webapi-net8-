using AutoMapper;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;


namespace VietausWebAPI.Core.Service
{
    public class InventoryReceiptsService : IInventoryReceiptsService
    {
        //private readonly IInventoryReceiptsRepository _inventoryReceiptsRepository;
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        /// <summary>
        /// Khởi tạo đối tượng InventoryReceiptsService
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public InventoryReceiptsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            //_inventoryReceiptsRepository = inventoryReceiptsRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Thêm mới danh sách phiếu nhập kho
        /// </summary>
        /// <param name="inventoryReceiptsDTO"></param>
        /// <returns></returns>
        public async Task AddInventoryReceiptsServiceAsync(InventoryReceiptsPostDTO inventoryReceiptsDTO)
        {

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var inventoryReceipts = _mapper.Map<List<InventoryReceiptsMaterialDatum>>(inventoryReceiptsDTO.Items);
                await _unitOfWork.InventoryReceiptsRepository.AddInventoryReceiptsRepositoryAsync(inventoryReceipts);

                await _unitOfWork.SupplyRequestsMaterialDatumRepository.UpdateRequestStatusAsyncRepository(
                    inventoryReceiptsDTO.RequestId,
                    inventoryReceiptsDTO.requestStatus
                );

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        /// <summary>
        /// Tìm kiếm danh sách phiếu nhập kho theo các tiêu chí tìm kiếm và trả về kết quả phân trang
        /// </summary>
        /// <param name="inventoryReceiptsQuery"></param>
        /// <returns></returns>
        public async Task<PagedResult<InventoryReceiptsGetDTO>> SearchInventoryReceiptsServiceAsync(InventoryReceiptsQuery inventoryReceiptsQuery)
        {
            var materials = await _unitOfWork.InventoryReceiptsRepository.SearchInventoryReceiptsRepositoryAsync(inventoryReceiptsQuery);
            var result = _mapper.Map<PagedResult<InventoryReceiptsGetDTO>>(materials);
            return result;

            //await _unitOfWork.BeginTransactionAsync();

            //try
            //{
            //    var materials = await _unitOfWork.InventoryReceiptsRepository.SearchInventoryReceiptsRepositoryAsync(inventoryReceiptsQuery);
            //    var result = _mapper.Map<PagedResult<InventoryReceiptsGetDTO>>(materials);
            //    await _unitOfWork.CommitTransactionAsync();
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    await _unitOfWork.RollbackTransactionAsync();
            //    throw ex;
            //}
        }

        /// <summary>
        /// Lấy tất cả danh sách phiếu nhập kho
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<InventoryReceiptsGetDTO>> GetAllInventoryReceiptsServiceAsync()
        {
            var inventoryReceipts = await _unitOfWork.InventoryReceiptsRepository.GetAllInventoryReceiptsRepositoryAsync();
            var result = _mapper.Map<IEnumerable<InventoryReceiptsGetDTO>>(inventoryReceipts);
            return result;
        }

        /// <summary>
        /// Cập nhật giá danh sách phiếu nhập kho 
        /// </summary>
        /// <param name="inventoryReceiptsUpdatePriceDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task UpdateInventoryReceiptsServiceAsync(InventoryReceiptsUpdatePriceDTO inventoryReceiptsUpdatePriceDTO)
        {
            await _unitOfWork.InventoryReceiptsRepository.UpdateInventoryReceiptsRepositoryAsync(inventoryReceiptsUpdatePriceDTO); ;
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
