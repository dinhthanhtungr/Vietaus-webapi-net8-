using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using VietausWebAPI.Core.DTO.GetDTO;
using VietausWebAPI.Core.DTO.QueryObject;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface IRequestMaterialRepository
    {
        /// <summary>
        /// Tạo một đề xuất mua vật tư với đầy đủ các thông số
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SupplyRequestsMaterialDatum> CreateRequestAsync(SupplyRequestsMaterialDatum request);
        /// <summary>
        /// Thêm chi tiết đề xuất mua vật tư
        /// </summary>
        /// <param name="requestDetail"></param>
        /// <returns></returns>
        Task AddRequestDetailMaterialAsync(List<RequestDetailMaterialDatum> requestDetail);
        /// <summary>
        /// Lấy ra mã đề xuất cuối cùng
        /// </summary>
        /// <returns></returns>
        Task<string> GetLastRequestIdRepository();
        /// <summary>
        /// Lấy ra danh sách đề xuất mua vật tư theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedResult<SupplyRequestsMaterialDatum>> GetRequestRepository(RequestMaterialQuery query);
        /// <summary>
        /// Lấy ra danh sách đã được trải phẳng vật tư theo các điều kiện tìm kiếm
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedResult<FlatRequestMaterialDto>> FlatRequestMaterialRepository(RequestMaterialQuery query);
    }
}
