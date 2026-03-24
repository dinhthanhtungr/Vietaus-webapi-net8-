using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier.GetDtos;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier.PatchDtos;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier.PostDtos;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Supplier;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts.SupplierFeatures
{
    public interface ISupplierService
    {
        // ======================================================================== Get ======================================================================== 
        /// <summary>
        /// Lấy danh sách nhà cung cấp với phân trang và bộ lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult<PagedResult<GetSupplierSummary>>> GetAllAsync(SupplierQuery query, CancellationToken ct = default);
        /// <summary>
        /// Lấy nhà cung cấp theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult<GetSupplier>> GetSupplierByIdAsync(Guid id, CancellationToken ct = default);   
       
        // ======================================================================== Post ========================================================================   
        /// <summary>
        /// Thêm nhà cung cấp mới
        /// </summary>
        /// <param name="supplier"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult> AddNewSuplier(PostSupplier supplier, CancellationToken ct = default);

        // ======================================================================== Update ======================================================================== 
        /// <summary>
        /// Cập nhật thông tin nhà cung cấp
        /// </summary>
        /// <param name="supplier"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult> UpdateSupplierAsync(PatchSupplier supplier, CancellationToken ct = default);
        /// <summary>
        /// Xóa nhà cung cấp theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OperationResult> DeleteSupplierByIdAsync(Guid id, CancellationToken ct = default);
    }
}
