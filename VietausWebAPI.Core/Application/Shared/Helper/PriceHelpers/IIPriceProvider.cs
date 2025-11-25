using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Formulas;

namespace VietausWebAPI.Core.Application.Shared.Helper.PriceHelpers
{
    public interface IPriceProvider
    {
        /// <summary>
        /// Trả về đơn giá mới nhất cho mỗi MaterialId.
        /// Yêu cầu: chỉ cần provide map {MaterialId -> UnitPrice}.
        /// </summary>
        Task<Dictionary<Guid, decimal>> GetLatestUnitPricesAsync(
            IEnumerable<Guid> materialIds,
            CancellationToken ct = default);

        /// <summary>
        /// Tính giá cho từng công thức được chọn ( chỉ đơn lẻ một công thức )
        /// </summary>
        /// <param name="formulaId"></param>
        /// <param name="source"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<decimal> CalculatePriceAsync(Guid formulaId, FormulaSource source, CancellationToken ct = default);

        /// <summary>
        /// Lấy giá bán (UnitPriceAgreed) của sản phẩm trong đơn sale
        /// đã liên kết với lệnh sản xuất qua MfgOrderPO.
        /// </summary>
        Task<decimal?> GetTargetPriceByMpoAsync(
            Guid mfgProductionOrderId,
            Guid productId,
            Guid companyId,
            CancellationToken ct = default);
    }
}
