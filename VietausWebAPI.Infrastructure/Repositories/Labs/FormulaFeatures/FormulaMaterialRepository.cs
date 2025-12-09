using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.FormulaFeatures;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Labs.FormulaFeatures
{
    public class FormulaMaterialRepository : IFormulaMaterialRepository
    {
        private readonly ApplicationDbContext _context;

        public FormulaMaterialRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(FormulaMaterial formulaMaterial, CancellationToken ct = default)
        {
            await _context.FormulaMaterials.AddAsync(formulaMaterial);
        }

        public IQueryable<FormulaMaterial> Query(bool track = false)
        {
            var db = _context.FormulaMaterials.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
