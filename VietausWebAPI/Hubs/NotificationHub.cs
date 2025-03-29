using Microsoft.AspNetCore.SignalR;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.WebAPI.Hubs
{
    // {"protocol":"json","version":1}
    public sealed class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;
        private readonly IUserConnectionService _userConnectionService;
        public NotificationHub(ILogger<NotificationHub> logger, IUserConnectionService userConnectionService)
        {
            _logger = logger;
            _userConnectionService = userConnectionService;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst("sub")?.Value;
            var role = Context.User?.FindFirst("role")?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                await _userConnectionService.AddUserConnection(userId, Context.ConnectionId, role);
                _logger.LogInformation($"User {userId} connected with connectionId {Context.ConnectionId}");
            }

            await base.OnConnectedAsync();

            //await Clients.All.SendAsync("UserConnected", /*Context.ConnectionId + */$"{Context.ConnectionId} has joined");
            //await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User?.FindFirst("sub")?.Value;
            //var role = Context.User?.FindFirst("role")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await _userConnectionService.RemoveUserConnection(userId, Context.ConnectionId);
                _logger.LogInformation($"User {userId} disconnected with connectionId {Context.ConnectionId}");
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}