using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.DTO
{
    public record RegisterResultDTO(
        Guid UserId,
        string UserName,
        string Email,
        Guid? EmployeeId,
        string AssignedRole);
}
