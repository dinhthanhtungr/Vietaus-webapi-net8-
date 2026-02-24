using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts.SupplierFeatures;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Materials.SupplierFeatures
{
    public class SupplierWriteRepository : ISupplierWriteRepository
    {
        private readonly ApplicationDbContext _context;
        public SupplierWriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ======================================================================== Post ======================================================================== 
        /// <summary>
        /// Thêm một nhà cung cấp mới
        /// </summary>
        /// <param name="supplier"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task AddAsync(Supplier supplier, CancellationToken ct)
        {
            await _context.Suppliers.AddAsync(supplier);
        }
       
        /// <summary>
        /// Thêm địa chỉ cho nhà cung cấp
        /// </summary>
        /// <param name="address"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task AddAddressAsync(IEnumerable<SupplierAddress> address, CancellationToken ct)
        {
            await _context.SupplierAddresses.AddRangeAsync(address, ct);
        }
     
        /// <summary>
        /// Thêm một liên hệ nhà cung cấp mới
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="ct"></param>
        /// <returns></returns> 
        public async Task AddContactAsync(IEnumerable<SupplierContact> contact, CancellationToken ct)
        {
            await _context.SupplierContacts.AddRangeAsync(contact, ct);
        }

        // ======================================================================== Patch ======================================================================== 
        /// <summary>
        /// Lấy nhà cung cấp để cập nhật
        /// Lấy luôn các thuộc tính liên quan
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<Supplier?> GetForUpdateAsync(Guid id, CancellationToken ct)
        {
            return _context.Suppliers
                .AsTracking()
                .Include(x => x.SupplierAddresses)
                .Include(x => x.SupplierContacts)
                .FirstOrDefaultAsync(x => x.SupplierId == id, ct);

        }

        /// <summary>
        /// Xóa mềm một nhà cung cấp
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedBy"></param>
        /// <param name="now"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<bool> SoftDeleteAsync(Guid id, Guid updatedBy, DateTime now, CancellationToken ct)
        {
            var supplier = await _context.Suppliers.FindAsync(new object?[] { id }, ct);
            if (supplier is null)
            {
                return false;
            }

            supplier.IsActive = false;
            supplier.UpdatedBy = updatedBy;
            supplier.UpdatedDate = now;

            return true;
        }
    }
}
