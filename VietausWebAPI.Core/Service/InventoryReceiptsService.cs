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
        public InventoryReceiptsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            //_inventoryReceiptsRepository = inventoryReceiptsRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //public async Task AddInventoryReceiptsServiceAsync(InventoryReceiptsDTO inventoryReceiptsDTO)
        //{
        //    var inventoryReceipts = _mapper.Map<List<InventoryReceiptsMaterialDatum>>(inventoryReceiptsDTO.Items);
        //    await _unitOfWork.InventoryReceiptsRepository.AddInventoryReceiptsRepositoryAsync(inventoryReceipts);
        //}

        public async Task AddInventoryReceiptsServiceAsync(InventoryReceiptsDTO inventoryReceiptsDTO)
        {
            //var inventoryReceipts = _mapper.Map<List<InventoryReceiptsMaterialDatum>>(inventoryReceiptsDTO.Items);
            //await _unitOfWork.InventoryReceiptsRepository.AddInventoryReceiptsRepositoryAsync(inventoryReceipts);
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var inventoryReceipts = _mapper.Map<List<InventoryReceiptsMaterialDatum>>(inventoryReceiptsDTO.Items);
                await _unitOfWork.InventoryReceiptsRepository.AddInventoryReceiptsRepositoryAsync(inventoryReceipts);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public async Task<PagedResult<InventoryReceiptsGetDTO>> AddInventoryReceiptsServiceAsync(InventoryReceiptsQuery inventoryReceiptsQuery)
        {
            var materials = await _unitOfWork.InventoryReceiptsRepository.SearchInventoryReceiptsRepositoryAsync(inventoryReceiptsQuery);
            var result = _mapper.Map<PagedResult<InventoryReceiptsGetDTO>>(materials);
            return result;
        }

        public async Task<IEnumerable<InventoryReceiptsGetDTO>> GetAllInventoryReceiptsServiceAsync()
        {
            var inventoryReceipts = await _unitOfWork.InventoryReceiptsRepository.GetAllInventoryReceiptsRepositoryAsync();
            var result = _mapper.Map<IEnumerable<InventoryReceiptsGetDTO>>(inventoryReceipts);
            return result;
        }

    }
}
