using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.Approval;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.DTO.QueryObject;

namespace VietausWebAPI.Core.Application.Usecases.Approvals.RepositoriesContracts
{
    public interface IApprovalRepository
    {
        public Task SaveApprovalHistoryHandler(ApprovalHistoryMaterialDatum approvalHistoryMaterialDatum);
    }
}
