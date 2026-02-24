using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts.SupplierFeatures
{
    public interface ISupplierWriteRepository
    {
        // ======================================================================== Post ======================================================================== 
        /// <summary>
        /// Thêm một nhà cung cấp mới
        /// </summary>
        /// <param name="supplier"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task AddAsync(Supplier supplier, CancellationToken ct);
        /// <summary>
        /// Thêm một đia chỉ nhà cung cấp mới
        /// </summary>
        /// <param name="address"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task AddAddressAsync(IEnumerable<SupplierAddress> address, CancellationToken ct);
        /// <summary>
        /// Thêm một liên hệ nhà cung cấp mới
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task AddContactAsync(IEnumerable<SupplierContact> contact, CancellationToken ct);

        // ======================================================================== Patch ======================================================================== 
        /// <summary>
        /// Xóa mềm nhà cung cấp theo Id
        /// </summary>
        Task<bool> SoftDeleteAsync(Guid id, Guid updatedBy, DateTime now, CancellationToken ct);
        /// <summary>
        /// Lấy aggreate Supplier để cập nhật
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<Supplier?> GetForUpdateAsync(Guid id, CancellationToken ct);

    }
}
