using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.RepositoriesContracts.ExtruderOperationHistoryRepositories;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public partial class UnitOfWork
    {
        // ExtruderOperationHistory
        public IExtruderOperationHistoryReadRepositories ExtruderOperationHistoryReadRepositories { get; }
    }
}
