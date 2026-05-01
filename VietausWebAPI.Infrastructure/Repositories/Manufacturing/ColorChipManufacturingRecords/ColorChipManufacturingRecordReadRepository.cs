using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.ColorChipManufacturingRecords.GetDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.ColorChipManufacturingRecords.PdfDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts.ColorChipManufacturingRecords;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using VietausWebAPI.Infrastructure.Helpers.Repositories;

namespace VietausWebAPI.Infrastructure.Repositories.Manufacturing.ColorChipManufacturingRecords
{
    public class ColorChipManufacturingRecordReadRepository : Repository<ColorChipManufacturingRecord>, IColorChipManufacturingRecordReadRepository
    {
        public ColorChipManufacturingRecordReadRepository(ApplicationDbContext context) : base(context)
        {
        }
        public Task<ColorChipManufacturingRecord?> GetByIdAsync(
            Guid colorChipMfgRecordId,
            bool track = false,
            CancellationToken ct = default)
        {
            return Query(track)
                .FirstOrDefaultAsync(x => x.ColorChipMfgRecordId == colorChipMfgRecordId, ct);
        }

        public Task<ColorChipManufacturingRecordPdfData?> GetPdfDataByIdAsync(
            Guid colorChipId, 
            CancellationToken ct = default)
        {
            return Query(false)
                 .Where(x => x.ColorChipMfgRecordId == colorChipId
                             && x.IsActive)
                 .Select(x => new ColorChipManufacturingRecordPdfData
                 {
                     ColorChipMfgRecordId = x.ColorChipMfgRecordId,

                     ResinType = x.ResinType,
                     LogoType = x.LogoType,
                     FormStyle = x.FormStyle,

                     RecordDate = x.RecordDate,
                     CreatedDate = x.CreatedDate,

                     Machine = x.Machine,

                     StandardFormula = x.StandardFormula,
                     Resin = x.Resin,
                     TemperatureLimit = x.TemperatureLimit,
                     SizeText = x.SizeText,
                     PelletWeightGram = x.PelletWeightGram,
                     NetWeightGram = x.NetWeightGram,
                     Electrostatic = x.Electrostatic,
                     Note = x.Note,
                     PrintNote = x.PrintNote,
                     DeltaE = x.DeltaE,

                     MfgProductionOrderExternalId = x.MfgProductionOrder != null
                         ? x.MfgProductionOrder.ExternalId
                         : null,

                     CustomerName = x.MfgProductionOrder != null
                         ? x.MfgProductionOrder.CustomerNameSnapshot
                         : null,

                     ProductExternalId = x.MfgProductionOrder != null
                         ? x.MfgProductionOrder.ProductExternalIdSnapshot
                         : null,

                     ProductName = x.MfgProductionOrder != null
                         ? x.MfgProductionOrder.ProductNameSnapshot
                         : null,

                     ColorName = x.MfgProductionOrder != null
                         ? x.MfgProductionOrder.ColorName
                         : null,

                     FormulaExternalIdSnapshot = x.MfgProductionOrder != null
                         ? x.MfgProductionOrder.FormulaExternalIdSnapshot
                         : null,

                     ProductUsageRate = x.MfgProductionOrder != null && x.MfgProductionOrder.Product != null
                         ? x.MfgProductionOrder.Product.UsageRate
                         : null,

                     ManufacturingFormulaExternalId = x.ManufacturingFormula != null
                         ? x.ManufacturingFormula.ExternalId
                         : null,

                     ManufacturingFormulaName = x.ManufacturingFormula != null
                         ? x.ManufacturingFormula.Name
                         : null,

                     PreparedByName = x.ManufacturingFormula != null
                         && x.ManufacturingFormula.CreatedByNavigation != null
                             ? x.ManufacturingFormula.CreatedByNavigation.FullName
                             : null
                 })
                 .FirstOrDefaultAsync(ct);
        }

        public Task<ColorChipManufacturingRecord?> GetActiveByIdForUpdateAsync(
            Guid colorChipId, 
            CancellationToken ct = default)
        {
            return Query(true)
                .FirstOrDefaultAsync(x => x.ColorChipMfgRecordId == colorChipId
                                          && x.IsActive, ct);
        }

        public Task<GetColorChipManufacturingRecord?> GetDetailByIdAsync(
            Guid colorChipId,
            CancellationToken ct = default)
        {
            return Query(false)
                .Where(x => x.ColorChipMfgRecordId == colorChipId
                            && x.IsActive)
                .Select(x => new GetColorChipManufacturingRecord
                {
                    ColorChipMfgRecordId = x.ColorChipMfgRecordId,
                    ResinType = x.ResinType,
                    LogoType = x.LogoType,
                    FormStyle = x.FormStyle,

                    MfgProductionOrderId = x.MfgProductionOrderId,
                    MfgProductionOrderExternalId = x.MfgProductionOrder != null
                        ? x.MfgProductionOrder.ExternalId
                        : null,

                    ManufacturingFormulaId = x.ManufacturingFormulaId,
                    ManufacturingFormulaExternalId = x.ManufacturingFormula != null
                        ? x.ManufacturingFormula.ExternalId
                        : null,
                    ManufacturingFormulaName = x.ManufacturingFormula != null
                        ? x.ManufacturingFormula.Name
                        : null,

                    StandardFormula = x.StandardFormula,
                    Machine = x.Machine,
                    Resin = x.Resin,
                    TemperatureLimit = x.TemperatureLimit,
                    SizeText = x.SizeText,
                    PelletWeightGram = x.PelletWeightGram,
                    NetWeightGram = x.NetWeightGram,
                    Electrostatic = x.Electrostatic,
                    RecordDate = x.RecordDate,
                    Note = x.Note,
                    PrintNote = x.PrintNote,
                    DeltaE = x.DeltaE,

                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    UpdatedDate = x.UpdatedDate,
                    UpdatedBy = x.UpdatedBy,
                    CompanyId = x.CompanyId,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync(ct);
        }

    }
}
