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
        public Task AddApprovalHistoryMaterialRepositoryAsync(IEnumerable<ApprovalHistoryMaterialDatum> approvalLevelsCommonData);
        public Task<IEnumerable<ApprovalHistoryMaterialDatum>> GetApprovalHistoryMaterialRepositoryAsync();
    }
}
