using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Core.Entities;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface IRequestMaterialRepository
    {
        Task<SupplyRequestsMaterialDatum> CreateRequestAsync(SupplyRequestsMaterialDatum request);
        Task AddRequestDetailMaterialAsync(List<RequestDetailMaterialDatum> requestDetail);
        Task<SupplyRequestsMaterialDatum> GetLastRequestIdRepository();

        Task<IEnumerable<SupplyRequestsMaterialDatum>> GetRequestRepository(RequestMaterialQuery query);
    }
}
