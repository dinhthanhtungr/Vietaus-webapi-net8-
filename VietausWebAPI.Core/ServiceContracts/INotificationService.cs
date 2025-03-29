using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.ServiceContracts
{
    public interface INotificationService
    {
        Task SendMessage(string user, string message);
    }
}
