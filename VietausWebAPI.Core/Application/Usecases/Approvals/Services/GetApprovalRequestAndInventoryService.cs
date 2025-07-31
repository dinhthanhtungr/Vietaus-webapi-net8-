using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietausWebAPI.Core.Application.DTOs.Approval;
using VietausWebAPI.Core.Application.Usecases.Approvals.ServiceContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Usecases.Approvals.Services
{
    public class GetApprovalRequestAndInventoryService : IGetApprovalRequestAndInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetApprovalRequestAndInventoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task ExecuteAsync(ApprovalHistoryAndInventoryRequestDTO approvalHistoryAndInventoryRequestDTO)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var approvalRequest = _mapper.Map<ApprovalHistoryMaterialDatum>(approvalHistoryAndInventoryRequestDTO);
                await _unitOfWork.ApprovalRepository.SaveApprovalHistoryHandler(approvalRequest);
                // Cập nhật trạng thái đề xuất
                await _unitOfWork.SupplyRequestsMaterialDatumRepository.UpdateRequestStatusAsyncRepository(approvalHistoryAndInventoryRequestDTO.requestId, approvalHistoryAndInventoryRequestDTO.requestStatus);
                // Thêm mới phiếu nhập kho
                var inventoryReceipts = _mapper.Map<List<InventoryReceiptsMaterialDatum>>(approvalHistoryAndInventoryRequestDTO.Items);
                await _unitOfWork.InventoryReceiptRepository.AddInventoryReceiptsRepositoryAsync(inventoryReceipts);
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
    }
}
