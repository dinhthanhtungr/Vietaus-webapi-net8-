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
        private readonly IApprovalHistoryMaterialRepository _approvalHistoryMaterialRepository;
        private readonly IMapper _mapper;

        public ApprovalHistoryMaterialService (IApprovalHistoryMaterialRepository approvalHistoryMaterialRepository, IMapper mapper)
        {
            _approvalHistoryMaterialRepository = approvalHistoryMaterialRepository;
            _mapper = mapper;
        }

        public async Task AddApprovalHistoryMaterialServiceAsync(ApprovalHistoryMaterialPostDTO approvalHistoryMaterialPostDTOs)
        {
            var result = _mapper.Map<IEnumerable<ApprovalHistoryMaterialDatum>>(approvalHistoryMaterialPostDTOs);
            await _approvalHistoryMaterialRepository.AddApprovalHistoryMaterialRepositoryAsync(result);
        }

        public async Task<IEnumerable<ApprovalHistoryMaterialPostDTO>> GetApprovalHistoryMaterialServiceAsync()
        {
            var approvalHistoryMaterial = _approvalHistoryMaterialRepository.GetApprovalHistoryMaterialRepositoryAsync();
            var result = _mapper.Map<IEnumerable<ApprovalHistoryMaterialPostDTO>>(approvalHistoryMaterial);
            return result;
        }
    }
}
