using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories
{
    public class MaterialSuppliersRepository : IMaterialSuppliersRepository
    {
        private readonly ApplicationDbContext _context;
        public MaterialSuppliersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddMaterialSupplierRepositoryAsync(MaterialSuppliersMaterialDatum materialSuppliers)
        {
            await _context.MaterialSuppliersMaterialData.AddRangeAsync(materialSuppliers);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MaterialSuppliersMaterialDatum>> GetAllMaterialSuppliersRepositoryAsync()
        {
            return await _context.MaterialSuppliersMaterialData.ToListAsync();
        }

    }
}
