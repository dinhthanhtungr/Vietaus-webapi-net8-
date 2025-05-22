using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories
{
    public class EmployeesCommonRepository : IEmployeesCommonRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public EmployeesCommonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Implement methods from IEmployeesCommonRepository here
        public async Task<IEnumerable<EmployeesCommonDatum>> GetEmployeesWithIdRepositoryAsync(string EmployeeName)
        {
            return await _context.EmployeesCommonData
                .Include(x => x.Part)
                .AsNoTracking()
                .Where(x => x.Email == EmployeeName)
                .ToListAsync();
        }
    }
}
