using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Labs
{
    public class MfgProductionOrdersPlanRepository : IMfgProductionOrdersPlanRepository
    {
        private readonly ApplicationDbContext _context;
        public MfgProductionOrdersPlanRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<MfgProductionOrdersPlan> GetMfgProductionOrdersPlanByIdAsync(Guid id)
        {
            var mfgProductionOrdersPlan = await _context.MfgProductionOrdersPlans
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (mfgProductionOrdersPlan == null)
            {
                throw new InvalidOperationException($"MfgProductionOrdersPlan with ID {id} not found.");
            }
            return mfgProductionOrdersPlan;
        }
    }
}
