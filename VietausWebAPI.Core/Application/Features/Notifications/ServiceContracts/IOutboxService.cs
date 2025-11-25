using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Notifications.DTOs;

namespace VietausWebAPI.Core.Application.Features.Notifications.ServiceContracts
{
    public interface IOutboxService 
    {
        Task InAppPushOutboxHandler (OutboxEnvelope envelope);
    }
}
