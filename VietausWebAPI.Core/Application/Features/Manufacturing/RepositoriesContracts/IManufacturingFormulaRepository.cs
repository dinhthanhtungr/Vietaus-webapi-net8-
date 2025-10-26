using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts
{
    public interface IManufacturingFormulaRepository
    {
        /// <summary>
        /// Tạo lệnh query để truy vấn sản phẩm từ cơ sở dữ liệu.
        /// </summary>
        /// <returns></returns>
        IQueryable<ManufacturingFormula> Query(bool track = false);
        /// <summary>
        /// Tạo mới một mẫu
        /// </summary>
        /// <param name="sampleRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task AddAsync(ManufacturingFormula sampleRequest, CancellationToken ct = default);

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
        Task<string?> GetLatestExternalIdStartsWithAsync(string prefix, Guid? id = null);
    }
}
