using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.RepositoriesContracts.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.ServiceContracts;

namespace VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        IProductInspectionRepository ProductInspectionRepository { get; }
        IProductStandardRepository ProductStandardRepository { get; }
        IProductTestRepository ProductTestRepository { get; }
        IQCInputByQCRepository QCInputByQCRepository { get; }

        // QCInputByQCFeatures
        IQCInputByQCReadRepository QCInputByQCReadRepository { get; }
        IQCInputByQCWriteRepository QCInputByQCWriteRepository { get; }
    }
}
