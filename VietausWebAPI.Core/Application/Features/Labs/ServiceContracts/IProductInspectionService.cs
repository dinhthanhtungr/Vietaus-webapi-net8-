using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductInspectionFeature;
using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductInspectionFeature;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Labs.ServiceContracts
{
    public interface IProductInspectionService
    {
        Task<OperationResult> PostProductInspectionServiceAsync(ProductInspectionInformation productInspection);
        Task<PagedResult<ProductInspectionSummary>> GetProductInspectionPagedAsync(ProductInspectionQuery? query);

        Task<ProductInspection> GetProductInspectionByIdAsync(Guid id);
        Task DeleteCOAService(Guid id);
        Task<byte[]> GeneralPdfService(Guid id);
    }
}
