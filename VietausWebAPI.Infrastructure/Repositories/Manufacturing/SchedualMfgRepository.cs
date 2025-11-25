using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Infrastructure.Helpers.Repositories;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Manufacturing
{
    public class SchedualMfgRepository : Repository<SchedualMfg>, ISchedualMfgRepository
    {
        private readonly ApplicationDbContext _context;
        public SchedualMfgRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
    }
}
