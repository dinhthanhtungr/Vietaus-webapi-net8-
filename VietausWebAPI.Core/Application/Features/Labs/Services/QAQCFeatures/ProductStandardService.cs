//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VietausWebAPI.Core.Application.Shared.Models.PageModels;
//using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
//using AutoMapper;
//using VietausWebAPI.Core.Domain.Entities;
//using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductStandardFeature;
//using VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.ProductStandardFeature;
//using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.QAQCFeatures;
//using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;

//namespace VietausWebAPI.Core.Application.Features.Labs.Services.QAQCFeatures
//{
//    public class ProductStandardService : IProductStandardService
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;
//        public ProductStandardService(IUnitOfWork unitOfWork, IMapper mapper)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//        }

//        public async Task<OperationResult> DeleteProductStandardService(Guid id)
//        {
//            await _unitOfWork.BeginTransactionAsync();

//            try
//            {
//                // Xóa ProductStandard
//                await _unitOfWork.ProductStandardRepository.DeleteProductStandard(id);

//                // Lưu thay đổi
//                var affected = await _unitOfWork.SaveChangesAsync();

//                // Commit giao dịch
//                await _unitOfWork.CommitTransactionAsync();

//                if (affected > 0)
//                {
//                    return OperationResult.Ok("Xóa tiêu chuẩn sản phẩm thành công.");
//                }
//                else
//                {
//                    return OperationResult.Fail("Không tìm thấy tiêu chuẩn sản phẩm để xóa hoặc không có thay đổi nào được thực hiện.");
//                }
//            }
//            catch (Exception ex)
//            {
//                // Rollback nếu có lỗi
//                await _unitOfWork.RollbackTransactionAsync();
//                return OperationResult.Fail($"Lỗi khi xóa tiêu chuẩn sản phẩm: {ex.Message}");
//            }
//        }

//        public async Task<ProductStandardInformation> GetProductStandardIdAsync(Guid id)
//        {
//            var productStandardRepository = await _unitOfWork.ProductStandardRepository.GetProductStandardIdAsync(id);
//            var productStandard = _mapper.Map<ProductStandardInformation>(productStandardRepository);

//            return productStandard;
//        }

//        public async Task<ProductStandardInformation> GetProductStandardProductIdAsync(Guid id)
//        {
//            var productStandardRepository = await _unitOfWork.ProductStandardRepository.GetProductStandardProductIdAsync(id);
//            var productStandard = _mapper.Map<ProductStandardInformation>(productStandardRepository);

//            return productStandard;
//        }

//        public async Task<PagedResult<ProductStandardSummaryDTO>> GetProductStandardsPagedAsync(ProductStandardQuery? query)
//        {
//            var productStandardRepository = await _unitOfWork.ProductStandardRepository.GetProductStandardsPagedAsync(query);
//            var pagedResult = _mapper.Map<PagedResult<ProductStandardSummaryDTO>>(productStandardRepository);

//            return pagedResult;

//        }

//        public async Task<OperationResult> PostProductStandardService(ProductStandardInformation productStandard)
//        {
//            await _unitOfWork.BeginTransactionAsync();

//            try
//            {
//                // Sinh ExternalId nếu chưa có
//                if (string.IsNullOrWhiteSpace(productStandard.ExternalId))
//                {
//                    productStandard.ExternalId = await ExternalIdGenerator.GenerateExternalId(
//                        "STD",
//                        prefix => _unitOfWork.ProductStandardRepository.GetLatestExternalIdStartsWithAsync(prefix)
//                        );
//                }

//                // Map và lưu vào DB
//                var result = _mapper.Map<ProductStandard>(productStandard);
//                await _unitOfWork.ProductStandardRepository.PostProductStandard(result);
//                // Lưu thay đổi
//                var affected = await _unitOfWork.SaveChangesAsync();

//                // Commit giao dịch
//                await _unitOfWork.CommitTransactionAsync();

//                if (affected > 0)
//                {
//                    return OperationResult.Ok("Tạo thành công.");
//                }
//                else
//                {
//                    return OperationResult.Fail("Không tạo được.");
//                }
//            }
//            catch (Exception ex)
//            {
//                // Rollback nếu có lỗi
//                await _unitOfWork.RollbackTransactionAsync();
//                return OperationResult.Fail($"Lỗi khi tạo tiêu chuẩn sản phẩm: {ex.Message}");
//            }
//        }

//        public async Task<OperationResult> UpdateProductStandardService(Guid id, ProductStandardInformation productStandard)
//        {
//            await _unitOfWork.BeginTransactionAsync();
//            try
//            {
//                var result = _mapper.Map<ProductStandard>(productStandard);
//                await _unitOfWork.ProductStandardRepository.UpdateProductStandard(id, productStandard);

//                var affected = await _unitOfWork.SaveChangesAsync();

//                // Commit giao dịch
//                await _unitOfWork.CommitTransactionAsync();

//                if (affected > 0)
//                {
//                    return OperationResult.Ok("Thành công.");
//                }
//                else
//                {
//                    return OperationResult.Fail("Thất bại.");
//                }

//            }

//            catch (Exception ex)
//            {
//                // Rollback nếu có lỗi
//                await _unitOfWork.RollbackTransactionAsync();
//                return OperationResult.Fail($"Lỗi khi update tiêu chuẩn sản phẩm: {ex.Message}");
//            }
//        }
//    }
//}
