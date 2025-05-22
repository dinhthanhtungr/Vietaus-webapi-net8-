using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface IMaterialGroupsRepository
    {
        /// <summary>
        /// Thêm mới nhóm vật liệu
        /// </summary>
        /// <param name="materialGroup"></param>
        /// <returns></returns>
        Task AddMaterialGroupRepositoryAsync(MaterialGroupsMaterialDatum materialGroup);
        /// <summary>
        /// Lấy tất cả nhóm vật liệu
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<MaterialGroupsMaterialDatum>> GetAllMaterialGroupsRepositoryAsync();
    }
}
