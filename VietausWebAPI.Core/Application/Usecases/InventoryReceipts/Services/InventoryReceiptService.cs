using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.Application.DTOs.InventoryReceipts;
using VietausWebAPI.Core.Application.Usecases.InventoryReceipts.ServiceContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Usecases.InventoryReceipts.Services
{
    public class InventoryReceiptService : IInventoryReceiptService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public InventoryReceiptService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<InventoryDetailMaterialDTO>> GetMaterialReceiptIdService(string id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var inventoryReceiptsMaterialDatum = await _unitOfWork.InventoryReceiptRepository.GetInventoryReceiptsByIdAsync(id);
                if (inventoryReceiptsMaterialDatum == null || !inventoryReceiptsMaterialDatum.Any())
                {
                    throw new Exception("No inventory receipts found for the provided ID.");
                }
                var inventoryReceiptsMaterialDTOs = _mapper.Map<List<InventoryDetailMaterialDTO>>(inventoryReceiptsMaterialDatum);
                await _unitOfWork.CommitTransactionAsync();
                return inventoryReceiptsMaterialDTOs;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"An error occurred while processing the inventory receipt: {ex.Message}");
            }
        }

        public async Task UpdateFieldChangeService(int id, FieldUpdateDTO field)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.InventoryReceiptRepository.UpdateFieldChangeRepository(id, field);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"An error occurred while updating the field: {ex.Message}");
            }
        }
    }
}
