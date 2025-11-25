using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;

namespace VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature
{
    public interface IProductRepository
    {
        /// <summary>
        /// Tạo lệnh query để truy vấn sản phẩm từ cơ sở dữ liệu.
        /// </summary>
        /// <returns></returns>
        IQueryable<Product> Query(bool track = false);
        /// <summary>
        /// Tạo mới một mẫu
        /// </summary>
        /// <param name="sampleRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task AddAsync(Product sampleRequest, CancellationToken ct = default);

        /// <summary>
        /// Kiểm tra xem mẫu có tồn tại hay không
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Guid productId, CancellationToken ct);


        /// <summary>
        /// lấy sô cuối cùng của code
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        Task<string?> GetLatestProductStartsWithAsync(
                        IQueryable<string> codes, string left, string right, CancellationToken ct = default);
    }
}
