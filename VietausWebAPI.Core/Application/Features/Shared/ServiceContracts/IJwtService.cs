using VietausWebAPI.Core.Identity;
using System;
using System.Security.Claims;
using VietausWebAPI.Core.Application.Features.Shared.DTO;

namespace VietausWebAPI.Core.Application.Features.Shared.ServiceContracts
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtJoken(JwtModels model);
        ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);

    }
}
