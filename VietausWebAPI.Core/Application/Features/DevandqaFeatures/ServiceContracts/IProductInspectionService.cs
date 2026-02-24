using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.ProductInspectionFeature;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.Queries.ProductInspectionFeature;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.ServiceContracts
{
    public interface IProductInspectionService
    {
        Task<OperationResult> PostProductInspectionServiceAsync(PostProductInspectionRequest productInspection, CancellationToken ct);
        Task<OperationResult<PagedResult<ProductInspectionSummary>>> GetProductInspectionPagedAsync(ProductInspectionQuery? query, CancellationToken ct);
        Task<OperationResult<ProductInspectionInformation>> GetProductInspectionByIdAsync(Guid id, CancellationToken ct);
        Task<OperationResult<PagedResult<GetProductCOA>>> GetProductCOAService(ProductInspectionQuery query, CancellationToken ct);

        Task<OperationResult> DeleteCOAService(Guid id, CancellationToken ct);
        Task<OperationResult<byte[]>> GeneralPdfService(Guid id, CancellationToken ct);
        Task<OperationResult<byte[]>> GeneralQCPdfService(StatisticalReportQuery query, CancellationToken ct);
    }
}
