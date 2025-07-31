using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductInspectionFeature;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts
{
    public interface IProductInspectionRepository
    {
        Task PostProductInspectionAsync(ProductInspection productInspection);
        Task<PagedResult<ProductInspection>> GetProductInspectionPagedAsync(ProductInspectionQuery? query);  
        Task<ProductInspection> GetProductInspectionByIdAsync(Guid id);
        Task<string?> GetLatestExternalIdStartsWithAsync(string prefix);
        Task DeleteCOARepository(Guid id);
        //Task<ProductInspection> GeneralPdf(Guid id);
        Task<int> CountAsync(Expression<Func<ProductInspection, bool>> predicate);
    }
}
