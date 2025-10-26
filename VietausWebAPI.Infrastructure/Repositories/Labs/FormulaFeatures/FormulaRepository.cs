using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.FormulaFeatures;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Labs.FormulaFeatures
{
    public class FormulaRepository : IFormulaRepository
    { 
        private readonly ApplicationDbContext _context;
        public FormulaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Thêm một công thức mới vào cơ sở dữ liệu.
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task AddAsync(Formula formula, CancellationToken ct = default)
        {
            await _context.Formulas.AddAsync(formula, ct);
        }

        public async Task<string?> GetLatestExternalIdStartsWithAsync(string prefix, Guid? id = null)
        {
            var query = _context.Formulas
                .AsNoTracking();


            if (id.HasValue)
            {
                query = query.Where(e => e.ProductId == id.Value);
                query = query.Where(e => e.Name.StartsWith(prefix));
                var temp = await query
                                .OrderByDescending(e => e.Name)
                                .Select(e => e.Name)
                                .FirstOrDefaultAsync();

                return temp;
            }    

            query = query.Where(e => e.ExternalId.StartsWith(prefix));
            var result = await query
                            .OrderByDescending(e => e.ExternalId)
                            .Select(e => e.ExternalId)
                            .FirstOrDefaultAsync(); 

            return result;
        }

        public IQueryable<Formula> Query(bool track = false)
        { 
            var db = _context.Formulas.AsQueryable();
            return track? db : db.AsNoTracking();
        }
    }
}
