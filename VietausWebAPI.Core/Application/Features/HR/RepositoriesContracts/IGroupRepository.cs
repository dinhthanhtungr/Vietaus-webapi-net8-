using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Groups;
using VietausWebAPI.Core.Application.Features.HR.Querys.Groups;
using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts
{
    public interface IGroupRepository : IRepository<Group>
    {
        /// <summary>
        /// Tạo nhóm mới
        /// </summary>
        /// <returns></returns>
        Task CreateNewGroupAsync(Group group);

        /// <summary>
        /// Lấy thông tin cụ thể một group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<Group> GetGroupByIdAsync(Guid groupId);
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
        Task<IEnumerable<MemberInGroup>> AllMembers(Guid Id, string? keywork = null);

        /// <summary>
        /// Đổi trạng thái leader
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<int> changeLeaderStatus(GroupMemberQuery query);

        /// <summary>
        /// Xóa thành viên trong nhóm cụ thể
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        Task<int> DeleteMemberInGroupAsync(GroupMemberQuery query);

        /// <summary>
        /// Lấy ID cuối cùng
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        Task<string?> GetLatestExternalIdStartsWithAsync(string prefix);


    }
}
