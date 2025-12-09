using VietausWebAPI.Core.Application.Features.DevandqaFeatures.RepositoriesContracts;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public sealed partial class UnitOfWork
    {
        public IProductStandardRepository ProductStandardRepository { get; }
        public IProductTestRepository ProductTestRepository { get; }
    }
}
