
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Query.Internal;
//using VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.ProductStandardFeature;
//using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductStandardFeature;
//using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.QAQCFeature;
//using VietausWebAPI.Core.Application.Shared.Models.PageModels;
//using VietausWebAPI.Core.Domain.Entities;
//using VietausWebAPI.Infrastructure.Utilities;
//using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;

//namespace VietausWebAPI.Infrastructure.Repositories.Labs.QAQC
//{
//    public class ProductStandardRepository : IProductStandardRepository
//    {
//        private readonly ApplicationDbContext _context;
//        public ProductStandardRepository(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<ProductStandard> GetProductStandardIdAsync(Guid id)
//        {
//            var productStandard = await _context.ProductStandards
//                .AsNoTracking()
//                .FirstOrDefaultAsync(x => x.Id == id);

//            // Ensure the method matches the interface by returning a non-nullable ProductStandard.
//            if (productStandard == null)
//            {
//                throw new InvalidOperationException($"ProductStandard with ID {id} not found.");
//            }

//            return productStandard;
//        }

//        public async Task<PagedResult<ProductStandard>> GetProductStandardsPagedAsync(ProductStandardQuery? query)
//        {
//            var queryable = _context.ProductStandards
//                .AsNoTracking()
//                .AsQueryable();

//            if (!string.IsNullOrWhiteSpace(query.keyword))
//            {
//                string keyword = query.keyword.ToLower();
//                queryable = queryable.Where(x =>
//                    x.ExternalId != null && EF.Functions.Collate(x.ExternalId, "Latin1_General_CI_AI").ToLower().Contains(keyword) ||
//                    x.ColourCode != null && EF.Functions.Collate(x.ColourCode, "Latin1_General_CI_AI").ToLower().Contains(keyword) ||
//                    x.CustomerExternalId != null && EF.Functions.Collate(x.CustomerExternalId, "Latin1_General_CI_AI").ToLower().Contains(keyword) ||
//                    x.ProductExternalId != null && EF.Functions.Collate(x.ProductExternalId, "Latin1_General_CI_AI").ToLower().Contains(keyword));
//            }
//            query.PageSize = 15;
//            queryable = queryable.OrderByDescending(x => x.CreatedDate);
//            return await QueryableExtensions.GetPagedAsync(queryable, query);
//        }

//        public async Task PostProductStandard(ProductStandard productStandard)
//        {
//            await _context.ProductStandards.AddAsync(productStandard);

//        }

//        public async Task UpdateProductStandard(Guid id , ProductStandardInformation productStandard)
//        {
//            var existing = await _context.ProductStandards.FirstOrDefaultAsync(x => x.Id == id);
//            if (existing is null)
//                throw new Exception("Không tìm thấy!");

//            // EF Core để gán tất cả property tự động 
//            _context.Entry(existing).CurrentValues.SetValues(productStandard);

//        }

//        public async Task<string?> GetLatestExternalIdStartsWithAsync(string prefix)
//        {
//            return await _context.ProductStandards
//                .Where(p => p.ExternalId.StartsWith(prefix))
//                .OrderByDescending(p => p.ExternalId)
//                .Select(p => p.ExternalId)
//                .FirstOrDefaultAsync();
//        }

//        public async Task DeleteProductStandard(Guid id)
//        {
//            var productStandard = await _context.ProductStandards.FirstOrDefaultAsync(x => x.Id == id);
//            if (productStandard == null)
//            {
//                throw new InvalidOperationException($"ProductStandard with ID {id} not found.");
//            }
//            _context.ProductStandards.Remove(productStandard);
//        }

//        public async Task<ProductStandard> GetProductStandardProductIdAsync(Guid id)
//        {
            
//            var productStandard = await _context.ProductStandards
//                .AsNoTracking()
//                .FirstOrDefaultAsync(x => x.ProductId == id);

//            // Ensure the method matches the interface by returning a non-nullable ProductStandard.
//            if (productStandard == null)
//            {
//                throw new InvalidOperationException($"ProductStandard with ID {id} not found.");
//            }

//            return productStandard;
            
//        }
//    }
//}
