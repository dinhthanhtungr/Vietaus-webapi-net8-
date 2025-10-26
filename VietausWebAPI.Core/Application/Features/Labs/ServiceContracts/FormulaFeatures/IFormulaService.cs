using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Queries.FormulaFeature;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.FormulaFeatures
{
    public interface IFormulaService
    {
        /// <summary>
        /// Tao mới một công thức với sản phẩm liên kết.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult> CreateAsync(PostFormula req, CancellationToken ct = default);

        /// <summary>
        /// Lấy danh sách công thức với phân trang và lọc.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PagedResult<GetFormula>> GetAllAsync(
         FormulaQuery query,
         CancellationToken ct = default);

        /// <summary>
        /// Cấp nhật công thức.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<OperationResult> UpsertFormulaAsync(PatchFormula req, CancellationToken? cancellationToken = default);

        Task<OperationResult> UpdateInformationAsync(PatchFormulaInformation patch, CancellationToken? cancellationToken = default);
    }
}
