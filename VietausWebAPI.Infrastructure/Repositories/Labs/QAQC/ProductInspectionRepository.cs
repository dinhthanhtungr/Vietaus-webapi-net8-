//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
//using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductInspectionFeature;
//using VietausWebAPI.Core.Application.Features.Labs.Queries.ProductStandardFeature;
//using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.QAQCFeature;
//using VietausWebAPI.Core.Application.Shared.Models.PageModels;
//using VietausWebAPI.Core.Domain.Entities;
//using VietausWebAPI.Infrastructure.Utilities;
//using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

//namespace VietausWebAPI.Infrastructure.Repositories.Labs.QAQC
//{
//    public class ProductInspectionRepository : IProductInspectionRepository
//    {
//        private readonly ApplicationDbContext _context;
//        public ProductInspectionRepository(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task DeleteCOARepository(Guid id)
//        {
//            var coa = await _context.ProductInspections.FirstOrDefaultAsync(x => x.Id == id);
//            if (coa == null)
//            {
//                throw new InvalidOperationException($"COA with ID {id} not found.");
//            }
//            _context.ProductInspections.Remove(coa);
//        }

//        public async Task<string?> GetLatestExternalIdStartsWithAsync(string prefix)
//        {
//            return await _context.ProductInspections
//                .Where(p => p.ExternalId.StartsWith(prefix))
//                .OrderByDescending(p => p.ExternalId)
//                .Select(p => p.ExternalId)
//                .FirstOrDefaultAsync();
//        }

//        //public async Task<ProductInspection> GeneralPdf(Guid id)
//        //{
//        //    var productInspection = await _context.ProductInspections
//        //        .AsNoTracking()
//        //        .FirstOrDefaultAsync(x => x.Id == id);
//        //    if (productInspection == null)
//        //    {
//        //        return new ProductInspection(); // Return an empty ProductInspection if not found
//        //    }

//        //    return productInspection;
//        //}

//        public async Task<ProductInspection> GetProductInspectionByIdAsync(Guid id)
//        {
//            var productInspection = await _context.ProductInspections
//                .Include(p => p.Qcdetails)
//                .AsNoTracking()
//                .FirstOrDefaultAsync(x => x.Id == id);

//            // Ensure the method never returns null to match the interface contract
//            if (productInspection == null)
//            {
//                return new ProductInspection(); // Return an empty ProductInspection if not found
//            }

//            return productInspection;
//        }

//        public async Task<PagedResult<ProductInspection>> GetProductInspectionPagedAsync(ProductInspectionQuery? query)
//        {
//            try
//            {
//                var queryable = _context.ProductInspections
//                    .Include(p => p.Qcdetails)
//                    .AsNoTracking()
//                    .AsQueryable();

//                if (!string.IsNullOrWhiteSpace(query.keyword))
//                {
//                    string keyword = query.keyword.ToLower();
//                    queryable = queryable.Where(x =>
//                        x.BatchId != null && EF.Functions.Collate(x.BatchId, "Latin1_General_CI_AI").ToLower().Contains(keyword) ||
//                        x.ProductCode != null && EF.Functions.Collate(x.ProductCode, "Latin1_General_CI_AI").ToLower().Contains(keyword));
//                }

//                if (!string.IsNullOrWhiteSpace(query.types))
//                {
//                    queryable = queryable.Where(x => x.Types.Contains(query.types));
//                }
//                query.PageSize = 15;
//                queryable = queryable.OrderByDescending(x => x.CreateDate);
//                return await QueryableExtensions.GetPagedAsync(queryable, query);
//            }
//            catch (Exception ex)
//            {
//                // Log the exception (if logging is set up)
//                throw new InvalidOperationException("An error occurred while retrieving paged product inspections.", ex);
//            }
//        }

//        public async Task PostProductInspectionAsync(ProductInspection productInspection)
//        {
//            await _context.ProductInspections.AddAsync(productInspection);
//        }

//        public async Task<int> CountAsync(Expression<Func<ProductInspection, bool>> predicate)
//        {
//            return await _context.ProductInspections.CountAsync(predicate);
//        }

//        public Task<List<ProductInspection>> GetProductInspectionListAsync(StatisticalReportQuery query)
//        {
//            var queryable = _context.ProductInspections
//                .AsNoTracking()
//                .AsQueryable();

//            if (!string.IsNullOrWhiteSpace(query.productCode))
//            {
//                queryable = queryable.Where(x => x.ProductCode != null && EF.Functions.Collate(x.ProductCode, "Latin1_General_CI_AI").ToLower().Contains(query.productCode.ToLower()));
//            }
//            if (query.fromDate.HasValue)
//            {
//                queryable = queryable.Where(x => x.CreateDate >= query.fromDate.Value);
//            }
//            if (query.toDate.HasValue)
//            {
//                queryable = queryable.Where(x => x.CreateDate <= query.toDate.Value);
//            }
//            if (!string.IsNullOrWhiteSpace(query.types))
//            {
//                queryable = queryable.Where(x => x.Types.Contains(query.types));
//            }

//            queryable = queryable.OrderByDescending(x => x.CreateDate);
//            return queryable.ToListAsync();
//        }
//    }
//}
