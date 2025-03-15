using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories
{
    public class RequestDetailMaterialRepository : IRequestDetailMaterialRepository
    {
        private readonly ApplicationDbContext _context;
        public RequestDetailMaterialRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddRequetMaterialRepository(IEnumerable<RequestDetailMaterialDatum> requestDetailMaterialDatum)
        {
            await _context.RequestDetailMaterialData.AddRangeAsync(requestDetailMaterialDatum);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<RequestDetailMaterialDatum>> GetAllRequestMaterialRepository()
        {
            return await _context.RequestDetailMaterialData.ToListAsync();
        }
    }
}
