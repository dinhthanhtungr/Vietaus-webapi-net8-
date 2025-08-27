
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts
{
    public interface ISupplierRepository
    {
        Task AddNewSuplier(Supplier supplier);
        IQueryable<Supplier> Query();
    }
}
