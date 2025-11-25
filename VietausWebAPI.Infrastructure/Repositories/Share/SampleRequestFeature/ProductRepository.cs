using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;

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

        public async Task<string?> GetLatestProductStartsWithAsync(
                        IQueryable<string> codes, string left, string right, CancellationToken ct = default)
        {
            int leftLen = left.Length, rightLen = right.Length;

            var q = await codes
                .Where(c => c != null
                         && c.StartsWith(left)
                         && c.EndsWith(right)
                         && c.Length > leftLen + rightLen)
                .Select(c => c.Substring(leftLen, c.Length - leftLen - rightLen))
                .ToListAsync(ct);

            // CHỈ chấp nhận tail toàn chữ số (loại "[]", "[1]", "01A", ...):
            var numeric = q.Where(s => !string.IsNullOrEmpty(s) && s.All(char.IsDigit));

            // Lấy lớn nhất: ưu tiên độ dài, rồi tới thứ tự chuỗi
            return numeric
                .OrderByDescending(s => s.Length)
                .ThenByDescending(s => s)
                .FirstOrDefault();

        }


        /// <summary>
        /// Tạo lệnh query để truy vấn sản phẩm từ cơ sở dữ liệu.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Product> Query(bool track = false)
        {
            var db = _context.Products.AsQueryable();
            return track ? db : db.AsNoTracking();
        }
    }
}
