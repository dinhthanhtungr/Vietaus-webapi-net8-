using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices;
using VietausWebAPI.Core.Application.Features.Warehouse.Queries;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts
{
    public interface IWarehouseReadService
    {
        /// <summary>
        /// Lấy danh sách tồn khả dụng của các NVL trong kho theo điều kiện lọc của WarehouseReadServiceQuery
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<OperationResult<PagedResult<GetStockAvaiable>>> GetStockAvailableAsync(WarehouseReadServiceQuery query);


        /// <summary>
        /// Dùng để hiển thị tồn khả dụng của các NVL trong công thức sản xuất (ManufacturingFormulaId)
        /// </summary>
        /// <param name="manufacturingFormulaId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<VaAvailabilityVm>> GetVaAvailabilityAsync(
                                Guid manufacturingFormulaId,
                                CancellationToken ct = default);


        /// <summary>
        /// Lấy danh sách tồn kho của sản phẩm ProductAvailabilityVm theo danh sách productExternalIds
        /// </summary>
        /// <param name="productExternalIds"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<ProductAvailabilityVm>> GetProductAvailabilityVmsAsync(
                                List<string> productExternalIds,
                                CancellationToken ct = default);

        /// <summary>
        /// Lấy dictionary thông tin tồn kho VA của các NVL trong công thức sản xuất
        /// </summary>
        /// <param name="manufacturingFormulaId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<Dictionary<string, VaAvailabilityVm>> GetVaAvailabilityDictAsync(
                                Guid manufacturingFormulaId,
                                CancellationToken ct = default);

        /// <summary>
        /// Lấy dictionary mapping giữa code của NVL trong công thức sản xuất và lotNo của NVL đó trong kho
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<Dictionary<string, string>> GetLotNoMapByCodesAsync(IEnumerable<string> codes, CancellationToken ct = default);


        /// <summary>
        /// Lấy dictionary mapping giữa code hàng hóa và danh sách lotNo còn tồn trong kho để người dùng chọn.
        /// Có phân biệt trạng thái chất lượng đạt/lỗi theo StockType.
        /// </summary>
        /// <param name="codes">Danh sách mã hàng hóa cần lấy lotNo.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Dictionary có key là mã hàng hóa đã chuẩn hóa và value là danh sách lotNo kèm trạng thái mới nhất trước.</returns>
        Task<Dictionary<string, List<LotNumberOptionDto>>> GetLotNoListMapByCodesAsync(IEnumerable<string> codes, CancellationToken ct = default);


        Task<OperationResult<List<StockAvailableExportRow>>> GetStockAvailableExportAsync(WarehouseReadServiceQuery query);
    }
}
