using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.TimelineFeature.RepositoriesContracts;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        IEventLogRepository EventLogRepository { get; }
    }
}
