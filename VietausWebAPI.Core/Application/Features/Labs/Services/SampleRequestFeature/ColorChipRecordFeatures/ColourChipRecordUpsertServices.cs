using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.GetDtos;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.PostDtos;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature.ColorChipRecordFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;

namespace VietausWebAPI.Core.Application.Features.Labs.Services.SampleRequestFeature.ColorChipRecordFeatures
{
    public class ColourChipRecordUpsertServices : IColourChipRecordUpsertServices
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public ColourChipRecordUpsertServices(
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<OperationResult<SaveColorChipRecordResultDto>> UpsertColourChipRecordAsync(
            CreateColorChipRecordRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request == null)
                return OperationResult<SaveColorChipRecordResultDto>.Fail("Dữ liệu gửi lên không hợp lệ.");

            if (request.ProductId == Guid.Empty)
                return OperationResult<SaveColorChipRecordResultDto>.Fail("ProductId không được để trống.");

            var employeeId = _currentUser.EmployeeId;
            var companyId = _currentUser.CompanyId;
            var now = DateTime.Now;

            try
            {
                // Tìm record cũ theo ProductId
                var entity = await _unitOfWork.ColorChipRecordReadRepositories
                    .GetByProductIdAsync(request.ProductId, cancellationToken);

                // =========================
                // CREATE
                // =========================
                if (entity == null)
                {
                    var ensuredCollectionId = await EnsureAttachmentCollectionAsync(request.AttachmentCollectionId, cancellationToken);

                    entity = new ColorChipRecord
                    {
                        ColorChipRecordId = Guid.CreateVersion7(),

                        RecordType = request.RecordType,
                        ResinType = request.ResinType,
                        LogoType = request.LogoType,
                        FormStyle = request.FormStyle,

                        ProductId = request.ProductId,

                        Machine = request.Machine,
                        Resin = request.Resin,
                        TemperatureLimit = request.TemperatureLimit,

                        SizeText = request.SizeText,
                        PelletWeightGram = request.PelletWeightGram,
                        NetWeightGram = request.NetWeightGram,
                        Electrostatic = request.Electrostatic,

                        AttachmentCollectionId = ensuredCollectionId,

                        RecordDate = request.RecordDate,
                        Note = request.Note,
                        PrintNote = request.PrintNote,

                        Lightness = request.Lightness,
                        AValue = request.AValue,
                        BValue = request.BValue,

                        CreatedDate = now,
                        CreatedBy = employeeId,
                        CompanyId = companyId,
                        IsActive = true
                    };

                    await SyncDevelopmentFormulasAsync(entity, request, cancellationToken);

                    await _unitOfWork.ColorChipRecordWriteRepositories.CreateAsync(entity, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return OperationResult<SaveColorChipRecordResultDto>.Ok(
                        new SaveColorChipRecordResultDto
                        {
                            ColorChipRecordId = entity.ColorChipRecordId,
                            ProductId = entity.ProductId!.Value,
                            AttachmentCollectionId = entity.AttachmentCollectionId
                        },
                        "Cập nhật ColorChipRecord thành công.");
                }

                // =========================
                // UPDATE
                // =========================

                if (!entity.AttachmentCollectionId.HasValue || entity.AttachmentCollectionId == Guid.Empty)
                {
                    entity.AttachmentCollectionId = await EnsureAttachmentCollectionAsync(request.AttachmentCollectionId, cancellationToken);
                }
                else if (request.AttachmentCollectionId.HasValue && request.AttachmentCollectionId != Guid.Empty)
                {
                    entity.AttachmentCollectionId = request.AttachmentCollectionId;
                }

                PatchHelper.SetIfEnum(request.RecordType, () => entity.RecordType, v => entity.RecordType = v);
                PatchHelper.SetIfEnum(request.ResinType, () => entity.ResinType, v => entity.ResinType = v);
                PatchHelper.SetIfEnum(request.LogoType, () => entity.LogoType, v => entity.LogoType = v);
                PatchHelper.SetIfEnum(request.FormStyle, () => entity.FormStyle, v => entity.FormStyle = v);

                // ProductId là khóa logic để tìm, thường không patch lại
                PatchHelper.SetIfRefNullable(request.Machine, () => entity.Machine, v => entity.Machine = v);
                PatchHelper.SetIfRefNullable(request.Resin, () => entity.Resin, v => entity.Resin = v);
                PatchHelper.SetIfRefNullable(request.TemperatureLimit, () => entity.TemperatureLimit, v => entity.TemperatureLimit = v);

                PatchHelper.SetIfRefNullable(request.SizeText, () => entity.SizeText, v => entity.SizeText = v);
                PatchHelper.SetIfNullable(request.PelletWeightGram, () => entity.PelletWeightGram, v => entity.PelletWeightGram = v);
                PatchHelper.SetIfRefNullable(request.NetWeightGram, () => entity.NetWeightGram, v => entity.NetWeightGram = v);
                PatchHelper.SetIfNullable(request.Electrostatic, () => entity.Electrostatic, v => entity.Electrostatic = v);

                //// AttachmentCollectionId được phép null => null sẽ set null luôn
                //PatchHelper.SetIfNullable(
                //    request.AttachmentCollectionId,
                //    () => entity.AttachmentCollectionId,
                //    v => entity.AttachmentCollectionId = v);

                PatchHelper.SetIfNullable(request.RecordDate, () => entity.RecordDate, v => entity.RecordDate = v);
                PatchHelper.SetIfRefNullable(request.Note, () => entity.Note, v => entity.Note = v);
                PatchHelper.SetIfRefNullable(request.PrintNote, () => entity.PrintNote, v => entity.PrintNote = v);

                PatchHelper.SetIf(request.Lightness, () => entity.Lightness, v => entity.Lightness = v);
                PatchHelper.SetIf(request.AValue, () => entity.AValue, v => entity.AValue = v);
                PatchHelper.SetIf(request.BValue, () => entity.BValue, v => entity.BValue = v);

                entity.UpdatedDate = now;
                entity.UpdatedBy = employeeId;

                await SyncDevelopmentFormulasAsync(entity, request, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return OperationResult<SaveColorChipRecordResultDto>.Ok(
                    new SaveColorChipRecordResultDto
                    {
                        ColorChipRecordId = entity.ColorChipRecordId,
                        ProductId = entity.ProductId!.Value,
                        AttachmentCollectionId = entity.AttachmentCollectionId
                    },
                    "Cập nhật ColorChipRecord thành công.");
            }
            catch (Exception ex)
            {
                return OperationResult<SaveColorChipRecordResultDto>.Fail($"Upsert ColorChipRecord thất bại: {ex.Message}");
            }
        }

        private async Task SyncDevelopmentFormulasAsync(
            ColorChipRecord entity,
            CreateColorChipRecordRequest request,
            CancellationToken cancellationToken = default)
        {
            var newFormulaId = request.DevelopmentFormulaIds?
                .FirstOrDefault(x => x.HasValue && x.Value != Guid.Empty);

            // Xóa toàn bộ link cũ của record này trực tiếp trong DB
            await _unitOfWork.ColorChipRecordWriteRepositories.QueryDetail()
                .Where(x => x.ColorChipRecordId == entity.ColorChipRecordId)
                .ExecuteDeleteAsync(cancellationToken);

            // Không có formula mới thì dừng
            if (!newFormulaId.HasValue)
                return;

            // Add đúng 1 link mới
            await _unitOfWork.ColorChipRecordWriteRepositories.ColorChipRecordDevelopmentFormulaCreateAsync(new ColorChipRecordDevelopmentFormula
            {
                ColorChipRecordDevelopmentFormulaId = Guid.CreateVersion7(),
                ColorChipRecordId = entity.ColorChipRecordId,
                    DevelopmentFormulaId = newFormulaId.Value,
                    IsActive = true
                }, cancellationToken);
        }

        private async Task<Guid> EnsureAttachmentCollectionAsync(Guid? attachmentCollectionId, CancellationToken ct)
        {
            if (attachmentCollectionId.HasValue && attachmentCollectionId.Value != Guid.Empty)
            {
                var exists = await _unitOfWork.AttachmentCollectionRepository.Query()
                    .AnyAsync(x => x.AttachmentCollectionId == attachmentCollectionId.Value, ct);

                if (exists)
                    return attachmentCollectionId.Value;
            }

            var newCollection = new AttachmentCollection
            {
                AttachmentCollectionId = Guid.CreateVersion7(),
            };

            await _unitOfWork.AttachmentCollectionRepository.AddAsync(newCollection, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return newCollection.AttachmentCollectionId;
        }
    }
}
