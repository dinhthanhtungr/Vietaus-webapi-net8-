using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Employees;
using VietausWebAPI.Core.Application.Features.HR.Querys.Employees;
using VietausWebAPI.Core.Application.Features.HR.Querys.Groups;
using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Identity;
using VietausWebAPI.Infrastructure.Utilities;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using VietausWebAPI.Core.Domain.Entities.HrSchema;

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
        //public async Task<IEnumerable<EmployeesCommonDatum>> GetEmployeesWithIdRepositoryAsync(string EmployeeName)
        //{
        //    return await _context.EmployeesCommonData
        //        .Include(x => x.Part)
        //        .AsNoTracking()
        //        .Where(x => x.Email == EmployeeName)
        //        .ToListAsync();
        //}

        public async Task<string?> GetLatestExternalIdStartsWithAsync(string prefix)
        {
            return await _context.Employees
                .Where(e => e.ExternalId.StartsWith(prefix))
                .OrderByDescending(e => e.ExternalId)
                .Select(e => e.ExternalId)
                .FirstOrDefaultAsync();
        }
        public async Task PostEmployees(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
        }



        public async Task<PagedResult<Employee>> GetPagedWithGroupsAsync(GetEmployeesWithGroupsQuery query)
        {
            // Base: Employees + navigation cần cho Groups (filtered include)
            var q = _context.Employees
                .AsNoTracking()
                .Include(e => e.MemberInGroups
                    .Where(m => !query.OnlyActiveMembership || m.IsActive == true)
                    .Where(m => !query.CompanyId.HasValue || m.Group.CompanyId == query.CompanyId)
                    // Dùng ILike thay cho Contains để case-insensitive
                    .Where(m => string.IsNullOrWhiteSpace(query.GroupType)
                                || EF.Functions.ILike(m.Group.GroupType, $"%{query.GroupType}%"))
                )
                .ThenInclude(m => m.Group)
                .AsQueryable();

            // Keyword (không đụng tới group để khỏi loại nhân viên không có group)
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var kw = $"%{query.Keyword.Trim()}%";
                q = q.Where(e =>
                    (e.FullName != null && EF.Functions.ILike(e.FullName, kw)) ||
                    (e.Email != null && EF.Functions.ILike(e.Email, kw)) ||
                    (e.ExternalId != null && EF.Functions.ILike(e.ExternalId, kw))
                );
            }

            // KHÔNG .Where(e => e.MemberInGroups.Any(...)) nữa, để không loại nhân viên không có group
            // (Nếu bạn thực sự muốn lọc ra chỉ những nhân viên thuộc GroupType nào đó,
            //  bỏ comment dưới và hiểu rằng sẽ loại người không có group phù hợp)
            // if (!string.IsNullOrWhiteSpace(query.GroupType))
            // {
            //     q = q.Where(e => e.MemberInGroups.Any(m => EF.Functions.ILike(m.Group.GroupType, $"%{query.GroupType}%")));
            // }

            // Lọc theo ExternalId của Part (case-insensitive)
            if (!string.IsNullOrWhiteSpace(query.ExternalId))
            {
                var kw = $"%{query.ExternalId.Trim()}%";
                q = q.Where(e => e.Part.ExternalId != null && EF.Functions.ILike(e.Part.ExternalId, kw));
            }

            query.PageSize = 15;
            q = q.OrderBy(e => e.FullName);

            return await QueryableExtensions.GetPagedAsync(q, query);
        }

        public IQueryable<Employee> Query(bool track = true)
        {
            var db = _context.Employees.AsQueryable();
            return track ? db : db.AsNoTracking();
        }

        public async Task<List<RoleDTOs>> GetRoleDTOsAsync(CancellationToken ct = default)
        {
            List<RoleDTOs> roles = await _context.Roles
                .Select(r => new RoleDTOs
                {
                    Id = r.Id,
                    Name = r.Name,
                }).ToListAsync(ct);
            return roles;
        }
    }
}
