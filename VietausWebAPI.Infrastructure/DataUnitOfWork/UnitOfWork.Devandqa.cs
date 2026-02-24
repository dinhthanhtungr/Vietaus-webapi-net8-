using VietausWebAPI.Core.Application.Features.DevandqaFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.RepositoriesContracts.QCInputByQCFeatures;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public sealed partial class UnitOfWork
    {
        public IProductStandardRepository ProductStandardRepository { get; }
        public IProductTestRepository ProductTestRepository { get; }
        public IProductInspectionRepository ProductInspectionRepository { get; }
        public IQCInputByQCRepository QCInputByQCRepository { get; }

        // QCInputByQCFeatures
        public IQCInputByQCReadRepository QCInputByQCReadRepository { get; }
        public IQCInputByQCWriteRepository QCInputByQCWriteRepository { get; }
    }
}
