using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices;
using VietausWebAPI.Core.Application.Features.Warehouse.Queries;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts
{
    public interface IWarehouseReadService
    {
        /// <summary>
        /// Tổng tồn thực(kg) trong Warehouse theo NVL(và theo lot nếu truyền lotKey).
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //Task<decimal> GetOnHandAsync(WarehouseReadServiceQuery query, CancellationToken cancellationToken);

        /// <summary>
        /// Tổng đang giữ chỗ (OPEN) trong TempStockTable/Reserve
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //Task<decimal> GetReservedOpenAsync(WarehouseReadServiceQuery query, CancellationToken cancellationToken);

        /// <summary>
        /// Khả dụng realtime = OnHand − Reserved(OPEN) 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //Task<decimal> GetAvailableAsync(WarehouseReadServiceQuery query, CancellationToken cancellationToken);


       Task<OperationResult<PagedResult<GetStockAvaiable>>> GetStockAvailableAsync(WarehouseReadServiceQuery query);


        /// <summary>
        /// Dùng để hiển thị tồn khả dụng của các NVL trong công thức sản xuất (ManufacturingFormulaId)
        /// </summary>
        /// <param name="manufacturingFormulaId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<VaAvailabilityVm>> GetVaAvailabilityAsync(
                                Guid manufacturingFormulaId,
                                CancellationToken ct = default);


        /// <summary>
        /// Lấy danh sách tồn kho của sản phẩm ProductAvailabilityVm theo danh sách productExternalIds
        /// </summary>
        /// <param name="productExternalIds"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<ProductAvailabilityVm>> GetProductAvailabilityVmsAsync(
                        List<string> productExternalIds,
                        CancellationToken ct = default);

        //IInventoryReadService — đọc tồn

        //Dùng để hiển thị và kiểm tra nhanh tồn.

        //GetOnHandAsync(companyId, code, lotKey)
        //→ Tổng tồn thực(kg) trong Hometown theo NVL(và theo lot nếu truyền lotKey).

        //GetReservedOpenAsync(companyId, code, lotKey)
        //→ Tổng đang giữ chỗ(OPEN) trong TempStockTable/Reserve.

        //GetAvailableAsync(companyId, code, lotKey)
        //→ Khả dụng realtime = OnHand − Reserved(OPEN).

        //Dùng khi: render màn lập công thức, kiểm tra vượt tồn, popup chọn lot, báo cáo nhanh.

        // -----------------------------------------------------------------------------------------------------------------------------------

        //ISnapshotService — chụp “tồn lúc lập công thức”

        //Tạo một SnapshotSet cho 1 VA (VD: “VA1”) và sinh ra các dòng TempType = Snapshot.

        //CreateSnapshotAsync(companyId, vaCode, materialCodes, createdBy)

        //Tạo SnapshotSet(gắn VaCode, SnapshotAtUtc).

        //Với mỗi materialCode, đọc OnHand và chèn 1 dòng SNAPSHOT(Code, QtyStock).
        //⇒ Trả về SnapshotSetId để các thao tác tiếp theo(đặt chỗ, xuất…) bám vào.

        //Dùng khi: người kỹ thuật mở/lưu công thức VA lần đầu hoặc tạo phiên bản mới của công thức (để “đóng băng” tồn thời điểm đó).
        //Lưu ý: Snapshot là lịch sử, không cập nhật đè; muốn snapshot mới → tạo SnapshotSetId mới.

        // -----------------------------------------------------------------------------------------------------------------------------------

        //IReservationService — giữ chỗ cho lệnh(VA)

        //Quản vòng đời đặt chỗ: OPEN → (CONSUMED | CANCELLED).

        //ReserveAsync(companyId, snapshotSetId, vaCode, vaLineCode, code, lotKey?, qtyRequest, createdBy)
        //→ Chèn 1 dòng TempType = Reserve, ReserveStatus = Open.
        //Dùng để “trừ tạm” khi công thức/đơn VA yêu cầu NVL.

        //CancelReserveAsync(companyId, snapshotSetId, vaCode, code, lotKey?)
        //→ Chuyển các reserve OPEN → CANCELLED(trả lại khả dụng).
        //Dùng khi: hủy VA, đổi công thức, giảm nhu cầu.

        //ConsumeReserveAsync(companyId, snapshotSetId, vaCode, code, lotKey?, issueId)
        //→ Chuyển các reserve OPEN → CONSUMED và gắn LinkedIssueId.
        //Dùng ngay sau khi kho POST phiếu xuất thực tương ứng (động tác chốt).

        //Quy tắc:

        //Chỉ OPEN mới được CANCELLED/CONSUMED.

        //CONSUMED/CANCELLED không quay lại OPEN.

        //Tổng Reserved(OPEN) được dùng để tính Available.

        // ======================================================================== Helper ======================================================================== 

        /// <summary>
        /// Lấy dictionary thông tin tồn kho VA của các NVL trong công thức sản xuất
        /// </summary>
        /// <param name="manufacturingFormulaId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<Dictionary<string, VaAvailabilityVm>> GetVaAvailabilityDictAsync(
            Guid manufacturingFormulaId,
            CancellationToken ct = default);
    }
}
