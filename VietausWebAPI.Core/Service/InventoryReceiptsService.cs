using AutoMapper;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;


namespace VietausWebAPI.Core.Service
{
    public class InventoryReceiptsService : IInventoryReceiptsService
    {
        private readonly IInventoryReceiptsRepository _inventoryReceiptsRepository;
        private readonly IMapper _mapper;
        public InventoryReceiptsService(IInventoryReceiptsRepository inventoryReceiptsRepository, IMapper mapper)
        {
            _inventoryReceiptsRepository = inventoryReceiptsRepository;
            _mapper = mapper;
        }

        public async Task AddInventoryReceiptsServiceAsync(InventoryReceiptsDTO inventoryReceiptsDTO)
        {
            var inventoryReceipts = _mapper.Map<List<InventoryReceiptsMaterialDatum>>(inventoryReceiptsDTO.Items);
            await _inventoryReceiptsRepository.AddInventoryReceiptsRepositoryAsync(inventoryReceipts);
        }

        public async Task<IEnumerable<InventoryReceiptsGetDTO>> GetAllInventoryReceiptsServiceAsync()
        {
            var inventoryReceipts = await _inventoryReceiptsRepository.GetAllInventoryReceiptsRepositoryAsync();
            var result = _mapper.Map<IEnumerable<InventoryReceiptsGetDTO>>(inventoryReceipts);
            //var inventoryReceipts = _mapper.Map<InventoryReceiptsMaterialDatum>(inventoryReceiptsDTO);
            return result;
        }

    }
}
