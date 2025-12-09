using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Employees;
using VietausWebAPI.Core.Application.Features.HR.Querys.Employees;
using VietausWebAPI.Core.Application.Features.HR.Querys.Groups;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.HrSchema;
using VietausWebAPI.Core.Identity;

namespace VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts
{
    public interface IEmployeesRepository
    {
        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        //public Task<IEnumerable<EmployeesCommonDatum>> GetEmployeesWithIdRepositoryAsync(string EmployeeId);

        /// <summary>
        /// Tạo nhân viên mới
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        Task PostEmployees(Employee employee);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        Task<string?> GetLatestExternalIdStartsWithAsync(string prefix);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedResult<Employee>> GetPagedWithGroupsAsync(GetEmployeesWithGroupsQuery query);

        Task<List<RoleDTOs>> GetRoleDTOsAsync(CancellationToken ct = default);  

        IQueryable<Employee> Query(bool track = true);
    }
}
