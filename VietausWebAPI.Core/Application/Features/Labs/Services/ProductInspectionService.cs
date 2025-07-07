using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using QuestPDF.Fluent;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductInspectionFeature;
using VietausWebAPI.Core.Application.Features.Labs.Helpers;
using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductInspectionFeature;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.Labs.Services
{
    public class ProductInspectionService : IProductInspectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductInspectionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task DeleteCOAService(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Delete COA
                await _unitOfWork.ProductInspectionRepository.DeleteCOARepository(id);
                // Save changes
                var affected = await _unitOfWork.SaveChangesAsync();
                // Commit transaction
                await _unitOfWork.CommitTransactionAsync();
                if (affected > 0)
                {
                    return;
                }
                else
                {
                    throw new InvalidOperationException("Failed to delete COA.");
                }
            }
            catch (Exception ex)
            {
                // Rollback transaction on error
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException($"Error deleting COA: {ex.Message}");
            }

        }

        /// <summary>
        /// Tao PDF tổng thể cho COA (Certificate of Analysis) dựa trên thông tin kiểm tra sản phẩm.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<byte[]> GeneralPdfService(Guid id)
        {
            var inspection = await _unitOfWork.ProductInspectionRepository.GetProductInspectionByIdAsync(id);
            if (inspection == null)
            {
                throw new InvalidOperationException($"ProductInspection with ID {id} not found.");
            }

            // Lấy thông tin tổng thể in  COA
            var inspectionMapped = _mapper.Map<PDFResultValue>(inspection);

            if (inspection.ProductStandardId == null)
            {
                if (string.IsNullOrWhiteSpace(inspection.BatchId))
                {
                    throw new InvalidOperationException("BatchId cannot be null or empty when ProductStandardId is null.");
                }

                // Lấy thông tin bao bì từ ProductTestRepository
                var bagtype = await _unitOfWork.ProductTestRepository.GetPagedByIdAsync(inspection.BatchId);

                if (bagtype == null)
                {
                    throw new InvalidOperationException($"No ProductTest found for BatchId {inspection.BatchId}.");
                }

                inspectionMapped.bagType = bagtype.Product_Package;

                var document = new COAPdf(inspectionMapped);
                return document.GeneratePdf();
            }

            // Lấy thông tin tiêu chuẩn sản phẩm từ ProductStandardRepository
            var standard = await _unitOfWork.ProductStandardRepository.GetProductStandardIdAsync(inspection.ProductStandardId.Value);
            if (standard == null)
            {
                var document = new COAPdf(inspectionMapped);
                return document.GeneratePdf();
            }

            var inspectionStandardMapped = _mapper.Map<PDFSpecificationsValue>(standard);
            var fullDocument = new COAPdf(inspectionMapped, inspectionStandardMapped);

            return fullDocument.GeneratePdf();
        }

        public Task<ProductInspection> GetProductInspectionByIdAsync(Guid id)
        {
            var productInspection = _unitOfWork.ProductInspectionRepository.GetProductInspectionByIdAsync(id);
            // Ensure the method never returns null to match the interface contract
            if (productInspection == null)
            {
                throw new InvalidOperationException($"ProductInspection with ID {id} not found.");
            }
            return productInspection;
        }

        public async Task<PagedResult<ProductInspectionSummary>> GetProductInspectionPagedAsync(ProductInspectionQuery? query)
        {
            var pagedResult = await _unitOfWork.ProductInspectionRepository.GetProductInspectionPagedAsync(query);
            var pagedResultMapped = _mapper.Map<PagedResult<ProductInspectionSummary>>(pagedResult);

            return pagedResultMapped;
        }

        public async Task<OperationResult> PostProductInspectionServiceAsync(ProductInspectionInformation productInspection)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Sinh ExternalId nếu chưa có
                if (string.IsNullOrWhiteSpace(productInspection.ExternalId))
                {
                    productInspection.ExternalId = await ExternalIdGenerator.GenerateExternalId(
                        "KSP",
                        prefix => _unitOfWork.ProductInspectionRepository.GetLatestExternalIdStartsWithAsync(prefix)
                        );
                }

                // Map DTO to Entity
                var productInspectionEntity = _mapper.Map<ProductInspection>(productInspection);
                // Add Product Inspection
                await _unitOfWork.ProductInspectionRepository.PostProductInspectionAsync(productInspectionEntity);
                // Save changes
                var affected = await _unitOfWork.SaveChangesAsync();
                // Commit transaction
                await _unitOfWork.CommitTransactionAsync();
                if (affected > 0)
                {
                    return OperationResult.Ok("Tạo thành công");
                }
                else
                {
                    return OperationResult.Fail("Thất bại.");
                }
            }
            catch (Exception ex)
            {
                // Rollback transaction on error
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Lỗi khi tạo {ex.Message}");
            }
        }
    }
}
