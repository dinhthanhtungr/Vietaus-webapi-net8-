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
        /// <summary>
        /// Thêm lịch sử phê duyệt vật tư
        /// </summary>
        /// <param name="approvalHistoryMaterialPostDTO"></param>
        /// <returns></returns>
        public Task AddApprovalHistoryMaterialServiceAsync(ApprovalHistoryMaterialPostDTO approvalHistoryMaterialPostDTO);
        /// <summary>
        /// Lấy lịch sử phê duyệt vật tư
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public Task<IEnumerable<ApprovalHistoryMaterialPostDTO>> GetApprovalHistoryMaterialServiceAsync(string requestId);
    }
}
