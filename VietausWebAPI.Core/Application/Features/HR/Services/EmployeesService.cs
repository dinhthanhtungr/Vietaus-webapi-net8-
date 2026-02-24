using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.CompanySchema;
using VietausWebAPI.Core.Domain.Entities.HrSchema;
using VietausWebAPI.Core.Identity;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Parts;
using VietausWebAPI.Core.Application.Features.HR.Querys.Parts;

namespace VietausWebAPI.Core.Application.Features.HR.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        public EmployeesService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _currentUser = currentUser;
        }



        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        //public async Task<IEnumerable<EmployeesCommonDatumDTO>> GetEmployeesWithIdServiceAsync(string EmployeeId)
        //{
        //    var employees = await _unitOfWork.EmployeesCommonRepository.GetEmployeesWithIdRepositoryAsync(EmployeeId);
        //    return _mapper.Map<IEnumerable<EmployeesCommonDatumDTO>>(employees);
        //}

        public async Task<PagedResult<AccountDTOs>> GetPagedAccoutAsync(EmployeeQuery? query)
        {
            // 1) Chuẩn hoá query
            query ??= new EmployeeQuery();

            var pageIndex = query.PageNumber > 0 ? query.PageNumber : 1;
            var pageSize = query.PageSize > 0 ? query.PageSize : 10;
            var keyword = query.keyword;

            // 2) Base query từ repository (mặc định NoTracking)
            var q = _unitOfWork.ApplicationUserRepository.Query()
                .Include(u => u.Employee)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .AsQueryable();

            // 3) Filter theo keyword
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var kw = keyword.Trim().ToLower();

                q = q.Where(u =>
                    (u.Employee.ExternalId ?? "").ToLower().Contains(kw) ||
                    (u.personName ?? "").ToLower().Contains(kw)
                );

            }

            // 4) Tổng số bản ghi sau khi filter
            var totalCount = await q.CountAsync();

            // 5) Lấy 1 trang & map thẳng sang DTO + Roles
            var items = await q
                .OrderBy(u => u.UserName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new AccountDTOs
                {
                    Id = u.Id,
                    personName = u.personName,
                    UserName = u.UserName,
                    Email = u.Email,
                    Roles = u.UserRoles
                        .Where(ur => ur.IsActive && ur.Role != null)
                        .Select(ur => ur.Role!.Name!)
                        .ToList(),
                    CancelRoles = new List<string>() // để trống, dùng khi cập nhật quyền
                })
                .ToListAsync();

            // 6) Trả về PagedResult
            return new PagedResult<AccountDTOs>(
                items,
                totalCount,
                pageIndex,
                pageSize
            );
        }

        /// <summary>
        /// Lấy danh sách nhóm
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PagedResult<GetGroupDTOs>> GetAllGroupsAsync(GroupQuery? query)
        {
            query ??= new GroupQuery();

            var pageIndex = query.PageNumber > 0 ? query.PageNumber : 1;
            var pageSize = query.PageSize > 0 ? query.PageSize : 10;
            var keyword = query.keyword?.Trim();
            var q = _unitOfWork.GroupRepository.Query();

            var totalCount = await q.CountAsync();   
            // 3) Filter theo keyword (không phân biệt hoa/thường)
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var kw = keyword.ToLower();

                q = q.Where(g =>
                    (g.Name != null && g.Name.ToLower().Contains(kw)) ||
                    (g.ExternalId != null && g.ExternalId.ToLower().Contains(kw)) ||
                    (g.GroupType != null && g.GroupType.ToLower().Contains(kw))
                );
            }


            var items = await q
                .Include(g => g.MemberInGroups) 
                    .ThenInclude(gm => gm.ProfileNavigation)// hoặc GroupMembers, tuỳ bạn đặt tên
                .OrderByDescending(g => g.CreatedDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(g => new GetGroupDTOs
                {
                    GroupId = g.GroupId,
                    GroupType = g.GroupType,
                    ExternalId = g.ExternalId,
                    Name = g.Name,
                    LeaderName = g.MemberInGroups
                                .FirstOrDefault(m => m.IsAdmin == true && m.ProfileNavigation != null && m.ProfileNavigation.FullName != null) != null
                                    ? g.MemberInGroups.FirstOrDefault(m => m.IsAdmin == true && m.ProfileNavigation != null && m.ProfileNavigation.FullName != null)!.ProfileNavigation!.FullName!
                                    : null,
                    CreatedDate = g.CreatedDate,
                    CreatedBy = g.CreatedBy,
                    MemberCount = g.MemberInGroups.Where(m => m.IsActive).Count()   // hoặc g.GroupMembers.Count()
                })
                .ToListAsync();

            return new PagedResult<GetGroupDTOs>(
                items,
                totalCount,
                pageIndex,
                pageSize
            );
        }

        public async Task<PagedResult<EmployeeSummary>> GetPagedAsync(
            EmployeeQuery? query)
        {
            try
            {
                // 1) Chuẩn hoá query
                query ??= new EmployeeQuery();

                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 15;

                // 2) Base query
                var q = _unitOfWork.EmployeesRepository.Query()
                    .Include(e => e.Part)
                    .AsQueryable(); // VERY IMPORTANT: để var / IQueryable, đừng gán kiểu IIncludableQueryable

                // 3) Lọc theo keyword (Postgres ILIKE)
                if (!string.IsNullOrWhiteSpace(query.keyword))
                {
                    var kw = $"%{query.keyword.Trim()}%";

                    q = q.Where(e =>
                        (e.FullName != null && EF.Functions.ILike(e.FullName, kw)) ||
                        (e.ExternalId != null && EF.Functions.ILike(e.ExternalId, kw))
                    );
                }

                // 4) Map sang EmployeeSummary + sort
                var qSummary = q
                    .OrderByDescending(e => e.DateHired)
                    .Select(e => new EmployeeSummary
                    {
                        EmployeeId = e.EmployeeId,
                        ExternalId = e.ExternalId,
                        FullName = e.FullName ?? string.Empty,
                        Email = e.Email,
                        Gender = e.Gender,
                        PartName = e.Part != null ? e.Part.PartName : null, // đổi Name nếu Part có prop khác
                        PhoneNumber = e.PhoneNumber,
                        DateHired = e.DateHired
                    });

                // 5) Đếm tổng + phân trang
                var totalCount = await qSummary.CountAsync();

                var items = await qSummary
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToListAsync();

                return new PagedResult<EmployeeSummary>(
                    items,
                    totalCount,
                    query.PageNumber,
                    query.PageSize
                );
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
            group.CreatedBy = _currentUser.EmployeeId;
            group.CreatedDate = DateTime.Now;
            group.CompanyId = _currentUser.CompanyId;

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

        public Task<IEnumerable<EmployeesCommonDatumDTO>> GetEmployeesWithIdServiceAsync(string EmployeeId)
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeesPostDTOs> GetEmployeesByIdsAsync(Guid EmployeeId)
        {
            var result = await _unitOfWork.EmployeesRepository.Query()
                .Where(e => e.EmployeeId == EmployeeId)
                .Select(e => _mapper.Map<EmployeesPostDTOs>(e))
                .FirstOrDefaultAsync();

            if (result == null)
                throw new InvalidOperationException($"Employee with ID '{EmployeeId}' not found.");

            return result;
        }

        public async Task<List<RoleDTOs>> GetRoleDTOsAsync(CancellationToken ct = default)
        {
            var roles = await _unitOfWork.EmployeesRepository.GetRoleDTOsAsync(ct);
            return roles;
        }

        public async Task<OperationResult<PagedResult<GetParts>>> GetSummaryParts(PartQuery query, CancellationToken ct = default)
        {
            try
            {
                query ??= new PartQuery();

                var pageSize = query.PageSize <= 0 ? 50 : query.PageSize;
                var pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;
                var skip = (pageNumber - 1) * pageSize;

                // Base query
                IQueryable<Part> q = _unitOfWork.PartRepository.Query();

                // Filter
                if (!string.IsNullOrWhiteSpace(query.keyword))
                {
                    var k = query.keyword.Trim();
                    q = q.Where(x =>
                        x.ExternalId.Contains(k) ||
                        x.PartName.Contains(k));
                }

                //q = query.SortDesc
                //    ? q.OrderByDescending(x => x.ExternalId)
                //    : q.OrderBy(x => x.ExternalId);

                // Total count
                var total = await q.CountAsync(ct);

                // Page items + projection DTO
                var items = await q
                    .Skip(skip)
                    .Take(pageSize)
                    .Select(x => new GetParts
                    {
                        Id = x.PartId,
                        ExternalId = x.ExternalId,
                        Name = x.PartName
                    })
                    .ToListAsync(ct);

                var result = new PagedResult<GetParts>(items, total, pageNumber, pageSize);
                return OperationResult<PagedResult<GetParts>>.Ok(result);
            }
            catch (Exception ex)
            {
                // log ex nếu bạn có ILogger
                return OperationResult<PagedResult<GetParts>>.Fail($"GetSummaryParts failed: {ex.Message}");
            }
        }
    }
}
