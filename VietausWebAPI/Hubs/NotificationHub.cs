using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.WebAPI.Hubs
{
    [Authorize] // quan trọng: kết nối phải có JWT
    public class NotificationHub : Hub
    {
        private readonly ICurrentUser _CurrentId = null!;
        public NotificationHub(ICurrentUser currentUser)
        {
            _CurrentId = currentUser;
        }
        public override async Task OnConnectedAsync()
        {
            
            //var companyClaim = Context.User?.FindFirst("company_id")?.Value;
            //var userClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
            //                ?? Context.User?.FindFirst("employee_id")?.Value;

            var userClaim = _CurrentId.EmployeeId.ToString();
            var companyClaim = _CurrentId.CompanyId.ToString();

            var companyKey = companyClaim is null ? null : Guid.Parse(companyClaim).ToString("N");

            if (companyKey != null)
                await Groups.AddToGroupAsync(Context.ConnectionId, $"company:{companyKey}");

            if (Guid.TryParse(userClaim, out var userId))
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");

            var roles = Context.User?.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value.Trim().ToUpperInvariant())
                .Distinct()
                .ToList() ?? new();

            foreach (var r in roles)
            {
                if (companyKey != null)
                    await Groups.AddToGroupAsync(Context.ConnectionId, $"role:{companyKey}:{r}");
            }

            await base.OnConnectedAsync();
        }

        // Giữ lại các API thủ công nếu cần
        public Task JoinCompany(string companyId)
            => Groups.AddToGroupAsync(Context.ConnectionId, $"company:{companyId}");

        public Task JoinUser(string userId)
            => Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");

        public async Task JoinRole(string companyId, string roleName)
        {
            var roles = Context.User?.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value.Trim().ToUpperInvariant())
                .ToHashSet() ?? new HashSet<string>();

            var normalized = roleName.Trim().ToUpperInvariant();
            if (roles.Contains(normalized))
                await Groups.AddToGroupAsync(Context.ConnectionId, $"role:{companyId}:{normalized}");
        }
    }
}