using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Employees;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Groups;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Parts;
using VietausWebAPI.Core.Application.Features.HR.Querys.Employees;
using VietausWebAPI.Core.Application.Features.HR.Querys.Groups;
using VietausWebAPI.Core.Application.Features.HR.Querys.Parts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Identity;

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
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public Task<EmployeesPostDTOs> GetEmployeesByIdsAsync(Guid EmployeeId);

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
     
        /// <summary>
        /// láy danh sách nhân viên có tài khoản
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        Task<PagedResult<AccountDTOs>> GetPagedAccoutAsync(EmployeeQuery? keyword);

        /// <summary>
        /// Lấy tất cả danh sách nhóm
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedResult<GetGroupDTOs>> GetAllGroupsAsync(GroupQuery? query);

        /// <summary>
        /// Tạo nhóm mới
        /// </summary>
        /// <returns></returns>
        Task<OperationResult> CreateNewGroupAsync(PostGroupDTOs group);

        /// <summary>
        /// Thêm danh sách nhân viên vào groups
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        Task<OperationResult> AddMembers(IEnumerable<PostMemberDTO> members);

        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<GetGroupInfor> AllMembers(Guid Id, string? keywork = null);

        /// <summary>
        /// Xóa một nhân viên ra khỏi group
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<OperationResult> DeleteMemberInGroupAsync(GroupMemberQuery query);

        /// <summary>
        /// Đổi trạng thái leader
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<OperationResult> changeLeaderStatus(GroupMemberQuery query);


        /// <summary>
        /// Lấy danh sách nhân viên có nhóm
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PagedResult<EmployeeGroupDTO>> GetEmployeesWithGroupsAsync(GetEmployeesWithGroupsQuery query, CancellationToken ct = default);

        Task<List<RoleDTOs>> GetRoleDTOsAsync(CancellationToken ct = default);

        Task<OperationResult<PagedResult<GetParts>>> GetSummaryParts(PartQuery query, CancellationToken ct = default);
    }
}
