using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Notifications.DTOs;

namespace VietausWebAPI.Core.Application.Features.Notifications.ServiceContracts
{
    public interface INotificationService
    {
        Task<Guid> PublishAsync(PublishNotificationRequest req, CancellationToken ct = default);
        Task<int> GetUnreadCountAsync(CancellationToken ct = default);
        Task<IReadOnlyList<NotificationDto>> GetFeedAsync(int take = 20, Guid? afterId = null, DateTime? afterCreated = null, CancellationToken ct = default);
        Task MarkReadAsync(Guid notiId, CancellationToken ct = default);
        Task<int> MarkAllReadAsync(CancellationToken ct = default);
        Task<NotificationDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    }
}
