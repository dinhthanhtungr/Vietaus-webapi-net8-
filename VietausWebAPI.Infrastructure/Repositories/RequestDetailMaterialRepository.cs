using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories
{
    public class RequestDetailMaterialRepository : IRequestDetailMaterialRepository
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public RequestDetailMaterialRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Thêm mới danh sách các vật tư đề xuất đi chung với lại request
        /// </summary>
        /// <param name="requestDetailMaterialDatum"></param>
        /// <returns></returns>
        public async Task AddRequetMaterialRepository(IEnumerable<RequestDetailMaterialDatum> requestDetailMaterialDatum)
        {
            await _context.RequestDetailMaterialData.AddRangeAsync(requestDetailMaterialDatum);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Lấy tất cả danh sách vật tư đề xuất
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<RequestDetailMaterialDatum>> GetAllRequestMaterialRepository()
        {
            return await _context.RequestDetailMaterialData.ToListAsync();
        }
    }
}
