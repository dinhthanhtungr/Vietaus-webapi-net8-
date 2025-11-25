using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Infrastructure.Helpers.Repositories;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Manufacturing
{
    public class ProductStandardFormulaRepository : Repository<ProductStandardFormula>, IProductStandardFormulaRepository
    { 
        private readonly ApplicationDbContext _context;
        public ProductStandardFormulaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task AddRangeAsync(IEnumerable<ProductStandardFormula> ProductStandardFormulas, CancellationToken ct)
        {
            await _context.ProductStandardFormulas.AddRangeAsync(ProductStandardFormulas, ct);
        }

        public void UpdateAsync(ProductStandardFormula ProductStandardFormula, CancellationToken ct)
        {
            _context.Update(ProductStandardFormula);
        }
    }
}
