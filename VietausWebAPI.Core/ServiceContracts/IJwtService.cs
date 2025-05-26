using VietausWebAPI.Core.DTO;
using VietausWebAPI.Core.Identity;
using System;
using System.Security.Claims;

namespace VietausWebAPI.Core.ServiceContracts
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtJoken(ApplicationUser user, string partId, string departmentName, string EmployeeId, IList<string> roles = null);
        ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);

    }
}
