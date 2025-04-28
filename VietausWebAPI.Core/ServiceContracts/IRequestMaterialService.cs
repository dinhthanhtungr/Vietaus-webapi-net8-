using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.DTO.PostDTO;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Core.Entities;

namespace VietausWebAPI.Core.ServiceContracts
{
    public interface IRequestMaterialService
    {
        /// <summary>
        /// Tạo một đề xuất mua vật tư với đầy đủ các thông số
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<string> CreateRequestMaterial(RequestMaterialDTO request);
        /// <summary>
        /// Lấy ra mã đề xuất cuối cùng
        /// </summary>
        /// <returns></returns>
        //Task<RequestIdDTO> GetLastRequestIdService();
        Task<string> GetLastRequestIdService();
        /// <summary>
        /// Lấy ra danh sách vật tư theo các điều kiện tìm kiếm
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedResult<RequestMaterialDTO>> GetMaterialAsyncService(RequestMaterialQuery query);
        /// <summary>
        /// Lấy ra danh sách đã được trải phẳng vật tư theo các điều kiện tìm kiếm
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedResult<FlatRequestMaterialDto>> FlatRequestMaterialService(RequestMaterialQuery query);
    }
}
