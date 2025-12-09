using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Identity.RepositoriesContracts;

namespace VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        IApplicationUserRepository ApplicationUserRepository { get; }
        IApplicationUserRoleRepository ApplicationUserRoleRepository { get; }
        IApplicationRoleRepository ApplicationRoleRepository { get; }
    }
}
