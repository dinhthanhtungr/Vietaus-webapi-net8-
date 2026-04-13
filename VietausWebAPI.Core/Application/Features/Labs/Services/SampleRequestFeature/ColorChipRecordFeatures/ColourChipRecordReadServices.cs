using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.GetDtos;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature.ColorChipRecordFeatures;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature.ColorChipRecordFeatures;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Labs.Services.SampleRequestFeature.ColorChipRecordFeatures
{
    public class ColourChipRecordReadServices : IColourChipRecordReadServices
    {
        private readonly IColorChipRecordReadRepositories _repository;

        public ColourChipRecordReadServices(IColorChipRecordReadRepositories repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<GetColorChipRecordDto>> GetByIdAsync(
            Guid colorChipRecordId,
            CancellationToken cancellationToken = default)
        {
            if (colorChipRecordId == Guid.Empty)
                return OperationResult<GetColorChipRecordDto>.Fail("ColorChipRecordId không hợp lệ.");

            var entity = await _repository.GetByIdAsync(colorChipRecordId, cancellationToken);
            if (entity == null)
                return OperationResult<GetColorChipRecordDto>.Fail("Không tìm thấy ColorChipRecord.");

            var dto = MapToDto(entity);
            return OperationResult<GetColorChipRecordDto>.Ok(dto);
        }

        public async Task<OperationResult<GetColorChipRecordDto>> GetByProductIdAsync(
            Guid productId,
            CancellationToken cancellationToken = default)
        {
            if (productId == Guid.Empty)
                return OperationResult<GetColorChipRecordDto>.Fail("ProductId không hợp lệ.");

            var entity = await _repository.GetByProductIdAsync(productId, cancellationToken);
            if (entity == null)
                return OperationResult<GetColorChipRecordDto>.Fail("Không tìm thấy ColorChipRecord theo ProductId.");

            var dto = MapToDto(entity);
            return OperationResult<GetColorChipRecordDto>.Ok(dto);
        }

        private static GetColorChipRecordDto MapToDto(Domain.Entities.SampleRequestSchema.ColorChipRecord entity)
        {
            return new GetColorChipRecordDto
            {
                ColorChipRecordId = entity.ColorChipRecordId,

                RecordType = entity.RecordType,
                ResinType = entity.ResinType,
                LogoType = entity.LogoType,
                FormStyle = entity.FormStyle,

                ProductId = entity.ProductId,

                Machine = entity.Machine,
                Resin = entity.Resin,
                TemperatureLimit = entity.TemperatureLimit,

                SizeText = entity.SizeText,
                PelletWeightGram = entity.PelletWeightGram,
                NetWeightGram = entity.NetWeightGram,
                Electrostatic = entity.Electrostatic,

                Lightness = entity.Lightness,
                AValue = entity.AValue,
                BValue = entity.BValue,

                AttachmentCollectionId = entity.AttachmentCollectionId,
                RecordDate = entity.RecordDate,
                Note = entity.Note,
                PrintNote = entity.PrintNote,

                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedBy,
                UpdatedDate = entity.UpdatedDate,
                UpdatedBy = entity.UpdatedBy,

                CompanyId = entity.CompanyId,
                IsActive = entity.IsActive,

                ProductName = entity.Product?.Name,
                ProductExternalId = entity.Product?.ColourCode,

                DevelopmentFormulas = new List<GetColorChipRecordDevelopmentFormulaDto>(entity.DevelopmentFormulas?
                    .Where(x => x.IsActive && x.DevelopmentFormula != null)
                    .Select(x => new GetColorChipRecordDevelopmentFormulaDto
                    {
                        ColorChipRecordDevelopmentFormulaId = x.ColorChipRecordDevelopmentFormulaId,
                        DevelopmentFormulaId = x.DevelopmentFormulaId,
                        DevelopmentFormulaExternalId = x.DevelopmentFormula?.ExternalId,
                        DevelopmentFormulaName = x.DevelopmentFormula?.Name
                    }) ?? Enumerable.Empty<GetColorChipRecordDevelopmentFormulaDto>())
            };
        }
    }
}