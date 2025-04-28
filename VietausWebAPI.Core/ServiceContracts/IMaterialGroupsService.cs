using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO.GetDTO;

namespace VietausWebAPI.Core.ServiceContracts
{
    public interface IMaterialGroupsService
    {
        /// <summary>
        /// Thêm mới nhóm vật liệu
        /// </summary>
        /// <param name="materialGroupDTO"></param>
        /// <returns></returns>
        Task AddMaterialGroupServiceAsync(MaterialGroupsDTO materialGroupDTO);
        /// <summary>
        /// Lấy tất cả nhóm vật liệu
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<MaterialGroupsDTO>> GetAllMaterialGroupsServiceAsync();
    }
}
