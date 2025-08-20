using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ProductFeature;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.Labs.Services.SampleRequestFeature
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
