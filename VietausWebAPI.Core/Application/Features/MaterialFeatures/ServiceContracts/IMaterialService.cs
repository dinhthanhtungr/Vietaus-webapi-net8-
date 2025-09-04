using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Material;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts
{
    public interface IMaterialService
    {
        /// <summary>
        /// Lấy danh sách vật tư, có phân trang, lọc, tìm kiếm
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PagedResult<GetMaterialSummary>> GetAllAsync(MaterialQuery query, CancellationToken ct = default);

        /// <summary>
        /// Thêm mới vật tư, kèm giá khởi tạo   
        /// </summary>
        /// <param name="material"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult> AddNewMaterialAsync(PostMaterial material, CancellationToken ct = default);

        /// <summary>
        /// Lấy danh sách nhà cung cấp cho vật tư, có phân trang, lọc, tìm kiếm 
        /// </summary>
        /// <param name="material"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PagedResult<GetMaterialSupplier>> GetMaterialSupplierAsync(MaterialQuery query, CancellationToken ct = default);

        /// <summary>
        /// Lấy danh sách nhà cung cấp cho vật tư, có phân trang, lọc, tìm kiếm 
        /// </summary>
        /// <param name="material"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<GetMaterial>GetMaterialByIdAsync(Guid Id, CancellationToken ct = default);

        /// <summary>
        /// Cập nhật vật tư
        /// </summary>
        /// <param name="req"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult> UpsertMaterialAsync(GetMaterial req, CancellationToken ct = default);

        /// <summary>
        /// Xóa mềm vật tư
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult> DeleteMaterialAsync(Guid Id, CancellationToken ct = default);
    }
}
