using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest;
using VietausWebAPI.Core.Application.Features.Labs.Queries.CreateSampleRequest;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
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

        public async Task<OperationResult> CreateAsync(CreateSampleWithProductRequest req, CancellationToken ct = default)
        {
            if(req.Sample.CompanyId == Guid.Empty) throw new ArgumentException("CompanyId cannot be empty", nameof(req.Sample.CompanyId));
            if(req.Sample.CreatedBy == Guid.Empty) throw new ArgumentException("CreatedBy  cannot be empty", nameof(req.Sample.CreatedBy));


            int affected = 0;
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
                    affected = await _unitOfWork.SaveChangesAsync();
                    productId = product.ProductId;
                }

                // Tạo mới mẫu
                var sample = _mapper.Map<SampleRequest>(req.Sample);
                sample.ProductId = productId;

                await _unitOfWork.SampleRequestRepository.AddAsync(sample, ct);
                affected = await _unitOfWork.SaveChangesAsync();

                await tx.CommitAsync(ct);

                return affected > 0
                    ? OperationResult.Ok("Tạo thành công")
                    : OperationResult.Fail("Thất bại.");


            }

            catch (Exception ex)
            {
                await tx.RollbackAsync(ct);
                return OperationResult.Fail($"Lỗi khi tạo {ex.Message}");
            }

        }

        public async Task<PagedResult<SampleRequestSummaryDTO>> GetAllAsync(SampleRequestQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var result = _unitOfWork.SampleRequestRepository.Query().AsNoTracking();

                if (!string.IsNullOrWhiteSpace(query.Keyword))
                {
                    var keyword = query.Keyword.Trim();

                    // Tìm theo tên/mã NV hoặc tên/mã khách trong batch
                    result = result.Where(x =>
                        (x.ExternalId ?? "").Contains(keyword) ||
                        (x.CreatedByNavigation.ExternalId ?? "").Contains(keyword) ||
                        (x.Product.ColourCode ?? "").Contains(keyword) ||
                        (x.Customer.ExternalId ?? "").Contains(keyword) ||
                        (x.Customer.CustomerName ?? "").Contains(keyword) 
                    );
                }

                if (query.CompanyId.HasValue && query.CompanyId.Value != Guid.Empty)
                {
                    result = result.Where(p => p.CompanyId == query.CompanyId.Value);
                }

                int totalCount = await result.CountAsync(ct);

                List<SampleRequestSummaryDTO> items = await result
                    .OrderBy(c => c.ExternalId)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(c => new SampleRequestSummaryDTO
                    {
                        SampleRequestId = c.SampleRequestId,
                        ExternalId = c.ExternalId,
                        ProductId = c.ProductId,
                        ColourCode = c.Product.ColourCode,
                        Status = c.Status,
                        CustomerName = c.Customer.CustomerName,
                        LabName = c.Product.CreatedByNavigation != null ? c.Product.CreatedByNavigation.FullName : null,
                        CreatedBy = c.CreatedByNavigation != null ? c.CreatedByNavigation.FullName : null,
                        CreatedDate = c.CreatedDate,
                        ExpectedDeliveryDate = c.ExpectedDeliveryDate,
                        RequestDeliveryDate = c.RequestDeliveryDate,
                        RealDeliveryDate = c.RealDeliveryDate,
                        RealPriceQuoteDate = c.RealPriceQuoteDate,
                        ExpectedPriceQuoteDate = c.ExpectedPriceQuoteDate
                    }).ToListAsync(ct);

                return new PagedResult<SampleRequestSummaryDTO>(items, totalCount, query.PageNumber, query.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách mẫu: {ex.Message}", ex);
            }
        }
    }
}
