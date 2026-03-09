using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;

namespace VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.FormulaFeatures
{
    public interface IFormulaMaterialSnapshotRepository : IRepository<FormulaMaterialSnapshot>
    {
        /// <summary>
        /// Repository này dùng để tạo snapshot cho công thức sản xuất, mỗi khi có thay đổi về công thức sản xuất thì sẽ tạo một snapshot mới để lưu lại lịch sử thay đổi của công thức sản xuất đó.
        /// </summary>
        /// <param name="manufacturingVUFormulaId">Id của công thức sản xuất</param>
        /// <param name="formulaId">Id của công thức</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task AddSnapshotsFromFormulaAsync(Guid manufacturingVUFormulaId, Guid formulaId, CancellationToken ct = default);
    }
}
