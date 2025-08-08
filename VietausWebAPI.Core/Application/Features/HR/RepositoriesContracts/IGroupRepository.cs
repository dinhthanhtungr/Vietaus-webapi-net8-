using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Groups;
using VietausWebAPI.Core.Application.Features.HR.Querys.Groups;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts
{
    public interface IGroupRepository
    {
        /// <summary>
        /// Tạo nhóm mới
        /// </summary>
        /// <returns></returns>
        Task CreateNewGroupAsync(Group group);
        /// <summary>
        /// Lấy danh sách nhóm
        /// </summary>
        /// <returns></returns>
        Task<PagedResult<Group>> GetAllGroupsAsync(GroupQuery? query);
        /// <summary>
        /// Xóa nhóm
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        Task<bool> DeleteGroupAsync(Guid groupId);
        /// <summary>
        /// Thêm thành viên trong groups
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        Task AddMembers(IEnumerable<MemberInGroup> members);
        
        /// <summary>
        /// Lấy danh sách tất cả thành viên trong group
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<IEnumerable<MemberInGroup>> AllMembers(Guid Id);

        /// <summary>
        /// Chỉnh thành leader
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task setChangeLeader(Guid Id); 

        /// <summary>
        /// Lấy ID cuối cùng
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        Task<string?> GetLatestExternalIdStartsWithAsync(string prefix);
    }
}
