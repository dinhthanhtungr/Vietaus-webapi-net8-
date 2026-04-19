using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.ColorChipManufacturingRecords.GetDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.ColorChipManufacturingRecords.PdfDtos;
using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts.ColorChipManufacturingRecords
{
    public interface IColorChipManufacturingRecordReadRepository : IRepository<ColorChipManufacturingRecord>
    {
        /// <summary>
        /// Lấy ColorChipManufacturingRecord theo Id
        /// </summary>
        /// <param name="colorChipMfgRecordId"></param>
        /// <param name="track"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ColorChipManufacturingRecord?> GetByIdAsync(
            Guid colorChipMfgRecordId,
            bool track = false,
            CancellationToken ct = default);

        /// <summary>
        /// Lấy dữ liệu cần thiết để in PDF cho bản ghi sản xuất color chip dựa trên ID của nó. Dữ liệu trả về được đóng gói trong một đối tượng ColorChipManufacturingRecordPdfData, chứa tất cả thông tin cần thiết để tạo ra PDF cho bản ghi sản xuất color chip đó. Nếu không tìm thấy bản ghi nào phù hợp với ID đã cho hoặc nếu bản ghi không còn hoạt động, phương thức sẽ trả về null.
        /// </summary>
        /// <param name="colorChipId">Id của bản ghi color chip</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ColorChipManufacturingRecordPdfData?> GetPdfDataByIdAsync(
            Guid colorChipId,
            CancellationToken ct = default);

        Task<ColorChipManufacturingRecord?> GetActiveByIdForUpdateAsync(
            Guid colorChipId,
            CancellationToken ct = default);

        Task<GetColorChipManufacturingRecord?> GetDetailByIdAsync(
            Guid colorChipId,
            CancellationToken ct = default);
    }
}
