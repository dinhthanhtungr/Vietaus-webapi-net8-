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
        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="context"></param>
        public MaterialSuppliersRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Thêm mới nhà cung cấp vật liệu
        /// </summary>
        /// <param name="materialSuppliers"></param>
        /// <returns></returns>
        public async Task AddMaterialSupplierRepositoryAsync(MaterialSuppliersMaterialDatum materialSuppliers)
        {
            await _context.MaterialSuppliersMaterialData.AddRangeAsync(materialSuppliers);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Lấy tất cả nhà cung cấp vật liệu
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MaterialSuppliersMaterialDatum>> GetAllMaterialSuppliersRepositoryAsync()
        {
            return await _context.MaterialSuppliersMaterialData.ToListAsync();
        }

    }
}
