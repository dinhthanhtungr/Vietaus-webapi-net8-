using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Usecases.SupplyRequests.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.SupplyRequest
{
    public class MaterialsRepository : IMaterialsRepository
    {
        private readonly ApplicationDbContext _context;
        public MaterialsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MaterialsMaterialDatum>> SearchByNameAsync(string name, Guid materialGroupId)
        {
            var queryable = _context.MaterialsMaterialData
                .AsNoTracking()
                .Include(x => x.MaterialGroup)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                string keyword = name.ToLower();
                queryable = queryable.Where(x => 
                    x.Name != null && 
                    EF.Functions.Collate(x.Name, "Latin1_General_CI_AI").ToLower().Contains(keyword));
            }

            if (materialGroupId != Guid.Empty)
            {
                queryable = queryable.Where(x => x.MaterialGroupId == materialGroupId);
            }

            return await queryable
                .OrderBy(x => x.Name)
                .Take(10)
                .ToListAsync();
        }

        public async Task CreateMaterialAsync(List<MaterialsMaterialDatum> material)
        {
            await _context.MaterialsMaterialData.AddRangeAsync(material);
        }
    }
}
