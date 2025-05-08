using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.Entities;

namespace VietausWebAPI.Core.ServiceContracts
{
    public interface ISupplyRequestsMaterialDatumService
    {
        /// <summary>
        /// Thêm đề xuất mới
        /// </summary>
        /// <param name="supplyRequestsMaterialDatumDTO"></param>
        /// <returns></returns>
        Task AddSupplyRequestsMaterialDatumAsync(SupplyRequestsMaterialDatumDTO supplyRequestsMaterialDatumDTO);
        /// <summary>
        /// Lấy tất cả đề xuất
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SupplyRequestsMaterialDatumDTO>> GetAllSupplyRequestsMaterialDatumAsync();
        /// <summary>
        /// Cập nhật trạng thái đề xuất
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="requestStatus"></param>
        /// <returns></returns>
        Task UpdateRequestStatusAsyncService(string requestId, string requestStatus);
        /// <summary>
        /// Cập nhật trạng thái đề xuất và thêm mới phiếu nhập kho
        /// </summary>
        /// <param name="inventoryReceiptsMaterialDatum"></param>
        /// <param name="requestId"></param>
        /// <param name="requestStatus"></param>
        /// <returns></returns>
        Task ApproveAndUpdateAsync(ApproveReceiptDTO inventoryReceiptsMaterialDatum);

        /// <summary>
        /// Cập nhật trạng thái đề xuất thành công
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="note"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task SuccessRequestStatusAsyncService(string requestId, string note, string status);

    }
}
