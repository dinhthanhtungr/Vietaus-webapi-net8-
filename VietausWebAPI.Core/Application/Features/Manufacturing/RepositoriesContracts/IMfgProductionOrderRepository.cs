using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts
{
    public interface IMfgProductionOrderRepository
    {
        /// <summary>
        /// Tạo câu lệnh query để truy vấn đơn hàng từ cơ sở dữ liệu.
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        IQueryable<MfgProductionOrder> Query(bool track = false);

        /// <summary>
        /// Tạo một lịch sản xuất mới vào cơ sở dữ liệu.
        /// </summary>
        /// <param name="mfgProductionOrder"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task AddAsync(MfgProductionOrder mfgProductionOrder, CancellationToken ct = default);

        /// <summary>
        /// Tạo một nhóm lịch sản xuất mới vào cơ sở dữ liệu.
        /// </summary>
        /// <param name="mfgProductionOrder"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task AddRangeAsync(IEnumerable<MfgProductionOrder> mfgProductionOrders, CancellationToken ct = default);

        /// <summary>
        /// Lấy số cuối cùng của code
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        Task<string?> GetLatestExternalIdStartsWithAsync(string prefix);
    }
}
