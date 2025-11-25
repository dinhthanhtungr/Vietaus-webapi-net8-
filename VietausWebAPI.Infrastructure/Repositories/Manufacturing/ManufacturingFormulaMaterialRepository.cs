using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Manufacturing
{
    public class ManufacturingFormulaMaterialRepository : IManufacturingFormulaMaterialRepository
    {
        private readonly ApplicationDbContext _context;

        public ManufacturingFormulaMaterialRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ManufacturingFormulaMaterial manufacturingFormulaMaterial, CancellationToken ct = default)
        {
            await _context.ManufacturingFormulaMaterials.AddAsync(manufacturingFormulaMaterial, ct);
        }

        public async Task AddRangeAsync(List<ManufacturingFormulaMaterial> manufacturingFormulaMaterials, CancellationToken ct = default)
        {
            await _context.ManufacturingFormulaMaterials.AddRangeAsync(manufacturingFormulaMaterials, ct);
        }

        public IQueryable<ManufacturingFormulaMaterial> Query(bool track = true)
        {
            var db = _context.ManufacturingFormulaMaterials.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
