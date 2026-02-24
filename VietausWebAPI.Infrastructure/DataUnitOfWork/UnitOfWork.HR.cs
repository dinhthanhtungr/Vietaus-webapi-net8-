using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public sealed partial class UnitOfWork
    {
        public IEmployeesRepository EmployeesRepository { get; }
        public IGroupRepository GroupRepository { get; }
        public IMemberInGroupRepository MemberInGroupRepository { get; }
        public IPartRepository PartRepository { get; }
    }
}
