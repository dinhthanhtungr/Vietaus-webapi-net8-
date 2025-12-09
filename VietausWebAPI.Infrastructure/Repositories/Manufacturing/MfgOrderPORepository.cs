using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Attachments.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Infrastructure.Helpers.Repositories;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Manufacturing
{
    public class MfgOrderPORepository : Repository<MfgOrderPO>, IMfgOrderPORepository
    {
        private readonly ApplicationDbContext _context;
        public MfgOrderPORepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task AddRangeAsync(IEnumerable<MfgOrderPO> MfgOrderPOs, CancellationToken ct)
        {
            await _context.MfgOrderPOs.AddRangeAsync(MfgOrderPOs, ct);
        }
    }
}
