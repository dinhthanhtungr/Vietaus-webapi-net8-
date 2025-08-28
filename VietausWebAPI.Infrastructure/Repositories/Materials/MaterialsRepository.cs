using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Materials
{
    public class MaterialsRepository : IMaterialRepository
    {
        private readonly ApplicationDbContext _context;

        public MaterialsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Material material, CancellationToken ct = default)
        {
            await _context.Materials.AddAsync(material);
        }

        public IQueryable<Material> Query()
        {
            return _context.Materials.AsNoTracking();
        }

        public async Task<string?> GetLatestExternalIdStartsWithAsync(string prefix)
        {
            return await _context.Materials
                .AsNoTracking()
                .Where(e => e.ExternalId != null && e.ExternalId.StartsWith(prefix))
                .OrderByDescending(e => e.ExternalId!.Length)  // số chữ số dài hơn ⇒ lớn hơn
                .ThenByDescending(e => e.ExternalId)           // cùng độ dài ⇒ so chuỗi
                .Select(e => e.ExternalId)
                .FirstOrDefaultAsync();
        }
    }
}
