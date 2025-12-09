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
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public SupplierService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
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
                var supplierEntity = _mapper.Map<Supplier>(supplier);

                supplierEntity.CreatedBy = _currentUser.EmployeeId;
                supplierEntity.CreatedDate = DateTime.Now;
                supplierEntity.CompanyId = _currentUser.CompanyId;  

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

        public async Task<OperationResult<PagedResult<GetSupplierSummary>>> GetAllAsync(
            SupplierQuery query, CancellationToken ct = default)
        {
            try
            {
                query ??= new SupplierQuery();
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var skip = (query.PageNumber - 1) * query.PageSize;

                // Base query + AsNoTracking để nhẹ
                var baseQ = _unitOfWork.SupplierRepository.Query()
                    .AsNoTracking()
                    .Where(s => s.IsActive == true);

                // ---- Filters ----
                // Nếu query.SupplierId là filter theo SupplierId:
                if (query.SupplierId.HasValue)
                    baseQ = baseQ.Where(s => s.SupplierId == query.SupplierId.Value);

                if (query.From.HasValue)
                    baseQ = baseQ.Where(s => s.CreatedDate >= query.From.Value);

                if (query.To.HasValue)
                    baseQ = baseQ.Where(s => s.CreatedDate <= query.To.Value);

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var kw = query.Keyword.Trim();
                    // ILike dành cho PostgreSQL (Npgsql)
                    baseQ = baseQ.Where(s =>
                        EF.Functions.ILike(s.ExternalId ?? string.Empty, $"%{kw}%") ||
                        EF.Functions.ILike(s.SupplierName ?? string.Empty, $"%{kw}%") ||
                        EF.Functions.ILike(s.RegistrationNumber ?? string.Empty, $"%{kw}%")
                    );
                }

                // Count TRÊN CÙNG TẬP baseQ (đã lọc IsActive + các filter)
                var total = await baseQ.CountAsync(ct);

                // Page: OrderBy -> Skip/Take -> Select -> ToListAsync
                var items = await baseQ
                    .OrderByDescending(s => s.CreatedDate)   // hoặc coalesce: .OrderByDescending(s => s.CreatedDate ?? DateTime.MinValue)
                    .Skip(skip)
                    .Take(query.PageSize)
                    .Select(s => new GetSupplierSummary
                    {
                        SupplierId = s.SupplierId,
                        ExternalId = s.ExternalId,
                        SupplierName = s.SupplierName,
                        RegistrationNumber = s.RegistrationNumber,

                        // Ưu tiên số phone từ contact primary, fallback Supplier.Phone
                        Phone = s.SupplierContacts
                                    .Where(c => c.IsPrimary == true)
                                    .Select(c => c.Phone)
                                    .FirstOrDefault()
                                ?? s.Phone,

                        // Lấy tên contact primary (nếu có)
                        FirstName = s.SupplierContacts
                                    .Where(c => c.IsPrimary == true)
                                    .Select(c => c.FirstName)
                                    .FirstOrDefault(),

                        LastName = s.SupplierContacts
                                    .Where(c => c.IsPrimary == true)
                                    .Select(c => c.LastName)
                                    .FirstOrDefault()
                    })
                    .ToListAsync(ct);

                var paged = new PagedResult<GetSupplierSummary>(items, total, query.PageNumber, query.PageSize);
                return OperationResult<PagedResult<GetSupplierSummary>>.Ok(paged);
            }
            catch (Exception ex)
            {
                return OperationResult<PagedResult<GetSupplierSummary>>.Fail(
                    $"Lỗi khi lấy danh sách nhà cung cấp: {ex.Message}");
            }
        }


        public async Task<OperationResult<GetSupplier>> GetSupplierByIdAsync(Guid id, CancellationToken ct = default)
        {
            try
            {
                var dto = await _unitOfWork.SupplierRepository.Query()
                    .AsNoTracking()
                    .Where(s => s.SupplierId == id)
                    .Select(s => new GetSupplier
                    {
                        SupplierId = s.SupplierId,
                        ExternalId = s.ExternalId,
                        SupplierName = s.SupplierName,
                        RegistrationNumber = s.RegistrationNumber,
                        IssueDate = s.IssueDate,
                        IssuedPlace = s.IssuedPlace,
                        FaxNumber = s.FaxNumber,

                        // Nếu bạn muốn show “product” nhà cung cấp cung ứng,
                        // có thể ghép từ bảng liên kết MaterialsSuppliers (nếu có Material.Name):
                        // Product = string.Join(", ",
                        //     s.MaterialsSuppliers
                        //      .Where(ms => ms.Material != null && ms.Material.Name != null)
                        //      .Select(ms => ms.Material.Name!)
                        //      .Distinct()),

                        TaxNumber = s.TaxNumber,

                        // Ưu tiên số phone từ contact primary, fallback Supplier.Phone
                        Phone = s.SupplierContacts
                                    .Where(c => c.IsPrimary == true && c.Phone != null)
                                    .Select(c => c.Phone!)
                                    .FirstOrDefault() ?? s.Phone,

                        Website = s.Website,
                        UpdatedDate = s.UpdatedDate,
                        UpdatedBy = s.UpdatedBy,
                        CompanyName = s.Company != null ? s.Company.Name : null,
                        IsActive = s.IsActive,

                        SupplierAddresses = s.SupplierAddresses
                            .Select(a => new GetSupplierAddress
                            {
                                AddressId = a.AddressId,
                                AddressLine = a.AddressLine,
                                City = a.City,
                                District = a.District,
                                Province = a.Province,
                                Country = a.Country,
                                PostalCode = a.PostalCode,
                                IsPrimary = a.IsPrimary
                            })
                            // đưa primary lên đầu cho dễ hiển thị
                            .OrderByDescending(a => a.IsPrimary ?? false)
                            .ToList(),

                        SupplierContacts = s.SupplierContacts
                            .Select(c => new GetSupplierContact
                            {
                                ContactId = c.ContactId,
                                FirstName = c.FirstName,
                                LastName = c.LastName,
                                Gender = c.Gender,
                                Phone = c.Phone,
                                Email = c.Email,
                                IsPrimary = c.IsPrimary
                            })
                            .OrderByDescending(c => c.IsPrimary ?? false)
                            .ToList()
                    })
                    .FirstOrDefaultAsync(ct);

                if (dto == null)
                    return OperationResult<GetSupplier>.Fail("Supplier không tồn tại.");

                return OperationResult<GetSupplier>.Ok(dto);
            }
            catch (Exception ex)
            {
                return OperationResult<GetSupplier>.Fail($"Lỗi khi lấy Supplier: {ex.Message}");
            }
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

