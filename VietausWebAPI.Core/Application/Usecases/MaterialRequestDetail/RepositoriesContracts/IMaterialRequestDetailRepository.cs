using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Usecases.MaterialRequestDetail.RepositoriesContracts
{
    public interface IMaterialRequestDetailRepository
    {
        Task AddRequetMaterialRepository(IEnumerable<RequestDetailMaterialDatum> requestDetailMaterialDatum);
        Task<IEnumerable<RequestDetailMaterialDatum>> GetRequestMaterialRepository(string requestId);
    }
}
