using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Entities;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface IRequestDetailMaterialRepository
    {
        /// <summary>
        /// Thêm mới danh sách các vật tư đề xuất đi chung với lại request
        /// </summary>
        /// <param name="requestDetailMaterialDatum"></param>
        /// <returns></returns>
        Task AddRequetMaterialRepository(IEnumerable<RequestDetailMaterialDatum> requestDetailMaterialDatum);
        /// <summary>
        /// Lấy tất cả danh sách vật tư đề xuất
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<RequestDetailMaterialDatum>> GetAllRequestMaterialRepository();
    }
}
