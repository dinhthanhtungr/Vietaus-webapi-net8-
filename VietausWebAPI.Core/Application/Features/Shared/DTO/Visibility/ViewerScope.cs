using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Visibilitys;

namespace VietausWebAPI.Core.Application.Features.Shared.DTO.Visibility
{
    public sealed record ViewerScope(
        Guid CompanyId,
        Guid EmployeeId,
        bool IsLeader,
        Guid? GroupId,
        ViewerScopeType ScopeType,          // AdminFull / LabFull / SalesScoped
        DateTime Now,
        IReadOnlySet<Guid> EmployeeIdsInScope   // <--- thêm
    );
}
