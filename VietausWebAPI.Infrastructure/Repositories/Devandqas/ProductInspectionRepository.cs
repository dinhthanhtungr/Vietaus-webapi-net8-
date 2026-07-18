using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.DevandqaSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.Devandqas
{
    public class ProductInspectionRepository : Repository<ProductInspection>, IProductInspectionRepository
    {
        public ProductInspectionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<ProductInspection?> GetByIdForUpdateAsync(Guid id, CancellationToken ct = default)
        {
            return _context.ProductInspections
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }
    }
}
