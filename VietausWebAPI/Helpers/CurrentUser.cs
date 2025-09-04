using System.Security.Claims;

namespace VietausWebAPI.WebAPI.Helpers
{
    public sealed class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _http;
        public CurrentUser(IHttpContextAccessor http) => _http = http;

        private ClaimsPrincipal? Principal => _http.HttpContext?.User;

        public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;

        public Guid? UserId =>
            GetGuid(ClaimTypes.NameIdentifier)   // ASP.NET Identity
            ?? GetGuid("sub")                    // JWT standard
            ?? GetGuid("uid");                   // tuỳ IdP

        public Guid? CompanyId =>
            GetGuid("company_id")                // tuỳ bạn nhúng claim này
            ?? GetGuid("tenant_id");

        public string? UserName =>
            Get(ClaimTypes.Name) ?? Get("preferred_username") ?? Get("unique_name");

        public string? Email => Get(ClaimTypes.Email) ?? Get("email");

        public IEnumerable<string> Roles =>
            Principal?.FindAll(ClaimTypes.Role).Select(c => c.Value) ?? Enumerable.Empty<string>();

        private string? Get(string type) =>
            Principal?.Claims.FirstOrDefault(c => c.Type == type)?.Value;

        private Guid? GetGuid(string type) =>
            Guid.TryParse(Get(type), out var g) ? g : null;
    }
}
