using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;
using VietausWebAPI.Core.Identity;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.HR
{
    public class AspNetUserRoleRepository
    : Repository<ApplicationUserRole>, IAspNetUserRoleRepository
    {
        public AspNetUserRoleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
