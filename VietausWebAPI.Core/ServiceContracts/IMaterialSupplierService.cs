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
        /// <summary>
        /// Thêm mới nhà cung cấp vật liệu
        /// </summary>
        /// <param name="materialSuppliersDTO"></param>
        /// <returns></returns>
        Task AddMaterialSuppliersServiceAsync(MaterialSuppliersDTO materialSuppliersDTO);
        /// <summary>
        /// Lấy tất cả nhà cung cấp vật liệu
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<MaterialSuppliersDTO>> GetAllMaterialSuppliersServiceAsync();
    }
}
