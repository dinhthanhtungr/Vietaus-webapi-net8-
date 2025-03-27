using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.DTO.QueryObject;


namespace VietausWebAPI.Core.ServiceContracts
{
    public interface IInventoryReceiptsService
    {
        /// <summary>
        /// Thêm mới danh sách phiếu nhập kho
        /// </summary>
        /// <param name="inventoryReceiptsDTO"></param>
        /// <returns></returns>
        Task AddInventoryReceiptsServiceAsync(InventoryReceiptsDTO inventoryReceiptsDTO);
        /// <summary>
        /// Lấy tất cả danh sách phiếu nhập kho
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<InventoryReceiptsGetDTO>> GetAllInventoryReceiptsServiceAsync();
        /// <summary>
        /// Tìm kiếm danh sách phiếu nhập kho theo các tiêu chí tìm kiếm và trả về kết quả phân trang
        /// </summary>
        /// <param name="inventoryReceiptsQuery"></param>
        /// <returns></returns>
        Task<PagedResult<InventoryReceiptsGetDTO>> AddInventoryReceiptsServiceAsync(InventoryReceiptsQuery inventoryReceiptsQuery);
        /// <summary>
        /// Cập nhật giá cho danh sách phiếu nhập kho
        /// </summary>
        /// <param name="inventoryReceiptsUpdatePriceDTO"></param>
        /// <returns></returns>
        Task UpdateInventoryReceiptsServiceAsync(InventoryReceiptsUpdatePriceDTO inventoryReceiptsUpdatePriceDTO);
    }
}
