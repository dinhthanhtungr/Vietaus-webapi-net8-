using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ProductFeature;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature
{
    public interface IProductService
    {
        /// <summary>
        /// Tạo mới một sản phẩm
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ProductDTO> CreateAsync(CreateProductRequest req, CancellationToken ct = default);
    }
}
