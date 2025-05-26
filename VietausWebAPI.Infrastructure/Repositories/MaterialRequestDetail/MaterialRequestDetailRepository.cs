using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Usecases.MaterialRequestDetail.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.MaterialRequestDetail
{
    public class MaterialRequestDetailRepository : IMaterialRequestDetailRepository
    {
        private readonly ApplicationDbContext _context;
        public MaterialRequestDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRequetMaterialRepository(IEnumerable<RequestDetailMaterialDatum> requestDetailMaterialDatum)
        {
            await _context.RequestDetailMaterialData.AddRangeAsync(requestDetailMaterialDatum);
        }

        public async Task<IEnumerable<RequestDetailMaterialDatum>> GetRequestMaterialRepository(string requestId)
        {
            var result = await _context.RequestDetailMaterialData
                .Include(x => x.Material)   
                .Where(x => x.RequestId == requestId)
                .ToListAsync();

            return result;
        }
    }
}
