using AutoMapper;
using AutoMapper.QueryableExtensions;
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
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

                req.Sample.ExternalId = await ExternalIdGenerator.GenerateCode(
                    "TP",
                    prefix => _unitOfWork.SampleRequestRepository.GetLatestExternalIdStartsWithAsync(prefix)
                );
                

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
                    //product.CompanyId ??= req.Sample.CompanyId;
                    //product.CreatedBy ??= req.Sample.CreatedBy;

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

        public async Task<OperationResult> DeleteSampleRequestAsync(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var affected = await _unitOfWork.SampleRequestRepository.DeleteSampleRequestAsync(id);

                await _unitOfWork.CommitTransactionAsync();


                return affected > 0
                    ? OperationResult.Ok("Thay đổi thành công")
                    : OperationResult.Fail("Thay đổi thất bại");
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Lỗi khi Thay đổi: {ex.Message}", ex);
            }
        }

        public async Task<PagedResult<SampleRequestSummaryDTO>> GetAllAsync(SampleRequestQuery query, CancellationToken ct = default)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                var result = _unitOfWork.SampleRequestRepository.Query();

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

                var items = await result
                    .Where(c => c.IsActive == true)
                    .OrderByDescending(c => c.ExternalId.Substring(3).PadLeft(10, '0')) // "1" -> "0000000001"
                    .ThenByDescending(c => c.ExternalId.Substring(0, 3))                // nếu cần giữ nhóm theo prefix
                    .ProjectTo<SampleRequestSummaryDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync(ct);
                return new PagedResult<SampleRequestSummaryDTO>(items, totalCount, query.PageNumber, query.PageSize);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách mẫu: {ex.Message}", ex);
            }
        }

        public async Task<GetSampleWithProductRequest> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            try
            {
                var dto = await _unitOfWork.SampleRequestRepository.Query()
                    .AsNoTracking()
                    .Where(c => c.SampleRequestId == id && c.IsActive == true)
                    .ProjectTo<GetSampleWithProductRequest>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(ct);

                if (dto is null) throw new KeyNotFoundException($"SampleRequest {id} not found");
                return dto;
            }

            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy thông tin mẫu: {ex.Message}", ex);
            }
        }

        public async Task<OperationResult> UpdateSampleRequestAsync(UpdateSampleRequest sampleRequest, CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                //var res = _mapper.Map<SampleRequest>(sampleRequest);

                var affected = await _unitOfWork.SampleRequestRepository.UpdateSampleRequestAsync(sampleRequest, ct);

                await _unitOfWork.CommitTransactionAsync();

                await _unitOfWork.SaveChangesAsync();
                return affected > 0
                    ? OperationResult.Ok("Thay đổi thành công")
                    : OperationResult.Fail("Thay đổi thất bại");


            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Lỗi khi Thay đổi: {ex.Message}", ex);
            }

        }
    }
}
