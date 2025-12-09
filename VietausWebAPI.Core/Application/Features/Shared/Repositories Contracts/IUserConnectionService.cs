using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts
{
    public interface IUserConnectionService
    {
        public Task AddUserConnection(string userId, string connectionId, string role);
        public Task RemoveUserConnection(string userId, string connectionId);
        public (string ConnectionId, string Role)? GetUserConnections(string userId);
        public IEnumerable <(string UserId, string ConnectionId)> GetAdminConnections();
    }
}
