using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.Helpers.FormulaFeatures
{
    public interface IProductFormulaRuleHelper
    {
        Task<HashSet<Guid>> GetProductIdsWithSingleMaterialFormulaAsync(
            IEnumerable<Guid> productIds,
            CancellationToken ct = default);
    }

}
