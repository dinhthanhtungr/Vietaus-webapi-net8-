using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductFeatures;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.ProductFeatures;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.Labs.Services.ProductFeatures
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PagedResult<GetProduct>> GetAllAsync(ProductQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 10;

                var result = _unitOfWork.ProductRepository.Query();

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var keyword = query.Keyword.Trim();
                    // Tìm theo tên/mã NV hoặc tên/mã khách trong batch
                    result = result.Where(x =>
                        (x.Name ?? "").Contains(keyword) ||
                        (x.ColourCode ?? "").Contains(keyword) 
                    );
                }

                if (query.CompanyId.HasValue && query.CompanyId.Value != Guid.Empty)
                {
                    result = result.Where(p => p.CompanyId == query.CompanyId.Value);
                }

                if (query.ProductId.HasValue && query.ProductId.Value != Guid.Empty)
                {
                    result = result.Where(p => p.ProductId == query.ProductId.Value);
                }

                int totalCount = await result.CountAsync(ct);

                var items = await result
                    .OrderByDescending(c => c.CreatedDate) // "F1" -> "F0000000001"
                    .ProjectTo<GetProduct>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);

                return new PagedResult<GetProduct>(items, totalCount, query.PageNumber, query.PageSize);
            }

            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách: {ex.Message}", ex);
            }
        }

        public async Task<ProductDTO> CreateAsync(CreateProductRequest req, CancellationToken ct = default)
        {
            await using var tx = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entity = _mapper.Map<Product>(req);

                await _unitOfWork.ProductRepository.AddAsync(entity, ct);
                await _unitOfWork.SaveChangesAsync();

                // (Ví dụ) thêm các bước khác cũng nằm chung transaction ở đây
                // await _uow.AuditLogs.AddAsync(...);
                // await _uow.SaveChangesAsync(ct);

                await tx.CommitAsync(ct);

                return _mapper.Map<ProductDTO>(entity);
            }
            catch (DbUpdateException)
            {
                await tx.RollbackAsync(ct);
                throw; // giữ nguyên stack trace
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }
    }
}
