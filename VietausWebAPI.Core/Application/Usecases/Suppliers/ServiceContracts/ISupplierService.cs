using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.Suppliers;

namespace VietausWebAPI.Core.Application.Usecases.Suppliers.ServiceContracts
{
    public interface ISupplierService
    {
        Task<List<SupplierInformationsDTO>> GetAllSupplierName();
        //Task<IEnumerable<SupplierAddressDTO>> GetSupplierAddress(Guid supplierId);
    }
}
