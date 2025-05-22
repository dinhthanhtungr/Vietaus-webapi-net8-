using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface IMaterialSuppliersRepository
    {
        /// <summary>
        /// Thêm mới nhà cung cấp vật liệu
        /// </summary>
        /// <param name="materialSuppliers"></param>
        /// <returns></returns>
        Task AddMaterialSupplierRepositoryAsync(MaterialsSuppliersMaterialDatum materialSuppliers);
        /// <summary>
        /// Lấy tất cả nhà cung cấp vật liệu
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<MaterialsSuppliersMaterialDatum>> GetAllMaterialSuppliersRepositoryAsync();
    }
}
