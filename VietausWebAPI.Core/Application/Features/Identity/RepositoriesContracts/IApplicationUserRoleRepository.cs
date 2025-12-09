using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Identity;

namespace VietausWebAPI.Core.Application.Features.Identity.RepositoriesContracts
{
    public interface IApplicationUserRoleRepository : IRepository<ApplicationUserRole>
    {
    }
}
