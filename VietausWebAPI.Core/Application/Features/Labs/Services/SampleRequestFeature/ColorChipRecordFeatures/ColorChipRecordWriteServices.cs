using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.PostDtos;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature.ColorChipRecordFeatures;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature.ColorChipRecordFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;

namespace VietausWebAPI.Core.Application.Features.Labs.Services.SampleRequestFeature.ColorChipRecordFeatures
{
    public class ColorChipRecordWriteServices : IColorChipRecordWriteServices
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;


        public ColorChipRecordWriteServices(IUnitOfWork unitOfWork, ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<OperationResult> CreateAsync(CreateColorChipRecordRequest request, CancellationToken cancellationToken = default)
        {
            var employeeId = _currentUser.EmployeeId;
            var companyId = _currentUser.CompanyId;

            try
            {
                if (request == null)
                    return OperationResult.Fail("Dữ liệu gửi lên không hợp lệ.");

                if (request.AttachmentCollectionId == Guid.Empty)
                    return OperationResult.Fail("AttachmentCollectionId không được để trống.");
                

                var entity = new ColorChipRecord
                {
                    ColorChipRecordId = Guid.CreateVersion7(),

                    RecordType = request.RecordType,
                    ChipPurpose = request.ChipPurpose,
                    ResinType = request.ResinType,

                    ProductId = request.ProductId,
                    ProductCodeSnapshot = request.ProductCodeSnapshot,
                    ProductNameSnapshot = request.ProductNameSnapshot,
                    ColorNameSnapshot = request.ColorNameSnapshot,

                    ManufacturingFormulaId = request.ManufacturingFormulaId,
                    ManufacturingFormulaExternalIdSnapshot = request.ManufacturingFormulaExternalIdSnapshot,

                    DevelopmentFormulaId = request.DevelopmentFormulaId,
                    DevelopmentFormulaExternalIdSnapshot = request.DevelopmentFormulaExternalIdSnapshot,

                    AttachmentCollectionId = request.AttachmentCollectionId,
                    RecordDate = request.RecordDate,

                    CustomerId = request.CustomerId,
                    CustomerExternalIdSnapshot = request.CustomerExternalIdSnapshot,
                    CustomerNameSnapshot = request.CustomerNameSnapshot,

                    AddRate = request.AddRate,
                    Resin = request.Resin,

                    TemperatureMin = request.TemperatureMin,
                    TemperatureMax = request.TemperatureMax,

                    SizeText = request.SizeText,
                    PelletWeightGram = request.PelletWeightGram,
                    AntiStaticInfo = request.AntiStaticInfo,

                    Note = request.Note,
                    PrintNote = request.PrintNote,

                    CreatedDate = DateTime.Now,
                    CreatedBy = employeeId,
                    CompanyId = companyId,
                    IsActive = true
                };

                await _unitOfWork.ColorChipRecordWriteRepositories.CreateAsync(entity, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return OperationResult.Ok();
            }
            catch (Exception ex)
            {
                return OperationResult.Fail($"Tạo ColorChipRecord thất bại: {ex.Message}");
            }
        }
    }
}
