using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.DevandqaSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.Devandqas
{
    public class ProductStandardRepository : Repository<ProductStandard>, IProductStandardRepository    
    {
        public ProductStandardRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
