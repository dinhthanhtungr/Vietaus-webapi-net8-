using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Core.Entities;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface IInventoryReceiptsRepository
    {
        /// <summary>
        /// Thêm mới danh sách phiếu nhập kho
        /// </summary>
        /// <param name="inventoryReceiptsMaterialDatum"></param>
        /// <returns></returns>
        Task AddInventoryReceiptsRepositoryAsync(List<InventoryReceiptsMaterialDatum> inventoryReceiptsMaterialDatum);
        /// <summary>
        /// Lấy tất cả danh sách phiếu nhập kho
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<InventoryReceiptsMaterialDatum>> GetAllInventoryReceiptsRepositoryAsync();
        /// <summary>
        /// Tìm kiếm danh sách phiếu nhập kho theo các tiêu chí tìm kiếm và trả về kết quả phân trang
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedResult<InventoryReceiptsMaterialDatum>> SearchInventoryReceiptsRepositoryAsync(InventoryReceiptsQuery query);
        /// <summary>
        /// Cập nhật danh sách phiếu nhập kho
        /// </summary>
        /// <param name="inventoryReceiptsMaterialDatum"></param>
        /// <returns></returns>
        Task UpdateInventoryReceiptsRepositoryAsync(InventoryReceiptsUpdatePriceDTO inventoryReceiptsUpdatePriceDTO);
    }
}
