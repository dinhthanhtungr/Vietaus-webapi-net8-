using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.Entities;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface IRequestMaterialRepository
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync(IDbContextTransaction transaction);
        Task RollbackAsync(IDbContextTransaction transaction);
        Task<SupplyRequestsMaterialDatum> CreateRequestAsync(SupplyRequestsMaterialDatum request);
        Task AddRequestDetailMaterialAsync(List<RequestDetailMaterialDatum> requestDetail);
        Task<SupplyRequestsMaterialDatum> GetLastRequestIdRepository();

        Task<IEnumerable<SupplyRequestsMaterialDatum>> GetRequestRepository(RequestMaterialQuery query);
    }
}
