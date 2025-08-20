using VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.ProductStandardFeature;
using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductStandardFeature;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.QAQCFeature
{
    public interface IProductStandardRepository
    {
        Task<PagedResult<ProductStandard>> GetProductStandardsPagedAsync(ProductStandardQuery? query);
        Task<ProductStandard> GetProductStandardIdAsync(Guid id);
        Task<ProductStandard> GetProductStandardProductIdAsync(Guid id);
        Task PostProductStandard(ProductStandard productStandard);
        Task<string?> GetLatestExternalIdStartsWithAsync(string prefix);
        Task UpdateProductStandard(Guid id, ProductStandardInformation productStandard);
        Task DeleteProductStandard(Guid id);
    }
}
