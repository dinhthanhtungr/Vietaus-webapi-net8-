using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;
using VietausWebAPI.Core.Identity;
using VietausWebAPI.Core.Application.Features.Shared.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Data;
using System.Security.Claims;
using System.Runtime.Intrinsics.X86;
using VietausWebAPI.Core.Application.Features.Shared.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Shared.DTO;

namespace VietausWebAPI.WebAPI.Controllers.v1._0
{
    // ❌ Các phương thức KHÔNG hoạt động khi sử dụng ApplicationUserRole
    // ❌ _userManager.AddToRoleAsync(user, roleName) -> 🔥 Thay bằng: _context.UserRoles.Add()
    // ❌ _userManager.RemoveFromRoleAsync(user, roleName) -> 🔥 Thay bằng: _context.UserRoles.Remove()
    // ❌ _userManager.GetRolesAsync(user) -> 🔥 Thay bằng: Truy vấn LINQ JOIN với _context.Roles
    // ❌ _userManager.IsInRoleAsync(user, roleName) -> 🔥 Thay bằng: Kiểm tra trực tiếp trong _context.UserRoles
    // ❌ _userManager.GetUsersInRoleAsync(roleName) -> 🔥 Thay bằng: Truy vấn LINQ JOIN với _context.Users


    /// <summary>
    /// Controller responsible for user authentication and account management.
    /// </summary>
    [AllowAnonymous] // Cho phép request kể cả khi không có token
    public class AccountController : CustomControllerBase
    {
        // Dependency-injected services for user management
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly SignInManager<ApplicationUser> _SignInManager;
        private readonly RoleManager<ApplicationRole> _RoleManager;
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;

        /// <summary>
        /// Contructor to Inject dependencies via Dependency Injectin (DI)
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="roleManager"></param>
        public AccountController(UserManager<ApplicationUser> userManager
            , RoleManager<ApplicationRole> roleManager
            , SignInManager<ApplicationUser> signInManager
            , IJwtService jwtService, ApplicationDbContext context
            )
        {
            _context = context;
            _UserManager = userManager;
            _SignInManager = signInManager;
            _RoleManager = roleManager;
            _jwtService = jwtService;
        }

        /// <summary>
        /// API to register a new user.
        /// </summary>
        /// <param name="registerDTO">Contains user registration information</param>
        /// <returns>Returns success or error message</returns>
        [HttpPost("Register")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<RegisterResultDTO>> PostRegister(RegisterDTO registerDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Problem(string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));

                var user = new ApplicationUser
                {
                    Email = registerDTO.Email,
                    UserName = registerDTO.UserName,
                    personName = registerDTO.PersonName,
                    EmployeeId = registerDTO.EmployeeId
                };

                var result = await _UserManager.CreateAsync(user, registerDTO.Password);
                if (!result.Succeeded)
                    return Problem(string.Join(" | ", result.Errors.Select(e => e.Description)));

                const string defaultRole = "User";
                // Gán role bằng UserManager (đỡ tự chọc DbContext)

                var role = await _RoleManager.FindByNameAsync(defaultRole);

                if (role == null)
                {
                    return Problem("Không tìm thấy role mặc định.");
                }

                await _context.UserRoles.AddAsync(new ApplicationUserRole { UserId = user.Id, RoleId = role.Id });
                await _context.SaveChangesAsync();
                var resp = new RegisterResultDTO(user.Id, user.UserName!, user.Email!, user.EmployeeId, defaultRole);
                return Ok(resp);
            }

            catch (Exception ex)
            {
                // Log the exception (optional)
                // _logger.LogError(ex, "An error occurred while registering a new user.");
                return Problem("An error occurred while registering a new user: " + ex.Message);
            }

        }


        // Existing code...

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailAlreadyRegistered(string UserName)
        {
            bool exists = await _context.Users.
                AnyAsync(u => u.UserName == UserName);

            if (!exists)
            {
                return BadRequest(new { Message = " Does not exist in the database" });
            }
            var user = await _context.Users
                    .Where(u => u.UserName == UserName) // Chuyển userId thành Guid nếu cần
                    .Select(u => new 
                    {
                        Id = u.Id,
                        Email = u.Email,
                        UserName = u.UserName,
                        PersonName = u.personName, // Lấy trường PersonName
                        Roles = _context.UserRoles
                            .Where(ur => ur.UserId == u.Id && ur.IsActive) // Lọc các roles đang hoạt động
                            .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                            .ToList()
                    })
                    .FirstOrDefaultAsync();

            return Ok(user);

        }

        /// <summary>
        /// API to authenticate a user and issue a JWT token
        /// </summary>
        /// <param name="loginDTO">Contain email and password</param>
        /// <returns>JWT token if authentication is successful</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<RegisterResultDTO>> PostLogin(LoginDTO loginDTO)
        {
            // Validate input model
            if (!ModelState.IsValid)
            {
                String errorMessage = string.Join(" | ",
                ModelState.Values.SelectMany(v => v.Errors).Select(e =>
                e.ErrorMessage));
                return Problem(errorMessage);
            }

            // Find user by email
            ApplicationUser? user = await
            _UserManager.FindByNameAsync(loginDTO.Username);

            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid username or password" });  // Không được phép
            }

            // Validate password
            bool isPasswordCorrect = await
            _UserManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!isPasswordCorrect)
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }

            //var roles = await _UserManager.GetRolesAsync(user);
            var roles = await _context.UserRoles.
                Where(ur => ur.UserId == user.Id && ur.IsActive).
                Join(_context.Roles, 
                    ur => ur.RoleId, 
                    r => r.Id, 
                    (ur, r) => r.Name).
                Where(rn => rn != null) // Filter out nulls
                .Select(rn => rn!) // Suppress nullable warning
                .ToListAsync();

            var info = await _context.Employees
                .Where(e => e.EmployeeId == user.EmployeeId)
                .Select(e => new
                {
                    DepartmentId = e.Part != null && e.Part.PartId != Guid.Empty ? e.Part.PartId : Guid.Empty,
                    EmployeeExternalId = e.ExternalId ?? string.Empty,
                    EmployeeId = e.EmployeeId,
                    DepartmentName = e.Part != null && !string.IsNullOrEmpty(e.Part.PartName) ? e.Part.PartName : string.Empty,
                    CompanyId = e.CompanyId
                })
                .FirstOrDefaultAsync();


            if (roles == null || roles.Count == 0 || info == null)
            {
                return Unauthorized(new { Message = "Invalid roles or user info" });
            }
            //var roles = await GetActiveRolesForUser(user, _context);
            // Generate JWT token
            AuthenticationResponse authenticationResponse = _jwtService.CreateJwtJoken(
                user,
                info.DepartmentId,
                info.DepartmentName,
                info.EmployeeExternalId,
                info.EmployeeId,
                info.CompanyId.GetValueOrDefault(),
                roles // Now List<string> (not List<string?>)
            );
            // Store refresh token for future authentication
            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
            await _UserManager.UpdateAsync(user);

            return Ok(new { Token = authenticationResponse, Roles = roles });

        }

        /// <summary>
        /// Create new role
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpPost("Create-role")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name is required");
            }

            if (!await _RoleManager.RoleExistsAsync(roleName))
            {
                var result = await _RoleManager.CreateAsync(new ApplicationRole { Name = roleName });
                if (result.Succeeded)
                {
                    return Ok($"Role {roleName} created");
                }

                return BadRequest(string.Join(" | ", result.Errors.Select(e => e.Description)));
            }
            return BadRequest("Role already exists");
        }

        /// <summary>
        /// Add role for user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpPost("assign-role")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(Guid employeeId,string roleName)
        {
            //var user = await _UserManager.FindByEmailAsync(email);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == employeeId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var role = await _RoleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound("Role Name not found");
            }

            if (!await _RoleManager.RoleExistsAsync(roleName))
            {
                await _RoleManager.CreateAsync(new ApplicationRole { Name = roleName });
            }

            //var result = await _UserManager.AddToRoleAsync(user, roleName);
            var result = await _context.UserRoles.AddAsync(new ApplicationUserRole { UserId = user.Id, RoleId = role.Id });
            await _context.SaveChangesAsync();
            if (result != null)
            {
                return Ok($"Assigned {roleName} to {user.personName}");
            }

            //return BadRequest(string.Join(" | ", result.Select(e => e.Description)));
            return BadRequest("Empty");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        public async Task<ActionResult<RegisterResultDTO>> GetLogout()
        {
            await _SignInManager.SignOutAsync();

            return NoContent();
        }

        /// <summary>
        /// Issues a new Access Token using a valid Refresh Token.
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        [HttpPost("generate-new-jwt-token")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateNewAccessToken
        (TokenModel tokenModel)
        {
            if (tokenModel == null)
            {
                return BadRequest("Invalid client request");
            }

            // Extract user identity from the expired access token.
            ClaimsPrincipal? principal = _jwtService.GetPrincipalFromJwtToken(tokenModel.Token);

            if (principal == null)
            {
                return BadRequest("Invalid jwt access token");
            }

            string? email = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            ApplicationUser? user = await _UserManager.FindByIdAsync(email);

            // Validate the refresh token
            if (user == null || user.RefreshToken != tokenModel.RefreshToken
            || user.RefreshTokenExpirationDateTime <= DateTime.Now)
            {
                return BadRequest("Invalid refresh token");
            }
            // Get role from user
            //var roles = await _UserManager.GetRolesAsync(user);

            var roles = await _context.UserRoles.
                Where(ur => ur.UserId == user.Id && ur.IsActive).
                Join(_context.Roles,
                    ur => ur.RoleId,
                    r => r.Id,
                    (ur, r) => r.Name).
                Where(rn => rn != null)
                .Select(rn => rn!)
                .ToListAsync();

            var info = await _context.Employees
                .Where(e => e.EmployeeId == user.EmployeeId)
                .Select(e => new
                {
                    DepartmentId = e.Part != null && e.Part.PartId != Guid.Empty ? e.Part.PartId : Guid.Empty,
                    EmployeeExternalId = e.ExternalId ?? string.Empty,
                    EmployeeId = e.EmployeeId,
                    DepartmentName = e.Part != null && !string.IsNullOrEmpty(e.Part.PartName) ? e.Part.PartName : string.Empty,
                    CompanyId = e.CompanyId
                })
                .FirstOrDefaultAsync();


            if (roles == null || roles.Count == 0 || info == null)
            {
                return Unauthorized(new { Message = "Invalid roles or user info" });
            }
            //var roles = await GetActiveRolesForUser(user, _context);
            // Generate JWT token
            AuthenticationResponse authenticationResponse = _jwtService.CreateJwtJoken(
                user,
                info.DepartmentId,
                info.DepartmentName,
                info.EmployeeExternalId,
                info.EmployeeId,
                info.CompanyId.GetValueOrDefault(),
                roles // Now List<string> (not List<string?>)
            );

            // Update Refresh Token buy not change expirationDatetime 
            user.RefreshToken = authenticationResponse.RefreshToken;
            authenticationResponse.RefreshTokenExpirationDateTime = user.RefreshTokenExpirationDateTime;
            await _UserManager.UpdateAsync(user);

            return Ok(new { Token = authenticationResponse, Roles = roles });
        }


        [HttpGet("admin-data")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminData()
        {
            return Ok("This is admin-only data!");
        }


        [HttpPatch("update-user-role-status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRoleStatus(string email, string roleName, bool isActive)
        {
            if (email == null || roleName == null)
            {
                return BadRequest(new { Message = "Email or roleName can't empty" });
            }

            var user = await _UserManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BadRequest(new { Message = "Invalid email name" });
            }

            var role = await _RoleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                return BadRequest(new { Message = "Invalid role name" });
            }

            var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.Role.Name == role.Name && ur.User.Id == user.Id);

            if (userRole == null)
            {
                return NotFound(new { Message = "User-role relationship not found" });
            }
            userRole.IsActive = isActive;
            await _context.SaveChangesAsync();
            
            return Ok($"Role = {role}, User = {user}, {userRole.UserId}, {userRole.Role} ");
        }

        [HttpPost("reset-password")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                String errorMessage = string.Join(" | ",
                ModelState.Values.SelectMany(v => v.Errors).Select(e =>
                e.ErrorMessage));
                return Problem(errorMessage);
            }

            //var user = await _context.Users.FindAsync(resetPasswordDTO.userName);
            var user = await _UserManager.FindByNameAsync(resetPasswordDTO.userName);

            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            var token = await _UserManager.GeneratePasswordResetTokenAsync(user);

            var result = await _UserManager.ResetPasswordAsync(user, token, resetPasswordDTO.newPassword);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Password reset successfully" });
            }
            else
            {
                string errorMessage = string.Join(" | ",
                result.Errors.Select(e => e.Description));
                return Problem(errorMessage);
            }
        }
    }
}
