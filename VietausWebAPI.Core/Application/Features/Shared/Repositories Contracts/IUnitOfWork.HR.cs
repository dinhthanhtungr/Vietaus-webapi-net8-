using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        IEmployeesRepository EmployeesRepository { get; }
        IApplicationUserRepository ApplicationUserRepository { get; }
        IAspNetUserRoleRepository AspNetUserRoleRepository { get; }
        IGroupRepository GroupRepository { get; }
        IMemberInGroupRepository MemberInGroupRepository { get; }
    }
}
