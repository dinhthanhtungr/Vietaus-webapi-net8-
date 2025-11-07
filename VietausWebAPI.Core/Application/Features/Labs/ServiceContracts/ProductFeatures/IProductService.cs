using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Queries.FormulaFeature;
using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductFeatures;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.ProductFeatures
{
    public interface IProductService
    {
        /// <summary>
        /// Lấy danh sách công thức với phân trang và lọc.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PagedResult<GetProduct>> GetAllAsync(
             ProductQuery query,
             CancellationToken ct = default);

        /// <summary>
        /// Tạo mới một sản phẩm
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        //Task<ProductDTO> CreateAsync(CreateProductRequest req, CancellationToken ct = default);

        /// <summary>
        /// Lấy thông tin chi tiết sản phẩm theo Id, kèm theo công thức được xác định chuẩn của sản phẩm đó
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<GetProductInformation> GetInformationByIdAsync(Guid productId, CancellationToken ct = default);
    }
}
