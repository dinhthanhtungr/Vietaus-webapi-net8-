

using VietausWebAPI.Core.Entities;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface IInventoryReceiptsRepository
    {
        Task AddInventoryReceiptsRepositoryAsync(List<InventoryReceiptsMaterialDatum> inventoryReceiptsMaterialDatum);
        Task<IEnumerable<InventoryReceiptsMaterialDatum>> GetAllInventoryReceiptsRepositoryAsync();
    }
}
