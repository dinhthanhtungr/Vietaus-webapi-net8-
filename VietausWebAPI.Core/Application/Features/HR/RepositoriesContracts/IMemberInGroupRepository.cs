using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts
{
    public interface IMemberInGroupRepository
    {
        IQueryable<MemberInGroup> Query();
    }
}
