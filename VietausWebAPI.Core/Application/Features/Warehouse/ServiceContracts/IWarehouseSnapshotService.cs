using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.Queries;

namespace VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts
{
    public interface IWarehouseSnapshotService
    {
        /// <summary>
        /// Tạo một SnapshotSet cho 1 VA (VD: “VA1”) và sinh ra các dòng TempType=Snapshot.
        /// CreateSnapshotAsync(companyId, vaCode, materialCodes, createdBy)
        /// Tạo SnapshotSet (gắn VaCode, SnapshotAtUtc).
        /// Với mỗi materialCode, đọc OnHand và chèn 1 dòng SNAPSHOT (Code, QtyStock).
        /// ⇒ Trả về SnapshotSetId để các thao tác tiếp theo(đặt chỗ, xuất…) bám vào.
        /// Dùng khi: người kỹ thuật mở/lưu công thức VA lần đầu hoặc tạo phiên bản mới của công thức (để “đóng băng” tồn thời điểm đó).
        /// Lưu ý: Snapshot là lịch sử, không cập nhật đè; muốn snapshot mới → tạo SnapshotSetId mới.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        //Task<Guid> CreateSnapshotAsync(WarehouseSnapshotServiceQuery query, CancellationToken ct);
    }
}
