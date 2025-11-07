using VietausWebAPI.Core.DTO;
using VietausWebAPI.Core.Identity;
using VietausWebAPI.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace VietausWebAPI.Core.Service
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService (IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a JWT token using the given user's information
        /// and the configuration settings
        /// </summary>
        /// <param name = "user" > ApplicationUser oject</param>
        /// <returns>AuthenticationResponse that includes token</returns>
        AuthenticationResponse CreateJwtJoken(ApplicationUser user, Guid partId, string partName, string EmployeeExternalId, Guid EmployeeId, Guid CompanyId, IList<string> roles = null)
        {
            // Create a DateTime ojcet representing the token 
            // expiration time by adding the number of minutes specified
            // in the configuration to the current UTC time
            DateTime expiration = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration
            ["Jwt:EXPIRATION_MINUTES"]));

            // Create an array of claim objects representing the user's claims,
            // such as their ID, name, email,etc.
            var claims = new List<Claim>
            {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()), // JWT unique ID
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),

                    new(ClaimTypes.Name, user.UserName ?? string.Empty),
                    new("employeeName", user.personName ?? string.Empty),
                    new("partId", partId.ToString()), // Part ID of the user
                    new("employeeExternalId", EmployeeExternalId ?? string.Empty), // Employee ID of the user
                    new("employeeId", EmployeeId.ToString()),
                    new("partName", partName), // Part name of the user
                    new("companyId", CompanyId.ToString()), // Company ID of the user
                };

            if (roles != null && roles.Any())
            {
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            // Create a SymmetricSecurityKey object using the key 
            // specified in the configuration.
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            SigningCredentials signingCredentials = new
            SigningCredentials(securityKey,
            SecurityAlgorithms.HmacSha256);

            // Create a JwtSecurrityToken object with the given issuer
            // audience, claims, expiration, and signing credentials.
            JwtSecurityToken tokenGenerator = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expiration,
                signingCredentials: signingCredentials
                );

            // Create a JwtSecurityTokenHandler oject and use it to 
            // write the token as a string
            JwtSecurityTokenHandler tokenHandler = new
            JwtSecurityTokenHandler();
            string token = tokenHandler.WriteToken(tokenGenerator);

            //Console.WriteLine("JWT Signing Key: " + _configuration["Jwt:Key"]);

            // Create and return an AuthenticationResponse object containing 
            // the token, user emil, user name, and token expiration time
            return new AuthenticationResponse()
            {
                Token = token,
                Email = user.Email,
                PersonName = user.personName,
                Expiration = expiration,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpirationDateTime = DateTime.Now.AddMinutes
                (Convert.ToInt32(_configuration["RefreshToken:EXPIRATION_MINUTES"]))
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ClaimsPrincipal? GetPrincipalFromJwtToken(string? token)
        {
            var tokenValdationParameters = new
            TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =  new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateLifetime = false // should be false
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new
            JwtSecurityTokenHandler();

            try
            {
                ClaimsPrincipal principal = jwtSecurityTokenHandler.ValidateToken(token,
                    tokenValdationParameters, out SecurityToken securityToken);

                if (securityToken is not JwtSecurityToken
                jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals
                (SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))

                {
                    //throw new SecurityTokenException("Invalid token");
                    //return "Invalid token";
                    return null;
                }

                return principal;
            }

            catch
            {
                return null;
            }

        }

        /// <summary>
        /// Create a refresh token (base 64 string of random numbers)
        /// </summary>
        /// <returns></returns>
        private string GenerateRefreshToken()
        {
            Byte[] bytes = new byte[64];
            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(bytes);
            Convert.ToBase64String(bytes);
            return Convert.ToBase64String(bytes);
        }

        AuthenticationResponse IJwtService.CreateJwtJoken(ApplicationUser user, Guid partId, string partName, string EmployeeExternalId, Guid EmployeeId, Guid CompanyId, IList<string> roles)
        {
            return CreateJwtJoken(user, partId, partName, EmployeeExternalId, EmployeeId, CompanyId, roles);
        }
    }
}
