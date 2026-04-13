using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.GetDtos;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.PostDtos;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature.ColorChipRecordFeatures;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature.ColorChipRecordFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;

namespace VietausWebAPI.Core.Application.Features.Labs.Services.SampleRequestFeature.ColorChipRecordFeatures
{
    public class ColourChipRecordWriteServices : IColourChipRecordWriteServices
    {
        private readonly IColourChipRecordUpsertServices _colorChipRecordUpsertServices;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;


        public ColourChipRecordWriteServices(IUnitOfWork unitOfWork
                                           , ICurrentUser currentUser
                                           , IColourChipRecordUpsertServices colorChipRecordUpsertServices)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _colorChipRecordUpsertServices = colorChipRecordUpsertServices;
        }

        public async Task<OperationResult<SaveColorChipRecordResultDto>> CreateAsync(
            CreateColorChipRecordRequest request,
            CancellationToken cancellationToken = default)
        {
            var employeeId = _currentUser.EmployeeId;
            var companyId = _currentUser.CompanyId;

            var existingProduct = await _unitOfWork.ColorChipRecordReadRepositories.CheckExisting(request.ProductId, cancellationToken);
            if (existingProduct)
            {
                await _colorChipRecordUpsertServices.UpsertColourChipRecordAsync(request, cancellationToken);
                return OperationResult<SaveColorChipRecordResultDto>.Ok(
                    new SaveColorChipRecordResultDto
                    {
                        ColorChipRecordId = request.ProductId,
                        ProductId = request.ProductId,
                        AttachmentCollectionId = request.AttachmentCollectionId
                    },
                    "Dữ liệu đã được cập nhật thành công.");
            }

            try
            {
                if (request == null)
                    return OperationResult<SaveColorChipRecordResultDto>.Fail("Dữ liệu gửi lên không hợp lệ.");
                var ensuredCollectionId = await EnsureAttachmentCollectionAsync(request.AttachmentCollectionId, cancellationToken);
                var now = DateTime.Now;

                var entity = new ColorChipRecord
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

                    Lightness = request.Lightness,
                    AValue = request.AValue,
                    BValue = request.BValue,

                    AttachmentCollectionId = ensuredCollectionId,
                    RecordDate = request.RecordDate,
                    Note = request.Note,
                    PrintNote = request.PrintNote,

                    CreatedDate = now,
                    CreatedBy = employeeId,
                    CompanyId = companyId,
                    IsActive = true
                };

                // Tạo bảng liên kết DevelopmentFormula
                if (request.DevelopmentFormulaIds != null && request.DevelopmentFormulaIds.Count > 0)
                {
                    var formulaIds = request.DevelopmentFormulaIds
                        .Where(x => x.HasValue && x.Value != Guid.Empty)
                        .Select(x => x!.Value)
                        .Distinct()
                        .ToList();

                    foreach (var formulaId in formulaIds)
                    {
                        entity.DevelopmentFormulas.Add(new ColorChipRecordDevelopmentFormula
                        {
                            ColorChipRecordDevelopmentFormulaId = Guid.CreateVersion7(),
                            DevelopmentFormulaId = formulaId,
                            IsActive = true
                        });
                    }
                }

                await _unitOfWork.ColorChipRecordWriteRepositories.CreateAsync(entity, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return OperationResult<SaveColorChipRecordResultDto>.Ok(
                    new SaveColorChipRecordResultDto
                    {
                        ColorChipRecordId = entity.ColorChipRecordId,
                        ProductId = entity.ProductId!.Value,
                        AttachmentCollectionId = entity.AttachmentCollectionId
                    },
                    "Tạo ColorChipRecord thành công.");
            }
            catch (Exception ex)
            {
                return OperationResult<SaveColorChipRecordResultDto>.Fail($"Tạo ColorChipRecord thất bại: {ex.Message}");
            }
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
