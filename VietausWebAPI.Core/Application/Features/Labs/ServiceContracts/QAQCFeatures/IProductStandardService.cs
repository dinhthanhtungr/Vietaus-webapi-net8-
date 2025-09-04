using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.ProductStandardFeature;
using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductStandardFeature;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.QAQCFeatures
{
    public interface IProductStandardService
    {
        Task<PagedResult<ProductStandardSummaryDTO>> GetProductStandardsPagedAsync(ProductStandardQuery? query);
        Task<ProductStandardInformation> GetProductStandardIdAsync(Guid id);
        Task<ProductStandardInformation> GetProductStandardProductIdAsync(Guid id);
  
        Task<OperationResult> PostProductStandardService(ProductStandardInformation productStandard);

        Task<OperationResult> UpdateProductStandardService(Guid id, ProductStandardInformation productStandard);
        Task<OperationResult> DeleteProductStandardService(Guid id);
    }
}
