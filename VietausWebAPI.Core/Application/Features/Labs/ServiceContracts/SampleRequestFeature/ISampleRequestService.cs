using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest;
using VietausWebAPI.Core.Application.Features.Labs.Queries.CreateSampleRequest;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.TransferCustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature
{
    public interface ISampleRequestService
    {
        /// <summary>
        /// Tạo mới một yêu cầu mẫu với sản phẩm liên kết.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult> CreateAsync(CreateSampleWithProductRequest req, CancellationToken ct = default);

        /// <summary>
        /// Lấy danh sách các yêu cầu mẫu với phân trang và lọc.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PagedResult<SampleRequestSummaryDTO>> GetAllAsync(
         SampleRequestQuery query,
         CancellationToken ct = default);

        /// <summary>
        /// Lấy thông tin chi tiết của một yêu cầu mẫu theo ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<GetSampleWithProductRequest> GetByIdAsync(Guid id, CancellationToken ct = default);

        /// <summary>
        /// Xóa mẫu theo điều kiện
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<OperationResult> DeleteSampleRequestAsync(Guid id);

        /// <summary>
        /// Thay đổi thông tin của một yêu cầu mẫu đã tồn tại.
        /// </summary>
        /// <param name="sampleRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult> UpdateSampleRequestAsync(UpdateSampleRequest sampleRequest, CancellationToken ct = default);
    }
}
