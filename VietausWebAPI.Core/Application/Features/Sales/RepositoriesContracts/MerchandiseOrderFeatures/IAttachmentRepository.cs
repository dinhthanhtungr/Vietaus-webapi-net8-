using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.MerchandiseOrderFeatures
{
    public interface IAttachmentRepository
    {
        Task AddRangeAsync(IEnumerable<OrderAttachment> attachments, CancellationToken ct = default);
        Task<List<OrderAttachment>> GetByOrderAsync(Guid orderId, CancellationToken ct = default);
        /// <summary>
        /// Tạo câu lệnh query để truy vấn đơn hàng từ cơ sở dữ liệu.
        /// </summary>
        /// <returns></returns>
        IQueryable<OrderAttachment> Query(bool track = false);
    }
}
