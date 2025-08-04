using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductInspectionFeature;
using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductTestFeature;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Infrastructure.Utilities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Labs
{
    public class ProductTestRepository : IProductTestRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductTestRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<PagedResult<ProductTest>> GetAllAsync(ProductTestQuery productTest)
        {
            var queryAble = _context.ProductTests.AsQueryable();
            if (!string.IsNullOrWhiteSpace(productTest.keyword))
            {
                string keyword = productTest.keyword.ToLower();
                queryAble = queryAble.Where(x =>
                    x.ExternalId != null && EF.Functions.Collate(x.ExternalId, "Latin1_General_CI_AI").ToLower().Contains(keyword) ||
                    x.ProductExternalId != null && EF.Functions.Collate(x.ProductExternalId, "Latin1_General_CI_AI").ToLower().Contains(keyword));
            }

            productTest.PageSize = 15;
            queryAble = queryAble.OrderByDescending(x => x.ManufacturingDate);
            return await QueryableExtensions.GetPagedAsync(queryAble, productTest);

        }

        public async Task<ProductTest> GetPagedByIdAsync(string ExternalId)
        {
            var queryAble = await _context.ProductTests.
                AsNoTracking()
                .FirstOrDefaultAsync(x => x.ExternalId == ExternalId);

            if (queryAble == null)
            {
                throw new InvalidOperationException($"ProductStandard with ID {ExternalId} not found.");
            }

            return queryAble;
        }
    }
}
