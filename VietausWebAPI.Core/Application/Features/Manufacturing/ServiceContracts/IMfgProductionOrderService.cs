using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.MerchandiseOrderDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Application.Shared.Models.SaleAndMfgs;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;

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
        //Task<OperationResult> CreateWithMerchadiseOrderAsync(MerchandiseOrder req, CancellationToken ct = default);

        /// <summary>
        /// Lấy danh sách đơn hàng với phân trang và lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult<PagedResult<GetSummaryMfgProductionOrder>>> GetAllAsync( MfgProductionOrderQuery query, CancellationToken ct = default);

        /// <summary>
        /// Lấy danh sách công thức theo lệnh sản xuất với phân trang và lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PagedResult<GetSampleMfgFormula>> GetAllMfgFormulaAsync( MfgProductionOrderQuery query, CancellationToken ct = default);

        /// <summary>
        /// Lấy danh sách công thức và lệnh sản xuất với phân trang và lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns> 
        Task<PagedResult<GetFormulaAndMfgFormula>> GetFormulaAndMfgFormulaAsync( FormulaAndMfgFormulaQuery query, CancellationToken ct = default);

        /// <summary>
        /// Lấy thông tin của cụ thể một lệnh sản xuất
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult<GetMfgProductionOrder>> GetByIdAsync( Guid id, CancellationToken ct = default);

        Task<OperationResult<Guid>> CreateInternalAsync(CreateMfgProductionOrderInternal req, CancellationToken ct = default);

        /// <summary>
        /// Cập nhật thông tin theo đơn hàng mới
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult> UpdateInformationAsync(PatchMfgProductionOrderInformation req, CancellationToken ct = default);

        /// <summary>
        /// Phương thưc tạo lệnh sản xuất khi đơn hàng được duyệt, nằm ở service merchadiseOrder
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<MfgContext> BuildMfgContextAsync(OrderSlim mo, CancellationToken ct = default);
        /// <summary>
        /// Gom toàn bộ dữ liệu <b>read-only</b> cần thiết để tạo <b>nhiều</b> lệnh sản xuất cho một đơn hàng,
        /// nhằm tránh N+1 queries và đảm bảo nhất quán trong cùng transaction.
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="detail"></param>
        /// <param name="ctx"></param>
        /// <param name="actorId"></param>
        /// <param name="now"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<(MfgProductionOrder order,
              MfgOrderPO link)>
                             CreateOneMfgBundleAsync(
                 OrderSlim mo,
                 OrderDetailSlim detail,
                 MfgContext ctx,
                 Guid actorId, DateTime now, CancellationToken ct);
    }
}
