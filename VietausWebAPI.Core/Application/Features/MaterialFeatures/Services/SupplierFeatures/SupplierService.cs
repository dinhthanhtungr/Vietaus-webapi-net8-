using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier.GetDtos;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier.PatchDtos;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier.PostDtos;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Helpers.SupplierFeatures;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Supplier;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts.SupplierFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.Services.SupplierFeatures
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public SupplierService(IUnitOfWork uow
                              ,ICurrentUser currentUser
                              ,IMapper mapper
                              )
        {
            _uow = uow;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        // ======================================================================== Get ======================================================================== 
        /// <summary>
        /// Lấy danh sách nhà cung cấp với phân trang và bộ lọc
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult<PagedResult<GetSupplierSummary>>> GetAllAsync(SupplierQuery query, CancellationToken ct = default)
        {
            try
            {
                var q = query ?? new SupplierQuery();
                var paged = await _uow.SupplierReadRepository.GetPagedSummaryAsync(q, ct);
                return OperationResult<PagedResult<GetSupplierSummary>>.Ok(paged);
            }
            catch (Exception ex)
            {
                return OperationResult<PagedResult<GetSupplierSummary>>.Fail(
                    $"Lỗi khi lấy danh sách nhà cung cấp: {ex.GetBaseException().Message}");
            }
        }
        /// <summary>
        /// Lấy nhà cung cấp theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult<GetSupplier>> GetSupplierByIdAsync(Guid id, CancellationToken ct = default)
        {
            try
            {
                var dto = await _uow.SupplierReadRepository.GetDetailAsync(id, ct);

                return dto != null
                    ? OperationResult<GetSupplier>.Ok(dto)
                    : OperationResult<GetSupplier>.Fail("Không tìm thấy nhà cung cấp");
            }
            catch (Exception ex)
            {
                return OperationResult<GetSupplier>.Fail(
                    $"Lỗi khi lấy thông tin nhà cung cấp: {ex.GetBaseException().Message}");
            }   
        }

        // ======================================================================== Post ========================================================================   
        /// <summary>
        /// Thêm nhà cung cấp mới
        /// </summary>
        /// <param name="supplier"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddNewSuplier(PostSupplier supplier, CancellationToken ct = default)
        {
            await _uow.BeginTransactionAsync(ct);
            try
            {
                if (string.IsNullOrWhiteSpace(supplier.ExternalId))
                {
                    supplier.ExternalId = await ExternalIdGenerator.GenerateCode(
                        "NCC",
                        prefix => _uow.SupplierReadRepository.GetLatestExternalIdStartsWithAsync(prefix, ct)
                    );
                }

                var entity = _mapper.Map<Supplier>(supplier);
                entity.CreatedBy = _currentUser.EmployeeId;
                entity.CreatedDate = DateTime.Now;
                entity.CompanyId = _currentUser.CompanyId;

                await _uow.SupplierWriteRepository.AddAsync(entity, ct);
                var affected = await _uow.SaveChangesAsync(ct);
                await _uow.CommitTransactionAsync(ct);

                return affected > 0
                    ? OperationResult.Ok("Thêm nhà cung cấp thành công")
                    : OperationResult.Fail("Thêm nhà cung cấp thất bại");
            }
            catch (Exception)
            {
                await _uow.RollbackTransactionAsync(ct);
                return OperationResult.Fail("Đã có lỗi xảy ra trong quá trình thêm nhà cung cấp");
            }
        }
        // ======================================================================== Update ======================================================================== 

        /// <summary>
        /// Cập nhật thông tin nhà cung cấp
        /// </summary>
        /// <param name="supplier"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateSupplierAsync(PatchSupplier supplier, CancellationToken ct = default)
        {
            await _uow.BeginTransactionAsync(ct);
            try
            {
                var entity = await _uow.SupplierWriteRepository.GetForUpdateAsync(supplier.SupplierId, ct);
                if (entity == null)
                {
                    await _uow.RollbackTransactionAsync(ct);
                    return OperationResult.Fail("Không tìm thấy nhà cung cấp");
                }

                // PATCH scalar fields (null => bỏ qua)
                PatchHelper.SetIfRef(supplier.SupplierName, () => entity.SupplierName, v => entity.SupplierName = v!);
                PatchHelper.SetIfRef(supplier.Phone, () => entity.Phone, v => entity.Phone = v);
                PatchHelper.SetIfRef(supplier.Website, () => entity.Website, v => entity.Website = v);

                PatchHelper.SetIfRef(supplier.RegistrationAddress, () => entity.RegistrationAddress, v => entity.RegistrationAddress = v);
                PatchHelper.SetIfRef(supplier.RegistrationNumber, () => entity.RegistrationNumber, v => entity.RegistrationNumber = v);

                PatchHelper.SetIfNullable(supplier.IssueDate, () => entity.IssueDate, v => entity.IssueDate = v);
                PatchHelper.SetIfRef(supplier.IssuedPlace, () => entity.IssuedPlace, v => entity.IssuedPlace = v);

                PatchHelper.SetIfRef(supplier.FaxNumber, () => entity.FaxNumber, v => entity.FaxNumber = v);
                PatchHelper.SetIfRef(supplier.TaxNumber, () => entity.TaxNumber, v => entity.TaxNumber = v);

                PatchHelper.SetIfNullable(supplier.IsActive, () => entity.IsActive, v => entity.IsActive = v);

                entity.UpdatedBy = _currentUser.EmployeeId;
                entity.UpdatedDate = DateTime.Now;

                // Sync aggregate: quan trọng nhất là APPLY IsActive theo dto (soft delete)
                var addedAddresses = SupplierAggregateSync.SyncAddresses(entity, supplier.SupplierAddresses , treatMissingAsSoftDelete: false);
                var addedContacts = SupplierAggregateSync.SyncContacts(entity, supplier.SupplierContacts , treatMissingAsSoftDelete: false);

                if (addedAddresses.Count > 0) await _uow.SupplierWriteRepository.AddAddressAsync(addedAddresses, ct);
                if (addedContacts.Count > 0) await _uow.SupplierWriteRepository.AddContactAsync(addedContacts, ct);

                var affected = await _uow.SaveChangesAsync(ct);
                await _uow.CommitTransactionAsync(ct);

                return affected > 0 ? OperationResult.Ok("Cập nhật nhà cung cấp thành công")
                                    : OperationResult.Fail("Cập nhật nhà cung cấp thất bại");
            }
            catch (DbUpdateConcurrencyException)
            {
                await _uow.RollbackTransactionAsync(ct);
                return OperationResult.Fail("Dữ liệu đã thay đổi ở nơi khác. Vui lòng tải lại và thử lại.");
            }
            catch
            {
                await _uow.RollbackTransactionAsync(ct);
                return OperationResult.Fail("Đã có lỗi xảy ra trong quá trình cập nhật nhà cung cấp");
            }
        }

        /// <summary>
        /// Xóa mềm nhà cung cấp theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OperationResult> DeleteSupplierByIdAsync(Guid id, CancellationToken ct = default)
        {
            await _uow.BeginTransactionAsync(ct);

            try
            {
                var ok = await _uow.SupplierWriteRepository.SoftDeleteAsync(id, _currentUser.EmployeeId, DateTime.Now, ct);
                
                if (!ok)
                {
                    await _uow.RollbackTransactionAsync(ct);
                    return OperationResult.Fail("Xoá nhà cung cấp thất bại. Vui lòng kiểm tra lại.");
                }

                var affected = await _uow.SaveChangesAsync(ct);
                await _uow.CommitTransactionAsync(ct);

                return affected > 0
                    ? OperationResult.Ok("Xoá nhà cung cấp thành công")
                    : OperationResult.Fail("Xoá nhà cung cấp thất bại");
            }
            catch (Exception)
            {
                await _uow.RollbackTransactionAsync(ct);
                return OperationResult.Fail("Đã có lỗi xảy ra trong quá trình xoá nhà cung cấp");
            }
        }
    }
}
