using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgProductionOrdersPlanRepository;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts
{
    public interface IMfgProductionOrdersPlanRepository
    {
        //Task<MfgProductionOrdersPlan> GetMfgProductionOrdersPlanByIdAsync(Guid id); // Hàm này bị con ghẻ mẹ nó rồi éo biết trước dùng để làm gì luôn, thôi cứ để lỡ đâu có lỗi, tương lai thấy ko lỗi thì xóa
        Task<PagedResult<MfgProductionOrdersPlan>> GetPagedAsync(MfgPOLQuery query);
        /// <summary>
        /// Thay đổi tên sản phẩm nhưng mà có thể xóa sau này nên để tạm vào service của lab nha.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="newProductName"></param>
        /// <returns></returns>
        Task UpdateProductNameInPlansAsync(Guid productId, string newProductName);
        Task<MfgProductionOrdersPlan> GetPagedByIdAsync(string id);
    }
}
