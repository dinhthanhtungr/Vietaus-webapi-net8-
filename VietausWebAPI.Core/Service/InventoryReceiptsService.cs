using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.Service;
using VietausWebAPI.Core.ServiceContracts;
using VietausWebAPI.Infrastructure.Models;

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

        public Task AddInventoryReceipts()
        {
            throw new NotImplementedException();
        }

        public async Task AddInventoryReceiptsAsync(InventoryReceiptsDTO inventoryReceiptsDTO)
        {
            var inventoryReceipts = _mapper.Map<InventoryReceiptsMaterialDatum>(inventoryReceiptsDTO);
            await _inventoryReceiptsRepository.AddInventoryReceipts(inventoryReceipts);
        }
    }
}
