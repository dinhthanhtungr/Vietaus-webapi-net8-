using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.UpsertInformationDtos;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseWriteServices;
using VietausWebAPI.Core.Application.Features.Warehouse.Queries;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;

namespace VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts
{
    public interface IWarehouseReservationService
    {
        /// <summary>
        /// Chèn các dòng TempType=Reserve, ReserveStatus=Open.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        //Task<OperationResult> ReserveAvailabilityAsync(CreateVaSnapshotAndReservations query, CancellationToken ct);
        Task<OperationResult> ReserveAvailabilityAsync(CreateVaSnapshotAndReservations req, CancellationToken ct);

        // ======================================================================== Helper ========================================================================

        /// <summary>
        /// Helper tạo phiếu xuất kho CHO NVL theo yêu cầu của đơn VA / PO sản xuất.
        /// </summary>
        /// <param name="existing"></param>
        /// <param name="now"></param>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        //Task EnsureWarehouseIssueRequestAsync(
        //            MfgProductionOrder existing,
        //            DateTime now,
        //            Guid userId,
        //            Guid companyId,
        //            CancellationToken ct);


        /// <summary>
        /// Helper tạo phiếu xuất kho CHO Sản phẩm theo yêu cầu của đơn giao hàng.
        /// </summary>
        /// <param name="deliveryOrder"></param>
        /// <param name="now"></param>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task EnsureWarehouseRequestForDOAsync(
                    DeliveryOrder deliveryOrder,
                    DateTime now,
                    Guid userId,
                    Guid companyId,
                    CancellationToken ct);

        /// <summary>
        /// Helper xóa phiếu xuất kho CHO Sản phẩm theo yêu cầu của đơn giao hàng khi đơn giao hàng bị hủy hoặc sửa lại.
        /// </summary>
        /// <param name="ExternalId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task EnsureWarehouseRequestDeletedAsync(
                    string ExternalId,
                    CancellationToken ct);


        // ===============================================================================
        Task<OperationResult> SyncReservationsByFormulaItemsAsync(
                    Guid mfgProductionOrderId,
                    decimal totalQuantity,
                    IEnumerable<PatchMfgProductionOrderFormulaItemRequest> formulaItems,
                    CancellationToken ct = default);

        Task<OperationResult> ReserveByFormulaMaterialsAsync(
    Guid mfgProductionOrderId,
    decimal totalQuantity,
    IEnumerable<PostManufacturingFormulaMaterial> materials,
    CancellationToken ct = default);
    }
}
