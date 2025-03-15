using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Entities;
using VietausWebAPI.Core.Repositories_Contracts;

using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories
{
    public class SupplyRequestsMaterialDatumRepository : ISupplyRequestsMaterialDatumRepository
    {
        private readonly ApplicationDbContext _context;

        public SupplyRequestsMaterialDatumRepository (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddSupplyRequestsMaterialDatumRepository(List<SupplyRequestsMaterialDatum> supplyRequestsMaterialData)
        {
            await _context.SupplyRequestsMaterialData.AddRangeAsync(supplyRequestsMaterialData);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SupplyRequestsMaterialDatum>> GetAllSupplyRequestsMaterialDatumRepository()
        {
            return await _context.SupplyRequestsMaterialData.ToListAsync();
        }
    }
}
