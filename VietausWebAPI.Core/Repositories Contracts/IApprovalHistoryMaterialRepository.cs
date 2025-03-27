using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Entities;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface IApprovalHistoryMaterialRepository
    {
        /// <summary>
        /// Thêm lịch sử phê duyệt vật tư
        /// </summary>
        /// <param name="approvalLevelsCommonData"></param>
        /// <returns></returns>
        public Task AddApprovalHistoryMaterialRepositoryAsync(ApprovalHistoryMaterialDatum approvalLevelsCommonData);
        /// <summary>
        /// Lấy lịch sử phê duyệt vật tư
        /// </summary>
        /// <param name="requestID"></param>
        /// <returns></returns>
        public Task<IEnumerable<ApprovalHistoryMaterialDatum>> GetApprovalHistoryMaterialRepositoryAsync(string requestID);
    }
}
