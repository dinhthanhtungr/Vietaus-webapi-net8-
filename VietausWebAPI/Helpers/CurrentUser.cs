using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;

namespace VietausWebAPI.WebAPI.Helpers
{
    public sealed class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _http;
        public CurrentUser(IHttpContextAccessor http) => _http = http;

        private ClaimsPrincipal? Principal => _http.HttpContext?.User;
        public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;

        // === Helpers ===
        private string? GetCI(params string[] types)
        {
            // case-insensitive find
            if (Principal is null) return null;
            foreach (var type in types)
            {
                var claim = Principal.Claims.FirstOrDefault(
                    c => string.Equals(c.Type, type, StringComparison.OrdinalIgnoreCase));
                if (claim is { Value: { } v } && !string.IsNullOrWhiteSpace(v)) return v;
            }
            return null;
        }

        private static Guid ToGuidOrDefault(string? s)
            => Guid.TryParse(s, out var g) ? g : Guid.Empty;

        private void EnsureAuthenticated()
        {
            if (!IsAuthenticated)
                throw new UnauthorizedAccessException("Unauthenticated.");
        }

        private string AvailableClaimTypes()
            => Principal is null
                ? "(no principal)"
                : string.Join(", ",
                    Principal.Claims
                             .Select(c => c.Type)
                             .Distinct(StringComparer.OrdinalIgnoreCase)
                             .OrderBy(x => x));

        // ====== ICurrentUser ======

        // 1) EmployeeId (GUID)
        private static readonly string[] EmployeeIdKeys = new[]
        {
            // bạn chuẩn hoá claim này càng tốt
            "EmployeeId", "Id", "employee_id", "empId", "employeeId",
            "employee_guid", "employeeGuid", "sid" // 'sid' đôi khi là GUID nội bộ
        };

        public Guid EmployeeId
        {
            get
            {
                EnsureAuthenticated();

                // thử lần lượt các key (không phân biệt hoa/thường)
                foreach (var key in EmployeeIdKeys)
                {
                    var g = ToGuidOrDefault(GetCI(key));
                    if (g != Guid.Empty) return g;
                }

                // nếu thất bại: báo rõ đang có những claim nào
                throw new UnauthorizedAccessException(
                    $"EmployeeId not found or not a GUID. Looked in [{string.Join(", ", EmployeeIdKeys)}]. " +
                    $"Available claim types: {AvailableClaimTypes()}");
            }
        }

        // 2) EMPxxxx (mã ngoài)
        public string EmployeeExternalId
        {
            get
            {
                EnsureAuthenticated();

                // CHÚ Ý: tránh trùng tên với GUID ở trên.
                // Chuẩn hoá: để mã ngoài vào 'employee_code' (không dùng 'employeeId' cho mã text)
                var s = GetCI("employee_code", "emp_code", "empCode", "external_id");
                if (!string.IsNullOrWhiteSpace(s)) return s;

                // fallback ít dùng (nếu hệ thống cũ để text trong employeeId)
                s = GetCI("employeeId");
                if (!string.IsNullOrWhiteSpace(s) && !Guid.TryParse(s, out _)) return s;

                throw new UnauthorizedAccessException(
                    "EmployeeExternalId not found. Expected claim: employee_code / emp_code.");
            }
        }

        // 3) CompanyId (GUID)
        private static readonly string[] CompanyIdKeys = new[]
        {
            "CompanyId", "companyId", "company_id",
            "tenantId", "tenant_id", "tid", // AAD tenant id
            "org_id", "organizationId"
        };

        public Guid CompanyId
        {
            get
            {
                EnsureAuthenticated();
                foreach (var key in CompanyIdKeys)
                {
                    var g = ToGuidOrDefault(GetCI(key));
                    if (g != Guid.Empty) return g;
                }

                // (tuỳ chọn) fallback header/query — chỉ bật nếu bạn kiểm soát uỷ quyền
                var h = _http.HttpContext?.Request.Headers["X-Company-Id"].FirstOrDefault();
                var gh = ToGuidOrDefault(h);
                if (gh != Guid.Empty) return gh;

                var qs = _http.HttpContext?.Request.Query["companyId"].FirstOrDefault();
                var gq = ToGuidOrDefault(qs);
                if (gq != Guid.Empty) return gq;

                throw new UnauthorizedAccessException(
                    $"CompanyId not found or not a GUID. Looked in [{string.Join(", ", CompanyIdKeys)}], " +
                    $"header 'X-Company-Id', query 'companyId'. " +
                    $"Available claim types: {AvailableClaimTypes()}");
            }
        }

        // 4) UserAccountId (GUID)
        public Guid UserAccountId
        {
            get
            {
                EnsureAuthenticated();
                var keys = new[] { "sub", "uid", "oid", ClaimTypes.NameIdentifier };
                foreach (var k in keys)
                {
                    var g = ToGuidOrDefault(GetCI(k));
                    if (g != Guid.Empty) return g;
                }
                throw new UnauthorizedAccessException(
                    "UserAccountId not found or not a GUID. Expected claims: sub / uid / oid / NameIdentifier (GUID). " +
                    $"Available claim types: {AvailableClaimTypes()}");
            }
        }

        // 5) personName (full name)
        public string personName
        {
            get
            {
                EnsureAuthenticated();

                var s = GetCI("name", ClaimTypes.Name);
                if (!string.IsNullOrWhiteSpace(s)) return s!;

                var given = GetCI("given_name", ClaimTypes.GivenName);
                var family = GetCI("family_name", ClaimTypes.Surname);
                if (!string.IsNullOrWhiteSpace(given) || !string.IsNullOrWhiteSpace(family))
                    return string.Join(' ', new[] { given, family }.Where(x => !string.IsNullOrWhiteSpace(x)));

                s = GetCI("preferred_username", "unique_name", "upn");
                if (!string.IsNullOrWhiteSpace(s)) return s!;

                // cuối cùng thử email
                s = GetCI(ClaimTypes.Email, "email");
                if (!string.IsNullOrWhiteSpace(s)) return s!;

                throw new UnauthorizedAccessException("personName not found in claims.");
            }
        }

        // 6) UserName (account/handle đăng nhập)
        public string UserName
        {
            get
            {
                EnsureAuthenticated();

                var s = GetCI("preferred_username", "unique_name", "upn");
                if (!string.IsNullOrWhiteSpace(s)) return s!;

                s = GetCI(ClaimTypes.Name, "name");
                if (!string.IsNullOrWhiteSpace(s)) return s!;

                s = GetCI(ClaimTypes.Email, "email");
                if (!string.IsNullOrWhiteSpace(s)) return s!;

                throw new UnauthorizedAccessException("UserName not found in claims.");
            }
        }

        public IEnumerable<string> Roles
        {
            get
            {
                if (Principal is null) return Enumerable.Empty<string>();
                var r1 = Principal.FindAll(ClaimTypes.Role).Select(c => c.Value);
                var r2 = Principal.FindAll("role").Select(c => c.Value);
                var r3 = Principal.FindAll("roles").Select(c => c.Value);
                return r1.Concat(r2).Concat(r3)
                         .Where(v => !string.IsNullOrWhiteSpace(v))
                         .Distinct(StringComparer.OrdinalIgnoreCase);
            }
        }
    }
}
