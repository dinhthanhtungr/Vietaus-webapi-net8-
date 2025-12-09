using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Notifications.RepositoriesContracts;

namespace VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        INotificationRepository Notifications { get; }
        INotificationRecipientRepository NotificationRecipients { get; }
        INotificationUserStateRepository NotificationUserStates { get; }
        IUserNotificationSettingRepository UserNotificationSettings { get; }
        INotificationTemplateRepository NotificationTemplates { get; }
        IOutboxMessageRepository OutboxMessages { get; }
    }
}
