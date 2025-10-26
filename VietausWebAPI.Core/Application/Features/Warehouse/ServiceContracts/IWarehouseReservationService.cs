using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseWriteServices;
using VietausWebAPI.Core.Application.Features.Warehouse.Queries;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

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
        Task<OperationResult> ReserveAvailabilityAsync(CreateVaSnapshotAndReservations query, CancellationToken ct);



        /// <summary>
        /// Chèn 1 dòng TempType=Reserve, ReserveStatus=Open.
        /// Dùng để “trừ tạm” khi công thức/đơn VA yêu cầu NVL.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        //Task<long> ReserveAsync(WarehouseReservationServiceQuery query, CancellationToken ct);




        /// <summary>
        /// Chuyển các reserve OPEN → CANCELLED (trả lại khả dụng).
        /// Dùng khi: hủy VA, đổi công thức, giảm nhu cầu.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        //Task<int> CancelReserveAsync(WarehouseReservationServiceQuery query, CancellationToken ct);

        /// <summary>
        /// Chuyển các reserve OPEN → CONSUMED và gắn LinkedIssueId.
        /// Dùng ngay sau khi kho POST phiếu xuất thực tương ứng(động tác chốt).
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        //Task<int> ConsumeReserveAsync(WarehouseReservationServiceQuery query, CancellationToken ct);

    }
}
