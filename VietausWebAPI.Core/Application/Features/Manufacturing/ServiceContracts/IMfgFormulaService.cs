using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.CompareFormulaDTOs;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.FormulaVersion;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgFormulas;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgFormulas;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgProductionOrders;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts
{
    public interface IMfgFormulaService
    {
        // ======================================================================== Get ======================================================================== 

        /// <summary>
        /// Lấy thông tin công thức sản xuất theo Id lấy theo Vu nếu là công thức mới tanh và lấy theo VA nếu đã có công thức chuẩn trước đó. Lấy Id này khi trang ở New hoặc Clone
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult<GetManufacturingFormula>> GetByIdAsync(Guid id, CancellationToken ct = default);

        /// <summary>
        /// Lấy thông tin công thức sản xuất theo Id. Lấy Id này khi trang ở View
        /// </summary>
        /// <param name="MfgOrderId">Truyền Id của lệnh sản xuất</param>
        /// <param name="MfgFormulaId">Truyền Id của công thức sản xuất</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult<GetManufacturingFormula>> GetByIdForViewAsync(Guid MfgOrderId, Guid MfgFormulaId, CancellationToken ct = default);

        /// <summary>
        /// Lấy danh sách nguyên vật liệu của công thức sản xuất theo Id công thức kèm thông tin có trong kho
        /// </summary>
        /// <param name="MfgFormulaId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult<GetSampleManufacturingFormula>> GetMaterialsByFormulaIdAsync(Guid MfgFormulaId, CancellationToken ct = default);

        /// <summary>
        /// Lấy thông tin chung của các công thức sản xuất (dùng để chọn công thức cho lệnh sản xuất)
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult<PagedResult<GetSummaryMfgFormula>>> GetAllAsync(MfgFormulaQuery query, CancellationToken ct = default);

        /// <summary>
        /// Lấy danh sách các phiên bản công thức của một công thức sản xuất trong lịch sử
        /// </summary>
        /// <param name="mfgFormulaId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult<PagedResult<FormulaVersionMetaDto>>> GetFormulaVersionsPagedAsync(MfgProductionOrderQuery queryable, CancellationToken ct = default);

        /// <summary>
        /// Lấy danh sách công thức để so sánh
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult<PagedResult<GetCompareFormula>>> GetCompareFormulaAsync(MfgFormulaQuery query, CancellationToken ct = default);

        // ======================================================================== Post =======================================================================

        /// <summary>
        /// Tạo công thức mới
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult> CreateAsync(PostMfgFormula req, CancellationToken ct = default);


        // ====================================================================== Patch ======================================================================

        /// <summary>
        /// Thay đổi công thức lần đầu tiên 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<OperationResult> UpsertFormulaAsync(PatchMfgFormula req, CancellationToken? cancellationToken = default);
    }
}
