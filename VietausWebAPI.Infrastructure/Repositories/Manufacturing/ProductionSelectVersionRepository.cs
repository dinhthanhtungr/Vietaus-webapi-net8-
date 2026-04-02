using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Infrastructure.Helpers.Repositories;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders.GetRepositories;

namespace VietausWebAPI.Infrastructure.Repositories.Manufacturing
{
    public class ProductionSelectVersionRepository : Repository<ProductionSelectVersion>, IProductionSelectVersionRepository
    { 
        private readonly ApplicationDbContext _context;
        public ProductionSelectVersionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task AddRangeAsync(IEnumerable<ProductionSelectVersion> ProductionSelectVersions, CancellationToken ct)
        {
            await _context.ProductionSelectVersions.AddRangeAsync(ProductionSelectVersions, ct);
        }


        public void UpdateAsync(ProductionSelectVersion ProductionSelectVersion, CancellationToken ct)
        {
            _context.Update(ProductionSelectVersion);
        }
    }
}
