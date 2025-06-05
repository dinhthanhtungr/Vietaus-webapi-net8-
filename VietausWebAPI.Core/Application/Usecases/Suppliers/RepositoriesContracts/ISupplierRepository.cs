using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.Suppliers;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Usecases.Suppliers.RepositoriesContracts
{
    public interface ISupplierRepository
    {
        Task<List<SuppliersMaterialDatum>> GetAllNameSupplierRepository();
        Task<IEnumerable<SupplierAddressesMaterialDatum>> GetSupplierAddress(Guid supplierId);
    }
}
