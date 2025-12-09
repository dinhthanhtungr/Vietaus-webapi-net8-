using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Notifications.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.Notifications;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.Notifications
{
    public class UserNotificationSettingRepository : Repository<UserNotificationSetting>, IUserNotificationSettingRepository
    {
        public UserNotificationSettingRepository(ApplicationDbContext context) : base(context) { }
    }
}
