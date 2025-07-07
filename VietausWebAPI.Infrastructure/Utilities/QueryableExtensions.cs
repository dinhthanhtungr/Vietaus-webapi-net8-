using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Infrastructure.Utilities
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> GetPagedAsync<T>(IQueryable<T> queryable, PaginationQuery query)
        {
            int pageNumber = Math.Max(1, query.PageNumber);
            int pageSize = Math.Max(1, query.PageSize);

            int totalItems = await queryable.CountAsync();
            var items = await queryable
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>(items, totalItems, pageNumber, pageSize);
        }
    }
}
