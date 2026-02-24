using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Supplier;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts.SupplierFeatures;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Materials.SupplierFeatures
{
    public class SupplierReadRepository : ISupplierReadRepository
    {
        private readonly ApplicationDbContext _context;
        public SupplierReadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // ======================================================================== Get ======================================================================== 

        /// <summary>
        /// Lấy chi tiết nhà cung cấp theo Id
        /// Với Phone, SupplierAddresses, SupplierContacts lấy từ SupplierContact IsPrimary
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<GetSupplier?> GetDetailAsync(Guid id, CancellationToken ct)
        {
            var result = _context.Suppliers.AsNoTracking()
                        .Where(e => e.SupplierId == id)
                        .Select(e => new GetSupplier
                        {
                            SupplierId = e.SupplierId,
                            ExternalId = e.ExternalId,
                            SupplierName = e.SupplierName,
                            RegistrationNumber = e.RegistrationNumber,
                            RegistrationAddress = e.RegistrationAddress,
                            IssueDate = e.IssueDate,
                            IssuedPlace = e.IssuedPlace,
                            FaxNumber = e.FaxNumber,
                            TaxNumber = e.TaxNumber,
                            Phone = e.SupplierContacts.Where(c => c.IsPrimary == true).Select(c => c.Phone).FirstOrDefault(),

                            Website = e.Website,
                            UpdatedDate = e.UpdatedDate,
                            UpdatedBy = e.UpdatedBy,
                            IsActive = e.IsActive,

                            SupplierAddresses = e.SupplierAddresses
                                .Where(a => a.IsActive == true)
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
                                .OrderByDescending(a => a.IsPrimary ?? false)
                                .ToList(),

                            SupplierContacts = e.SupplierContacts
                                .Where(c => c.IsActive == true)
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
                                .ToList(),

                        })
                        .FirstOrDefaultAsync(ct);

            return await result;
        }

        /// <summary>
        /// Lấy mã ExternalId lớn nhất bắt đầu với tiền tố cho trước
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<string?> GetLatestExternalIdStartsWithAsync(string prefix, CancellationToken ct)
        {
            return _context.Suppliers
                        .Where(e => e.ExternalId.StartsWith(prefix))
                        .OrderByDescending(e => e.ExternalId.Length)   // dài hơn => số lớn hơn
                        .ThenByDescending(e => e.ExternalId)           // cùng độ dài thì so chuỗi
                        .Select(e => e.ExternalId)
                        .FirstOrDefaultAsync(ct);
        }

        /// <summary>
        /// Lấy danh sách nhà cung cấp có phân trang
        /// với Phone, FirstName, LastName lấy từ SupplierContact IsPrimary
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<PagedResult<GetSupplierSummary>> GetPagedSummaryAsync(SupplierQuery query, CancellationToken ct)
        {
            if(query.PageNumber <= 0) query.PageNumber = 1;
            if(query.PageSize <= 0) query.PageSize = 15;

            var skip = (query.PageNumber - 1) * query.PageSize;

            var q = _context.Suppliers.AsQueryable().Where(e => e.IsActive == true);

            if(query.SupplierId.HasValue)
            {
                q = q.Where(e => e.SupplierId == query.SupplierId.Value);
            }

            if(query.From.HasValue)
            {
                q = q.Where(e => e.CreatedDate >= query.From.Value);
            }

            if(query.To.HasValue)
            {
                q = q.Where(e => e.CreatedDate <= query.To.Value);
            }

            if(!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var keyword = query.Keyword.Trim().ToLower();
                q = q.Where(e => e.SupplierName!.ToLower().Contains(keyword)
                             || e.RegistrationNumber!.ToLower().Contains(keyword)
                             || e.ExternalId!.ToLower().Contains(keyword)
                );
            }

            var total = await q.CountAsync(ct);

            var items = await q
                .OrderByDescending(e => e.CreatedDate)
                .Skip(skip)
                .Take(query.PageSize)
                .Select(e => new GetSupplierSummary
                {
                    SupplierId = e.SupplierId,
                    ExternalId = e.ExternalId,
                    SupplierName = e.SupplierName,
                    RegistrationNumber = e.RegistrationNumber,
                    RegistrationAddress = e.RegistrationAddress,
                    Phone = e.SupplierContacts.Where(c => c.IsPrimary == true).Select(c => c.FirstName).FirstOrDefault(),
                    FirstName = e.SupplierContacts.Where(c => c.IsPrimary == true).Select(c => c.FirstName).FirstOrDefault(),
                    LastName = e.SupplierContacts.Where(c => c.IsPrimary == true).Select(c => c.LastName).FirstOrDefault(),
                })
                .ToListAsync(ct);

            return new PagedResult<GetSupplierSummary>(
                items,
                total,
                query.PageNumber,
                query.PageSize
            );
        }
    }
}
