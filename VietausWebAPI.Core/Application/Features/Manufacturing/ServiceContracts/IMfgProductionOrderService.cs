using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts
{
    public interface IMfgProductionOrderService
    {
        /// <summary>
        /// Tạo đơn hàng mới
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult> CreateAsync(PostMfgProductionOrder req, CancellationToken ct = default);

        /// <summary>
        /// Lấy danh sách đơn hàng với phân trang và lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PagedResult<GetSummaryMfgProductionOrder>> GetAllAsync(
         MfgProductionOrderQuery query,
         CancellationToken ct = default);

        /// <summary>
        /// Lấy danh sách đơn hàng với phân trang và lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PagedResult<GetSampleMfgFormula>> GetAllMfgFormulaAsync(
         MfgProductionOrderQuery query,
         CancellationToken ct = default);

        /// <summary>
        /// Lấy danh sách công thức và lệnh sản xuất với phân trang và lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PagedResult<GetFormulaAndMfgFormula>> GetFormulaAndMfgFormulaAsync(
         FormulaAndMfgFormulaQuery query,
         CancellationToken ct = default);

        /// <summary>
        /// Lấy thông tin của cụ thể một lệnh sản xuất
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<GetMfgProductionOrder?> GetByIdAsync(
         Guid id,
         CancellationToken ct = default);

        /// <summary>
        /// Cập nhật thông tin đơn hàng mới
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult> UpdateInformationAsync(PatchMfgProductionOrderInformation req, CancellationToken ct = default);
    }
}
