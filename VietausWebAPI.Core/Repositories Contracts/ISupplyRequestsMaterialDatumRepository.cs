

using VietausWebAPI.Core.Domain.Entities;

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
        /// Lấy đề xuất theo id
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        Task<SupplyRequestsMaterialDatum> GetWithId(string requestId);
        /// <summary>
        /// Cập nhật trạng thái đề xuất
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="requestStatus"></param>
        /// <returns></returns>
        Task UpdateRequestStatusAsyncRepository(string requestId, string requestStatus);

        /// <summary>
        /// Cập nhật trạng thái đề xuất thành công
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="note"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task SuccessRequestStatusAsyncRepository(string requestId, string note, string status);
    }
}
