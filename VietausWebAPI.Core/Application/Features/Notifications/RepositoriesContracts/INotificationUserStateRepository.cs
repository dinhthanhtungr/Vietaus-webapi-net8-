using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Domain.Entities.Notifications;

namespace VietausWebAPI.Core.Application.Features.Notifications.RepositoriesContracts
{
    public interface INotificationUserStateRepository : IRepository<NotificationUserState> { }
}
