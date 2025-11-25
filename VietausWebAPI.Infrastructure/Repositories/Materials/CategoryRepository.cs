using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;
using VietausWebAPI.Infrastructure.Helpers.Repositories;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Materials
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
