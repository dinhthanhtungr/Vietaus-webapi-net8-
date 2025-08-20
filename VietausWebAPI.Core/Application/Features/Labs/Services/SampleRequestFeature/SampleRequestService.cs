using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.Labs.Services.SampleRequestFeature
{
    public class SampleRequestService : ISampleRequestService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SampleRequestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SampleRequestDTO> CreateAsync(CreateSampleWithProductRequest req, CancellationToken ct = default)
        {
            if(req.Sample.CompanyId == Guid.Empty) throw new ArgumentException("CompanyId cannot be empty", nameof(req.Sample.CompanyId));
            if(req.Sample.CreatedBy == Guid.Empty) throw new ArgumentException("CreatedBy  cannot be empty", nameof(req.Sample.CreatedBy));

            await using var tx = await _unitOfWork.BeginTransactionAsync();

            try
            {
                Guid productId;

                // Case1:  Tạo mới sản phẩm
                if (req.ProductId.HasValue)
                {
                    var exists = await _unitOfWork.ProductRepository.Query()
                        .AnyAsync(p => p.ProductId == req.ProductId.Value, ct);

                    if (!exists) throw new ArgumentException("Product does not exist", nameof(req.ProductId));
                    productId = req.ProductId.Value;
                }
                // Case2: Tạo mới mẫu
                else
                {
                    var product = _mapper.Map<Product>(req.Product);

                    // (tuỳ chọn) nếu muốn đồng bộ tenant/audit với Sample:
                    product.CompanyId ??= req.Sample.CompanyId;
                    product.CreatedBy ??= req.Sample.CreatedBy;

                    await _unitOfWork.ProductRepository.AddAsync(product, ct);
                    await _unitOfWork.SaveChangesAsync();
                    productId = product.ProductId;
                }

                // Tạo mới mẫu
                var sample = _mapper.Map<SampleRequest>(req.Sample);
                sample.ProductId = productId;

                await _unitOfWork.SampleRequestRepository.AddAsync(sample, ct);
                await _unitOfWork.SaveChangesAsync();

                await tx.CommitAsync(ct);

                return _mapper.Map<SampleRequestDTO>(sample);

            }

            catch
            {
                await tx.RollbackAsync(ct);
                throw; // giữ nguyên stack trace
            }

        }
    }
}
