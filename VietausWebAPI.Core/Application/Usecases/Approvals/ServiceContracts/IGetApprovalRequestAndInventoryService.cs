using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.Approval;
using VietausWebAPI.Core.DTO.PostDTO;

namespace VietausWebAPI.Core.Application.Usecases.Approvals.ServiceContracts
{
    public interface IGetApprovalRequestAndInventoryService
    {
        public Task ExecuteAsync(ApprovalHistoryAndInventoryRequestDTO approvalHistoryAndInventoryRequestDTO);
    }
}
