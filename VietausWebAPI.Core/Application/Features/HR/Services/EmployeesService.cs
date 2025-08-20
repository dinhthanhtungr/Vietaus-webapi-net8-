using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Employees;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Groups;
using VietausWebAPI.Core.Application.Features.HR.Querys.Employees;
using VietausWebAPI.Core.Application.Features.HR.Querys.Groups;
using VietausWebAPI.Core.Application.Features.HR.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Helper;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Identity;
using VietausWebAPI.Core.Repositories_Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VietausWebAPI.Core.Application.Features.HR.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public EmployeesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeesCommonDatumDTO>> GetEmployeesWithIdServiceAsync(string EmployeeId)
        {
            var employees = await _unitOfWork.EmployeesCommonRepository.GetEmployeesWithIdRepositoryAsync(EmployeeId);
            return _mapper.Map<IEnumerable<EmployeesCommonDatumDTO>>(employees);
        }

        public async Task<PagedResult<AccountDTOs>> GetPagedAccoutAsync(EmployeeQuery? query)
        {
            var pagedResult = await _unitOfWork.EmployeesRepository.GetPagedAccoutAsync(query);
            try
            {
                var pagedResultMapped = _mapper.Map<PagedResult<AccountDTOs>>(pagedResult);

                // lấy danh sách userId từ kết quả đã phân trang
                //var userIds = pagedResultMapped.Items.Select(x => x.Id).ToList();

                // Truy ấn thông tin về phân quyền của người dùng


                return pagedResultMapped;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhân viên: {ex.Message}", ex);
            }
        }

        public async Task<PagedResult<GetGroupDTOs>> GetAllGroupsAsync(GroupQuery? query)
        {
            var pagedResult = await _unitOfWork.GroupRepository.GetAllGroupsAsync(query);

            try
            {
                var pagedResultMapped = _mapper.Map<PagedResult<GetGroupDTOs>>(pagedResult);
                return pagedResultMapped;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhóm: {ex.Message}", ex);
            }
        }

        public async Task<PagedResult<EmployeeSummary>> GetPagedAsync(EmployeeQuery? query)
        {
            var pagedResult = await _unitOfWork.EmployeesRepository.GetPagedAsync(query);
            try
            {
                var pagedResultMapped = _mapper.Map<PagedResult<EmployeeSummary>>(pagedResult);
                return pagedResultMapped;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhân viên: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Thêm mới nhân viên
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OperationResult> PostEmployees(EmployeesPostDTOs employee)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if(string.IsNullOrWhiteSpace(employee.ExternalId))
                {
                    employee.ExternalId = await ExternalIdGenerator.GenerateExternalId(
                        "EMP",
                        prefix => _unitOfWork.EmployeesRepository.GetLatestExternalIdStartsWithAsync(prefix)
                    );
                }

                var employeeEntity = _mapper.Map<Employee>(employee);
                await _unitOfWork.EmployeesRepository.PostEmployees(employeeEntity);


                var affected = await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return affected > 0
                    ? OperationResult.Ok("Tạo thành công")
                    : OperationResult.Fail("Thất bại.");
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return OperationResult.Fail($"Lỗi khi tạo nhân viên: {ex.Message}");
            }
        }

        public async Task<OperationResult> CreateNewGroupAsync(PostGroupDTOs group)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                group.ExternalId = await ExternalIdGenerator.GenerateExternalId(
                    "GRP",
                    prefix => _unitOfWork.GroupRepository.GetLatestExternalIdStartsWithAsync(prefix)
                );

                var groupEntity = _mapper.Map<Group>(group);
                await _unitOfWork.GroupRepository.CreateNewGroupAsync(groupEntity);

                var affected = await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return affected > 0
                    ? OperationResult.Ok("Tạo thành công")
                    : OperationResult.Fail("Thất bại.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Lỗi khi tạo nhóm mới: {ex.Message}", ex);
            }
        }

        public async Task<OperationResult> AddMembers(IEnumerable<PostMemberDTO> members)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var groupMembers = _mapper.Map<IEnumerable<MemberInGroup>>(members);

                await _unitOfWork.GroupRepository.AddMembers(groupMembers);

                var affected = await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return affected > 0
                    ? OperationResult.Ok("Thêm thành công")
                    : OperationResult.Fail("Thất bại");
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Lỗi khi thêm nhân viên mới: {ex.Message}", ex);
            }
        }

        public async Task<GetGroupInfor> AllMembers(Guid Id, string? keywork = null)
        {
            var pagedResult = await _unitOfWork.GroupRepository.AllMembers(Id, keywork);
            try
            {
                var result = new GetGroupInfor();

                var pagedResultMapped = _mapper.Map<IEnumerable<GetMemberDTO>>(pagedResult);
                result.members = pagedResultMapped;

                var group = await _unitOfWork.GroupRepository.GetGroupByIdAsync(Id);
                result.ExternalId = group.ExternalId;
                result.GroupId = group.GroupId;
                result.Name = group.Name;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhân viên: {ex.Message}", ex);
            }
        }

        public async Task<OperationResult> changeLeaderStatus(GroupMemberQuery query)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var affected = await _unitOfWork.GroupRepository.changeLeaderStatus(query);

                await _unitOfWork.CommitTransactionAsync();

                return affected > 0
                    ? OperationResult.Ok("Thay đổi thành công")
                    : OperationResult.Fail("Thay đổi thất bại");
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Lỗi khi Thay đổi: {ex.Message}", ex);
            }
        }

        public async Task<OperationResult> DeleteMemberInGroupAsync(GroupMemberQuery query)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var affected = await _unitOfWork.GroupRepository.DeleteMemberInGroupAsync(query);

                await _unitOfWork.CommitTransactionAsync();


                return affected > 0
                    ? OperationResult.Ok("Thay đổi thành công")
                    : OperationResult.Fail("Thay đổi thất bại");
            }

            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Lỗi khi Thay đổi: {ex.Message}", ex);
            }
        }

        public async Task<PagedResult<EmployeeGroupDTO>> GetEmployeesWithGroupsAsync(GetEmployeesWithGroupsQuery query, CancellationToken ct = default)
        {
            var paged = await _unitOfWork.EmployeesRepository.GetPagedWithGroupsAsync(query);

            // Map list
            var items = _mapper.Map<List<EmployeeGroupDTO>>(paged.Items);

            // (tuỳ chọn) loại trùng group nếu dữ liệu membership có thể bị double
            foreach (var e in items)
            {
                e.Groups = e.Groups
                    .GroupBy(g => new { g.GroupId, g.IsAdmin }) // hoặc chỉ GroupId tuỳ mong muốn
                    .Select(g => g.First())
                    .ToList();
            }

            return new PagedResult<EmployeeGroupDTO>(items, paged.TotalCount, paged.Page, paged.PageSize);
        }
        // Implement methods from IEmployeesCommonService here
    }
}
