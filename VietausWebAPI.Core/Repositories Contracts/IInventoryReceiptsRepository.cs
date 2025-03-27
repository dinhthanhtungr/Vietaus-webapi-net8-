using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Core.Entities;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface IInventoryReceiptsRepository
    {
        Task AddInventoryReceiptsRepositoryAsync(List<InventoryReceiptsMaterialDatum> inventoryReceiptsMaterialDatum);
        Task<IEnumerable<InventoryReceiptsMaterialDatum>> GetAllInventoryReceiptsRepositoryAsync();
        Task<PagedResult<InventoryReceiptsMaterialDatum>> SearchInventoryReceiptsRepositoryAsync(InventoryReceiptsQuery query);
        //Task<IEnumerable<InventoryReceiptsMaterialDatum>> SearchInventoryReceiptsRepositoryAsync(InventoryReceiptsQuery query);

        Task<bool> UpdateInventoryReceiptsRepositoryAsync(List<InventoryReceiptsMaterialDatum> inventoryReceiptsMaterialDatum);
    }
}
