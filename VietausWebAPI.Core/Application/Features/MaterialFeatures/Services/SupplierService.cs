using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Supplier;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.TransferCustomerDTOs;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SupplierService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OperationResult> AddNewSuplier(PostSupplier supplier, CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (string.IsNullOrWhiteSpace(supplier.ExternalId))
                {
                    supplier.ExternalId = await ExternalIdGenerator.GenerateCode(
                        "NCC",
                        prefix => _unitOfWork.SupplierRepository.GetLatestExternalIdStartsWithAsync(prefix)
                    );
                }

                if (supplier.CompanyId == null)
                {
                    throw new ArgumentNullException(nameof(supplier.CompanyId), "CompanyId cannot be null.");
                }

                var supplierEntity = _mapper.Map<Supplier>(supplier);

                await _unitOfWork.SupplierRepository.AddNewSuplier(supplierEntity);

                var affected = await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return affected > 0
                    ? OperationResult.Ok("Supplier added successfully.")
                    : OperationResult.Fail("Failed to add supplier.");
            }

            catch(Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Lỗi khi tạo nhà cung cấp mới: {ex.Message}");
            }

        }

        public async Task<OperationResult> DeleteSupplierByIdAsync(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _unitOfWork.SupplierRepository.DeleteSupplierByIdAsync(id);
                var affected = await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return affected > 0
                    ? OperationResult.Ok("Xóa nhà cung cấp thành công")
                    : OperationResult.Fail("Không tìm thấy nhà cung cấp hoặc xóa không thành công.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Lỗi khi xóa nhà cung cấp: {ex.Message}");

            }
        }

        public async Task<PagedResult<GetSupplierSummary>> GetAllAsync(SupplierQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                // Base Query
                IQueryable<Supplier> result = _unitOfWork.SupplierRepository.Query();

                // Filter
                if (query.SupplierId.HasValue)
                    result = result.Where(x => x.CompanyId == query.SupplierId.Value);

                if (query.From.HasValue)
                    result = result.Where(x => x.CreatedDate >= query.From.Value);

                if (query.To.HasValue)
                    result = result.Where(x => x.CreatedDate <= query.To.Value);

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    // Tìm theo tên/mã NV hoặc tên/mã khách trong batch
                    result = result.Where(x =>
                        EF.Functions.ILike(x.ExternalId, $"%{query.Keyword}%") ||
                        EF.Functions.ILike(x.SupplierName, $"%{query.Keyword}%")    

                    );
                }

                result = result.OrderByDescending(x => x.CreatedDate);


                int total = await result.CountAsync(ct);

                var items = await result
                    .Where(c => c.IsActive == true)
                    .ProjectTo<GetSupplierSummary>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);
                
                return new PagedResult<GetSupplierSummary>(items, total, query.PageNumber, query.PageSize);
            }

            catch(Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhà cung cấp: {ex.Message}");
            }

        }

        public async Task<GetSupplier?> GetSupplierByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _unitOfWork.SupplierRepository.Query()
                .Where(s => s.SupplierId == id)
                .ProjectTo<GetSupplier>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync(ct);
        }

        public async Task<OperationResult> UpdateCustomerAsync(PatchSupplier supplier)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var result = await _unitOfWork.SupplierRepository.UpdateSupplierAsync(supplier);
                var affected = await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return affected > 0
                    ? OperationResult.Ok("Cập nhật nhà cung cấp thành công")
                    : OperationResult.Fail("Cập nhật nhà cung cấp thất bại");
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Lỗi khi cập nhật nhà cung cấp: {ex.Message}");
            }
        }
    }
}

