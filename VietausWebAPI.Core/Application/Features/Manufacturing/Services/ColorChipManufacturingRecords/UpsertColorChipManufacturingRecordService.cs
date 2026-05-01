using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.ColorChipManufacturingRecords.GetDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.ColorChipManufacturingRecords.PatchDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts.ColorChipManufacturingRecords;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Services.ColorChipManufacturingRecords
{
    public class UpsertColorChipManufacturingRecordService : IUpsertColorChipManufacturingRecordService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public UpsertColorChipManufacturingRecordService(IUnitOfWork unitOfWork, ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<OperationResult<GetColorChipManufacturingRecord>> UpsertAsync(
            UpsertColorChipManufacturingRecordRequest request,
            CancellationToken ct = default)
        {
            if (request == null)
                return OperationResult<GetColorChipManufacturingRecord>.Fail("Request không được để trống.");

            if (!request.ColorChipMfgRecordId.HasValue || request.ColorChipMfgRecordId.Value == Guid.Empty)
                return OperationResult<GetColorChipManufacturingRecord>.Fail("ColorChipMfgRecordId là bắt buộc khi cập nhật.");

            var now = DateTime.Now;
            var userId = _currentUser.EmployeeId;
            var companyId = _currentUser.CompanyId;

            var entity = await _unitOfWork.ColorChipManufacturingRecordReadRepository
                .GetActiveByIdForUpdateAsync(request.ColorChipMfgRecordId.Value, ct);

            if (entity == null)
                return OperationResult<GetColorChipManufacturingRecord>.Fail(
                    "Không tìm thấy ColorChipManufacturingRecord để cập nhật.");

            var changed = ApplyPatch(entity, request);

            if (changed)
            {
                entity.UpdatedDate = now;
                entity.UpdatedBy = userId;

                await _unitOfWork.SaveChangesAsync(ct);
            }

            var data = await _unitOfWork.ColorChipManufacturingRecordReadRepository
                .GetDetailByIdAsync(entity.ColorChipMfgRecordId, ct);

            return data == null
                ? OperationResult<GetColorChipManufacturingRecord>.Fail("Cập nhật thành công nhưng không lấy lại được dữ liệu.")
                : OperationResult<GetColorChipManufacturingRecord>.Ok(
                    data,
                    changed
                        ? "Cập nhật ColorChipManufacturingRecord thành công."
                        : "Không có dữ liệu thay đổi.");
        }




        private static bool ApplyPatch(
            ColorChipManufacturingRecord entity,
            UpsertColorChipManufacturingRecordRequest request)
        {
            var changed = false;

            changed |= PatchHelper.SetIfEnum(request.ResinType, () => entity.ResinType, v => entity.ResinType = v);
            changed |= PatchHelper.SetIfEnum(request.LogoType, () => entity.LogoType, v => entity.LogoType = v);
            changed |= PatchHelper.SetIfEnum(request.FormStyle, () => entity.FormStyle, v => entity.FormStyle = v);

            changed |= PatchHelper.SetIfNullable(request.MfgProductionOrderId, () => entity.MfgProductionOrderId, v => entity.MfgProductionOrderId = v);
            changed |= PatchHelper.SetIfNullable(request.ManufacturingFormulaId, () => entity.ManufacturingFormulaId, v => entity.ManufacturingFormulaId = v);

            changed |= PatchHelper.SetIfRefNullable(request.StandardFormula, () => entity.StandardFormula, v => entity.StandardFormula = v);
            changed |= PatchHelper.SetIfRefNullable(request.Machine, () => entity.Machine, v => entity.Machine = v);
            changed |= PatchHelper.SetIfRefNullable(request.Resin, () => entity.Resin, v => entity.Resin = v);
            changed |= PatchHelper.SetIfRefNullable(request.TemperatureLimit, () => entity.TemperatureLimit, v => entity.TemperatureLimit = v);

            changed |= PatchHelper.SetIfRefNullable(request.SizeText, () => entity.SizeText, v => entity.SizeText = v);
            changed |= PatchHelper.SetIfNullable(request.PelletWeightGram, () => entity.PelletWeightGram, v => entity.PelletWeightGram = v);
            changed |= PatchHelper.SetIfRefNullable(request.NetWeightGram, () => entity.NetWeightGram, v => entity.NetWeightGram = v);
            changed |= PatchHelper.SetIfNullable(request.Electrostatic, () => entity.Electrostatic, v => entity.Electrostatic = v);
            changed |= PatchHelper.SetIfRefNullable(request.DeltaE, () => entity.DeltaE, v => entity.DeltaE = v);   

            changed |= PatchHelper.SetIfNullable(request.RecordDate, () => entity.RecordDate, v => entity.RecordDate = v);
            changed |= PatchHelper.SetIfRefNullable(request.Note, () => entity.Note, v => entity.Note = v);
            changed |= PatchHelper.SetIfRefNullable(request.PrintNote, () => entity.PrintNote, v => entity.PrintNote = v);

            return changed;
        }

  

    }
}
