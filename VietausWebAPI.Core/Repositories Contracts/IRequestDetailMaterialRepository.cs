using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Entities;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface IRequestDetailMaterialRepository
    {
        Task AddRequetMaterialRepository(IEnumerable<RequestDetailMaterialDatum> requestDetailMaterialDatum);
        Task<IEnumerable<RequestDetailMaterialDatum>> GetAllRequestMaterialRepository();
        //Task UpdateRequestDetailStatusRepository(string requestId, string requestStatus);
    }
}
