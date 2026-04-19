using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts.ColorChipManufacturingRecords
{
    public interface IColorChipManufacturingRecordWriteRepository : IRepository<ColorChipManufacturingRecord>
    {
        /// <summary>
        /// Tạo mới một ColorChipManufacturingRecord
        /// </summary>
        /// <param name="entity">Thực thể ColorChipManufacturingRecord cần tạo</param>
        /// <param name="ct">Token hủy bỏ</param>
        /// <returns>Thực thể ColorChipManufacturingRecord đã được tạo</returns>
        Task<ColorChipManufacturingRecord> CreateAsync(
            ColorChipManufacturingRecord entity,
            CancellationToken ct = default);
    }
}
