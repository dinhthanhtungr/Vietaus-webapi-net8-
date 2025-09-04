
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts
{
    public interface ISupplierRepository
    {
        Task AddNewSuplier(Supplier supplier);
        IQueryable<Supplier> Query(bool track = false);
        /// <summary>
        /// lấy sô cuối cùng của code
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        Task<string?> GetLatestExternalIdStartsWithAsync(string prefix);

        Task<bool> UpdateSupplierAsync(PatchSupplier supplier);

        Task<bool> DeleteSupplierByIdAsync(Guid id);
    }
}
