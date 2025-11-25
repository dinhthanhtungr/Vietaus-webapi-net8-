using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Notifications.RepositoriesContracts;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public sealed partial class UnitOfWork
    {
        // ====== Properties nhóm Notifications ======
        public INotificationRepository Notifications { get; }
        public INotificationRecipientRepository NotificationRecipients { get; }
        public INotificationUserStateRepository NotificationUserStates { get; }
        public IUserNotificationSettingRepository UserNotificationSettings { get; }
        public INotificationTemplateRepository NotificationTemplates { get; }
        public IOutboxMessageRepository OutboxMessages { get; }
    }
}
