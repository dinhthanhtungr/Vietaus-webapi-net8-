using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.Approval;
using VietausWebAPI.Core.Application.DTOs.SupplyRequests;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Usecases.SupplyRequests.RepositoriesContracts
{
    public interface ISupplyRequestRepository
    {
        public Task<PagedResult<SupplyRequestsMaterialDatum>> GetSupplyRequestRepository(SupplyRequestsQuery query);
        Task<SupplyRequestsMaterialDatum> CreateRequestAsync(SupplyRequestsMaterialDatum request);
    }
}
