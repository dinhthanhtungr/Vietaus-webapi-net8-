using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Helpers.Repositories
{

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context) => _context = context;

        public async Task AddAsync(T entity, CancellationToken ct = default)
        {
            await _context.Set<T>().AddAsync(entity, ct);
        }

        public IQueryable<T> Query(bool track = false)
        {
            var q = _context.Set<T>().AsQueryable();
            return track ? q : q.AsNoTracking(); // <- mặc định NoTracking
        }

        public void Remove(T entity) => _context.Set<T>().Remove(entity);
    }
}
