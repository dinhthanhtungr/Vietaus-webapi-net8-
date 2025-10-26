using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgFormulas;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgProductionOrders;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts
{
    public interface IMfgFormulaService
    {

        /// <summary>
        /// Tạo công thức mới
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult> CreateAsync(PostMfgFormula req, CancellationToken ct = default);


        /// <summary>
        /// Lấy thông tin của cụ thể một công thức sản xuất
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<GetManufacturingFormula?> GetByIdAsync(
         Guid id,
         CancellationToken ct = default);

        /// <summary>
        /// Lấy thông tin chung của các công thức sản xuất (dùng để chọn công thức cho lệnh sản xuất)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PagedResult<GetSummaryMfgFormula>> GetAllAsync(
         MfgFormulaQuery query,
         CancellationToken ct = default);

        /// <summary>
        /// Thay đổi công thức lần đầu tiên 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<OperationResult> UpsertFormulaAsync (PatchMfgFormula req, CancellationToken? cancellationToken = default);
    }
}
