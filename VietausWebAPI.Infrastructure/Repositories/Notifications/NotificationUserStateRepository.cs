using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Notifications.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.Notifications;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.Notifications
{
    public class NotificationUserStateRepository : Repository<NotificationUserState>, INotificationUserStateRepository
    {
        public NotificationUserStateRepository(ApplicationDbContext context) : base(context) { }
    }
}
