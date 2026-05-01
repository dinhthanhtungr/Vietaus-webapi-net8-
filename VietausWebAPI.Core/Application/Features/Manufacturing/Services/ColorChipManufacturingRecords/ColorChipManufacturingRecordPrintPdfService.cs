using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.PDFDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.ColorChipManufacturingRecords.PdfDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts.ColorChipManufacturingRecords;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Helper.Pdfs.ColorChipRecords;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Core.Domain.Enums.SampleRequests;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Services.ColorChipManufacturingRecords
{
    public class ColorChipManufacturingRecordPrintPdfService : IColorChipManufacturingRecordPrintPdfService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly IColorChipRecordLandscapePdf _colorChipRecordLandscapePdf; 
        private readonly IColorChipRecordPortraitPdf _colorChipRecordPortraitPdf;
        private readonly IColorChipRecordTanPhuPdf _colorChipRecordTanPhuPdf;
        private readonly IColorChipRecordTanPhuBacNinhPdf _colorChipRecordTanPhuBacNinhPdf;


        public ColorChipManufacturingRecordPrintPdfService(IUnitOfWork unitOfWork, 
            ICurrentUser currentUser, 
            IColorChipRecordLandscapePdf colorChipRecordLandscapePdf, 
            IColorChipRecordPortraitPdf colorChipRecordPortraitPdf, 
            IColorChipRecordTanPhuPdf colorChipRecordTanPhuPdf,
            IColorChipRecordTanPhuBacNinhPdf colorChipRecordTanPhuBacNinhPdf)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _colorChipRecordLandscapePdf = colorChipRecordLandscapePdf;
            _colorChipRecordPortraitPdf = colorChipRecordPortraitPdf;
            _colorChipRecordTanPhuPdf = colorChipRecordTanPhuPdf;
            _colorChipRecordTanPhuBacNinhPdf = colorChipRecordTanPhuBacNinhPdf;
        }

        public async Task<OperationResult<byte[]>> PrintByIdAsync(Guid colorChipMfgRecordId, CancellationToken ct = default)
        {
            if (colorChipMfgRecordId == Guid.Empty)
                return OperationResult<byte[]>.Fail("colorChipMfgRecordId không được để trống.");

            var data = await _unitOfWork.ColorChipManufacturingRecordReadRepository
                .GetPdfDataByIdAsync(colorChipMfgRecordId, ct);

            var userId = _currentUser.EmployeeId;

            var userName = await _unitOfWork.EmployeesRepository.Query()
                .Where(x => x.EmployeeId == userId)
                .Select(x => x.FullName)
                .FirstOrDefaultAsync(ct);


            if (data == null)
                return OperationResult<byte[]>.Fail("Không tìm thấy ColorChipManufacturingRecord.");

            var model = MapToPdfModel(data, userName);

            var pdfBytes = data.FormStyle switch
            {
                FormStyle.Chips2 => _colorChipRecordPortraitPdf.Render(model),
                FormStyle.Chips3 => _colorChipRecordLandscapePdf.Render(model),
                FormStyle.ChipsTanPhu => _colorChipRecordTanPhuPdf.Render(model),
                FormStyle.ChipsTanPhuBacNinh => _colorChipRecordTanPhuBacNinhPdf.Render(model),
                FormStyle.Chips2_NonStandard => _colorChipRecordPortraitPdf.Render(model),
                _ => _colorChipRecordLandscapePdf.Render(model)
            };


            if (pdfBytes == null || pdfBytes.Length == 0)
                return OperationResult<byte[]>.Fail("Không tạo được file PDF.");

            return OperationResult<byte[]>.Ok(pdfBytes);
        }

        private static ColorChipRecordPdfModel MapToPdfModel(ColorChipManufacturingRecordPdfData data, string userName)
        {

            return new ColorChipRecordPdfModel
            {
                BatchNo = data.ManufacturingFormulaExternalId
                    ?? data.MfgProductionOrderExternalId
                    ?? string.Empty,

                Date = data.RecordDate ?? data.CreatedDate,

                Customer = data.CustomerName ?? string.Empty,

                Code = data.ProductExternalId
                    ?? string.Empty,

                Color = data.ProductName
                    ?? string.Empty,

                AddRate = data.ProductUsageRate != null
                    ? $"{data.ProductUsageRate}%"
                    : string.Empty,

                Resin = !string.IsNullOrWhiteSpace(data.Resin)
                    ? data.Resin
                    : data.ResinType.ToString(),

                PreparedBy = userName ?? string.Empty,
                Signature = string.Empty,

                StandardText = data.StandardFormula
                    ?? data.StandardFormula
                    ?? string.Empty,

                Machine = data.Machine,
                TemperatureLimit = data.TemperatureLimit,
                SizeText = data.SizeText,
                PelletWeightGram = data.PelletWeightGram,
                NetWeightGram = data.NetWeightGram,
                Electrostatic = data.Electrostatic,
                Note = data.Note,
                PrintNote = data.PrintNote,
                DeltaE = data.DeltaE,

                RecordTypeText = string.Empty,
                ResinTypeText = data.ResinType.ToString(),
                LogoTypeText = data.LogoType.ToString(),
                FormStyleText = data.FormStyle.ToString(),

                DevelopmentFormulaCodes = string.IsNullOrWhiteSpace(data.ManufacturingFormulaExternalId)
                    ? new List<string>()
                    : new List<string> { data.ManufacturingFormulaExternalId }
            };
        }

    }
}

