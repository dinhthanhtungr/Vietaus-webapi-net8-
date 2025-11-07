using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.MerchandiseOrderFeatures
{
    public interface IMerchandiseOrderService
    {
        /// <summary>
        /// Tạo đơn hàng mới
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult<Guid>> CreateAsync(PostMerchandiseOrder req, CancellationToken ct = default);

        /// <summary>
        /// Lấy danh sách đơn hàng với phân trang và lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PagedResult<GetMerchadiseOrder>> GetAllAsync(
         MerchandiseOrderQuery query,
         CancellationToken ct = default);

        /// <summary>
        /// Lấy đơn hàng theo Id
        /// </summary>
        /// <param name="merchandiseOrderId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<GetMerchadiseOrderWithId?> GetByIdAsync(Guid merchandiseOrderId, CancellationToken ct = default);

        /// <summary>
        /// Cập nhật thông tin đơn hàng mới
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult> UpdateInformationAsync(PatchMerchandiseOrderInformation req, CancellationToken ct = default);

        /// <summary>
        /// Lấy thông tin đơn hàng của khách hàng này theo lần bán gần nhất
        /// </summary>
        /// <param name="merchandiseOrderId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<GetOldProductInformation?> GetLastMerchandiseOrderByCustomerIdAsync(Guid customerId, Guid productId, CancellationToken ct = default);

        /// <summary>
        /// Cập nhật trạng thái duyệt đơn hàngm
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<OperationResult> UpdateApproveStatus(PatchMerchandiseOrderInformation query, CancellationToken ct = default);

        /// <summary>
        /// Xóa mềm đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult> SoftDelete(PatchMerchandiseOrderInformation query, CancellationToken ct = default);

    }
}
