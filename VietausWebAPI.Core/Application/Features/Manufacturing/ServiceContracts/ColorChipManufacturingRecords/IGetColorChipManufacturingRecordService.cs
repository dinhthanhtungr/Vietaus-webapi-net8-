using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.ColorChipManufacturingRecords.GetDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.ColorChipManufacturingRecords;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts.ColorChipManufacturingRecords
{
    public interface IGetColorChipManufacturingRecordService
    {
        /// <summary>
        /// Lấy thông tin chi tiết của một bản ghi sản xuất chip màu dựa trên ID. Chỉ trả về bản ghi nếu nó thuộc công ty của người dùng hiện tại.
        /// </summary>
        /// <param name="colorChipMfgRecordId">ID của bản ghi sản xuất chip màu</param>
        /// <param name="ct">Token hủy bỏ</param>
        /// <returns>Kết quả chứa thông tin chi tiết của bản ghi hoặc thông báo lỗi nếu không tìm thấy</returns>
        Task<OperationResult<GetColorChipManufacturingRecord>> GetByIdAsync(
            Guid colorChipMfgRecordId,
            CancellationToken ct = default);

        /// <summary>
        /// Lấy danh sách các bản ghi sản xuất chip màu theo điều kiện lọc và phân trang. Chỉ trả về các bản ghi thuộc công ty của người dùng hiện tại.
        /// </summary>
        /// <param name="query">Điều kiện lọc và phân trang</param>
        /// <param name="ct">Token hủy bỏ</param>
        /// <returns>Kết quả chứa danh sách các bản ghi hoặc thông báo lỗi nếu không tìm thấy</returns>
        Task<OperationResult<PagedResult<GetColorChipManufacturingRecordSummary>>> GetPagedAsync(
            ColorChipManufacturingRecordQuery query,
            CancellationToken ct = default);
    }
}
