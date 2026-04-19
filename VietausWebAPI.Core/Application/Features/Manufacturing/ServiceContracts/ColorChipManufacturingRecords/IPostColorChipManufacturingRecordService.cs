using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.ColorChipManufacturingRecords.GetDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.ColorChipManufacturingRecords.PostDtos;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts.ColorChipManufacturingRecords
{
    public interface IPostColorChipManufacturingRecordService
    {
        /// <summary>
        /// Tạo một bản ghi sản xuất chip màu mới. Chỉ tạo bản ghi nếu nó thuộc công ty của người dùng hiện tại.
        /// </summary>
        /// <param name="request">Yêu cầu chứa thông tin để tạo bản ghi mới</param>
        /// <param name="ct">Token hủy bỏ</param>
        /// <returns>Kết quả chứa thông tin chi tiết của bản ghi mới hoặc thông báo lỗi nếu không thể tạo</returns>
        Task<OperationResult<GetColorChipManufacturingRecord>> CreateAsync(
            PostColorChipManufacturingRecordRequest request,
            CancellationToken ct = default);
    }
}
