using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.PDFDtos;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature.ColorChipRecordFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Helper.Pdfs.ColorChipRecords;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;
using VietausWebAPI.Core.Domain.Enums.SampleRequests;

namespace VietausWebAPI.Core.Application.Features.Labs.Services.SampleRequestFeature.ColorChipRecordFeatures
{
    public class ColourChipRecordPrintPDFService : IColourChipRecordPrintPDFService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IColorChipRecordLandscapePdf _colorChipRecordPdf;
        private readonly IColorChipRecordPortraitPdf _colorChipRecordPortraitPdf;
        private readonly IColorChipRecordTanPhuPdf _ColorChipRecordTanPhuPdf;
        private readonly IColorChipRecordTanPhuBacNinhPdf _colorChipRecordTanPhuBacNinhPdf;
        private readonly ICurrentUser _currentUser;

        public ColourChipRecordPrintPDFService(
            IUnitOfWork unitOfWork,
            IColorChipRecordLandscapePdf colorChipRecordPdf,
            IColorChipRecordPortraitPdf colorChipRecordPortraitPdf,
            IColorChipRecordTanPhuPdf colorChipRecordTanPhuPdf,
            IColorChipRecordTanPhuBacNinhPdf colorChipRecordTanPhuBacNinhPdf,
            ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _colorChipRecordPdf = colorChipRecordPdf;
            _colorChipRecordPortraitPdf = colorChipRecordPortraitPdf;
            _ColorChipRecordTanPhuPdf = colorChipRecordTanPhuPdf;
            _colorChipRecordTanPhuBacNinhPdf = colorChipRecordTanPhuBacNinhPdf;
            _currentUser = currentUser;
        }

        public async Task<OperationResult<byte[]>> PrintByProductIdAsync(
            Guid productId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (productId == Guid.Empty)
                    return OperationResult<byte[]>.Fail("ProductId không hợp lệ.");

                var sampleRequest = await _unitOfWork.SampleRequestRepository.Query()
                    .Where(x => x.ProductId == productId && x.IsActive)
                    .OrderByDescending(x => x.CreatedDate)
                    .Select(x => new
                    {
                        x.SampleRequestId,
                        x.ProductId,
                        CustomerName = x.Customer != null ? x.Customer.CustomerName : string.Empty,
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                var result = await _unitOfWork.ColorChipRecordReadRepositories
                    .GetPdfDataByProductIdAsync(productId, cancellationToken);

                if (result == null)
                    return OperationResult<byte[]>.Fail("Không tìm thấy ColorChipRecord theo ProductId.");

                if(result.FormStyle == FormStyle.ChipsTanPhu)
                {
                    result.PdfModel.StandardText = string.Empty;
                }

                byte[] pdfBytes = result.FormStyle switch
                {
                    FormStyle.Chips2 => _colorChipRecordPortraitPdf.Render(result.PdfModel),
                    FormStyle.Chips3 => _colorChipRecordPdf.Render(result.PdfModel),
                    FormStyle.ChipsTanPhu => _ColorChipRecordTanPhuPdf.Render(result.PdfModel),
                    FormStyle.ChipsTanPhuBacNinh => _colorChipRecordTanPhuBacNinhPdf.Render(result.PdfModel),
                    FormStyle.Chips2_NonStandard => _colorChipRecordPortraitPdf.Render(result.PdfModel),
                    _ => _colorChipRecordPdf.Render(result.PdfModel)
                };

                if (pdfBytes == null || pdfBytes.Length == 0)
                    return OperationResult<byte[]>.Fail("Không tạo được file PDF.");

                return OperationResult<byte[]>.Ok(pdfBytes);
            }
            catch (Exception ex)
            {
                return OperationResult<byte[]>.Fail($"Lỗi khi in PDF: {ex.Message}");
            }
        }

    }
}
