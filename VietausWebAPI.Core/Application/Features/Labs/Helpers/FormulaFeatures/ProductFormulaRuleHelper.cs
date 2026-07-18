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
                            && f.IsActive)
                .Select(f => new
                {
                    f.ProductId,
                    MaterialCount = f.FormulaMaterials.Count(m =>
                        m.IsActive)
                })
                .Where(x => x.MaterialCount == 1)
                .Select(x => x.ProductId)
                .Distinct()
                .ToListAsync(ct);

            return result.ToHashSet();
        }

        public async Task<HashSet<string>> GetManufacturingFormulaExternalIdsWithSingleMaterialAsync(
            IEnumerable<string> externalIds,
            CancellationToken ct = default)
        {
            var ids = externalIds
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (ids.Count == 0)
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var result = await _unitOfWork.ManufacturingFormulaRepository.Query(track: false)
                .Where(f => ids.Contains(f.ExternalId)
                            && f.IsActive)
                .Select(f => new
                {
                    f.ExternalId,
                    MaterialCount = f.ManufacturingFormulaMaterials.Count(m =>
                        m.IsActive )
                })
                .Where(x => x.MaterialCount == 1)
                .Select(x => x.ExternalId)
                .Distinct()
                .ToListAsync(ct);

            return result.ToHashSet(StringComparer.OrdinalIgnoreCase);
        }
    }
}
