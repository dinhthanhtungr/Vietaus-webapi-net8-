using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.DTOs;
using VietausWebAPI.Core.Application.Features.HR.Querys;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.HR.ServiceContracts
{
    public interface IEmployeesService
    {
        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public Task<IEnumerable<EmployeesCommonDatumDTO>> GetEmployeesWithIdServiceAsync(string EmployeeId);
        /// <summary>
        /// Thêm mới nhân viên
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<OperationResult> PostEmployees(EmployeesPostDTOs employee);

        /// <summary>
        /// Lấy danh sách nhân viên phân trang
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedResult<EmployeeSummary>> GetPagedAsync(EmployeeQuery? query);
    }
}
