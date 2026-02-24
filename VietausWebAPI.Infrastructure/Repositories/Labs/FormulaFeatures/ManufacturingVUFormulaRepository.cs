using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.FormulaFeatures;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.Labs.FormulaFeatures
{
    public class ManufacturingVUFormulaRepository : Repository<ManufacturingVUFormula>, IManufacturingVUFormulaRepository
    {
        public ManufacturingVUFormulaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
