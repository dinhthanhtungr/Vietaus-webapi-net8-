using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.GetDtos;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.PDFDtos;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature.ColorChipRecordFeatures;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

namespace VietausWebAPI.Infrastructure.Repositories.Share.SampleRequestFeature.ColorChipRecordFeatures
{
    public class ColourChipRecordReadRepositories : IColorChipRecordReadRepositories
    {
        private readonly ApplicationDbContext _context;

        public ColourChipRecordReadRepositories(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ColorChipRecord?> GetByIdAsync(
          Guid colorChipRecordId,
          CancellationToken cancellationToken = default)
        {
            return await _context.ColorChipRecords
                .AsNoTracking()
                .Include(x => x.AttachmentCollection)
                .Include(x => x.Product)
                .Include(x => x.DevelopmentFormulas.Where(df => df.IsActive))
                    .ThenInclude(df => df.DevelopmentFormula)
                .FirstOrDefaultAsync(
                    x => x.ColorChipRecordId == colorChipRecordId && x.IsActive,
                    cancellationToken);
        }

        public async Task<bool> CheckExisting(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _context.ColorChipRecords
                .AsNoTracking()
                .AnyAsync(c => c.ProductId == productId, cancellationToken);    
        }

        public async Task<ColorChipRecord?> GetByProductIdAsync(
            Guid productId,
            CancellationToken cancellationToken = default)
        {
            return await _context.ColorChipRecords
                .Include(x => x.DevelopmentFormulas)
                    .ThenInclude(x => x.DevelopmentFormula)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.IsActive, cancellationToken);
        }


        public async Task<ColorChipRecordPdfQueryResult?> GetPdfDataByProductIdAsync(
      Guid productId,
      CancellationToken cancellationToken = default)
        {
            var sampleRequest = await _context.SampleRequests
                .AsNoTracking()
                .Where(x => x.ProductId == productId && x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new
                {
                    CustomerName = x.Customer != null ? x.Customer.CustomerName : string.Empty
                })
                .FirstOrDefaultAsync(cancellationToken);

            var data = await _context.ColorChipRecords
                .AsNoTracking()
                .Where(x => x.ProductId == productId && x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new
                {
                    x.ColorChipRecordId,
                    x.FormStyle,
                    x.RecordDate,
                    x.CreatedDate,
                    x.Resin,
                    x.ResinType,
                    x.RecordType,
                    x.LogoType,

                    x.Machine,
                    x.TemperatureLimit,
                    x.SizeText,
                    x.PelletWeightGram,
                    x.NetWeightGram,
                    x.Electrostatic,
                    x.Note,
                    x.PrintNote,

                    ProductColourCode = x.Product != null ? x.Product.ColourCode : null,
                    ProductName = x.Product != null ? x.Product.Name : null,
                    ProductUsageRate = x.Product != null ? x.Product.UsageRate : null,
                    ProductDeltaE = x.Product != null ? x.Product.DeltaE : null,
                    ProductCreatedByName = x.Product != null && x.Product.CreatedByNavigation != null
                        ? x.Product.CreatedByNavigation.FullName
                        : null,

                    FirstDevelopmentFormulaCode = x.DevelopmentFormulas
                        .Where(df => df.IsActive && df.DevelopmentFormula != null)
                        .Select(df => df.DevelopmentFormula!.ExternalId)
                        .FirstOrDefault(),

                    FirstPreparedByName = x.DevelopmentFormulas
                        .Where(df => df.IsActive && df.DevelopmentFormula != null)
                        .Select(df => df.DevelopmentFormula!.CreatedByNavigation != null
                            ? df.DevelopmentFormula.CreatedByNavigation.FullName
                            : null)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (data == null)
                return null;

            var developmentFormulaCodes = await _context.ColorChipRecordDevelopmentFormulas
                .AsNoTracking()
                .Where(df => df.ColorChipRecordId == data.ColorChipRecordId
                             && df.IsActive
                             && df.DevelopmentFormula != null)
                .Select(df => df.DevelopmentFormula!.ExternalId ?? string.Empty)
                .Where(code => !string.IsNullOrWhiteSpace(code))
                .Distinct()
                .ToListAsync(cancellationToken);

            return new ColorChipRecordPdfQueryResult
            {
                FormStyle = data.FormStyle,
                PdfModel = new ColorChipRecordPdfModel
                {
                    BatchNo = data.FirstDevelopmentFormulaCode ?? string.Empty,
                    Date = data.RecordDate ?? data.CreatedDate,

                    Customer = sampleRequest?.CustomerName ?? string.Empty,
                    Code = data.ProductColourCode ?? string.Empty,
                    Color = data.ProductName ?? string.Empty,

                    AddRate = data.ProductUsageRate != null
                        ? $"{data.ProductUsageRate}%"
                        : string.Empty,

                    Resin = !string.IsNullOrWhiteSpace(data.Resin)
                        ? data.Resin!
                        : data.ResinType.ToString(),

                    PreparedBy = !string.IsNullOrWhiteSpace(data.FirstPreparedByName)
                        ? data.FirstPreparedByName!
                        : data.ProductCreatedByName ?? string.Empty,

                    Signature = string.Empty,

                    Machine = data.Machine,
                    TemperatureLimit = data.TemperatureLimit,
                    SizeText = data.SizeText,
                    PelletWeightGram = data.PelletWeightGram,
                    NetWeightGram = data.NetWeightGram,
                    Electrostatic = data.Electrostatic,
                    Note = data.Note,
                    PrintNote = data.PrintNote,

                    RecordTypeText = data.RecordType.ToString(),
                    ResinTypeText = data.ResinType.ToString(),
                    LogoTypeText = data.LogoType.ToString(),
                    FormStyleText = data.FormStyle.ToString(),
                    DeltaE = data.ProductDeltaE ?? string.Empty,

                    DevelopmentFormulaCodes = developmentFormulaCodes
                }
            };
        }
    }
}
