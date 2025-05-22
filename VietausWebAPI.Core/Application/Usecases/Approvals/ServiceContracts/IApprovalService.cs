using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.Approval;
using VietausWebAPI.Core.Application.DTOs.SupplyRequests;
using VietausWebAPI.Core.DTO.QueryObject;

namespace VietausWebAPI.Core.Application.Usecases.Approvals.ServiceContracts
{
    public interface IApprovalService
    {
        public Task SaveApprovalRequestService(ApprovalRequestDTO approvalRequestDTO);
        public Task<PagedResult<ApprovalResponceListDTO>> SendApprovalRequestService(SupplyRequestsQuery approvalRequestDTO);
    }
}
