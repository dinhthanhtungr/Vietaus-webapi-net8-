using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

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

        public IQueryable<Material> Query(bool track = false)
        {
            var db = _context.Materials.AsQueryable();
            return track ? db : db.AsNoTracking();
        }

        public async Task<string?> GetLatestExternalIdStartsWithAsync(string prefix)
        {
            var items = await _context.Materials
                .AsNoTracking()
                .Where(x => x.ExternalId != null && x.ExternalId.StartsWith(prefix))
                .Select(x => x.ExternalId!)
                .ToListAsync();

            var latest = items
                .Select(id =>
                {
                    var suffix = id.Substring(prefix.Length);
                    return int.TryParse(suffix, out var num)
                        ? new { Id = id, Num = num }
                        : null;
                })
                .Where(x => x != null)
                .OrderByDescending(x => x!.Num)
                .FirstOrDefault();

            return latest?.Id;
        }

        public async Task<bool> DeleteMaterialAsync(Guid Id, CancellationToken ct = default)
        {
            var material = await _context.Materials.FindAsync(Id); 
            if (material != null)
            {
                material.IsActive = false;
                _context.Materials.Update(material);
                return true;
            }

            return false;
        }
    }
}
