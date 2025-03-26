using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO.PostDTO;

namespace VietausWebAPI.Core.ServiceContracts
{
    public interface IApprovalHistoryMaterialService
    {
        public Task AddApprovalHistoryMaterialServiceAsync(ApprovalHistoryMaterialPostDTO approvalHistoryMaterialPostDTO);
        public Task<IEnumerable<ApprovalHistoryMaterialPostDTO>> GetApprovalHistoryMaterialServiceAsync(string requestId);
    }
}
