using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.Core.Service
{
    public class ApprovalHistoryMaterialService : IApprovalHistoryMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ApprovalHistoryMaterialService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddApprovalHistoryMaterialServiceAsync(ApprovalHistoryMaterialPostDTO approvalHistoryMaterialPostDTO)
        {
            //var approvalHistory = _mapper.Map<ApprovalHistoryMaterialDatum>(approvalHistoryMaterialPostDTO);
            //await _unitOfWork.ApprovalHistoryMaterialRepository.AddApprovalHistoryMaterialRepositoryAsync(approvalHistory);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var approvalHistory = _mapper.Map<ApprovalHistoryMaterialDatum>(approvalHistoryMaterialPostDTO);
                await _unitOfWork.ApprovalHistoryMaterialRepository.AddApprovalHistoryMaterialRepositoryAsync(approvalHistory);

                await _unitOfWork.SupplyRequestsMaterialDatumRepository.UpdateRequestStatusAsyncRepository(
                    approvalHistoryMaterialPostDTO.RequestId,
                    approvalHistoryMaterialPostDTO.requestStatus
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
        public async Task<IEnumerable<ApprovalHistoryMaterialPostDTO>> GetApprovalHistoryMaterialServiceAsync(string requestId)
        {
            var approvalHistories = await _unitOfWork.ApprovalHistoryMaterialRepository.GetApprovalHistoryMaterialRepositoryAsync(requestId);
            return _mapper.Map<IEnumerable<ApprovalHistoryMaterialPostDTO>>(approvalHistories);
        }
    }
}
