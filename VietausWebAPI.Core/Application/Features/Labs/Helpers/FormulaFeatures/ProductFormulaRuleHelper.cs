using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Domain.Enums.Formulas;

namespace VietausWebAPI.Core.Application.Features.Labs.Helpers.FormulaFeatures
{
    public class ProductFormulaRuleHelper : IProductFormulaRuleHelper
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductFormulaRuleHelper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<HashSet<Guid>> GetProductIdsWithSingleMaterialFormulaAsync(
            IEnumerable<Guid> productIds,
            CancellationToken ct = default)
        {
            var ids = productIds
                .Where(x => x != Guid.Empty)
                .Distinct()
                .ToList();

            if (ids.Count == 0)
                return new HashSet<Guid>();

            var result = await _unitOfWork.FormulaRepository.Query(track: false)
                .Where(f => ids.Contains(f.ProductId)
                            && f.IsActive
                            && f.IsSelect)
                .Select(f => new
                {
                    f.ProductId,
                    MaterialCount = f.FormulaMaterials.Count(m =>
                        m.IsActive &&
                        m.itemType == ItemType.Material)
                })
                .Where(x => x.MaterialCount == 1)
                .Select(x => x.ProductId)
                .Distinct()
                .ToListAsync(ct);

            return result.ToHashSet();
        }
    }
}
