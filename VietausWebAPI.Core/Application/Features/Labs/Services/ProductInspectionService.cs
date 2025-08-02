using AutoMapper;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ProductInspectionService(IUnitOfWork unitOfWork, IMapper mapper )
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
        public async Task<byte[]> GeneralQCPdfService(StatisticalReportQuery query)
        {
            var datum = await _unitOfWork.ProductInspectionRepository.GetProductInspectionListAsync(query);

            var htmlContent = GenerateQCHtmlFromData.Generate(datum);

            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe"
            });

            using var page = await browser.NewPageAsync();
            await page.SetContentAsync(htmlContent, new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Load } });

            var pdfBytes = await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                MarginOptions = new MarginOptions { Top = "10mm", Bottom = "10mm", Left = "10mm", Right = "10mm" }
            });

            await browser.CloseAsync();

            return pdfBytes;
        }




        /// <summary>
        /// Taoj pdf tổng thể cho QC (Quality Control) dựa trên thông tin kiểm tra sản phẩm.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        //public async Task<byte[]> GeneralQCPdfService(StatisticalReportQuery query)
        //{

        //}

        public async Task<ProductInspectionInformation> GetProductInspectionByIdAsync(Guid id)
        {
            var productInspection = await _unitOfWork.ProductInspectionRepository.GetProductInspectionByIdAsync(id);
            var result = _mapper.Map<ProductInspectionInformation>(productInspection);
            // Ensure the method never returns null to match the interface contract
            if (productInspection == null)
            {
                throw new InvalidOperationException($"ProductInspection with ID {id} not found.");
            }
            return result;
        }

        public async Task<PagedResult<ProductInspectionSummary>> GetProductInspectionPagedAsync(ProductInspectionQuery? query)
        {
            var pagedResult = await _unitOfWork.ProductInspectionRepository.GetProductInspectionPagedAsync(query);
            var pagedResultMapped = _mapper.Map<PagedResult<ProductInspectionSummary>>(pagedResult);

            return pagedResultMapped;
        }

        public async Task<OperationResult> PostProductInspectionServiceAsync(PostProductInspectionRequest productInspection)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Sinh ExternalId nếu chưa có
                if (string.IsNullOrWhiteSpace(productInspection.ProductInspection.ExternalId))
                {
                    productInspection.ProductInspection.ExternalId = await ExternalIdGenerator.GenerateExternalId(
                        "KSP",
                        prefix => _unitOfWork.ProductInspectionRepository.GetLatestExternalIdStartsWithAsync(prefix)
                    );
                }

                // Gán Types nếu là QCOUTPUT -> dạng QCOUT_x
                if (productInspection.ProductInspection.Types == "QCOUT_")
                {
                    var count = await _unitOfWork.ProductInspectionRepository
                        .CountAsync(x => x.BatchId == productInspection.ProductInspection.BatchId
                                      && x.Types.StartsWith("QCOUT_"));

                    productInspection.ProductInspection.Types = $"QCOUT_{count + 1}";
                }

                // Map và lưu ProductInspection
                //productInspection.QCDetail = null; // Xoá navigation nếu có
                var productInspectionEntity = _mapper.Map<ProductInspection>(productInspection.ProductInspection);
                await _unitOfWork.ProductInspectionRepository.PostProductInspectionAsync(productInspectionEntity);

                // Nếu có QCDetail -> map và lưu
                if (productInspection.QCDetail != null)
                {
                    var map = _mapper.Map<QCDetail>(productInspection.QCDetail);
                    map.BatchId = productInspectionEntity.Id;
                    await _unitOfWork.IQCDetailRepository.AddQCDetail(map);
                }

                var affected = await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return affected > 0
                    ? OperationResult.Ok("Tạo thành công")
                    : OperationResult.Fail("Thất bại.");
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
