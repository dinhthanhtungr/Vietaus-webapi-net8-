using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.GetDtos;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature.ColorChipRecordFeatures
{
    public interface IColourChipRecordReadServices
    {
        /// <summary>
        /// Lấy dữ liệu ColorChipRecord theo Id. Nếu không tìm thấy, trả về lỗi "Không tìm thấy ColorChipRecord."
        /// </summary>
        /// <param name="colorChipRecordId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<OperationResult<GetColorChipRecordDto>> GetByIdAsync(
            Guid colorChipRecordId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Lấy dữ liệu ColorChipRecord theo ProductId. Nếu không tìm thấy, trả về lỗi "Không tìm thấy ColorChipRecord cho sản phẩm này."
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<OperationResult<GetColorChipRecordDto>> GetByProductIdAsync(
            Guid productId,
            CancellationToken cancellationToken = default);
    }
}
