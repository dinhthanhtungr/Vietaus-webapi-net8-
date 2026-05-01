using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.ColorChipManufacturingRecords.GetDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.ColorChipManufacturingRecords.PostDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts.ColorChipManufacturingRecords;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts.ColorChipManufacturingRecords;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Services.ColorChipManufacturingRecords
{
    public class PostColorChipManufacturingRecordService : IPostColorChipManufacturingRecordService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IColorChipManufacturingRecordWriteRepository _writeRepository;
        private readonly ICurrentUser _currentUser;

        public PostColorChipManufacturingRecordService(
            IUnitOfWork unitOfWork,
            IColorChipManufacturingRecordWriteRepository writeRepository,
            ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _writeRepository = writeRepository;
            _currentUser = currentUser;
        }

        public async Task<OperationResult<GetColorChipManufacturingRecord>> CreateAsync(
            PostColorChipManufacturingRecordRequest request,
            CancellationToken ct = default)
        {
            if (request == null)
                return OperationResult<GetColorChipManufacturingRecord>.Fail("Request không được để trống.");

            var now = DateTime.Now;
            var userId = _currentUser.EmployeeId;
            var companyId = _currentUser.CompanyId;

            await _unitOfWork.BeginTransactionAsync(ct);

            try
            {
                var entity = new ColorChipManufacturingRecord
                {
                    ColorChipMfgRecordId = Guid.CreateVersion7(),
                    ResinType = request.ResinType,
                    LogoType = request.LogoType,
                    FormStyle = request.FormStyle,
                    MfgProductionOrderId = request.MfgProductionOrderId,
                    ManufacturingFormulaId = request.ManufacturingFormulaId,
                    
                    StandardFormula = request.StandardFormula,
                    Machine = request.Machine,
                    Resin = request.Resin,
                    TemperatureLimit = request.TemperatureLimit ?? string.Empty,
                    SizeText = request.SizeText,
                    DeltaE = request.DeltaE,

                    PelletWeightGram = request.PelletWeightGram,
                    NetWeightGram = request.NetWeightGram,
                    Electrostatic = request.Electrostatic,
                    RecordDate = request.RecordDate ?? now,
                    Note = request.Note,
                    PrintNote = request.PrintNote,
                    CreatedDate = now,
                    CreatedBy = userId,
                    UpdatedDate = null,
                    UpdatedBy = null,
                    CompanyId = companyId,
                    IsActive = true
                };

                await _writeRepository.CreateAsync(entity, ct);
                await _unitOfWork.SaveChangesAsync(ct);
                await _unitOfWork.CommitTransactionAsync(ct);

                var data = await _writeRepository.Query()
                    .Where(x => x.ColorChipMfgRecordId == entity.ColorChipMfgRecordId)
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
                        DeltaE = x.DeltaE,

                        PelletWeightGram = x.PelletWeightGram,
                        NetWeightGram = x.NetWeightGram,
                        Electrostatic = x.Electrostatic,
                        RecordDate = x.RecordDate,
                        Note = x.Note,
                        PrintNote = x.PrintNote,
                        CreatedDate = x.CreatedDate,
                        CreatedBy = x.CreatedBy,
                        UpdatedDate = x.UpdatedDate,
                        UpdatedBy = x.UpdatedBy,
                        CompanyId = x.CompanyId,
                        IsActive = x.IsActive
                    })
                    .FirstAsync(ct);

                return OperationResult<GetColorChipManufacturingRecord>.Ok(
                    data,
                    "Tạo ColorChipManufacturingRecord thành công.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                return OperationResult<GetColorChipManufacturingRecord>.Fail(
                    $"Tạo ColorChipManufacturingRecord thất bại: {ex.Message}");
            }
        }
    }
}
