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
        private readonly ICurrentUser _currentUser;

        public ColourChipRecordPrintPDFService(
            IUnitOfWork unitOfWork,
            IColorChipRecordLandscapePdf colorChipRecordPdf,
            IColorChipRecordPortraitPdf colorChipRecordPortraitPdf,
            IColorChipRecordTanPhuPdf colorChipRecordTanPhuPdf,
            ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _colorChipRecordPdf = colorChipRecordPdf;
            _colorChipRecordPortraitPdf = colorChipRecordPortraitPdf;
            _ColorChipRecordTanPhuPdf = colorChipRecordTanPhuPdf;
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

                byte[] pdfBytes = result.FormStyle switch
                {
                    FormStyle.Chips2 => _colorChipRecordPortraitPdf.Render(result.PdfModel),
                    FormStyle.Chips3 => _colorChipRecordPdf.Render(result.PdfModel),
                    FormStyle.ChipsTanPhu => _ColorChipRecordTanPhuPdf.Render(result.PdfModel),
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

        private ColorChipRecordPdfModel MapToPdfModel(
            ColorChipRecord entity,
            dynamic? sampleRequest)
        {
            var firstDevelopmentFormulaCode = entity.DevelopmentFormulas?
                .Where(x => x.IsActive && x.DevelopmentFormula != null)
                .Select(x => x.DevelopmentFormula!.ExternalId)
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

            var preparedByName =
                entity.DevelopmentFormulas?
                    .Where(x => x.IsActive && x.DevelopmentFormula != null)
                    .Select(x => x.DevelopmentFormula!.CreatedByNavigation != null
                        ? x.DevelopmentFormula.CreatedByNavigation.FullName
                        : null)
                    .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))
                ??
                entity.Product?.CreatedByNavigation?.FullName
                ??
                string.Empty;

            return new ColorChipRecordPdfModel
            {
                BatchNo = firstDevelopmentFormulaCode ?? string.Empty,
                Date = entity.RecordDate ?? entity.CreatedDate,

                Customer = sampleRequest?.CustomerName ?? string.Empty,
                Code = entity.Product?.ColourCode ?? string.Empty,
                Color = entity.Product?.Name ?? string.Empty,

                AddRate = entity.Product?.UsageRate != null
                    ? $"{entity.Product.UsageRate}%"
                    : string.Empty,

                Resin = !string.IsNullOrWhiteSpace(entity.Resin)
                    ? entity.Resin!
                    : entity.ResinType.ToString(),

                PreparedBy = preparedByName,
                Signature = string.Empty,

                Machine = entity.Machine,
                TemperatureLimit = entity.TemperatureLimit,
                SizeText = entity.SizeText,
                PelletWeightGram = entity.PelletWeightGram,
                NetWeightGram = entity.NetWeightGram,
                Electrostatic = entity.Electrostatic,
                Note = entity.Note,
                PrintNote = entity.PrintNote,

                RecordTypeText = entity.RecordType.ToString(),
                ResinTypeText = entity.ResinType.ToString(),
                LogoTypeText = entity.LogoType.ToString(),
                FormStyleText = entity.FormStyle.ToString(),
                DeltaE = entity.Product?.DeltaE ?? string.Empty,

                DevelopmentFormulaCodes = entity.DevelopmentFormulas?
                    .Where(x => x.IsActive && x.DevelopmentFormula != null)
                    .Select(x => x.DevelopmentFormula!.ExternalId ?? string.Empty)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .ToList()
                    ?? new()
            };
        }
    }
}