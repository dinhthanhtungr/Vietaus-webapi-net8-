using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Share.SampleRequestFeature
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) { _context = context; }

        /// <summary>
        /// Thêm một sản phẩm mới vào cơ sở dữ liệu.
        /// </summary>
        /// <param name="sampleRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task AddAsync(Product sampleRequest, CancellationToken ct = default)
        {
            await _context.Products.AddAsync(sampleRequest, ct);
        }

        /// <summary>
        /// Kiêm tra xem sản phẩm có tồn tại trong cơ sở dữ liệu hay không.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(Guid productId, CancellationToken ct)
        {
            return await _context.Products.AsNoTracking().AnyAsync(p => p.ProductId == productId, ct);
        }

        /// <summary>
        /// Tạo lệnh query để truy vấn sản phẩm từ cơ sở dữ liệu.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Product> Query()
        {
            return _context.Products.AsNoTracking();
        }
    }
}
