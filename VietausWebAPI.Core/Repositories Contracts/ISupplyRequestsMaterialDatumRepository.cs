

using VietausWebAPI.Core.Entities;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface ISupplyRequestsMaterialDatumRepository
    {
        Task AddSupplyRequestsMaterialDatumRepository(List<SupplyRequestsMaterialDatum> supplyRequestsMaterialData);
        Task<IEnumerable<SupplyRequestsMaterialDatum>> GetAllSupplyRequestsMaterialDatumRepository();

        Task UpdateRequestStatusAsyncRepository(string requestId, string requestStatus);
    }
}
