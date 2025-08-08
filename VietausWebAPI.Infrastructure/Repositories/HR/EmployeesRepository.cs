using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.Querys.Employees;
using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Identity;
using VietausWebAPI.Infrastructure.Utilities;
using VietausWebAPI.WebAPI.DatabaseContext;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VietausWebAPI.Infrastructure.Repositories.HR
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public EmployeesRepository(ApplicationDbContext context)
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

        public async Task<string?> GetLatestExternalIdStartsWithAsync(string prefix)
        {
            return await _context.Employees
                .Where(e => e.ExternalId.StartsWith(prefix))
                .OrderByDescending(e => e.ExternalId)
                .Select(e => e.ExternalId)
                .FirstOrDefaultAsync();
        }

        public async Task<PagedResult<ApplicationUser>> GetPagedAccoutAsync(EmployeeQuery? query)
        {
            var queryable = _context.Users
                .Include(u => u.UserRoles)         // ✅ Bỏ Where!
                    .ThenInclude(ur => ur.Role)    // ✅ Đảm bảo load được Role.Name
                .AsNoTracking()
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.keyword))
            {
                string keywordLower = query.keyword.ToLower();
                queryable = queryable.Where(x =>
                    x.personName != null && EF.Functions.Collate(x.personName, "Latin1_General_CI_AI").ToLower().Contains(keywordLower) ||
                    x.Email != null && EF.Functions.Collate(x.Email, "Latin1_General_CI_AI").ToLower().Contains(keywordLower) ||
                    x.UserName != null && EF.Functions.Collate(x.UserName, "Latin1_General_CI_AI").ToLower().Contains(keywordLower));
            }

            query.PageSize = 15;
            queryable = queryable.OrderByDescending(x => x.UserName);
            return await QueryableExtensions.GetPagedAsync(queryable, query);
        }

        public async Task<PagedResult<Employee>> GetPagedAsync(EmployeeQuery? query)
        {
            var queryable = _context.Employees
                .Include(e => e.Part)
                .AsNoTracking()
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.keyword))
            {
                string keywordLower = query.keyword.ToLower();
                queryable = queryable.Where(x =>
                    x.FullName != null && EF.Functions.Collate(x.FullName, "Latin1_General_CI_AI").ToLower().Contains(keywordLower) ||
                    x.Email != null && EF.Functions.Collate(x.Email, "Latin1_General_CI_AI").ToLower().Contains(keywordLower) ||
                    x.ExternalId != null && EF.Functions.Collate(x.ExternalId, "Latin1_General_CI_AI").ToLower().Contains(keywordLower));
            }

            query.PageSize = 15;
            queryable = queryable.OrderByDescending(x => x.DateHired);
            return await QueryableExtensions.GetPagedAsync(queryable, query);
        }

        public async Task PostEmployees(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
        }
    }
}
