using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.GetDtos;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;

namespace VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature.ColorChipRecordFeatures
{
    public interface IColorChipRecordReadRepositories
    {
        /// <summary>
        /// Lấy thông tin bản ghi ColorChipRecord theo ID. Nếu không tìm thấy, trả về null.
        /// </summary>
        /// <param name="colorChipRecordId">ID của bản ghi ColorChipRecord cần lấy thông tin.</param>
        /// <param name="cancellationToken">Token hủy bỏ tác vụ.</param>
        /// <returns>Trả về bản ghi ColorChipRecord nếu tìm thấy, ngược lại trả về null.</returns>
        Task<ColorChipRecord?> GetByIdAsync(
            Guid colorChipRecordId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Kiểm tra bản ghi cho product đã tồn tại hay chưa. Nếu đã tồn tại, sẽ không cho phép tạo mới nữa.
        /// </summary>
        /// <param name="productId">ID của sản phẩm cần kiểm tra.</param>
        /// <param name="cancellationToken">Token hủy bỏ tác vụ.</param>
        /// <returns>Trả về true nếu bản ghi đã tồn tại, ngược lại trả về false.</returns>
        public Task<bool> CheckExisting(Guid productId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy thông tin bản ghi ColorChipRecord theo ProductId. Nếu không tìm thấy, trả về null.
        /// </summary>
        /// <param name="productId">ID của sản phẩm cần lấy thông tin.</param>
        /// <param name="cancellationToken">Token hủy bỏ tác vụ.</param>
        /// <returns>Trả về bản ghi ColorChipRecord nếu tìm thấy, ngược lại trả về null.</returns>
        Task<ColorChipRecord?> GetByProductIdAsync(
            Guid productId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy thông tin bản ghi ColorChipRecord theo ProductId để phục vụ cho việc xuất PDF. Nếu không tìm thấy, trả về null.
        /// </summary>
        /// <param name="productId">ID của sản phẩm cần lấy thông tin.</param>
        /// <param name="cancellationToken">Token hủy bỏ tác vụ.</param>
        /// <returns>Trả về dữ liệu PDF của bản ghi ColorChipRecord nếu tìm thấy, ngược lại trả về null.</returns>
        Task<ColorChipRecordPdfQueryResult?> GetPdfDataByProductIdAsync(
            Guid productId,
            CancellationToken cancellationToken = default);
    }
}
