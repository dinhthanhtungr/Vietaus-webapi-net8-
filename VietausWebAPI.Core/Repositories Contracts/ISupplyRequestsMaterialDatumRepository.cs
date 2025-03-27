

using VietausWebAPI.Core.Entities;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface ISupplyRequestsMaterialDatumRepository
    {
        /// <summary>
        /// Thêm đề xuất mới
        /// </summary>
        /// <param name="supplyRequestsMaterialData"></param>
        /// <returns></returns>
        Task AddSupplyRequestsMaterialDatumRepository(List<SupplyRequestsMaterialDatum> supplyRequestsMaterialData);
        /// <summary>
        /// Lấy tất cả đề xuất
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SupplyRequestsMaterialDatum>> GetAllSupplyRequestsMaterialDatumRepository();
        /// <summary>
        /// Cập nhật trạng thái đề xuất
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="requestStatus"></param>
        /// <returns></returns>
        Task UpdateRequestStatusAsyncRepository(string requestId, string requestStatus);
    }
}
