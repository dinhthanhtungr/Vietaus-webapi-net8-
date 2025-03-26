using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.DTO.QueryObject;

namespace VietausWebAPI.Infrastructure.Utilities
{
    public class QueryableExtensions
    {
        public static async Task<PagedResult<T>> GetPagedAsync<T>(IQueryable<T> queryable, int page, int pageSize)
        {
            var totalCount = queryable.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var items = await queryable.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new PagedResult<T>(items, totalCount, page, pageSize);
            return result;
        }
    }
}
