using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.Core.Service
{
    public class NotificationService : INotificationService
    {
        public Task SendMessage(string user, string message)
        {
            throw new NotImplementedException();
        }
    }
}
