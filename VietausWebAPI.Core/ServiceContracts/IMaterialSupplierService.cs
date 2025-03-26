using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO.PostDTO;

namespace VietausWebAPI.Core.ServiceContracts
{
    public interface IMaterialSupplierService
    {
        Task AddMaterialSuppliersServiceAsync(MaterialSuppliersDTO materialSuppliersDTO);
        Task<IEnumerable<MaterialSuppliersDTO>> GetAllMaterialSuppliersServiceAsync();
    }
}
