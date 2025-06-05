using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.MaterialRequestDetails.Query;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.DTO.QueryObject;

namespace VietausWebAPI.Core.Application.Usecases.MaterialRequestDetail.RepositoriesContracts
{
    public interface IMaterialRequestDetailRepository
    {
        Task AddRequetMaterialRepository(IEnumerable<RequestDetailMaterialDatum> requestDetailMaterialDatum);
        Task<IEnumerable<RequestDetailMaterialDatum>> GetRequestMaterialRepository(string requestId);
        Task<PagedResult<RequestDetailMaterialDatum>> GetRequestMaterialStatusPayRepository(CreatePOQuery createPOQuery);
        Task<IEnumerable<RequestDetailMaterialDatum>> GetOpenRequestsByMaterialIdRepository(Guid materialId);
        Task UpdatePurchasedQuantityAsync(int detailId, int quantityToAdd);

        Task RollbackPurchasedQuantityAsync(Guid materialId, int quantityToRollback);

    }
}
