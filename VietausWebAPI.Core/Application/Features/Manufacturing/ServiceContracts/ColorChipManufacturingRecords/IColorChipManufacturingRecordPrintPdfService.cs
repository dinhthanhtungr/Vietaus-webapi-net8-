using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts.ColorChipManufacturingRecords
{
    public interface IColorChipManufacturingRecordPrintPdfService
    {
        /// <summary>
        /// In PDF cho bản ghi sản xuất color chip dựa trên ID của nó. Kết quả trả về là một mảng byte đại diện cho nội dung PDF đã được tạo ra. Mảng byte này có thể được sử dụng để lưu trữ, gửi qua mạng hoặc hiển thị trực tiếp dưới dạng PDF.
        /// </summary>
        /// <param name="colorChipMfgRecordId">ID của bản ghi sản xuất color chip cần in PDF.</param>
        /// <param name="ct">Token hủy bỏ tác vụ.</param>
        /// <returns>Một đối tượng OperationResult chứa mảng byte của PDF.</returns>
        Task<OperationResult<byte[]>> PrintByIdAsync(
                Guid colorChipMfgRecordId,
                CancellationToken ct = default);
    }
}
