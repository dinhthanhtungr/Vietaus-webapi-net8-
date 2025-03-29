using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Service
{
    /// <summary>
    /// Service quản lý kết nối người dùng
    /// </summary>
    public class UserConnectionService : IUserConnectionService
    {
        private readonly Dictionary<string, (string ConnectionId, string Role)> _connections = new();
        /// <summary>
        /// Thêm kết nối của người dùng
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="connectionId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task AddUserConnection(string userId, string connectionId, string role)
        {
            lock (_connections)
            {
                _connections[userId] = (connectionId, role);
            }
            return Task.CompletedTask;
        }
        /// <summary>
        /// Xóa kết nối của người dùng
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task RemoveUserConnection(string userId, string connectionId)
        {
            lock (_connections)
            {
                if (_connections.TryGetValue(userId, out var conn) && conn.ConnectionId == connectionId)
                {
                    _connections.Remove(userId);
                }
            }
            return Task.CompletedTask;
        }
        /// <summary>
        /// Lấy danh sách kết nối của người dùng
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public (string ConnectionId, string Role)? GetUserConnections(string userId)
        {
            lock (_connections)
            {
                if (_connections.TryGetValue(userId, out var conn))
                {
                    return conn;
                }
            }
            return null;
        }
        /// <summary>
        /// Lấy danh sách kết nối của admin
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<(string UserId, string ConnectionId)> GetAdminConnections()
        {
            lock (_connections)
            {
                return _connections
                    .Where(x => x.Value.Role == "Admin")
                    .Select(x =>  (x.Key, x.Value.ConnectionId));
            }
        }

    }
}
