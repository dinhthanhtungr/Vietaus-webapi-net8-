using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.DTOs;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.Queries;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Categories;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Catergories;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<PagedResult<GetCategory>>> GetCategoriesAsync(
            CategoryQuery query,
            CancellationToken ct)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 100;

                var baseQuery = _unitOfWork.CategoryRepository.Query();

                var totalItems = await baseQuery.CountAsync(ct);

                baseQuery = baseQuery.Where(x => x.Types != null && x.Types.Contains(query.Type.ToString()));

                var items = await baseQuery
                    .OrderBy(c => c.Name) // hoặc CategoryId cho ổn định
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(c => new GetCategory
                    {
                        CategoryId = c.CategoryId,
                        CategoryExternalId = c.ExternalId ?? "",
                        CategoryName = c.Name,
                    })
                    .ToListAsync(ct);

                var pagedResult = new PagedResult<GetCategory>(
                    items,
                    totalItems,
                    query.PageNumber,
                    query.PageSize
                );

                return OperationResult<PagedResult<GetCategory>>.Ok(pagedResult);
            }
            catch (Exception ex)
            {
                // nếu có logger thì log chi tiết ở đây
                // _logger.LogError(ex, "Lỗi khi lấy danh mục");

                return OperationResult<PagedResult<GetCategory>>
                    .Fail($"Lỗi khi lấy danh sách nhóm hàng: {ex.Message}");
            }
        }

        public async Task<OperationResult<PagedResult<GetUnit>>> GetUnitsAsync(UnitQuery query, CancellationToken ct)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 100;

                var baseQuery = _unitOfWork.UnitRepository.Query();

                var totalItems = await baseQuery.CountAsync(ct);


                var items = await baseQuery
                    .OrderBy(c => c.Name) // hoặc CategoryId cho ổn định
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(c => new GetUnit
                    {
                        Id = c.UnitId,
                        Name = c.Name ?? "",
                    })
                    .ToListAsync(ct);

                var pagedResult = new PagedResult<GetUnit>(
                    items,
                    totalItems,
                    query.PageNumber,
                    query.PageSize
                );

                return OperationResult<PagedResult<GetUnit>>.Ok(pagedResult);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
