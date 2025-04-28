using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories
{
    public class MaterialGroupsRepository : IMaterialGroupsRepository
    {
        private readonly ApplicationDbContext _context;

        public MaterialGroupsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Thêm mới nhóm vật liệu
        /// </summary>
        /// <param name="materialGroup"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task AddMaterialGroupRepositoryAsync(MaterialsMaterialGroupsDatum materialGroup)
        {
            await _context.MaterialsMaterialGroupsData.AddRangeAsync(materialGroup);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Lấy tất cả nhóm vật liệu
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<MaterialsMaterialGroupsDatum>> GetAllMaterialGroupsRepositoryAsync()
        {
            return await _context.MaterialsMaterialGroupsData.ToListAsync();
        }
    }
}
