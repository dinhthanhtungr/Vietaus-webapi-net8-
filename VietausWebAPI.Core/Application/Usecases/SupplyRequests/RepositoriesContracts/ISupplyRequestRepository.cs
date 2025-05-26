using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.Approval;
using VietausWebAPI.Core.Application.DTOs.SupplyRequests;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.DTO.QueryObject;

namespace VietausWebAPI.Core.Application.Usecases.SupplyRequests.RepositoriesContracts
{
    public interface ISupplyRequestRepository
    {
        public Task<PagedResult<SupplyRequestsMaterialDatum>> GetSupplyRequestRepository(SupplyRequestsQuery query);
        Task<SupplyRequestsMaterialDatum> CreateRequestAsync(SupplyRequestsMaterialDatum request);
    }
}
