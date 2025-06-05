using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Usecases.Materials.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Materials
{
    class MaterialsRepository : IMaterialsRepository
    {
        private readonly ApplicationDbContext _context;

        public MaterialsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task CreateMaterialAsync(List<MaterialsMaterialDatum> material)
        //{
        //    await _context.MaterialsMaterialData.AddRangeAsync(material);
        //}
    }
}
