using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Helpers.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query(bool track = false);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context) => _context = context;

        public IQueryable<T> Query(bool track = false)
        {
            var q = _context.Set<T>().AsQueryable();
            return track ? q : q.AsNoTracking(); // <- mặc định NoTracking
        }
    }
}
