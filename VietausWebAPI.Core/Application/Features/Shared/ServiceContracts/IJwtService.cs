using VietausWebAPI.Core.Identity;
using System;
using System.Security.Claims;
using VietausWebAPI.Core.Application.Features.Shared.DTO;

namespace VietausWebAPI.Core.Application.Features.Shared.ServiceContracts
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtJoken(ApplicationUser user, Guid partId, string partName, string EmployeeExternalId, Guid EmployeeId, Guid CompanyId, IList<string> roles = null);
        ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);

    }
}
