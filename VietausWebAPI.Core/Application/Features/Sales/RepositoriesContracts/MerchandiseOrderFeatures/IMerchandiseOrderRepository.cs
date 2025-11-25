using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;

namespace VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.MerchandiseOrderFeatures
{
    public interface IMerchandiseOrderRepository
    {
        /// <summary>
        /// Tạo câu lệnh query để truy vấn đơn hàng từ cơ sở dữ liệu.
        /// </summary>
        /// <returns></returns>
        IQueryable<MerchandiseOrder> Query(bool track = false);
        /// <summary>
        /// Tạo câu lệnh query để truy vấn đơn hàng từ cơ sở dữ liệu.
        /// </summary>
        /// <returns></returns>
        IQueryable<MerchandiseOrderDetail> QueryDetail(bool track = false);

        /// <summary>
        /// Tạo một đơn hàng mới vào cơ sở dữ liệu.
        /// </summary>
        /// <param name="merchandiseOrder"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task AddAsync(MerchandiseOrder merchandiseOrder, CancellationToken ct = default);

        /// <summary>
        /// Lấy số cuối cùng của code
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>        
        Task<string?> GetLatestExternalIdStartsWithAsync(string prefix);
    }
}
