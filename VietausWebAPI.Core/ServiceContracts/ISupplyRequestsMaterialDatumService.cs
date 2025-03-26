using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO.GetDTO;

namespace VietausWebAPI.Core.ServiceContracts
{
    public interface ISupplyRequestsMaterialDatumService
    {
        Task AddSupplyRequestsMaterialDatumAsync(SupplyRequestsMaterialDatumDTO supplyRequestsMaterialDatumDTO);
        Task<IEnumerable<SupplyRequestsMaterialDatumDTO>> GetAllSupplyRequestsMaterialDatumAsync();
        Task UpdateRequestStatusAsyncService(string requestId, string requestStatus);

    }
}
