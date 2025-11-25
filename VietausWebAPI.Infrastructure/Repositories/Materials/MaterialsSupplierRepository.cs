using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;
using VietausWebAPI.Infrastructure.Helpers.Repositories;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Materials
{
    public class MaterialsSupplierRepository : IMaterialsSupplierRepository
    {
        private readonly ApplicationDbContext _context;

        public MaterialsSupplierRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MaterialsSupplier material, CancellationToken ct = default)
        {
            await _context.MaterialsSuppliers.AddAsync(material);
        }

        public IQueryable<MaterialsSupplier> Query(bool track = false)
        {
            var db = _context.MaterialsSuppliers.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
