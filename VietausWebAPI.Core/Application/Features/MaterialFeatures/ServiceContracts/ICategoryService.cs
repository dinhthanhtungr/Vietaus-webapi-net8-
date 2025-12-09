using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.DTOs;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.Queries;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Categories;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Catergories;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts
{
    public interface ICategoryService
    {
        Task<OperationResult<PagedResult<GetCategory>>> GetCategoriesAsync(CategoryQuery query, CancellationToken ct);
        Task<OperationResult<PagedResult<GetUnit>>> GetUnitsAsync(UnitQuery query, CancellationToken ct);
    }
}
