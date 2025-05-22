using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VietausWebAPI.Core.Application.DTOs.Approval;
using VietausWebAPI.Core.Application.DTOs.SupplyRequests;
using VietausWebAPI.Core.Application.Usecases.Approvals.ServiceContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Usecases.Approvals.Services
{
    public class ApprovalService : IApprovalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;   

        public ApprovalService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Lưu lích sử người phê duyệt và thay đổi trạng thái yêu cầu 
        /// </summary>
        /// <param name="approvalRequestDTO"></param>
        /// <returns></returns>
        public async Task SaveApprovalRequestService(ApprovalRequestDTO approvalRequestDTO)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var approvalRequest = _mapper.Map<ApprovalHistoryMaterialDatum>(approvalRequestDTO);
                await _unitOfWork.ApprovalRepository.SaveApprovalHistoryHandler(approvalRequest);

                await _unitOfWork.SupplyRequestsMaterialDatumRepository.UpdateRequestStatusAsyncRepository(
                    approvalRequestDTO.requestId,
                    approvalRequestDTO.requestStatus
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

        public async Task<PagedResult<ApprovalResponceListDTO>> SendApprovalRequestService(SupplyRequestsQuery approvalQuery)
        {
            var supplyRequest = await _unitOfWork.SupplyRequestRepository.GetSupplyRequestRepository(approvalQuery);
            var result = _mapper.Map<PagedResult<ApprovalResponceListDTO>>(supplyRequest);

            return result;

        }
    }
}
