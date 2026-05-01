using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.ColorChipManufacturingRecords.GetDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.Queries.ColorChipManufacturingRecords;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts.ColorChipManufacturingRecords;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts.ColorChipManufacturingRecords;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Services.ColorChipManufacturingRecords
{
    public class GetColorChipManufacturingRecordService : IGetColorChipManufacturingRecordService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public GetColorChipManufacturingRecordService(
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<OperationResult<GetColorChipManufacturingRecord>> GetByIdAsync(
            Guid colorChipMfgRecordId,
            CancellationToken ct = default)
        {
            if (colorChipMfgRecordId == Guid.Empty)
                return OperationResult<GetColorChipManufacturingRecord>.Fail("ColorChipMfgRecordId không hợp lệ.");

            var companyId = _currentUser.CompanyId;

            var item = await _unitOfWork.ColorChipManufacturingRecordReadRepository.Query(false)
                .Where(x => x.ColorChipMfgRecordId == colorChipMfgRecordId &&
                            x.CompanyId == companyId)
                .Select(x => new GetColorChipManufacturingRecord
                {
                    ColorChipMfgRecordId = x.ColorChipMfgRecordId,
                    ResinType = x.ResinType,
                    LogoType = x.LogoType,
                    FormStyle = x.FormStyle,
                    MfgProductionOrderId = x.MfgProductionOrderId,
                    MfgProductionOrderExternalId = x.MfgProductionOrder != null ? x.MfgProductionOrder.ExternalId : null,
                    ManufacturingFormulaId = x.ManufacturingFormulaId,
                    ManufacturingFormulaExternalId = x.ManufacturingFormula != null ? x.ManufacturingFormula.ExternalId : null,
                    ManufacturingFormulaName = x.ManufacturingFormula != null ? x.ManufacturingFormula.Name : null,

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
                    DeltaE = x.DeltaE,

                    PrintNote = x.PrintNote,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    UpdatedDate = x.UpdatedDate,
                    UpdatedBy = x.UpdatedBy,
                    CompanyId = x.CompanyId,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync(ct);

            return item == null
                ? OperationResult<GetColorChipManufacturingRecord>.Fail("Không tìm thấy ColorChipManufacturingRecord.")
                : OperationResult<GetColorChipManufacturingRecord>.Ok(item);
        }

        public async Task<OperationResult<PagedResult<GetColorChipManufacturingRecordSummary>>> GetPagedAsync(
            ColorChipManufacturingRecordQuery query,
            CancellationToken ct = default)
        {
            query ??= new ColorChipManufacturingRecordQuery();
            if (query.PageNumber <= 0) query.PageNumber = 1;
            if (query.PageSize <= 0) query.PageSize = 15;

            var companyId = query.CompanyId ?? _currentUser.CompanyId;

            var db = _unitOfWork.ColorChipManufacturingRecordReadRepository.Query(false)
                .Where(x => x.CompanyId == companyId);

            if (query.ColorChipMfgRecordId.HasValue)
                db = db.Where(x => x.ColorChipMfgRecordId == query.ColorChipMfgRecordId.Value);

            if (query.MfgProductionOrderId.HasValue)
                db = db.Where(x => x.MfgProductionOrderId == query.MfgProductionOrderId.Value);

            if (query.ManufacturingFormulaId.HasValue)
                db = db.Where(x => x.ManufacturingFormulaId == query.ManufacturingFormulaId.Value);

            if (query.ResinType.HasValue)
                db = db.Where(x => x.ResinType == query.ResinType.Value);

            if (query.LogoType.HasValue)
                db = db.Where(x => x.LogoType == query.LogoType.Value);

            if (query.FormStyle.HasValue)
                db = db.Where(x => x.FormStyle == query.FormStyle.Value);

            if (query.From.HasValue)
                db = db.Where(x => x.RecordDate >= query.From.Value);

            if (query.To.HasValue)
                db = db.Where(x => x.RecordDate <= query.To.Value);

            if (query.IsActive.HasValue)
                db = db.Where(x => x.IsActive == query.IsActive.Value);

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var keyword = query.Keyword.Trim();
                db = db.Where(x =>
                    (x.Machine != null && EF.Functions.ILike(x.Machine, $"%{keyword}%")) ||
                    (x.Resin != null && EF.Functions.ILike(x.Resin, $"%{keyword}%")) ||
                    (x.Note != null && EF.Functions.ILike(x.Note, $"%{keyword}%")) ||
                    (x.MfgProductionOrder != null && x.MfgProductionOrder.ExternalId != null &&
                        EF.Functions.ILike(x.MfgProductionOrder.ExternalId, $"%{keyword}%")) ||
                    (x.ManufacturingFormula != null && x.ManufacturingFormula.ExternalId != null &&
                        EF.Functions.ILike(x.ManufacturingFormula.ExternalId, $"%{keyword}%")));
            }

            var total = await db.CountAsync(ct);
            var skip = (query.PageNumber - 1) * query.PageSize;

            var items = await db
                .OrderByDescending(x => x.RecordDate ?? x.CreatedDate)
                .ThenByDescending(x => x.ColorChipMfgRecordId)
                .Skip(skip)
                .Take(query.PageSize)
                .Select(x => new GetColorChipManufacturingRecordSummary
                {
                    ColorChipMfgRecordId = x.ColorChipMfgRecordId,
                    MfgProductionOrderId = x.MfgProductionOrderId,
                    MfgProductionOrderExternalId = x.MfgProductionOrder != null ? x.MfgProductionOrder.ExternalId : null,
                    ManufacturingFormulaId = x.ManufacturingFormulaId,
                    ManufacturingFormulaExternalId = x.ManufacturingFormula != null ? x.ManufacturingFormula.ExternalId : null,
                    ResinType = x.ResinType,
                    LogoType = x.LogoType,
                    FormStyle = x.FormStyle,
                    RecordDate = x.RecordDate,
                    Machine = x.Machine,
                    IsActive = x.IsActive
                })
                .ToListAsync(ct);

            var result = new PagedResult<GetColorChipManufacturingRecordSummary>(
                items,
                total,
                query.PageNumber,
                query.PageSize);

            return OperationResult<PagedResult<GetColorChipManufacturingRecordSummary>>.Ok(result);
        }

    }
}
