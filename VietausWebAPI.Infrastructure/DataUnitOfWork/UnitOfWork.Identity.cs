using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Identity.RepositoriesContracts;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public sealed partial class UnitOfWork
    {
        public IApplicationUserRepository ApplicationUserRepository { get; }
        public IApplicationUserRoleRepository ApplicationUserRoleRepository { get; }
        public IApplicationRoleRepository ApplicationRoleRepository  { get; }  
    }
}
